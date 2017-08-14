namespace FloorPlanTool
{
    partial class FloorPlanTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FloorPlanTool));
            this.toolbar_panel = new System.Windows.Forms.Panel();
            this.tri_button = new System.Windows.Forms.Button();
            this.fill_circle_button = new System.Windows.Forms.Button();
            this.fill_rectangle_button = new System.Windows.Forms.Button();
            this.dotted_line_button = new System.Windows.Forms.Button();
            this.redo_button = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            this.text_button = new System.Windows.Forms.Button();
            this.undo_button = new System.Windows.Forms.Button();
            this.eraser_button = new System.Windows.Forms.Button();
            this.circle_button = new System.Windows.Forms.Button();
            this.clear_all_button = new System.Windows.Forms.Button();
            this.rectangle_button = new System.Windows.Forms.Button();
            this.blue_button = new System.Windows.Forms.Button();
            this.line_button = new System.Windows.Forms.Button();
            this.green_button = new System.Windows.Forms.Button();
            this.red_button = new System.Windows.Forms.Button();
            this.black_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawing_panel = new System.Windows.Forms.Panel();
            this.toolbar_panel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbar_panel
            // 
            this.toolbar_panel.Controls.Add(this.tri_button);
            this.toolbar_panel.Controls.Add(this.fill_circle_button);
            this.toolbar_panel.Controls.Add(this.fill_rectangle_button);
            this.toolbar_panel.Controls.Add(this.dotted_line_button);
            this.toolbar_panel.Controls.Add(this.redo_button);
            this.toolbar_panel.Controls.Add(this.selectButton);
            this.toolbar_panel.Controls.Add(this.text_button);
            this.toolbar_panel.Controls.Add(this.undo_button);
            this.toolbar_panel.Controls.Add(this.eraser_button);
            this.toolbar_panel.Controls.Add(this.circle_button);
            this.toolbar_panel.Controls.Add(this.clear_all_button);
            this.toolbar_panel.Controls.Add(this.rectangle_button);
            this.toolbar_panel.Controls.Add(this.blue_button);
            this.toolbar_panel.Controls.Add(this.line_button);
            this.toolbar_panel.Controls.Add(this.green_button);
            this.toolbar_panel.Controls.Add(this.red_button);
            this.toolbar_panel.Controls.Add(this.black_button);
            this.toolbar_panel.Controls.Add(this.menuStrip1);
            this.toolbar_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolbar_panel.Location = new System.Drawing.Point(0, 0);
            this.toolbar_panel.Name = "toolbar_panel";
            this.toolbar_panel.Size = new System.Drawing.Size(126, 632);
            this.toolbar_panel.TabIndex = 0;
            // 
            // tri_button
            // 
            this.tri_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tri_button.Image = ((System.Drawing.Image)(resources.GetObject("tri_button.Image")));
            this.tri_button.Location = new System.Drawing.Point(61, 185);
            this.tri_button.Name = "tri_button";
            this.tri_button.Size = new System.Drawing.Size(34, 23);
            this.tri_button.TabIndex = 41;
            this.tri_button.UseVisualStyleBackColor = true;
            this.tri_button.Click += new System.EventHandler(this.tri_button_Click);
            // 
            // fill_circle_button
            // 
            this.fill_circle_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fill_circle_button.Image = ((System.Drawing.Image)(resources.GetObject("fill_circle_button.Image")));
            this.fill_circle_button.Location = new System.Drawing.Point(22, 130);
            this.fill_circle_button.Name = "fill_circle_button";
            this.fill_circle_button.Size = new System.Drawing.Size(34, 23);
            this.fill_circle_button.TabIndex = 39;
            this.fill_circle_button.UseVisualStyleBackColor = true;
            this.fill_circle_button.Click += new System.EventHandler(this.fill_circle_button_Click);
            // 
            // fill_rectangle_button
            // 
            this.fill_rectangle_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fill_rectangle_button.Image = ((System.Drawing.Image)(resources.GetObject("fill_rectangle_button.Image")));
            this.fill_rectangle_button.Location = new System.Drawing.Point(61, 130);
            this.fill_rectangle_button.Name = "fill_rectangle_button";
            this.fill_rectangle_button.Size = new System.Drawing.Size(34, 23);
            this.fill_rectangle_button.TabIndex = 40;
            this.fill_rectangle_button.UseVisualStyleBackColor = true;
            this.fill_rectangle_button.Click += new System.EventHandler(this.fill_rectangle_button_Click);
            // 
            // dotted_line_button
            // 
            this.dotted_line_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dotted_line_button.Image = global::FloorPlanTool.Properties.Resources.dashed_line___newest;
            this.dotted_line_button.Location = new System.Drawing.Point(61, 101);
            this.dotted_line_button.Name = "dotted_line_button";
            this.dotted_line_button.Size = new System.Drawing.Size(34, 23);
            this.dotted_line_button.TabIndex = 38;
            this.dotted_line_button.UseVisualStyleBackColor = true;
            this.dotted_line_button.Click += new System.EventHandler(this.dotted_line_button_Click);
            // 
            // redo_button
            // 
            this.redo_button.Enabled = false;
            this.redo_button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.redo_button.Location = new System.Drawing.Point(22, 392);
            this.redo_button.Name = "redo_button";
            this.redo_button.Size = new System.Drawing.Size(81, 33);
            this.redo_button.TabIndex = 36;
            this.redo_button.Text = "Redo";
            this.redo_button.UseVisualStyleBackColor = true;
            this.redo_button.Click += new System.EventHandler(this.redoButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectButton.Image = global::FloorPlanTool.Properties.Resources.one_finger_tap_gesture_of_outlined_hand_symbol;
            this.selectButton.Location = new System.Drawing.Point(22, 214);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(34, 23);
            this.selectButton.TabIndex = 35;
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // text_button
            // 
            this.text_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.text_button.Image = ((System.Drawing.Image)(resources.GetObject("text_button.Image")));
            this.text_button.Location = new System.Drawing.Point(22, 185);
            this.text_button.Name = "text_button";
            this.text_button.Size = new System.Drawing.Size(36, 23);
            this.text_button.TabIndex = 32;
            this.text_button.UseVisualStyleBackColor = true;
            this.text_button.Click += new System.EventHandler(this.text_button_Click);
            // 
            // undo_button
            // 
            this.undo_button.Enabled = false;
            this.undo_button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.undo_button.Location = new System.Drawing.Point(22, 431);
            this.undo_button.Name = "undo_button";
            this.undo_button.Size = new System.Drawing.Size(82, 33);
            this.undo_button.TabIndex = 30;
            this.undo_button.Text = "Undo";
            this.undo_button.UseVisualStyleBackColor = true;
            this.undo_button.Click += new System.EventHandler(this.undo_button_Click);
            // 
            // eraser_button
            // 
            this.eraser_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.eraser_button.Image = ((System.Drawing.Image)(resources.GetObject("eraser_button.Image")));
            this.eraser_button.Location = new System.Drawing.Point(61, 214);
            this.eraser_button.Name = "eraser_button";
            this.eraser_button.Size = new System.Drawing.Size(34, 23);
            this.eraser_button.TabIndex = 31;
            this.eraser_button.UseVisualStyleBackColor = true;
            this.eraser_button.Click += new System.EventHandler(this.eraser_button_Click);
            // 
            // circle_button
            // 
            this.circle_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.circle_button.Image = ((System.Drawing.Image)(resources.GetObject("circle_button.Image")));
            this.circle_button.Location = new System.Drawing.Point(22, 156);
            this.circle_button.Name = "circle_button";
            this.circle_button.Size = new System.Drawing.Size(34, 23);
            this.circle_button.TabIndex = 26;
            this.circle_button.UseVisualStyleBackColor = true;
            this.circle_button.Click += new System.EventHandler(this.circle_button_Click);
            // 
            // clear_all_button
            // 
            this.clear_all_button.Enabled = false;
            this.clear_all_button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clear_all_button.Location = new System.Drawing.Point(22, 470);
            this.clear_all_button.Name = "clear_all_button";
            this.clear_all_button.Size = new System.Drawing.Size(82, 38);
            this.clear_all_button.TabIndex = 25;
            this.clear_all_button.Text = "Clear All";
            this.clear_all_button.UseVisualStyleBackColor = true;
            this.clear_all_button.Click += new System.EventHandler(this.clear_all_button_Click);
            // 
            // rectangle_button
            // 
            this.rectangle_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rectangle_button.Image = ((System.Drawing.Image)(resources.GetObject("rectangle_button.Image")));
            this.rectangle_button.Location = new System.Drawing.Point(61, 156);
            this.rectangle_button.Name = "rectangle_button";
            this.rectangle_button.Size = new System.Drawing.Size(34, 23);
            this.rectangle_button.TabIndex = 27;
            this.rectangle_button.UseVisualStyleBackColor = true;
            this.rectangle_button.Click += new System.EventHandler(this.rectangle_button_Click);
            // 
            // blue_button
            // 
            this.blue_button.BackColor = System.Drawing.Color.Blue;
            this.blue_button.Location = new System.Drawing.Point(62, 63);
            this.blue_button.Name = "blue_button";
            this.blue_button.Size = new System.Drawing.Size(30, 23);
            this.blue_button.TabIndex = 23;
            this.blue_button.UseVisualStyleBackColor = false;
            this.blue_button.Click += new System.EventHandler(this.blue_button_Click);
            // 
            // line_button
            // 
            this.line_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.line_button.Image = ((System.Drawing.Image)(resources.GetObject("line_button.Image")));
            this.line_button.Location = new System.Drawing.Point(22, 101);
            this.line_button.Name = "line_button";
            this.line_button.Size = new System.Drawing.Size(34, 23);
            this.line_button.TabIndex = 28;
            this.line_button.UseVisualStyleBackColor = true;
            this.line_button.Click += new System.EventHandler(this.line_button_Click);
            // 
            // green_button
            // 
            this.green_button.BackColor = System.Drawing.Color.Green;
            this.green_button.Location = new System.Drawing.Point(26, 63);
            this.green_button.Name = "green_button";
            this.green_button.Size = new System.Drawing.Size(30, 23);
            this.green_button.TabIndex = 22;
            this.green_button.UseVisualStyleBackColor = false;
            this.green_button.Click += new System.EventHandler(this.green_button_Click);
            // 
            // red_button
            // 
            this.red_button.BackColor = System.Drawing.Color.Red;
            this.red_button.Location = new System.Drawing.Point(62, 34);
            this.red_button.Name = "red_button";
            this.red_button.Size = new System.Drawing.Size(30, 23);
            this.red_button.TabIndex = 21;
            this.red_button.UseVisualStyleBackColor = false;
            this.red_button.Click += new System.EventHandler(this.red_button_Click);
            // 
            // black_button
            // 
            this.black_button.BackColor = System.Drawing.Color.Black;
            this.black_button.Location = new System.Drawing.Point(26, 34);
            this.black_button.Name = "black_button";
            this.black_button.Size = new System.Drawing.Size(30, 23);
            this.black_button.TabIndex = 20;
            this.black_button.UseVisualStyleBackColor = false;
            this.black_button.Click += new System.EventHandler(this.black_button_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(126, 24);
            this.menuStrip1.TabIndex = 37;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.fileToolStripMenuItem.Text = "File..";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // drawing_panel
            // 
            this.drawing_panel.BackColor = System.Drawing.Color.White;
            this.drawing_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawing_panel.Location = new System.Drawing.Point(126, 0);
            this.drawing_panel.Name = "drawing_panel";
            this.drawing_panel.Size = new System.Drawing.Size(874, 632);
            this.drawing_panel.TabIndex = 1;
            this.drawing_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.drawing_panel_Paint);
            this.drawing_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawing_panel_MouseDown);
            this.drawing_panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawing_panel_MouseMove);
            this.drawing_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawing_panel_MouseUp);
            // 
            // FloorPlanTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 632);
            this.Controls.Add(this.drawing_panel);
            this.Controls.Add(this.toolbar_panel);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FloorPlanTool";
            this.Text = "FloorPlan Tool";
            this.toolbar_panel.ResumeLayout(false);
            this.toolbar_panel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel toolbar_panel;
        private System.Windows.Forms.Panel drawing_panel;
        private System.Windows.Forms.Button text_button;
        private System.Windows.Forms.Button undo_button;
        private System.Windows.Forms.Button eraser_button;
        private System.Windows.Forms.Button circle_button;
        private System.Windows.Forms.Button clear_all_button;
        private System.Windows.Forms.Button rectangle_button;
        private System.Windows.Forms.Button blue_button;
        private System.Windows.Forms.Button line_button;
        private System.Windows.Forms.Button green_button;
        private System.Windows.Forms.Button red_button;
        private System.Windows.Forms.Button black_button;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button redo_button;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.Button dotted_line_button;
        private System.Windows.Forms.Button fill_circle_button;
        private System.Windows.Forms.Button fill_rectangle_button;
        private System.Windows.Forms.Button tri_button;
    }
}

