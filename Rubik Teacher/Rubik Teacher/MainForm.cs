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
					debugOut.AppendText(rubikTeacher.cube.ToString());
				else if(debugIn.Text.ToLower().StartsWith("fromstr")) {
					rubikTeacher.cube.fromString(debugIn.Text.Split(' ')[1]);
					rubikTeacher.refresh();
				}
				else {
					string[] moves = debugIn.Text.ToUpper().Split(' ');
					foreach(string move in moves)
						rubikTeacher.performMove(move);
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
