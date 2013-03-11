using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rubik_Teacher {
	public class Tutorial {
		public MainForm form;

		public int stage { // wrapper for stage property to reset upon change
			get { 
				return _stage;
			}
			set { 
				_stage = value;
				resetStage();
			}
		}
		private int _stage;

		public int sequence { // wrapper for sequence property to reset upon change
			get {
				return _sequence;
			}
			set {
				_sequence = value;
				resetSequence();
			}
		}
		private int _sequence = 0;

		public string[] text = { // text to be given to user at each stage
			"Welcome to Rubik Teacher, the program that will teach you how to solve a Rubik's cube! Just press the next button on the right to begin, and we will start with a bit of theory",
			"The first thing I'd like you to understand is how the cube is to be treated. A lot of people look at a cube and just see squares of colours. If you notice, the squares of colour on a corner are actually connected to 2 others on that piece. The trick is to think about getting a piece into it's right place between faces, and not just get a coloured square on the right side.",
			"Now that you have developed the basic understanding of how the cube works, I want to teach you some common notation for sequences to be performed on the cube. Each move you perform on a cube can be denoted by one or two characters, where one character identifies the face of the cube and the other is the type of rotation. F B L R U D are the face characters, which stand for Front (green), Back (blue), Left (yellow), Right (white), Up (orange), and down (red). The colour in brackets is just what I will be using in this tutorial. The move F is the rotation of the front face clockwise, for an anticlockwise rotation you would use F', and for 180 degrees you would use 2F. Try out some moves below with the sequence picker",
			"As you are familiar with enough theory behind the Rubik's Cube, you are ready to begin solving. We will start by taking the top face (orange for me) and moving pieces onto it to make a cross. There aren't particularly specific ways of doing this but there are a couple of examples below. Be sure to put the right piece into each place in the cross, as in the ones with the other colour matching the middle piece of the adjacent side.\r\n\r\nLet's start by moving the orange+green piece into position with a U'. After this we now can move the orange+yellow piece into position with D' F L' F', with the F' removing the change to the orange+green piece. The orange+white piece can be put in place with B R'. Finally the orange+blue piece with U R' U'",
			"Once the cross is done, we now want to finish the top face with the corners. As with the edge pieces, there isn't a specific way of doing this you must just think logically about it. A couple of sequences showing how it'd be done on this cube are below.\r\n\r\nStarting with the orange+green+yellow corner need to rotate it with F' 2D F D then put it in place with F' D' F. We can do similar sequences with the orange+blue+yellow by moving it first with a D, on the different faces the new sequence is L' 2D L D L' D' L. This conveniently places the orange+green+white piece in a very similar position needing the exact same sequence from the right face, so we get R' 2D R D R' D' R. The last corner can be rotated with B' D B R D R'",
			"The second layer is much more straightforward in terms of sequences, with just one you need to remember. Take a look at the bottom for an edge piece which doesn't contain red, so blue+white, green+white, blue+yellow, or green+yellow. Now whatever colour is on the bottom face you want to put opposite to where it's face (shown in the sequences below). If the face of the other colour's side is to the right of the first colour's you want to do the following moves as normal, otherwise if it's on the left you want to do it mirrored starting with your left hand. Holding 2nd colour's face with your right hand and first with your left, looking at the bottom, you want to twist your right-hand clockwise, left-hand anti-clockwise, right-hand anti-clockwise, left-hand anti-clockwise, right-hand anti-clockwise, the bottom face anti-clockwise, right-hand clockwise. If a piece is already in place but the wrong orientation, put a different piece in temporary to take it out then put it back in the right orientation.\r\n\r\nTo get the blue+yellow piece in we need to do that sequence mirrored starting with your left hand. Grab the blue face with your left-hand, and the yellow face with your right-hand, and perform the sequence mirrored as follows: B' L B L' B D B'. The same sequence can be used on the green+white piece if we first move it with 2D, with the mirrored left-hand version: 2D F' R F R' F D F'. The next 2 pieces to solve are both in the second layer, so we need to get the green+yellow one out first with B R' B' R B' D' B. After this we can put it into the right place with the mirrored sequence starting with your left-hand with 2D L' F L F' L D L'. The last blue+white piece, having been removed from it's position in the second layer when we put the green+yellow piece in, can now be made correct using the normal sequence starting with your right-hand: 2D B R' B' R B' D' B",
			"For the final layer we want to start by making a cross on it, similar to how we did on the top face. We begin by rotating the pieces of the cross to be facing the right way, so with the red square adjacent to the red center. We do this by putting two we want to rotate at the front (looking at the corner between them), we hold the left face in our left-hand the right in the other. We then rotate right-hand anti-clockwise, the bottom face anti-clockwise, the left hand anti-clockwise, the top face clockwise, the left hand clockwise, and finally the right-hand clockwise. How the orientation works is slightly complicated, so take a look at the example.\r\n\r\nAs all 4 of the edge pieces are not oriented correctly, we can now just perform the sequence from any side to the same result. Picking the green and white sides to hold in my right and left hands respectively, I perform the sequence once. On this cube, this looks like F' D' R' D R F. In this position we don't have adjacent pieces to rotate, so we perform the sequence again from the same position: F' D' R' D R F. We now have the ideal position for this sequence so we hold the cube between the blue and yellow faces and perform one final time from there: B' D' L' D L B",
			"Once the cross is rotate correctly like this, we need to swap the edge pieces without changing their orientation to complete it. One simple sequence to do this is if you hold the 2 pieces you want to swap around on the corner opposite you are holding (in the same way you held it in the previous step with the corner of two side faces and the bottom to you), we spin our right-hand anti-clockwise, the bottom face anti-clockwise, the right-hand clockwise, bottom face anti-clockwise again, the right-hand clockwise, the bottom face 180 degrees, the right-hand clockwise again, and finally the bottom face anti-clockwise again.\r\n\r\nIn the example below, the yellow and green pieces are correct relative to each other, so a D' will put them on their respective positions. Now we need to swap the white and blue pieces around, so holding the green face in the left hand and yellow face in the right hand perform the swap sequence: L' D' L D' L' 2D L D'",
			"Unfortunately in my example solve below we have what is known as a PLL skip, which means skipping the corner positioning in the last layer. If this didn't happen we would need move the bottom face around as opposed just rotating them as in the example. For this reason I will explain the sequences we use as they undo each other. In most cases on the last face you will find 3 corner pieces on the last face out of position, and they will rotate in a clockwise or anti-clockwise position across two edges and the diagonal on the last face. If these pieces need to be rotated clockwise we will hold the cube such that the bottom face is pointing upwards, with the face containing two of the unsolved corner pieces in the right-hand and the opposite face (which will only have one unsolved piece, towards you). We rotate our right-hand anti-clockwise, the bottom face clockwise, the left-hand clockwise, the bottom face anti-clockwise, the right-hand clockwise, the bottom face clockwise, the left-hand anti-clockwise, and finally the bottom face anti-clockwise. To rotate the 3 pieces anti-clockwise you simply reverse the sequence there.\r\n\r\nIn this example, I will just show you the sequences to no effect on the cube. Hold the yellow face in your right-hand and the white face in your left to perform the sequence below. This looks like: L' D R D' L D R' D'. To reverse this just perform the opposite moves in the reverse order: D R D' L' D R' D' L",
			"Now we'll actually finish the rotation of the finally pieces. We have two sequences for this, where one is the reverse of the other. The sequences only affect the one corner on the top face so we can happily move different corners into the same position to undo changes on the other faces affected. These sequences take one corner on the top and rotate it either clockwise, or the reverse sequence which rotates it clockwise. On a cube mixed normally it is impossible to encounter a situation where you have to only rotate one corner, or rotate an even number of corners in the same direction. To rotate a corner anti-clockwise we hold it towards us with the right-hand holding the face with the red part of it (in this situation, with red on the bottom face). We then rotate the right-hand anti-clockwise, the top face anti-clockwise, the right-hand clockwise, the top face clockwise, the right-hand anti-clockwise, the top face anti-clockwise, and finally the right-hand clockwise. To rotate clockwise you just reverse that sequence.\r\n\r\nIn this example we want to rotate the green+yellow+red piece anti-clockwise and the other unsolved one clockwise. We start by holding the yellow face in our right-hand, so the sequence is: L' U' L U L' U' L. To rotate the next piece clockwise move it into the position where the yellow+green+red one was with D' and perform the reverse: L' U L U' L' U L. Once the top face is solved we just rotate it back with D."
		};

		public string[][] moves = { // move sequences to demonstrate during each stage
			new string[] {},
			new string[] {},
			new string[] {"F F' 2F", "B B' 2B", "L L' 2L", "R R' 2R", "U U' 2U", "D D' 2D"},
			new string[] {"U'", "D' F L' F'", "B R'", "U R' U'"},
			new string[] {"F' 2D F D F' D' F", "D L' 2D L D L' D' L", "R' 2D R D R' D' R", "B' D B R D R'"},
			new string[] {"B' L B L' B D B'", "2D F' R F R' F D F'", "B R' B' R B' D' B 2D L' F L F' L D L'", "2D B R' B' R B' D' B"},
			new string[] {"F' D' R' D R F", "F' D' R' D R F", "B' D' L' D L B"},
			new string[] {"D' L' D' L D' L' 2D L D'"},
			new string[] {"L' D R D' L D R' D'", "D R D' L' D R' D' L"},
			new string[] {"L' U' L U L' U' L", "D' L' U L U' L' U L D"}
		};

		public string[][] positions = { // states of cube to be used in each stage if applicable
			new string[] {},
			new string[] {},
			new string[] {"000000000111111111222222222333333333444444444555555555", "000000111000111111222222222333333333445445445455455455", "111000111000111000222222222333333333545545545454454454", "011100011100011100322322322332332332545545545454454454", "010101010101010101323323323232232232545545545454454454", "010101010101010101232323323323232232454545545545454454", "010101010101010101232323232323232323454545454545454545"},
			new string[] {"435001433233514200110222551140435431323141400542552502", "513303404233514200323222551542435431140141400110552502", "513003404055510252323222432541433405340441112110552031", "025000404053512342321223434145034531340141512252153150"},
			new string[] {"503000404052513344321222435530234331240141512250455111", "503000004135415410221322513530234321244145332250455114", "003000004150414331221322315533231514444245135250455212", "003000000345213153222324131533231514444245244550155211"},
			new string[] {"000000000153514215222324224333231135444245114555154133", "000000000213115414222324121333233151444445335555154522", "000000000531111244222322215333233311444445421555554351", "000000000242115131222222415333233415444444351555551133"},
			new string[] {"000000000122314354222222511333333311444444512555555411", "000000000411315113222222142333333523444444412555555511", "000000000514311141222222252333333415444444311555555123"},
			new string[] {"000000000411111113222222142333333533444444452555555521"},
			new string[] {"000000000212111111222222425333333333444444441555555155", "000000000311111511222222124333333331444444244555555255"},
			new string[] {"000000000212111111222222425333333333444444441555555155", "500200302112111111030022225553333333044444444422555155"}
		};

		public Vector3[][] solvingPiece = { // pieces being solved (needs to be highlighted) in each stage if applicable
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {new Vector3(0, 0, 1), new Vector3(2, 2, 1), new Vector3(1, 2, 2), new Vector3(2, 1, 2)},
			new Vector3[] {new Vector3(0, 2, 0), new Vector3(2, 2, 2), new Vector3(2, 2, 0), new Vector3(2, 0, 2)},
			new Vector3[] {new Vector3(2, 2, 1), new Vector3(2, 2, 1), new Vector3(2, 1, 2), new Vector3(2, 2, 1)},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {new Vector3(0, 2, 0), new Vector3(2, 2, 0)}
		};

		public Vector3[][] targetPlace = { // pieces being solved's target places if applicable
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(2, 0, 1), new Vector3(1, 0, 2)},
			new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 0, 2), new Vector3(2, 0, 0), new Vector3(2, 0, 2)},
			new Vector3[] {new Vector3(0, 1, 2), new Vector3(2, 1, 0), new Vector3(0, 1, 0), new Vector3(2, 1, 2)},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {},
			new Vector3[] {new Vector3(0, 2, 0), new Vector3(2, 2, 0)}
		};

		public Tutorial(MainForm f) {
			form = f;
			stage = 0;
		}

		public bool previousStage() { // go to previous stage and return if should be called again
			if(stage > 0)
				stage--;
			return stage > 0;
		}

		public bool nextStage() { // go to next stage and return if should be called again
			if(stage < text.Length - 1)
				stage++;
			return stage < text.Length - 1;
		}

		public bool nextSequence() { // go to next sequence and return if should be called again
			if(sequence < moves[stage].Length - 1)
				sequence++;
			return sequence < moves[stage].Length - 1;
		}

		public bool prevSequence() { // go to previous sequence and return if should be called again
			if(sequence > 0)
				sequence--;
			return sequence > 0;
		}

		public void resetStage() { // reset the stage of the tutorial with updated strings
			_sequence = 0;

			form.tutorialTextbox.Text = "Stage " + (stage + 1) + " of " + text.Length + ": " + text[stage];
			form.tutorialTextbox.Select(0, 0);

			resetSequence();
		}

		public void resetSequence() { // reset the sequence of the tutorial with updated strings and cube positions and highlights
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

		public void updateButtons() { // update button strings and enables
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
