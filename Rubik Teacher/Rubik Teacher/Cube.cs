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

	public struct FaceCoord {
		public FaceID face;
		public int i;
		public int j;

		public FaceCoord(FaceID f, int i, int j) { face = f; this.i = i; this.j = j; }
	}

	public class Cube {

		public readonly string faceLetters = "UDFBLR";
		public readonly string validStringCharacters = "UDFBLR'2";
		public FaceID[,,] faceColours = new FaceID[6, 3, 3];

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
			actualMove(face, rot);
		}

		private void actualMove(FaceID face, CubeMove rot) {
			FaceID[,] transposed = new FaceID[5, 5];
			for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) {
				FaceCoord coords = transposeFromFront(face, FaceID.Front, i, j);
				transposed[i + 1, j + 1] = faceColours[(int) coords.face, coords.i, coords.j];
			}
			for(int i = 0; i < 3; i++) {
				FaceCoord coords = transposeFromFront(face, FaceID.Top, i, 2);
				transposed[i + 1, 0] = faceColours[(int) coords.face, coords.i, coords.j];
				coords = transposeFromFront(face, FaceID.Left, 2, i);
				transposed[0, i + 1] = faceColours[(int) coords.face, coords.i, coords.j];
				coords = transposeFromFront(face, FaceID.Right, 0, i);
				transposed[4, i + 1] = faceColours[(int) coords.face, coords.i, coords.j];
				coords = transposeFromFront(face, FaceID.Bottom, i, 0);
				transposed[i + 1, 4] = faceColours[(int) coords.face, coords.i, coords.j];
			}
			transposed = rotateFace(transposed, rot);
			for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) {
				FaceCoord coords = transposeFromFront(face, FaceID.Front, i, j);
				faceColours[(int) coords.face, coords.i, coords.j] = transposed[i + 1, j + 1];
			}
			for(int i = 0; i < 3; i++) {
				FaceCoord coords = transposeFromFront(face, FaceID.Top, i, 2);
				faceColours[(int) coords.face, coords.i, coords.j] = transposed[i + 1, 0];
				coords = transposeFromFront(face, FaceID.Left, 2, i);
				faceColours[(int) coords.face, coords.i, coords.j] = transposed[0, i + 1];
				coords = transposeFromFront(face, FaceID.Right, 0, i);
				faceColours[(int) coords.face, coords.i, coords.j] = transposed[4, i + 1];
				coords = transposeFromFront(face, FaceID.Bottom, i, 0);
				faceColours[(int) coords.face, coords.i, coords.j] = transposed[i + 1, 4];
			}
		}

		public FaceCoord transposeFromFront(FaceID target, FaceID face, int i, int j) {
			return transposeFromFront(target, new FaceCoord(face, i, j));
		}

		public FaceCoord transposeFromFront(FaceID target, FaceCoord coords) {
			FaceCoord newCoords = new FaceCoord(coords.face, coords.i, coords.j);
			switch(target) {
				case FaceID.Top:
					switch(coords.face) {
						case FaceID.Front:
							newCoords.face = FaceID.Top;
							break;
						case FaceID.Back:
							newCoords.face = FaceID.Bottom;
							newCoords.i = 2 - coords.i;
							newCoords.j = 2 - coords.j;
							break;
						case FaceID.Left:
							newCoords.i = coords.j;
							newCoords.j = 2 - coords.i;
							break;
						case FaceID.Right:
							newCoords.i = 2 - coords.j;
							newCoords.j = coords.i;
							break;
						case FaceID.Top:
							newCoords.face = FaceID.Back;
							newCoords.i = 2 - coords.i;
							newCoords.j = 2 - coords.j;
							break;
						case FaceID.Bottom:
							newCoords.face = FaceID.Front;
							break;
					}
					break;
				case FaceID.Bottom:
					switch(coords.face) {
						case FaceID.Front:
							newCoords.face = FaceID.Bottom;
							break;
						case FaceID.Back:
							newCoords.face = FaceID.Top;
							newCoords.i = 2 - coords.i;
							newCoords.j = 2 - coords.j;
							break;
						case FaceID.Left:
							newCoords.i = 2 - coords.j;
							newCoords.j = coords.i;
							break;
						case FaceID.Right:
							newCoords.i = coords.j;
							newCoords.j = 2 - coords.i;
							break;
						case FaceID.Top:
							newCoords.face = FaceID.Front;
							break;
						case FaceID.Bottom:
							newCoords.face = FaceID.Back;
							newCoords.i = 2 - coords.i;
							newCoords.j = 2 - coords.j;
							break;
					}
					break;
				case FaceID.Left:
					switch(coords.face) {
						case FaceID.Front:
							newCoords.face = FaceID.Left;
							break;
						case FaceID.Back:
							newCoords.face = FaceID.Right;
							break;
						case FaceID.Left:
							newCoords.face = FaceID.Back;
							break;
						case FaceID.Right:
							newCoords.face = FaceID.Front;
							break;
						case FaceID.Top:
							newCoords.i = 2 - coords.j;
							newCoords.j = coords.i;
							break;
						case FaceID.Bottom:
							newCoords.i = coords.j;
							newCoords.j = 2 - coords.i;
							break;
					}
					break;
				case FaceID.Right:
					switch(coords.face) {
						case FaceID.Front:
							newCoords.face = FaceID.Right;
							break;
						case FaceID.Back:
							newCoords.face = FaceID.Left;
							break;
						case FaceID.Left:
							newCoords.face = FaceID.Front;
							break;
						case FaceID.Right:
							newCoords.face = FaceID.Back;
							break;
						case FaceID.Top:
							newCoords.i = coords.j;
							newCoords.j = 2 - coords.i;
							break;
						case FaceID.Bottom:
							newCoords.i = 2 - coords.j;
							newCoords.j = coords.i;
							break;
					}
					break;
				case FaceID.Front:
					return coords;
				case FaceID.Back:
					switch(coords.face) {
						case FaceID.Front:
							newCoords.face = FaceID.Back;
							break;
						case FaceID.Back:
							newCoords.face = FaceID.Front;
							break;
						case FaceID.Left:
							newCoords.face = FaceID.Right;
							break;
						case FaceID.Right:
							newCoords.face = FaceID.Left;
							break;
						case FaceID.Top:
							newCoords.i = 2 - coords.i;
							newCoords.j = 2 - coords.j;
							break;
						case FaceID.Bottom:
							newCoords.i = 2 - coords.i;
							newCoords.j = 2 - coords.j;
							break;
					}
					break;
			}
			return newCoords;
		}
	}
}
