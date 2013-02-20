using System.Windows.Forms;
namespace Rubik_Teacher {
	public partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		public Tutorial tutorial;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>s
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.consolePanel = new System.Windows.Forms.Panel();
			this.speedLabel = new System.Windows.Forms.Label();
			this.rotateSpeedSlider = new System.Windows.Forms.TrackBar();
			this.animatedFacesButton = new System.Windows.Forms.Button();
			this.displayPanel = new System.Windows.Forms.Panel();
			this.pauseButton = new System.Windows.Forms.Button();
			this.showNetButton = new System.Windows.Forms.Button();
			this.shufflePanel = new System.Windows.Forms.Panel();
			this.movesLabel = new System.Windows.Forms.Label();
			this.shuffleButton = new System.Windows.Forms.Button();
			this.shuffleInput = new System.Windows.Forms.NumericUpDown();
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.cubeControlPage = new System.Windows.Forms.TabPage();
			this.tutorialPage = new System.Windows.Forms.TabPage();
			this.playStageButton = new System.Windows.Forms.Button();
			this.tutorialTextbox = new System.Windows.Forms.TextBox();
			this.nextStageButton = new System.Windows.Forms.Button();
			this.previousStageButton = new System.Windows.Forms.Button();
			this.debugTab = new System.Windows.Forms.TabPage();
			this.debugIn = new System.Windows.Forms.TextBox();
			this.debugOut = new System.Windows.Forms.TextBox();
			this.rubikTeacher = new Rubik_Teacher.RubikTeacher();
			this.consolePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.rotateSpeedSlider)).BeginInit();
			this.displayPanel.SuspendLayout();
			this.shufflePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.shuffleInput)).BeginInit();
			this.mainTabControl.SuspendLayout();
			this.cubeControlPage.SuspendLayout();
			this.tutorialPage.SuspendLayout();
			this.debugTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// consolePanel
			// 
			this.consolePanel.Controls.Add(this.speedLabel);
			this.consolePanel.Controls.Add(this.rotateSpeedSlider);
			this.consolePanel.Controls.Add(this.animatedFacesButton);
			this.consolePanel.Location = new System.Drawing.Point(531, 4);
			this.consolePanel.Margin = new System.Windows.Forms.Padding(4);
			this.consolePanel.Name = "consolePanel";
			this.consolePanel.Size = new System.Drawing.Size(258, 107);
			this.consolePanel.TabIndex = 2;
			// 
			// speedLabel
			// 
			this.speedLabel.AutoSize = true;
			this.speedLabel.Location = new System.Drawing.Point(8, 81);
			this.speedLabel.Name = "speedLabel";
			this.speedLabel.Size = new System.Drawing.Size(49, 17);
			this.speedLabel.TabIndex = 3;
			this.speedLabel.Text = "Speed";
			// 
			// rotateSpeedSlider
			// 
			this.rotateSpeedSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rotateSpeedSlider.AutoSize = false;
			this.rotateSpeedSlider.BackColor = System.Drawing.Color.White;
			this.rotateSpeedSlider.Location = new System.Drawing.Point(59, 81);
			this.rotateSpeedSlider.Margin = new System.Windows.Forms.Padding(4);
			this.rotateSpeedSlider.Maximum = 9;
			this.rotateSpeedSlider.Minimum = 1;
			this.rotateSpeedSlider.Name = "rotateSpeedSlider";
			this.rotateSpeedSlider.Size = new System.Drawing.Size(195, 22);
			this.rotateSpeedSlider.TabIndex = 2;
			this.rotateSpeedSlider.Text = "Speed";
			this.rotateSpeedSlider.TickStyle = System.Windows.Forms.TickStyle.None;
			this.rotateSpeedSlider.Value = 5;
			this.rotateSpeedSlider.ValueChanged += new System.EventHandler(this.rotateSpeedSlider_ValueChanged);
			// 
			// animatedFacesButton
			// 
			this.animatedFacesButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.animatedFacesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.animatedFacesButton.Location = new System.Drawing.Point(4, 4);
			this.animatedFacesButton.Margin = new System.Windows.Forms.Padding(4);
			this.animatedFacesButton.Name = "animatedFacesButton";
			this.animatedFacesButton.Size = new System.Drawing.Size(250, 73);
			this.animatedFacesButton.TabIndex = 1;
			this.animatedFacesButton.Text = "Hide Rotate Animations";
			this.animatedFacesButton.Click += new System.EventHandler(this.animatedFacesButton_Click);
			// 
			// displayPanel
			// 
			this.displayPanel.Controls.Add(this.pauseButton);
			this.displayPanel.Controls.Add(this.showNetButton);
			this.displayPanel.Location = new System.Drawing.Point(269, 4);
			this.displayPanel.Margin = new System.Windows.Forms.Padding(4);
			this.displayPanel.Name = "displayPanel";
			this.displayPanel.Size = new System.Drawing.Size(258, 107);
			this.displayPanel.TabIndex = 1;
			// 
			// pauseButton
			// 
			this.pauseButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pauseButton.Location = new System.Drawing.Point(4, 54);
			this.pauseButton.Margin = new System.Windows.Forms.Padding(4);
			this.pauseButton.Name = "pauseButton";
			this.pauseButton.Size = new System.Drawing.Size(250, 49);
			this.pauseButton.TabIndex = 2;
			this.pauseButton.Text = "Pause";
			this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
			// 
			// showNetButton
			// 
			this.showNetButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.showNetButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.showNetButton.Location = new System.Drawing.Point(4, 4);
			this.showNetButton.Margin = new System.Windows.Forms.Padding(4);
			this.showNetButton.Name = "showNetButton";
			this.showNetButton.Size = new System.Drawing.Size(250, 49);
			this.showNetButton.TabIndex = 0;
			this.showNetButton.Text = "Hide Net";
			this.showNetButton.UseVisualStyleBackColor = false;
			this.showNetButton.Click += new System.EventHandler(this.showNetButton_Click);
			// 
			// shufflePanel
			// 
			this.shufflePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.shufflePanel.Controls.Add(this.movesLabel);
			this.shufflePanel.Controls.Add(this.shuffleButton);
			this.shufflePanel.Controls.Add(this.shuffleInput);
			this.shufflePanel.Location = new System.Drawing.Point(8, 4);
			this.shufflePanel.Margin = new System.Windows.Forms.Padding(4);
			this.shufflePanel.Name = "shufflePanel";
			this.shufflePanel.Size = new System.Drawing.Size(258, 107);
			this.shufflePanel.TabIndex = 0;
			// 
			// movesLabel
			// 
			this.movesLabel.Location = new System.Drawing.Point(9, 81);
			this.movesLabel.Margin = new System.Windows.Forms.Padding(4);
			this.movesLabel.Name = "movesLabel";
			this.movesLabel.Size = new System.Drawing.Size(52, 20);
			this.movesLabel.TabIndex = 2;
			this.movesLabel.Text = "Moves";
			// 
			// shuffleButton
			// 
			this.shuffleButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.shuffleButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.shuffleButton.Location = new System.Drawing.Point(4, 4);
			this.shuffleButton.Margin = new System.Windows.Forms.Padding(4);
			this.shuffleButton.Name = "shuffleButton";
			this.shuffleButton.Size = new System.Drawing.Size(249, 73);
			this.shuffleButton.TabIndex = 1;
			this.shuffleButton.Text = "Shuffle";
			this.shuffleButton.Click += new System.EventHandler(this.shuffleButton_Click);
			// 
			// shuffleInput
			// 
			this.shuffleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.shuffleInput.Location = new System.Drawing.Point(61, 79);
			this.shuffleInput.Margin = new System.Windows.Forms.Padding(4);
			this.shuffleInput.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.shuffleInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.shuffleInput.Name = "shuffleInput";
			this.shuffleInput.Size = new System.Drawing.Size(192, 22);
			this.shuffleInput.TabIndex = 0;
			this.shuffleInput.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// mainTabControl
			// 
			this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mainTabControl.Controls.Add(this.cubeControlPage);
			this.mainTabControl.Controls.Add(this.tutorialPage);
			this.mainTabControl.Controls.Add(this.debugTab);
			this.mainTabControl.Location = new System.Drawing.Point(12, 12);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(805, 144);
			this.mainTabControl.TabIndex = 3;
			// 
			// cubeControlPage
			// 
			this.cubeControlPage.Controls.Add(this.shufflePanel);
			this.cubeControlPage.Controls.Add(this.displayPanel);
			this.cubeControlPage.Controls.Add(this.consolePanel);
			this.cubeControlPage.Location = new System.Drawing.Point(4, 25);
			this.cubeControlPage.Name = "cubeControlPage";
			this.cubeControlPage.Padding = new System.Windows.Forms.Padding(3);
			this.cubeControlPage.Size = new System.Drawing.Size(797, 115);
			this.cubeControlPage.TabIndex = 0;
			this.cubeControlPage.Text = "Cube Controls";
			this.cubeControlPage.UseVisualStyleBackColor = true;
			// 
			// tutorialPage
			// 
			this.tutorialPage.Controls.Add(this.playStageButton);
			this.tutorialPage.Controls.Add(this.tutorialTextbox);
			this.tutorialPage.Controls.Add(this.nextStageButton);
			this.tutorialPage.Controls.Add(this.previousStageButton);
			this.tutorialPage.Location = new System.Drawing.Point(4, 25);
			this.tutorialPage.Name = "tutorialPage";
			this.tutorialPage.Padding = new System.Windows.Forms.Padding(3);
			this.tutorialPage.Size = new System.Drawing.Size(797, 115);
			this.tutorialPage.TabIndex = 1;
			this.tutorialPage.Text = "Tutorial";
			this.tutorialPage.UseVisualStyleBackColor = true;
			this.tutorialPage.Enter += new System.EventHandler(this.tutorialPage_Enter);
			// 
			// playStageButton
			// 
			this.playStageButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.playStageButton.Enabled = false;
			this.playStageButton.Location = new System.Drawing.Point(87, 81);
			this.playStageButton.Name = "playStageButton";
			this.playStageButton.Size = new System.Drawing.Size(623, 28);
			this.playStageButton.TabIndex = 3;
			this.playStageButton.Text = "No sequences for this stage";
			this.playStageButton.UseVisualStyleBackColor = true;
			this.playStageButton.Click += new System.EventHandler(this.playStageButton_Click);
			// 
			// tutorialTextbox
			// 
			this.tutorialTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tutorialTextbox.Location = new System.Drawing.Point(87, 6);
			this.tutorialTextbox.Multiline = true;
			this.tutorialTextbox.Name = "tutorialTextbox";
			this.tutorialTextbox.ReadOnly = true;
			this.tutorialTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tutorialTextbox.Size = new System.Drawing.Size(623, 72);
			this.tutorialTextbox.TabIndex = 2;
			// 
			// nextStageButton
			// 
			this.nextStageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nextStageButton.Location = new System.Drawing.Point(716, 6);
			this.nextStageButton.Name = "nextStageButton";
			this.nextStageButton.Size = new System.Drawing.Size(75, 103);
			this.nextStageButton.TabIndex = 1;
			this.nextStageButton.Text = "Next";
			this.nextStageButton.UseVisualStyleBackColor = true;
			this.nextStageButton.Click += new System.EventHandler(this.nextStageButton_Click);
			// 
			// previousStageButton
			// 
			this.previousStageButton.Enabled = false;
			this.previousStageButton.Location = new System.Drawing.Point(6, 6);
			this.previousStageButton.Name = "previousStageButton";
			this.previousStageButton.Size = new System.Drawing.Size(75, 103);
			this.previousStageButton.TabIndex = 0;
			this.previousStageButton.Text = "Previous";
			this.previousStageButton.UseVisualStyleBackColor = true;
			this.previousStageButton.Click += new System.EventHandler(this.previousStageButton_Click);
			// 
			// debugTab
			// 
			this.debugTab.Controls.Add(this.debugIn);
			this.debugTab.Controls.Add(this.debugOut);
			this.debugTab.Location = new System.Drawing.Point(4, 25);
			this.debugTab.Name = "debugTab";
			this.debugTab.Padding = new System.Windows.Forms.Padding(3);
			this.debugTab.Size = new System.Drawing.Size(797, 115);
			this.debugTab.TabIndex = 2;
			this.debugTab.Text = "Debugging";
			this.debugTab.UseVisualStyleBackColor = true;
			// 
			// debugIn
			// 
			this.debugIn.Location = new System.Drawing.Point(6, 87);
			this.debugIn.Name = "debugIn";
			this.debugIn.Size = new System.Drawing.Size(785, 22);
			this.debugIn.TabIndex = 1;
			this.debugIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.debugIn_KeyPress);
			// 
			// debugOut
			// 
			this.debugOut.BackColor = System.Drawing.Color.Black;
			this.debugOut.ForeColor = System.Drawing.Color.White;
			this.debugOut.Location = new System.Drawing.Point(6, 6);
			this.debugOut.Multiline = true;
			this.debugOut.Name = "debugOut";
			this.debugOut.ReadOnly = true;
			this.debugOut.Size = new System.Drawing.Size(785, 78);
			this.debugOut.TabIndex = 0;
			// 
			// rubikTeacher
			// 
			this.rubikTeacher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rubikTeacher.Location = new System.Drawing.Point(12, 159);
			this.rubikTeacher.Margin = new System.Windows.Forms.Padding(0);
			this.rubikTeacher.Name = "rubikTeacher";
			this.rubikTeacher.Size = new System.Drawing.Size(805, 365);
			this.rubikTeacher.TabIndex = 0;
			this.rubikTeacher.Text = "rubikTeacher";
			this.rubikTeacher.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.rubikTeacher_MouseWheel);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(829, 533);
			this.Controls.Add(this.rubikTeacher);
			this.Controls.Add(this.mainTabControl);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.Text = "Rubik Teacher";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.consolePanel.ResumeLayout(false);
			this.consolePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.rotateSpeedSlider)).EndInit();
			this.displayPanel.ResumeLayout(false);
			this.shufflePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.shuffleInput)).EndInit();
			this.mainTabControl.ResumeLayout(false);
			this.cubeControlPage.ResumeLayout(false);
			this.tutorialPage.ResumeLayout(false);
			this.tutorialPage.PerformLayout();
			this.debugTab.ResumeLayout(false);
			this.debugTab.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public RubikTeacher rubikTeacher;
		private TrackBar rotateSpeedSlider;
		private Panel shufflePanel;
		private Label movesLabel;
		private Button shuffleButton;
		private NumericUpDown shuffleInput;
		private Panel displayPanel;
		private Button showNetButton;
		private Panel consolePanel;
		private Button animatedFacesButton;
		private Button pauseButton;
		private TabControl mainTabControl;
		private TabPage cubeControlPage;
		private TabPage tutorialPage;
		private Label speedLabel;
		private Button nextStageButton;
		private Button previousStageButton;
		public TextBox tutorialTextbox;
		public Button playStageButton;
		private TabPage debugTab;
		private TextBox debugIn;
		private TextBox debugOut;
	}
}