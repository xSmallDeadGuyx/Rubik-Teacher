using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace Rubik_Teacher {
	public partial class MainForm : Form {

		public MainForm() {
			InitializeComponent();
		}

		private void shuffleButton_Click(object sender, EventArgs e) {
			Random rand = new Random();

			List<FaceID> facesChanged = new List<FaceID>();
			Move lastMove = new Move(FaceID.Top, CubeMove.Clockwise);
			for(int i = 0; i < shuffleInput.Value; i++) {
				FaceID changed = (FaceID) rand.Next(6);
				while(changed == lastMove.face) changed = (FaceID) rand.Next(6);
				CubeMove cmove = (CubeMove) rand.Next(3);
				lastMove.face = changed;
				lastMove.twist = cmove;
				rubikTeacher.performMove(changed, cmove);
			}
		}

		private void showNetButton_Click(object sender, EventArgs e) {
			rubikTeacher.showNet = !rubikTeacher.showNet;
			showNetButton.Text = rubikTeacher.showNet ? "Hide Net" : "Show Net";
		}

		private void animatedFacesButton_Click(object sender, EventArgs e) {
			rubikTeacher.animateFaces = !rubikTeacher.animateFaces;
			animatedFacesButton.Text = rubikTeacher.animateFaces ? "Hide Rotate Animation" : "Show Rotate Animation";
		}

		private void pauseButton_Click(object sender, EventArgs e) {
			rubikTeacher.paused = !rubikTeacher.paused;
			pauseButton.Text = rubikTeacher.paused ? "Resume" : "Pause";
		}

		private void rotateSpeedSlider_ValueChanged(object sender, EventArgs e) {
			rubikTeacher.rotatePerStep = MathHelper.PiOver4 * rotateSpeedSlider.Value / 500.0F;
		}

		private void MainForm_Load(object sender, EventArgs e) {
			this.tutorial = new Tutorial(this);
			rubikTeacher.bgColor = new Microsoft.Xna.Framework.Color(BackColor.R, BackColor.G, BackColor.B);
		}

		private void previousStageButton_Click(object sender, EventArgs e) {
			previousStageButton.Enabled = tutorial.previousStage();
			nextStageButton.Enabled = true;
		}

		private void nextStageButton_Click(object sender, EventArgs e) {
			nextStageButton.Enabled = tutorial.nextStage();
			previousStageButton.Enabled = true;
		}

		private void playStageButton_Click(object sender, EventArgs e) {
			string[] moves = tutorial.moves[(int) tutorial.stage].Split(' ');
			foreach(string move in moves)
				rubikTeacher.performMove(move);
			tutorial.resetStage();
		}

		private void debugIn_KeyPress(object sender, KeyPressEventArgs e) {
			if(e.KeyChar == (char)13) {
				if(debugIn.Text.ToLower() == "tostr")
					debugOut.AppendText("Current cube state: " + rubikTeacher.cubeToString() + "\n");
				else if(debugIn.Text.ToLower().StartsWith("fromstr")) {
					string[] input = debugIn.Text.Split(' ');
					if(input.Length == 2 && input[1].Length == 54) {
						bool otherColours = false;
						for(int i = 0; i < 54; i++)
							if(int.Parse(input[1][i].ToString()) < 0 || int.Parse(input[1][i].ToString()) > 5)
								otherColours = true;
						if(otherColours)
							debugOut.AppendText("Invalid cube string\n");
						else {
							debugOut.AppendText("Cube updated\n");
							rubikTeacher.fromString(input[1]);
						}
					}
					else
						debugOut.AppendText("Invalid cube string\n");
				}
				else if(debugIn.Text.ToLower().StartsWith("moves")) {
					string[] moves = debugIn.Text.ToUpper().Split(' ');
					if(moves.Length < 2)
						debugOut.AppendText("No moves inputted\n");
					else {
						bool allValid = true;
						for(int i = 1; i < moves.Length; i++) {
							if(!rubikTeacher.isValidMove(moves[i])) {
								debugOut.AppendText("Invalid move: " + moves[i] + "\n");
								allValid = false;
								break;
							}
						}
						if(allValid) {
							debugOut.AppendText("Performed moves:");
							for(int i = 1; i < moves.Length; i++) {
								rubikTeacher.performMove(moves[i]);
								debugOut.AppendText(" " + moves[i]);
							}
							debugOut.AppendText("\n");
						}
					}
				}
				else if(debugIn.Text.ToLower().StartsWith("highlight")) {
					string[] args = debugIn.Text.Split(' ');
					if(args.Length != 4)
						debugOut.AppendText("You must provide an x, y, and z value\n");
					else {
						int x, y, z;
						if(!int.TryParse(args[1], out x))
							debugOut.AppendText("Invalid x-value: " + args[1] + "\n");
						else if(!int.TryParse(args[2], out y))
							debugOut.AppendText("Invalid y-value: " + args[2] + "\n");
						else if(!int.TryParse(args[3], out z))
							debugOut.AppendText("Invalid z-value: " + args[3] + "\n");
						else {
							rubikTeacher.pieceHighlighted[x, y, z] = !rubikTeacher.pieceHighlighted[x, y, z];
							debugOut.AppendText((rubikTeacher.pieceHighlighted[x, y, z] ? "Highlighting " : "No longer highlighting ") + "piece at (" + x + ", " + y + ", " + z + ")\n");
						}
					}
				}
				else if(debugIn.Text.ToLower() == "help") {
					debugOut.AppendText("Commands:\r\n\ttostr\t\t- Print cube state to console as a string\r\n\tfromstr <string>\t- Update cube state to match that encoded in <string>\r\n\tmoves <moves>\t- Perform sequence of moves in normal cube notation\r\n\thighlight <x> <y> <z>\t- Toggle highlight of piece at (x, y, z)\n");
				}
				else {
					debugOut.AppendText("Unknown command or invalid arguments: " + debugIn.Text + "\n");
				}
				debugIn.Text = "";
			}
		}

		private void tutorialPage_Enter(object sender, EventArgs e) {
			tutorial.resetStage();
		}

		private void rubikTeacher_MouseWheel(object sender, MouseEventArgs e) {
			rubikTeacher.zoom += e.Delta / 120;
			if(rubikTeacher.zoom > 0)
				rubikTeacher.zoom = 0;
			if(rubikTeacher.zoom < -10)
				rubikTeacher.zoom = -10;
		}
	}
}
