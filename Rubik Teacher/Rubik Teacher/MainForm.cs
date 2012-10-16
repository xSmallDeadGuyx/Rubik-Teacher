using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Text.RegularExpressions;

namespace Rubik_Teacher {
	public partial class MainForm : Office2007RibbonForm {

		public MainForm() {
			InitializeComponent();
		}

		private void shuffleButton_Click(object sender, EventArgs e) {
			Random rand = new Random();
			

			List<FaceID> facesChanged = new List<FaceID>();
			for(int i = 0; i < shuffleInput.Value; i++) {
				FaceID changed = (FaceID) rand.Next(6);
				if(!facesChanged.Contains(changed)) facesChanged.Add(changed);
				rubikTeacher.cube.performMove(changed, (CubeMove) rand.Next(3));
			}
			foreach(FaceID face in facesChanged) {
				rubikTeacher.verticesChanged[(int) face] = true;
				for(int i = 0; i < 6; i++)
					if(rubikTeacher.areAdjacent(face, (FaceID) i))
						rubikTeacher.verticesChanged[i] = true;
			}
		}

		private void showNetButton_Click(object sender, EventArgs e) {
			showNetButton.Checked = !showNetButton.Checked;
		}

		private void consoleInput_KeyPress(object sender, KeyPressEventArgs e) {
			if(e.KeyChar == '\r' && consoleInput.Text != string.Empty) {
				if(!Regex.IsMatch(consoleInput.Text.ToUpper(), "^\\s*([" + rubikTeacher.cube.validStringCharacters + "]{1,2}\\s+)*[" + rubikTeacher.cube.validStringCharacters + "]{1,2}\\s*$")) {
					console.AppendText("Cannot understand move list: " + consoleInput.Text + "\n");
					return;
				}
				String parsed = Regex.Replace(consoleInput.Text.ToUpper().Trim(), "\\s+", " ");
				string[] moves = parsed.Split(' ');
				List<string> performed = new List<string>();
				List<string> invalid = new List<string>();
				foreach(string m in moves)
					if(!rubikTeacher.cube.isValidMove(m)) {
						console.AppendText("Invalid move: " + m + "\n");
						return;
					}
				List<FaceID> facesChanged = new List<FaceID>();
				foreach(string m in moves) {
					FaceID changed = rubikTeacher.cube.toMove(m).face;
					if(!facesChanged.Contains(changed)) facesChanged.Add(changed);
					rubikTeacher.cube.performMove(m);
				}
				foreach(FaceID face in facesChanged) {
					rubikTeacher.verticesChanged[(int) face] = true;
					for(int i = 0; i < 6; i++)
						if(rubikTeacher.areAdjacent(face, (FaceID) i))
							rubikTeacher.verticesChanged[i] = true;
				}

				console.AppendText("Performed moves: " + parsed + "\n");
				consoleInput.Text = "";
			}
		}

		private void undoButton_Click(object sender, EventArgs e) {

		}
	}
}
