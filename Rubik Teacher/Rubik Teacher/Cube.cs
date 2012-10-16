using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Rubik_Teacher {
	public enum FaceID {Top, Bottom, Front, Back, Left, Right};
	public enum CubeMove {Clockwise, AntiClockwise, Double};

	public struct Move {
		public FaceID face;
		public CubeMove twist;
		public Move(FaceID f, CubeMove t) { face = f; twist = t; }
	}

	public class Cube {

		public readonly string faceLetters = "UDFBLR";
		public readonly string validStringCharacters = "UDFBLR'2";
		public FaceID[,,] faceColours = new FaceID[6, 3, 3];

		private List<Move> undoMoves = new List<Move>();
		private List<Move> redoMoves = new List<Move>();

		public Color[] colourIDs = {Color.Orange, Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.White};

		public Cube() {
			for(int i = 0; i < 6; i++)
				for(int x = 0; x < 3; x++) for(int y = 0; y < 3; y++)
					faceColours[i, x, y] = (FaceID) i;
		}

		public int xFromBottomLeft(int x, int y, int rot) {
			switch(rot) {
				case 1:
					return y;
				case 2:
					return 2 - x;
				case -1:
					return 2 - y;
			}
			return x;
		}

		public int yFromBottomLeft(int x, int y, int rot) {
			switch(rot) {
				case 1:
					return 2 - x;
				case 2:
					return 2 - y;
				case -1:
					return x;
			}
			return x;
		}

		private FaceID[,] rotateFace(FaceID[,] face, CubeMove rot) {
			FaceID[,] newFace = new FaceID[5, 5];
			for(int i = 0; i < 5; i++) for(int j = 0; j < 5; j++)
				switch(rot) {
					case CubeMove.Clockwise:
						newFace[4 - j, i] = face[i, j];
						break;
					case CubeMove.Double:
						newFace[4 - i, 4 - j] = face[i, j];
						break;
					case CubeMove.AntiClockwise:
						newFace[j, 4 - i] = face[i, j];
						break;
				}
			return newFace;
		}

		public bool isValidMove(String s) {
			for(int i = 0; i < 6; i++) {
				char l = faceLetters[i];
				if(s == l.ToString()) return true;
				if(s == l.ToString() + "'") return true;
				if(s == "2" + l.ToString()) return true;
			}
			return false;
		}

		public void performMove(String s) {
			for(int i = 0; i < 6; i++) {
				char l = faceLetters[i];
				if(s == l.ToString()) {
					performMove((FaceID) i, CubeMove.Clockwise);
					return;
				}
				if(s == l.ToString() + "'") {
					performMove((FaceID) i, CubeMove.AntiClockwise);
					return;
				}
				if(s == "2" + l.ToString()) {
					performMove((FaceID) i, CubeMove.Double);
					return;
				}
			}
		}

		public Move toMove(String s) {
			for(int i = 0; i < 6; i++) {
				char l = faceLetters[i];
				if(s == l.ToString())
					return new Move((FaceID) i, CubeMove.Clockwise);
				if(s == l.ToString() + "'")
					return new Move((FaceID) i, CubeMove.AntiClockwise);
				if(s == "2" + l.ToString())
					return new Move((FaceID) i, CubeMove.Double);
			}
			return new Move();
		}

		public void performMove(Move move) {
			performMove(move.face, move.twist);
		}

		public void performMove(FaceID face, CubeMove rot) {
			if(redoMoves.Count > 0) {
				redoMoves.Clear();
				undoMoves.Clear();
			}
			undoMoves.Add(new Move(face, rot));
			actualMove(face, rot);
		}

		public void undoNMoves(int n) {
			for(int i = 0; i < n; i++) undoMove();
		}

		public void undoMove() {
			if(undoMoves.Count > 0) {
				Move move = undoMoves.Last<Move>();
				undoMoves.RemoveAt(undoMoves.Count - 1);
				redoMoves.Add(move);
				actualMove(move.face, move.twist);
			}
		}

		public void redoNMoves(int n) {
			for(int i = 0; i < n; i++) redoMove();
		}

		public void redoMove() {
			if(redoMoves.Count > 0) {
				Move move = redoMoves.Last<Move>();
				redoMoves.RemoveAt(undoMoves.Count - 1);
				undoMoves.Add(move);
				actualMove(move.face, move.twist);
			}
		}

		private void actualMove(FaceID face, CubeMove rot) {
			FaceID[,] transposed = new FaceID[5, 5];
			switch(face) {
				case FaceID.Top:
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
						transposed[i + 1, j + 1] = faceColours[(int) FaceID.Top, i, j];
					for(int i = 0; i < 3; i++) {
						transposed[i + 1, 0] = faceColours[(int) FaceID.Back, 2 - i, 0];
						transposed[0, i + 1] = faceColours[(int) FaceID.Left, i, 0];
						transposed[4, i + 1] = faceColours[(int) FaceID.Right, 2 - i, 0];
						transposed[i + 1, 4] = faceColours[(int) FaceID.Front, i, 0];
					}
					transposed = rotateFace(transposed, rot);
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
						faceColours[(int) FaceID.Top, i, j] = transposed[i + 1, j + 1];
					for(int i = 0; i < 3; i++) {
						faceColours[(int) FaceID.Back, 2 - i, 0] = transposed[i + 1, 0];
						faceColours[(int) FaceID.Left, i, 0] = transposed[0, i + 1];
						faceColours[(int) FaceID.Right, 2 - i, 0] = transposed[4, i + 1];
						faceColours[(int) FaceID.Front, i, 0] = transposed[i + 1, 4];
					}
					break;
				case FaceID.Left:
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							transposed[i + 1, j + 1] = faceColours[(int) FaceID.Left, i, j];
					for(int i = 0; i < 3; i++) {
						transposed[i + 1, 0] = faceColours[(int) FaceID.Top, 0, i];
						transposed[0, i + 1] = faceColours[(int) FaceID.Back, 2, i];
						transposed[4, i + 1] = faceColours[(int) FaceID.Front, 0, i];
						transposed[i + 1, 4] = faceColours[(int) FaceID.Bottom, 0, 2 - i];
					}
					transposed = rotateFace(transposed, rot);
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							faceColours[(int) FaceID.Left, i, j] = transposed[i + 1, j + 1];
					for(int i = 0; i < 3; i++) {
						faceColours[(int) FaceID.Top, 0, i] = transposed[i + 1, 0];
						faceColours[(int) FaceID.Back, 2, i] = transposed[0, i + 1];
						faceColours[(int) FaceID.Front, 0, i] = transposed[4, i + 1];
						faceColours[(int) FaceID.Bottom, 0, 2 - i] = transposed[i + 1, 4];
					}
					break;
				case FaceID.Front:
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							transposed[i + 1, j + 1] = faceColours[(int) FaceID.Front, i, j];
					for(int i = 0; i < 3; i++) {
						transposed[i + 1, 0] = faceColours[(int) FaceID.Top, i, 2];
						transposed[0, i + 1] = faceColours[(int) FaceID.Left, 2, i];
						transposed[4, i + 1] = faceColours[(int) FaceID.Right, 0, i];
						transposed[i + 1, 4] = faceColours[(int) FaceID.Bottom, i, 0];
					}
					transposed = rotateFace(transposed, rot);
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							faceColours[(int) FaceID.Front, i, j] = transposed[i + 1, j + 1];
					for(int i = 0; i < 3; i++) {
						faceColours[(int) FaceID.Top, i, 2] = transposed[i + 1, 0];
						faceColours[(int) FaceID.Left, 2, i] = transposed[0, i + 1];
						faceColours[(int) FaceID.Right, 0, i] = transposed[4, i + 1];
						faceColours[(int) FaceID.Bottom, i, 0] = transposed[i + 1, 4];
					}
					break;
				case FaceID.Right:
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							transposed[i + 1, j + 1] = faceColours[(int) FaceID.Right, i, j];
					for(int i = 0; i < 3; i++) {
						transposed[i + 1, 0] = faceColours[(int) FaceID.Top, 2, 2 - i];
						transposed[0, i + 1] = faceColours[(int) FaceID.Front, 2, i];
						transposed[4, i + 1] = faceColours[(int) FaceID.Back, 0, i];
						transposed[i + 1, 4] = faceColours[(int) FaceID.Bottom, 2, i];
					}
					transposed = rotateFace(transposed, rot);
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							faceColours[(int) FaceID.Right, i, j] = transposed[i + 1, j + 1];
					for(int i = 0; i < 3; i++) {
						faceColours[(int) FaceID.Top, 2, 2 - i] = transposed[i + 1, 0];
						faceColours[(int) FaceID.Front, 2, i] = transposed[0, i + 1];
						faceColours[(int) FaceID.Back, 0, i] = transposed[4, i + 1];
						faceColours[(int) FaceID.Bottom, 2, i] = transposed[i + 1, 4];
					}
					break;
				case FaceID.Bottom:
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							transposed[i + 1, j + 1] = faceColours[(int) FaceID.Bottom, i, j];
					for(int i = 0; i < 3; i++) {
						transposed[i + 1, 0] = faceColours[(int) FaceID.Front, i, 2];
						transposed[0, i + 1] = faceColours[(int) FaceID.Left, 2 - i, 2];
						transposed[4, i + 1] = faceColours[(int) FaceID.Right, i, 2];
						transposed[i + 1, 4] = faceColours[(int) FaceID.Back, 2 - i, 2];
					}
					transposed = rotateFace(transposed, rot);
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							faceColours[(int) FaceID.Bottom, i, j] = transposed[i + 1, j + 1];
					for(int i = 0; i < 3; i++) {
						faceColours[(int) FaceID.Front, i, 2] = transposed[i + 1, 0];
						faceColours[(int) FaceID.Left, 2 - i, 2] = transposed[0, i + 1];
						faceColours[(int) FaceID.Right, i, 2] = transposed[4, i + 1];
						faceColours[(int) FaceID.Back, 2 - i, 2] = transposed[i + 1, 4];
					}
					break;
				case FaceID.Back:
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							transposed[i + 1, j + 1] = faceColours[(int) FaceID.Back, i, j];
					for(int i = 0; i < 3; i++) {
						transposed[i + 1, 0] = faceColours[(int) FaceID.Top, 2 - i, 0];
						transposed[0, i + 1] = faceColours[(int) FaceID.Right, 2, i];
						transposed[4, i + 1] = faceColours[(int) FaceID.Left, 0, i];
						transposed[i + 1, 4] = faceColours[(int) FaceID.Bottom, 2 - i, 2];
					}
					transposed = rotateFace(transposed, rot);
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++)
							faceColours[(int) FaceID.Back, i, j] = transposed[i + 1, j + 1];
					for(int i = 0; i < 3; i++) {
						faceColours[(int) FaceID.Top, 2 - i, 0] = transposed[i + 1, 0];
						faceColours[(int) FaceID.Right, 2, i] = transposed[0, i + 1];
						faceColours[(int) FaceID.Left, 0, i] = transposed[4, i + 1];
						faceColours[(int) FaceID.Bottom, 2 - i, 2] = transposed[i + 1, 4];
					}
					break;
			}
		}
	}
}
