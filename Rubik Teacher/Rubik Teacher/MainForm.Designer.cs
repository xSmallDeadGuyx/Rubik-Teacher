namespace Rubik_Teacher {
	public partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.components = new System.ComponentModel.Container();
			this.ribbonControl = new DevComponents.DotNetBar.RibbonControl();
			this.cubeControlPanel = new DevComponents.DotNetBar.RibbonPanel();
			this.consolePanel = new DevComponents.DotNetBar.PanelEx();
			this.undoButton = new DevComponents.DotNetBar.ButtonX();
			this.console = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.consoleInput = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.displayPanel = new DevComponents.DotNetBar.PanelEx();
			this.showNetButton = new DevComponents.DotNetBar.ButtonX();
			this.shufflePanel = new DevComponents.DotNetBar.PanelEx();
			this.movesLabel = new DevComponents.DotNetBar.LabelX();
			this.shuffleButton = new DevComponents.DotNetBar.ButtonX();
			this.shuffleInput = new DevComponents.Editors.IntegerInput();
			this.ribbonPanel1 = new DevComponents.DotNetBar.RibbonPanel();
			this.solverPanel = new DevComponents.DotNetBar.RibbonPanel();
			this.cubeTab = new DevComponents.DotNetBar.RibbonTabItem();
			this.Solver = new DevComponents.DotNetBar.RibbonTabItem();
			this.Style = new DevComponents.DotNetBar.RibbonTabItem();
			this.styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
			this.renderPanel = new DevComponents.DotNetBar.PanelEx();
			this.rubikTeacher = new Rubik_Teacher.RubikTeacher();
			this.ribbonControl.SuspendLayout();
			this.cubeControlPanel.SuspendLayout();
			this.consolePanel.SuspendLayout();
			this.displayPanel.SuspendLayout();
			this.shufflePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.shuffleInput)).BeginInit();
			this.renderPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ribbonControl
			// 
			// 
			// 
			// 
			this.ribbonControl.BackgroundStyle.Class = "";
			this.ribbonControl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.ribbonControl.CanCustomize = false;
			this.ribbonControl.CaptionVisible = true;
			this.ribbonControl.Controls.Add(this.cubeControlPanel);
			this.ribbonControl.Controls.Add(this.ribbonPanel1);
			this.ribbonControl.Controls.Add(this.solverPanel);
			this.ribbonControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.ribbonControl.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.cubeTab,
            this.Solver,
            this.Style});
			this.ribbonControl.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
			this.ribbonControl.Location = new System.Drawing.Point(5, 1);
			this.ribbonControl.Margin = new System.Windows.Forms.Padding(0);
			this.ribbonControl.Name = "ribbonControl";
			this.ribbonControl.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.ribbonControl.Size = new System.Drawing.Size(630, 154);
			this.ribbonControl.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.ribbonControl.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
			this.ribbonControl.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
			this.ribbonControl.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
			this.ribbonControl.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
			this.ribbonControl.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
			this.ribbonControl.SystemText.QatDialogAddButton = "&Add >>";
			this.ribbonControl.SystemText.QatDialogCancelButton = "Cancel";
			this.ribbonControl.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
			this.ribbonControl.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
			this.ribbonControl.SystemText.QatDialogOkButton = "OK";
			this.ribbonControl.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
			this.ribbonControl.SystemText.QatDialogRemoveButton = "&Remove";
			this.ribbonControl.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
			this.ribbonControl.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
			this.ribbonControl.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
			this.ribbonControl.TabGroupHeight = 14;
			this.ribbonControl.TabIndex = 1;
			this.ribbonControl.Text = "Rubik Teacher";
			// 
			// cubeControlPanel
			// 
			this.cubeControlPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.cubeControlPanel.Controls.Add(this.consolePanel);
			this.cubeControlPanel.Controls.Add(this.displayPanel);
			this.cubeControlPanel.Controls.Add(this.shufflePanel);
			this.cubeControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cubeControlPanel.Location = new System.Drawing.Point(0, 56);
			this.cubeControlPanel.Name = "cubeControlPanel";
			this.cubeControlPanel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.cubeControlPanel.Size = new System.Drawing.Size(630, 96);
			// 
			// 
			// 
			this.cubeControlPanel.Style.Class = "";
			this.cubeControlPanel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.cubeControlPanel.StyleMouseDown.Class = "";
			this.cubeControlPanel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.cubeControlPanel.StyleMouseOver.Class = "";
			this.cubeControlPanel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.cubeControlPanel.TabIndex = 1;
			// 
			// consolePanel
			// 
			this.consolePanel.CanvasColor = System.Drawing.SystemColors.Control;
			this.consolePanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.consolePanel.Controls.Add(this.undoButton);
			this.consolePanel.Controls.Add(this.console);
			this.consolePanel.Controls.Add(this.consoleInput);
			this.consolePanel.Location = new System.Drawing.Point(312, 3);
			this.consolePanel.Name = "consolePanel";
			this.consolePanel.Size = new System.Drawing.Size(312, 87);
			this.consolePanel.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.consolePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.consolePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.consolePanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.consolePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.consolePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.consolePanel.Style.GradientAngle = 90;
			this.consolePanel.TabIndex = 2;
			// 
			// undoButton
			// 
			this.undoButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.undoButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.undoButton.Location = new System.Drawing.Point(231, 66);
			this.undoButton.Name = "undoButton";
			this.undoButton.Size = new System.Drawing.Size(78, 18);
			this.undoButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.undoButton.TabIndex = 2;
			this.undoButton.Text = "Undo";
			this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
			// 
			// console
			// 
			this.console.BackColor = System.Drawing.Color.Black;
			// 
			// 
			// 
			this.console.Border.Class = "TextBoxBorder";
			this.console.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.console.ForeColor = System.Drawing.Color.White;
			this.console.Location = new System.Drawing.Point(3, 3);
			this.console.Multiline = true;
			this.console.Name = "console";
			this.console.ReadOnly = true;
			this.console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.console.Size = new System.Drawing.Size(306, 59);
			this.console.TabIndex = 1;
			// 
			// consoleInput
			// 
			// 
			// 
			// 
			this.consoleInput.Border.Class = "TextBoxBorder";
			this.consoleInput.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.consoleInput.Location = new System.Drawing.Point(3, 64);
			this.consoleInput.Name = "consoleInput";
			this.consoleInput.Size = new System.Drawing.Size(226, 20);
			this.consoleInput.TabIndex = 0;
			this.consoleInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.consoleInput_KeyPress);
			// 
			// displayPanel
			// 
			this.displayPanel.CanvasColor = System.Drawing.SystemColors.Control;
			this.displayPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.displayPanel.Controls.Add(this.showNetButton);
			this.displayPanel.Location = new System.Drawing.Point(167, 3);
			this.displayPanel.Name = "displayPanel";
			this.displayPanel.Size = new System.Drawing.Size(142, 87);
			this.displayPanel.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.displayPanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.displayPanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.displayPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.displayPanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.displayPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.displayPanel.Style.GradientAngle = 90;
			this.displayPanel.TabIndex = 1;
			// 
			// showNetButton
			// 
			this.showNetButton.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
			this.showNetButton.Checked = true;
			this.showNetButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.showNetButton.Location = new System.Drawing.Point(3, 3);
			this.showNetButton.Name = "showNetButton";
			this.showNetButton.Size = new System.Drawing.Size(136, 40);
			this.showNetButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.showNetButton.TabIndex = 0;
			this.showNetButton.Text = "Show Net";
			this.showNetButton.Click += new System.EventHandler(this.showNetButton_Click);
			// 
			// shufflePanel
			// 
			this.shufflePanel.CanvasColor = System.Drawing.SystemColors.Control;
			this.shufflePanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.shufflePanel.Controls.Add(this.movesLabel);
			this.shufflePanel.Controls.Add(this.shuffleButton);
			this.shufflePanel.Controls.Add(this.shuffleInput);
			this.shufflePanel.Location = new System.Drawing.Point(6, 3);
			this.shufflePanel.Name = "shufflePanel";
			this.shufflePanel.Size = new System.Drawing.Size(158, 87);
			this.shufflePanel.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.shufflePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.shufflePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.shufflePanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.shufflePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.shufflePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.shufflePanel.Style.GradientAngle = 90;
			this.shufflePanel.TabIndex = 0;
			// 
			// movesLabel
			// 
			// 
			// 
			// 
			this.movesLabel.BackgroundStyle.Class = "";
			this.movesLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.movesLabel.Location = new System.Drawing.Point(7, 62);
			this.movesLabel.Name = "movesLabel";
			this.movesLabel.Size = new System.Drawing.Size(33, 23);
			this.movesLabel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.movesLabel.TabIndex = 2;
			this.movesLabel.Text = "Moves";
			// 
			// shuffleButton
			// 
			this.shuffleButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.shuffleButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.shuffleButton.Location = new System.Drawing.Point(3, 3);
			this.shuffleButton.Name = "shuffleButton";
			this.shuffleButton.Size = new System.Drawing.Size(152, 59);
			this.shuffleButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.shuffleButton.TabIndex = 1;
			this.shuffleButton.Text = "Shuffle";
			this.shuffleButton.Click += new System.EventHandler(this.shuffleButton_Click);
			// 
			// shuffleInput
			// 
			// 
			// 
			// 
			this.shuffleInput.BackgroundStyle.Class = "DateTimeInputBackground";
			this.shuffleInput.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.shuffleInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
			this.shuffleInput.Location = new System.Drawing.Point(46, 64);
			this.shuffleInput.MaxValue = 200;
			this.shuffleInput.MinValue = 1;
			this.shuffleInput.Name = "shuffleInput";
			this.shuffleInput.ShowUpDown = true;
			this.shuffleInput.Size = new System.Drawing.Size(109, 20);
			this.shuffleInput.TabIndex = 0;
			this.shuffleInput.Value = 50;
			// 
			// ribbonPanel1
			// 
			this.ribbonPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.ribbonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ribbonPanel1.Location = new System.Drawing.Point(0, 0);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.ribbonPanel1.Size = new System.Drawing.Size(630, 152);
			// 
			// 
			// 
			this.ribbonPanel1.Style.Class = "";
			this.ribbonPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.ribbonPanel1.StyleMouseDown.Class = "";
			this.ribbonPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.ribbonPanel1.StyleMouseOver.Class = "";
			this.ribbonPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.ribbonPanel1.TabIndex = 3;
			this.ribbonPanel1.Visible = false;
			// 
			// solverPanel
			// 
			this.solverPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.solverPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.solverPanel.Location = new System.Drawing.Point(0, 0);
			this.solverPanel.Name = "solverPanel";
			this.solverPanel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.solverPanel.Size = new System.Drawing.Size(630, 152);
			// 
			// 
			// 
			this.solverPanel.Style.Class = "";
			this.solverPanel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.solverPanel.StyleMouseDown.Class = "";
			this.solverPanel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.solverPanel.StyleMouseOver.Class = "";
			this.solverPanel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.solverPanel.TabIndex = 2;
			this.solverPanel.Visible = false;
			// 
			// cubeTab
			// 
			this.cubeTab.Checked = true;
			this.cubeTab.Name = "cubeTab";
			this.cubeTab.Panel = this.cubeControlPanel;
			this.cubeTab.Text = "Cube Controls";
			// 
			// Solver
			// 
			this.Solver.Name = "Solver";
			this.Solver.Panel = this.solverPanel;
			this.Solver.Text = "Solver";
			// 
			// Style
			// 
			this.Style.Name = "Style";
			this.Style.Panel = this.ribbonPanel1;
			this.Style.Text = "Style";
			// 
			// styleManager
			// 
			this.styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Blue;
			// 
			// renderPanel
			// 
			this.renderPanel.CanvasColor = System.Drawing.SystemColors.Control;
			this.renderPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.renderPanel.Controls.Add(this.rubikTeacher);
			this.renderPanel.Location = new System.Drawing.Point(5, 155);
			this.renderPanel.Name = "renderPanel";
			this.renderPanel.Padding = new System.Windows.Forms.Padding(4);
			this.renderPanel.Size = new System.Drawing.Size(630, 290);
			this.renderPanel.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.renderPanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.renderPanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.renderPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.renderPanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.renderPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.renderPanel.Style.GradientAngle = 90;
			this.renderPanel.TabIndex = 2;
			// 
			// rubikTeacher
			// 
			this.rubikTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rubikTeacher.Location = new System.Drawing.Point(4, 4);
			this.rubikTeacher.Name = "rubikTeacher";
			this.rubikTeacher.Size = new System.Drawing.Size(622, 282);
			this.rubikTeacher.TabIndex = 0;
			this.rubikTeacher.Text = "rubikTeacher";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.Controls.Add(this.renderPanel);
			this.Controls.Add(this.ribbonControl);
			this.EnableGlass = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Rubik Teacher";
			this.ribbonControl.ResumeLayout(false);
			this.ribbonControl.PerformLayout();
			this.cubeControlPanel.ResumeLayout(false);
			this.consolePanel.ResumeLayout(false);
			this.displayPanel.ResumeLayout(false);
			this.shufflePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize) (this.shuffleInput)).EndInit();
			this.renderPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public DevComponents.DotNetBar.RibbonControl ribbonControl;
		public DevComponents.DotNetBar.RibbonPanel cubeControlPanel;
		public DevComponents.DotNetBar.RibbonPanel solverPanel;
		public DevComponents.DotNetBar.RibbonTabItem cubeTab;
		public DevComponents.DotNetBar.RibbonTabItem Solver;
		public DevComponents.DotNetBar.StyleManager styleManager;
		public DevComponents.DotNetBar.PanelEx shufflePanel;
		public DevComponents.DotNetBar.LabelX movesLabel;
		public DevComponents.DotNetBar.ButtonX shuffleButton;
		public DevComponents.Editors.IntegerInput shuffleInput;
		public DevComponents.DotNetBar.PanelEx renderPanel;
		public RubikTeacher rubikTeacher;
		public DevComponents.DotNetBar.PanelEx displayPanel;
		public DevComponents.DotNetBar.ButtonX showNetButton;
		public DevComponents.DotNetBar.RibbonPanel ribbonPanel1;
		public DevComponents.DotNetBar.RibbonTabItem Style;
		public DevComponents.DotNetBar.PanelEx consolePanel;
		public DevComponents.DotNetBar.Controls.TextBoxX consoleInput;
		public DevComponents.DotNetBar.Controls.TextBoxX console;
		public DevComponents.DotNetBar.ButtonX undoButton;
	}
}