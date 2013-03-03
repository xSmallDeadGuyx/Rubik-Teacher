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
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.optionsPage = new System.Windows.Forms.TabPage();
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
			((System.ComponentModel.ISupportInitialize) (this.rotateSpeedSlider)).BeginInit();
			this.displayPanel.SuspendLayout();
			this.mainTabControl.SuspendLayout();
			this.optionsPage.SuspendLayout();
			this.tutorialPage.SuspendLayout();
			this.debugTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// consolePanel
			// 
			this.consolePanel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.consolePanel.AutoSize = true;
			this.consolePanel.Controls.Add(this.speedLabel);
			this.consolePanel.Controls.Add(this.rotateSpeedSlider);
			this.consolePanel.Controls.Add(this.animatedFacesButton);
			this.consolePanel.Location = new System.Drawing.Point(401, 4);
			this.consolePanel.Margin = new System.Windows.Forms.Padding(4);
			this.consolePanel.Name = "consolePanel";
			this.consolePanel.Size = new System.Drawing.Size(392, 107);
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
			this.rotateSpeedSlider.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rotateSpeedSlider.AutoSize = false;
			this.rotateSpeedSlider.BackColor = System.Drawing.Color.White;
			this.rotateSpeedSlider.Location = new System.Drawing.Point(59, 81);
			this.rotateSpeedSlider.Margin = new System.Windows.Forms.Padding(4);
			this.rotateSpeedSlider.Maximum = 9;
			this.rotateSpeedSlider.Minimum = 1;
			this.rotateSpeedSlider.Name = "rotateSpeedSlider";
			this.rotateSpeedSlider.Size = new System.Drawing.Size(329, 22);
			this.rotateSpeedSlider.TabIndex = 2;
			this.rotateSpeedSlider.Text = "Speed";
			this.rotateSpeedSlider.TickStyle = System.Windows.Forms.TickStyle.None;
			this.rotateSpeedSlider.Value = 5;
			this.rotateSpeedSlider.ValueChanged += new System.EventHandler(this.rotateSpeedSlider_ValueChanged);
			// 
			// animatedFacesButton
			// 
			this.animatedFacesButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.animatedFacesButton.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.animatedFacesButton.Location = new System.Drawing.Point(4, 4);
			this.animatedFacesButton.Margin = new System.Windows.Forms.Padding(4);
			this.animatedFacesButton.Name = "animatedFacesButton";
			this.animatedFacesButton.Size = new System.Drawing.Size(384, 73);
			this.animatedFacesButton.TabIndex = 1;
			this.animatedFacesButton.Text = "Hide Rotate Animations";
			this.animatedFacesButton.Click += new System.EventHandler(this.animatedFacesButton_Click);
			// 
			// displayPanel
			// 
			this.displayPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.displayPanel.AutoSize = true;
			this.displayPanel.Controls.Add(this.pauseButton);
			this.displayPanel.Controls.Add(this.showNetButton);
			this.displayPanel.Location = new System.Drawing.Point(4, 4);
			this.displayPanel.Margin = new System.Windows.Forms.Padding(4);
			this.displayPanel.Name = "displayPanel";
			this.displayPanel.Size = new System.Drawing.Size(392, 107);
			this.displayPanel.TabIndex = 1;
			// 
			// pauseButton
			// 
			this.pauseButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pauseButton.Location = new System.Drawing.Point(4, 54);
			this.pauseButton.Margin = new System.Windows.Forms.Padding(4);
			this.pauseButton.Name = "pauseButton";
			this.pauseButton.Size = new System.Drawing.Size(384, 49);
			this.pauseButton.TabIndex = 2;
			this.pauseButton.Text = "Pause";
			this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
			// 
			// showNetButton
			// 
			this.showNetButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.showNetButton.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.showNetButton.Location = new System.Drawing.Point(4, 4);
			this.showNetButton.Margin = new System.Windows.Forms.Padding(4);
			this.showNetButton.Name = "showNetButton";
			this.showNetButton.Size = new System.Drawing.Size(384, 49);
			this.showNetButton.TabIndex = 0;
			this.showNetButton.Text = "Hide Net";
			this.showNetButton.UseVisualStyleBackColor = false;
			this.showNetButton.Click += new System.EventHandler(this.showNetButton_Click);
			// 
			// mainTabControl
			// 
			this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mainTabControl.Controls.Add(this.tutorialPage);
			this.mainTabControl.Controls.Add(this.debugTab);
			this.mainTabControl.Controls.Add(this.optionsPage);
			this.mainTabControl.Location = new System.Drawing.Point(12, 12);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(805, 144);
			this.mainTabControl.TabIndex = 3;
			// 
			// optionsPage
			// 
			this.optionsPage.Controls.Add(this.displayPanel);
			this.optionsPage.Controls.Add(this.consolePanel);
			this.optionsPage.Location = new System.Drawing.Point(4, 25);
			this.optionsPage.Name = "optionsPage";
			this.optionsPage.Padding = new System.Windows.Forms.Padding(3);
			this.optionsPage.Size = new System.Drawing.Size(797, 115);
			this.optionsPage.TabIndex = 0;
			this.optionsPage.Text = "Options";
			this.optionsPage.UseVisualStyleBackColor = true;
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
			this.playStageButton.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.tutorialTextbox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.nextStageButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
			this.debugIn.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.debugIn.Location = new System.Drawing.Point(6, 87);
			this.debugIn.Name = "debugIn";
			this.debugIn.Size = new System.Drawing.Size(785, 22);
			this.debugIn.TabIndex = 1;
			this.debugIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.debugIn_KeyPress);
			// 
			// debugOut
			// 
			this.debugOut.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.debugOut.BackColor = System.Drawing.Color.Black;
			this.debugOut.ForeColor = System.Drawing.Color.White;
			this.debugOut.Location = new System.Drawing.Point(6, 6);
			this.debugOut.Multiline = true;
			this.debugOut.Name = "debugOut";
			this.debugOut.ReadOnly = true;
			this.debugOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.debugOut.Size = new System.Drawing.Size(785, 78);
			this.debugOut.TabIndex = 0;
			this.debugOut.Text = "Type \"help\" for a list of commands\r\n";
			// 
			// rubikTeacher
			// 
			this.rubikTeacher.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
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
			((System.ComponentModel.ISupportInitialize) (this.rotateSpeedSlider)).EndInit();
			this.displayPanel.ResumeLayout(false);
			this.mainTabControl.ResumeLayout(false);
			this.optionsPage.ResumeLayout(false);
			this.optionsPage.PerformLayout();
			this.tutorialPage.ResumeLayout(false);
			this.tutorialPage.PerformLayout();
			this.debugTab.ResumeLayout(false);
			this.debugTab.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public RubikTeacher rubikTeacher;
		private TrackBar rotateSpeedSlider;
		private Panel displayPanel;
		private Button showNetButton;
		private Panel consolePanel;
		private Button animatedFacesButton;
		private Button pauseButton;
		private TabControl mainTabControl;
		private TabPage optionsPage;
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