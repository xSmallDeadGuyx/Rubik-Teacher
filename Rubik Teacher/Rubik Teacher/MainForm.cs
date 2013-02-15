﻿using System;
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
	}
}
