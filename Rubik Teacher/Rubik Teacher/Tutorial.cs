using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rubik_Teacher {
	public class Tutorial {
		public MainForm form;

		public enum Stage { theory, topCross, topCorners, middleLayer, bottomCrossAlign, bottomCrossRotate, bottomCornersAlign, bottomCornersRotate, end };
		public Stage stage {
			get { 
				return _stage;
			}
			set { 
				_stage = value;
				resetStage();
			}
		}
		private Stage _stage;

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
			"Now that you have developed the basic understanding of how the cube works, I want to teach you some common notation for sequences to be performed on the cube. Each move you perform on a cube can be denoted by one or two characters, where one character identifies the face of the cube and the other is the type of rotation. F B L R U D are the face characters, which stand for Front (green), Back (blue), Left (yellow), Right (white), Up (orange), and down (red). The colour in brackets is just what I will be using in this tutorial. The move F is the rotation of the front face clockwise, for an anticlockwise rotation you would use F', and for 180 degrees you would use 2F. Try out some moves below with the sequence picker"
		};

		public string[][] moves = {
			new string[] {},
			new string[] {},
			new string[] {"F F' 2F", "B B' 2B", "L L' 2L", "R R' 2R", "U U' 2U", "D D' 2D"}
		};

		public string[][] positions = {
			new string[] {},
			new string[] {},
			new string[] {"000000000111111111222222222333333333444444444555555555"}
		};

		public Tutorial(MainForm f) {
			form = f;
			stage = Stage.theory;
		}

		public bool previousStage() {
				stage = (Stage) ((int) stage - 1);
			return (int) stage > 0;
		}

		public bool nextStage() {
			if((int) stage < text.Length - 1)
				stage = (Stage) ((int) stage + 1);
			return (int) stage < text.Length - 1;
		}

		public bool nextSequence() {
			if(sequence < moves[(int) stage].Length - 1)
				_sequence++;
			return sequence < moves[(int) stage].Length - 1;
		}

		public bool prevSequence() {
			if(sequence > 0)
				sequence--;
			return sequence > 0;
		}

		public void resetStage() {
			_sequence = 0;

			form.tutorialTextbox.Text = "Stage " + ((int) stage + 1) + " of " + text.Length + ": " + text[(int) stage];
			form.tutorialTextbox.Select(0, 0);
			form.prevSequenceButton.Enabled = false;

			if(moves[(int) stage].Length > 0) {
				form.playStageButton.Enabled = true;
				form.playStageButton.Text = "Play sequence: " + moves[(int) stage][0];

				form.nextSequenceButton.Enabled = moves[(int) stage].Length > 1;
			}
			else {
				form.playStageButton.Enabled = false;
				form.playStageButton.Text = "No sequences for this stage";

				form.nextSequenceButton.Enabled = false;
			}

			if(positions[(int) stage].Length > 0)
				form.rubikTeacher.fromString(positions[(int) stage][0]);
		}

		public void resetSequence() {
			if(moves[(int) stage].Length > 0) {
				if(sequence < moves[(int) stage].Length) {
					form.playStageButton.Enabled = true;
					form.playStageButton.Text = "Play sequence: " + moves[(int) stage][sequence];

					form.nextSequenceButton.Enabled = true;
				}
			}
			else {
				form.playStageButton.Enabled = false;
				form.playStageButton.Text = "No sequences for this stage";

				form.nextSequenceButton.Enabled = false;
			}
			if(sequence < positions[(int) stage].Length)
				form.rubikTeacher.fromString(positions[(int) stage][sequence]);
		}
	}
}
