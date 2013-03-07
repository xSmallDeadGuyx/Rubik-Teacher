using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rubik_Teacher {
	public class Tutorial {
		public MainForm form;

		public int stage {
			get { 
				return _stage;
			}
			set { 
				_stage = value;
				resetStage();
			}
		}
		private int _stage;

		public int sequence {
			get {
				return _sequence;
			}
			set {
				_sequence = value;
				resetSequence();
			}
		}
		private int _sequence = 0;

		public string[] text = {
			"Welcome to Rubik Teacher, the program that will teach you how to solve a Rubik's cube! Just press the next button on the right to begin, and we will start with a bit of theory",
			"The first thing I'd like you to understand is how the cube is to be treated. A lot of people look at a cube and just see squares of colours. If you notice, the squares of colour on a corner are actually connected to 2 others on that piece. The trick is to think about getting a piece into it's right place between faces, and not just get a coloured square on the right side.",
			"Now that you have developed the basic understanding of how the cube works, I want to teach you some common notation for sequences to be performed on the cube. Each move you perform on a cube can be denoted by one or two characters, where one character identifies the face of the cube and the other is the type of rotation. F B L R U D are the face characters, which stand for Front (green), Back (blue), Left (yellow), Right (white), Up (orange), and down (red). The colour in brackets is just what I will be using in this tutorial. The move F is the rotation of the front face clockwise, for an anticlockwise rotation you would use F', and for 180 degrees you would use 2F. Try out some moves below with the sequence picker",
			"As you are familiar with enough theory behind the Rubik's Cube, you are ready to begin solving. We will start by taking the top face (orange for me) and moving pieces onto it to make a cross. There aren't particularly specific ways of doing this but there are a couple of examples below. Be sure to put the right piece into each place in the cross, as in the ones with the other colour matching the middle piece of the adjacent side.\r\n\r\nLet's start by moving the orange+green piece into position with a U'. After this we now can move the orange+yellow piece into position with D' F L' F', with the F' removing the change to the orange+green piece. The orange+white piece can be put in place with B R'. Finally the orange+blue piece with U R' U'",
			"Once the cross is done, we now want to finish the top face with the corners. As with the edge pieces, there isn't a specific way of doing this you must just think logically about it. A couple of sequences showing how it'd be done on this cube are below.\r\n\r\nStarting with the orange+green+yellow corner need to rotate it with F' 2D F D then put it in place with F' D' F. We can do similar sequences with the orange+blue+yellow by moving it first with a D, on the different faces the new sequence is L' 2D L D L' D' L. This conveniently places the orange+green+white piece in a very similar position needing the exact same sequence from the right face, so we get R' 2D R D R' D' R. The last corner can be rotated with B' D B R D R'",
			"The second layer is much more straightforward in terms of sequences, with just one you need to remember. Take a look at the bottom for an edge piece which doesn't contain red, so blue+white, green+white, blue+yellow, or green+yellow. Now whatever colour is on the bottom face you want to put opposite to where it's face (shown in the sequences below). If the face of the other colour's side is to the right of the first colour's you want to do the following moves as normal, otherwise if it's on the left you want to do it mirrored starting with your left hand. Holding 2nd colour's face with your right hand and first with your left, looking at the bottom, you want to twist your right-hand clockwise, left-hand anti-clockwise, right-hand anti-clockwise, left-hand anti-clockwise, right-hand anti-clockwise, the bottom face anti-clockwise, right-hand clockwise. If a piece is already in place but the wrong orientation, put a different piece in temporary to take it out then put it back in the right orientation.\r\n\r\nTo get the blue+yellow piece in we need to do that sequence mirrored starting with your left hand. Grab the blue face with your left-hand, and the yellow face with your right-hand, and perform the sequence mirrored as follows: B' L B L' B D B'. The same sequence can be used on the green+white piece if we first move it with 2D, with the mirrored left-hand version: 2D F' R F R' F D F'. The next 2 pieces to solve are both in the second layer, so we need to get the green+yellow one out first with B R' B' R B' D' B. After this we can put it into the right place with the mirrored sequence starting with your left-hand with 2D L' F L F' L D L'. The last blue+white piece, having been removed from it's position in the second layer when we put the green+yellow piece in, can now be made correct using the normal sequence starting with your right-hand: 2D B R' B' R B' D' B"
		};

		public string[][] moves = {
			new string[] {},
			new string[] {},
			new string[] {"F F' 2F", "B B' 2B", "L L' 2L", "R R' 2R", "U U' 2U", "D D' 2D"},
			new string[] {"U'", "D' F L' F'", "B R'", "U R' U'"},
			new string[] {"F' 2D F D F' D' F", "D L' 2D L D L' D' L", "R' 2D R D R' D' R", "B' D B R D R'"},
			new string[] {"B' L B L' B D B'", "2D F' R F R' F D F'", "B R' B' R B' D' B 2D L' F L F' L D L'", "2D B R' B' R B' D' B"}
		};

		public string[][] positions = {
			new string[] {},
			new string[] {},
			new string[] {"000000000111111111222222222333333333444444444555555555", "000000111000111111222222222333333333445445445455455455", "111000111000111000222222222333333333545545545454454454", "011100011100011100322322322332332332545545545454454454", "010101010101010101323323323232232232545545545454454454", "010101010101010101232323323323232232454545545545454454", "010101010101010101232323232323232323454545454545454545"},
			new string[] {"435001433233514200110222551140435431323141400542552502", "513303404233514200323222551542435431140141400110552502", "513003404055510252323222432541433405340441112110552031", "025000404053512342321223434145034531340141512252153150"},
			new string[] {"503000404052513344321222435530234331240141512250455111", "503000004135415410221322513530234321244145332250455114", "003000004150414331221322315533231514444245135250455212", "003000000345213153222324131533231514444245244550155211"},
			new string[] {"000000000153514215222324224333231135444245114555154133", "000000000213115414222324121333233151444445335555154522", "000000000531111244222322215333233311444445421555554351", "000000000242115131222222415333233415444444351555551133"}
		};

		public Vector3[][] solvingPiece = {
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {new Vector3(0, 0, 1), new Vector3(2, 2, 1), new Vector3(1, 2, 2), new Vector3(2, 1, 2)},
			new Vector3[] {new Vector3(0, 2, 0), new Vector3(2, 2, 2), new Vector3(2, 2, 0), new Vector3(2, 0, 2)},
			new Vector3[] {new Vector3(2, 2, 1), new Vector3(2, 2, 1), new Vector3(2, 1, 2), new Vector3(2, 2, 1)}
		};

		public Vector3[][] targetPlace = {
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(2, 0, 1), new Vector3(1, 0, 2)},
			new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 0, 2), new Vector3(2, 0, 0), new Vector3(2, 0, 2)},
			new Vector3[] {new Vector3(0, 1, 2), new Vector3(2, 1, 0), new Vector3(0, 1, 0), new Vector3(2, 1, 2)}
		};

		public Tutorial(MainForm f) {
			form = f;
			stage = 0;
		}

		public bool previousStage() {
			if(stage > 0)
				stage--;
			return stage > 0;
		}

		public bool nextStage() {
			if(stage < text.Length - 1)
				stage++;
			return stage < text.Length - 1;
		}

		public bool nextSequence() {
			if(sequence < moves[stage].Length - 1)
				sequence++;
			return sequence < moves[stage].Length - 1;
		}

		public bool prevSequence() {
			if(sequence > 0)
				sequence--;
			return sequence > 0;
		}

		public void resetStage() {
			_sequence = 0;

			form.tutorialTextbox.Text = "Stage " + (stage + 1) + " of " + text.Length + ": " + text[stage];
			form.tutorialTextbox.Select(0, 0);

			resetSequence();
		}

		public void resetSequence() {
			updateButtons();

			if(stage < positions.Length)
				if(sequence < positions[stage].Length)
					form.rubikTeacher.fromString(positions[stage][sequence]);

			form.rubikTeacher.clearHighlights();
			if(stage < solvingPiece.Length)
				if(sequence < solvingPiece[stage].Length) {
					Vector3 v = solvingPiece[stage][sequence];
					form.rubikTeacher.pieceHighlighted[(int) v.X, (int) v.Y, (int) v.Z] = true;
				}

			if(stage < targetPlace.Length)
				if(sequence < targetPlace[stage].Length) {
					Vector3 v = targetPlace[stage][sequence];
					form.rubikTeacher.targetHighlighted[(int) v.X, (int) v.Y, (int) v.Z] = true;
				}
		}

		public void updateButtons() {
			if(stage < moves.Length && moves[stage].Length > 0) {
				if(sequence < moves[stage].Length) {
					form.playStageButton.Enabled = true;
					form.playStageButton.Text = "Play sequence: " + moves[stage][sequence];

					form.nextSequenceButton.Enabled = sequence < moves[stage].Length - 1;
				}
			}
			else {
				form.playStageButton.Enabled = false;
				form.playStageButton.Text = "No sequences for this stage";

				form.nextSequenceButton.Enabled = false;
			}

			form.prevSequenceButton.Enabled = sequence > 0;
		}
	}
}
