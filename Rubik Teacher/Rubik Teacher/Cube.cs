using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Rubik_Teacher {
	public enum FaceID {Top, Bottom, Front, Back, Left, Right}; // values and data types for convenient storage
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

		public readonly string faceLetters = "UDFBLR"; // information about valid move strings
		public readonly string validStringCharacters = "UDFBLR'2";
		public FaceID[,,] faceColours = new FaceID[6, 3, 3]; // actual cube data array

		public Color[] colourIDs = {Color.Orange, Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.White}; // colour ids for each face

		public Cube() { // reset cube upon initialize
			reset();
		}

		public void reset() {
			for(int i = 0; i < 6; i++) // iterate all faces and reset the pieces to normal
				for(int x = 0; x < 3; x++) for(int y = 0; y < 3; y++)
						faceColours[i, x, y] = (FaceID) i;
		}

		public override string ToString() { // output the cube as a string
			string str = "";
			for(int i = 0; i < 6; i++)
				for(int j = 0; j < 3; j++)
					for(int k = 0; k < 3; k++)
						str += "" + ((int)faceColours[i, k, j]);
			return str;
		}

		public void fromString(string str) { // update the cube based on a string
			for(int i = 0; i < 6; i++)
				for(int j = 0; j < 3; j++)
					for(int k = 0; k < 3; k++) {
						faceColours[i, k, j] = (FaceID)(int.Parse(str.Substring(0, 1)));
						str = str.Remove(0, 1);
					}
		}

		private FaceID[,] rotateFace(FaceID[,] face, CubeMove rot) { // rotate all the colours on a face and adjacent squares
			FaceID[,] newFace = new FaceID[5, 5];
			for(int i = 0; i < 5; i++) for(int j = 0; j < 5; j++) // for each piece on the face
				switch(rot) {
					case CubeMove.Clockwise:
						newFace[4 - j, i] = face[i, j]; // if clockwise rotation move all pieces around clockwise
						break;
					case CubeMove.Double:
						newFace[4 - i, 4 - j] = face[i, j]; // if double rotation move them around 180 degrees
						break;
					case CubeMove.AntiClockwise: // if anti-clockwise move them around anti-clockwise
						newFace[j, 4 - i] = face[i, j];
						break;
				}
			return newFace;
		}

		public bool isValidMove(String s) { // check if string contains valid move
			for(int i = 0; i < 6; i++) {
				char l = faceLetters[i];
				if(s == l.ToString()) return true;
				if(s == l.ToString() + "'") return true;
				if(s == "2" + l.ToString()) return true;
			}
			return false;
		}

		public void performMove(String s) { // perform the move in string
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

		public Move toMove(String s) { // return a Move object based on move stored as a string
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

		public void performMove(Move move) { // perform move stored in Move object
			performMove(move.face, move.twist);
		}

		public void performMove(FaceID face, CubeMove rot) { // perform move stored in FaceID value and CubeMove value
			actualMove(face, rot);
		}

		private void actualMove(FaceID face, CubeMove rot) {
			FaceID[,] transposed = new FaceID[5, 5]; // create a temporary store for face being moved and adjacent piece colours
			for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) {
				FaceCoord coords = transposeFromFront(face, FaceID.Front, i, j); // pull off the colours of the face and put them into the transpose array
				transposed[i + 1, j + 1] = faceColours[(int) coords.face, coords.i, coords.j];
			}
			for(int i = 0; i < 3; i++) { // pull off the affected colours from the 4 adjacent edges
				FaceCoord coords = transposeFromFront(face, FaceID.Top, i, 2);
				transposed[i + 1, 0] = faceColours[(int) coords.face, coords.i, coords.j];
				coords = transposeFromFront(face, FaceID.Left, 2, i);
				transposed[0, i + 1] = faceColours[(int) coords.face, coords.i, coords.j];
				coords = transposeFromFront(face, FaceID.Right, 0, i);
				transposed[4, i + 1] = faceColours[(int) coords.face, coords.i, coords.j];
				coords = transposeFromFront(face, FaceID.Bottom, i, 0);
				transposed[i + 1, 4] = faceColours[(int) coords.face, coords.i, coords.j];
			}
			transposed = rotateFace(transposed, rot); // perform the actual rotation on the colours
			for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) { // place the newly rotated colours back onto the rotated face
				FaceCoord coords = transposeFromFront(face, FaceID.Front, i, j);
				faceColours[(int) coords.face, coords.i, coords.j] = transposed[i + 1, j + 1];
			}
			for(int i = 0; i < 3; i++) { // place the newly rotated colours back onto the adjacent faces
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

		public FaceCoord transposeFromFront(FaceID target, FaceID face, int i, int j) { // take a co-ordinate to a colour on a face and convert it for use on another face
			return transposeFromFront(target, new FaceCoord(face, i, j));
		}

        public FaceCoord transposeFromFront(FaceID target, FaceCoord coords) { // take a co-ordinate to a colour on a face and convert it for use on another face
			FaceCoord newCoords = new FaceCoord(coords.face, coords.i, coords.j);
			switch(target) { // hard-coded changes for each face to change the co-ordinates in the relevant way
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
