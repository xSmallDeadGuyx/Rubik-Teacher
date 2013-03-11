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

		private void showNetButton_Click(object sender, EventArgs e) { // invert the setting of showing 2d cube net and update button text
			rubikTeacher.showNet = !rubikTeacher.showNet;
			showNetButton.Text = rubikTeacher.showNet ? "Hide Net" : "Show Net";
		}

		private void animatedFacesButton_Click(object sender, EventArgs e) { // invert the setting of face animating and update button text
			rubikTeacher.animateFaces = !rubikTeacher.animateFaces;
			animatedFacesButton.Text = rubikTeacher.animateFaces ? "Hide Rotate Animation" : "Show Rotate Animation";
		}

		private void pauseButton_Click(object sender, EventArgs e) { // invert the setting of pause and update button text
			rubikTeacher.paused = !rubikTeacher.paused;
			pauseButton.Text = rubikTeacher.paused ? "Resume" : "Pause";
		}

		private void rotateSpeedSlider_ValueChanged(object sender, EventArgs e) { // set the rotation per step of the rubik's cube proportional to the slider value
			rubikTeacher.rotatePerStep = MathHelper.PiOver4 * rotateSpeedSlider.Value / 500.0F;
		}

		private void MainForm_Load(object sender, EventArgs e) {
			rubikTeacher.form = this; // allow rubikTeacher to access this class
			this.tutorial = new Tutorial(this); // start the tutorial loading class
			rubikTeacher.bgColor = new Microsoft.Xna.Framework.Color(BackColor.R, BackColor.G, BackColor.B); // update the background colour of the rubik's cube control
		}

		private void previousStageButton_Click(object sender, EventArgs e) { // move to previous stage and update button enables
			previousStageButton.Enabled = tutorial.previousStage();
			nextStageButton.Enabled = true;
		}

		private void nextStageButton_Click(object sender, EventArgs e) { // move to next stage and update button enables
			nextStageButton.Enabled = tutorial.nextStage();
			previousStageButton.Enabled = true;
		}

		private void playStageButton_Click(object sender, EventArgs e) {
			tutorial.resetSequence(); // reset cube to position for sequence (in most cases already there)

			string[] moves = tutorial.moves[(int) tutorial.stage][tutorial.sequence].Split(' '); // select all moves for the current sequence of the tutorial and perform them in turn
			foreach(string move in moves)
				rubikTeacher.performMove(move);

			playStageButton.Enabled = nextSequenceButton.Enabled = prevSequenceButton.Enabled = false; // disable buttons for the duration of the sequence
		}

		private void debugIn_KeyPress(object sender, KeyPressEventArgs e) {
			if(e.KeyChar == (char)13) { // if pressed key is the enter key
				e.Handled = true;

				if(debugIn.Text.ToLower() == "tostr") // if command is "tostr" then output state of the cube as a string of numbers
					debugOut.AppendText("Current cube state: " + rubikTeacher.cubeToString() + "\n");
				else if(debugIn.Text.ToLower().StartsWith("fromstr")) {  // if command is "fromstr"
					string[] input = debugIn.Text.Split(' '); // split command arguments into an array
					if(input.Length == 2 && input[1].Length == 54) { // check if arguments are valid
						bool otherColours = false;
						for(int i = 0; i < 54; i++)
							if(int.Parse(input[1][i].ToString()) < 0 || int.Parse(input[1][i].ToString()) > 5) // check if any colours are invalid
								otherColours = true;
						if(otherColours) // cube is not valid so error and do nothing
							debugOut.AppendText("Invalid cube string\n");
						else { // cube is valid so update cube
							debugOut.AppendText("Cube updated\n");
							rubikTeacher.fromString(input[1]);
						}
					}
					else
						debugOut.AppendText("Invalid cube string\n"); // error on invalid arguments
				}
				else if(debugIn.Text.ToLower().StartsWith("moves")) { // if command is "moves"
					string[] moves = debugIn.Text.ToUpper().Split(' '); // split arguments into array
					if(moves.Length < 2) // not enough arguments so output error
						debugOut.AppendText("No moves inputted\n");
					else { // otherwise perform all moves
						bool allValid = true;
						for(int i = 1; i < moves.Length; i++) { // iterate the moves and check if all are valid, error if not
							if(!rubikTeacher.isValidMove(moves[i])) {
								debugOut.AppendText("Invalid move: " + moves[i] + "\n");
								allValid = false;
								break;
							}
						}
						if(allValid) { // if all moves are valid then iterate and perform
							debugOut.AppendText("Performed moves:");
							for(int i = 1; i < moves.Length; i++) {
								rubikTeacher.performMove(moves[i]);
								debugOut.AppendText(" " + moves[i]);
							}
							debugOut.AppendText("\n");
						}
					}
				}
				else if(debugIn.Text.ToLower().StartsWith("highlight")) { // if command is "highlight"
					string[] args = debugIn.Text.Split(' '); // split arguments into array
					if(args.Length != 4) // check that the number of arguments is valid and error if not
						debugOut.AppendText("You must provide an x, y, and z value\r\n");
					else { // otherwise update the highlight on the cube
						int x, y, z;
						if(!int.TryParse(args[1], out x))
							debugOut.AppendText("Invalid x-value: " + args[1] + "\r\n");
						else if(!int.TryParse(args[2], out y))
							debugOut.AppendText("Invalid y-value: " + args[2] + "\r\n");
						else if(!int.TryParse(args[3], out z))
							debugOut.AppendText("Invalid z-value: " + args[3] + "\r\n");
						else {
							rubikTeacher.pieceHighlighted[x, y, z] = !rubikTeacher.pieceHighlighted[x, y, z];
							debugOut.AppendText((rubikTeacher.pieceHighlighted[x, y, z] ? "Highlighting " : "No longer highlighting ") + "piece at (" + x + ", " + y + ", " + z + ")\n");
						}
					}
				}
				else if(debugIn.Text.ToLower() == "help") { // if command is help, print the help string
					debugOut.AppendText("Commands:\r\n\ttostr\t\t- Print cube state to console as a string\r\n\tfromstr <string>\t- Update cube state to match that encoded in <string>\r\n\tmoves <moves>\t- Perform sequence of moves in normal cube notation\r\n\thighlight <x> <y> <z>\t- Toggle highlight of piece at (x, y, z)\r\n");
				}
				else { // otherwise command is not known and error
					debugOut.AppendText("Unknown command or invalid arguments: " + debugIn.Text + "\r\n");
				}
				debugIn.Text = ""; // reset the input
			}
		}

		private void rubikTeacher_MouseWheel(object sender, MouseEventArgs e) { // on mouse wheel scroll, update the rubik's cube zoom
			rubikTeacher.zoom += e.Delta / 120;
			if(rubikTeacher.zoom > 0)
				rubikTeacher.zoom = 0;
			if(rubikTeacher.zoom < -10)
				rubikTeacher.zoom = -10;
		}

		private void nextSequenceButton_Click(object sender, EventArgs e) { // move to next sequence and update buttons
			nextSequenceButton.Enabled = tutorial.nextSequence();
			prevSequenceButton.Enabled = true;
		}

		private void prevSequenceButton_Click(object sender, EventArgs e) { // move to previous sequence and update buttons
			prevSequenceButton.Enabled = tutorial.prevSequence();
			nextSequenceButton.Enabled = true;
		}
	}
}
