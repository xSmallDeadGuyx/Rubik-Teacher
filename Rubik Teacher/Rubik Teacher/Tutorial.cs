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

		public string[] text = {
			"The first thing I'd like you to understand is how the cube is to be treated. A lot of people look at a cube and just see squares of colours. If you notice, the squares of colour on a corner are actually connected to 2 others on that piece. The trick is to think about getting a piece into it's right place between faces, and not just get a coloured square on the right side.",
			"Now that you understand how the cube works, you should start by picking a face on the cube as the top face. I tend to pick orange. After this, I want you to try to get the edge piece in the right place on this face in order to create a cross shape. Make sure that the other colour on the piece is matched up with the right adjacent face."
		};

		public string[] moves = {
			"",
			"D 2F"
		};

		public string[] position = {
			"",
			"345303215533012442153322004125032013144045042012155154"
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

		public void resetStage() {
			form.tutorialTextbox.Text = ((int) stage + 1) + " of " + text.Length + ": " + text[(int) stage];
			form.tutorialTextbox.Select(0, 0);

			if(stage > 0) {
				form.playStageButton.Enabled = true;
				form.playStageButton.Text = "Play sequence: " + moves[(int)stage];
			}
			else {
				form.playStageButton.Enabled = false;
				form.playStageButton.Text = "No sequences for this stage";
			}

			if(stage > 0)
				form.rubikTeacher.fromString(position[(int) stage]);
		}
	}
}
