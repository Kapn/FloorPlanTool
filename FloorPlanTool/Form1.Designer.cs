namespace FloorPlanTool
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolbar_panel = new System.Windows.Forms.Panel();
            this.selectButton = new System.Windows.Forms.Button();
            this.drawX_button = new System.Windows.Forms.Button();
            this.preview_panel = new System.Windows.Forms.Panel();
            this.text_button = new System.Windows.Forms.Button();
            this.undo_button = new System.Windows.Forms.Button();
            this.pen_tool_button = new System.Windows.Forms.Button();
            this.eraser_button = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.circle_button = new System.Windows.Forms.Button();
            this.clear_all_button = new System.Windows.Forms.Button();
            this.rectangle_button = new System.Windows.Forms.Button();
            this.blue_button = new System.Windows.Forms.Button();
            this.line_button = new System.Windows.Forms.Button();
            this.green_button = new System.Windows.Forms.Button();
            this.red_button = new System.Windows.Forms.Button();
            this.black_button = new System.Windows.Forms.Button();
            this.drawing_panel = new System.Windows.Forms.Panel();
            this.redoButton = new System.Windows.Forms.Button();
            this.toolbar_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolbar_panel
            // 
            this.toolbar_panel.Controls.Add(this.redoButton);
            this.toolbar_panel.Controls.Add(this.selectButton);
            this.toolbar_panel.Controls.Add(this.drawX_button);
            this.toolbar_panel.Controls.Add(this.preview_panel);
            this.toolbar_panel.Controls.Add(this.text_button);
            this.toolbar_panel.Controls.Add(this.undo_button);
            this.toolbar_panel.Controls.Add(this.pen_tool_button);
            this.toolbar_panel.Controls.Add(this.eraser_button);
            this.toolbar_panel.Controls.Add(this.trackBar1);
            this.toolbar_panel.Controls.Add(this.circle_button);
            this.toolbar_panel.Controls.Add(this.clear_all_button);
            this.toolbar_panel.Controls.Add(this.rectangle_button);
            this.toolbar_panel.Controls.Add(this.blue_button);
            this.toolbar_panel.Controls.Add(this.line_button);
            this.toolbar_panel.Controls.Add(this.green_button);
            this.toolbar_panel.Controls.Add(this.red_button);
            this.toolbar_panel.Controls.Add(this.black_button);
            this.toolbar_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolbar_panel.Location = new System.Drawing.Point(0, 0);
            this.toolbar_panel.Name = "toolbar_panel";
            this.toolbar_panel.Size = new System.Drawing.Size(126, 523);
            this.toolbar_panel.TabIndex = 0;
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(26, 287);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 35;
            this.selectButton.Text = "Selector";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // drawX_button
            // 
            this.drawX_button.Location = new System.Drawing.Point(68, 229);
            this.drawX_button.Name = "drawX_button";
            this.drawX_button.Size = new System.Drawing.Size(34, 23);
            this.drawX_button.TabIndex = 34;
            this.drawX_button.Text = "X";
            this.drawX_button.UseVisualStyleBackColor = true;
            // 
            // preview_panel
            // 
            this.preview_panel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.preview_panel.Location = new System.Drawing.Point(26, 327);
            this.preview_panel.Name = "preview_panel";
            this.preview_panel.Size = new System.Drawing.Size(76, 63);
            this.preview_panel.TabIndex = 33;
            // 
            // text_button
            // 
            this.text_button.Image = ((System.Drawing.Image)(resources.GetObject("text_button.Image")));
            this.text_button.Location = new System.Drawing.Point(26, 229);
            this.text_button.Name = "text_button";
            this.text_button.Size = new System.Drawing.Size(36, 23);
            this.text_button.TabIndex = 32;
            this.text_button.UseVisualStyleBackColor = true;
            // 
            // undo_button
            // 
            this.undo_button.Location = new System.Drawing.Point(26, 434);
            this.undo_button.Name = "undo_button";
            this.undo_button.Size = new System.Drawing.Size(82, 33);
            this.undo_button.TabIndex = 30;
            this.undo_button.Text = "Undo";
            this.undo_button.UseVisualStyleBackColor = true;
            this.undo_button.Click += new System.EventHandler(this.undo_button_Click);
            // 
            // pen_tool_button
            // 
            this.pen_tool_button.Image = ((System.Drawing.Image)(resources.GetObject("pen_tool_button.Image")));
            this.pen_tool_button.Location = new System.Drawing.Point(26, 171);
            this.pen_tool_button.Name = "pen_tool_button";
            this.pen_tool_button.Size = new System.Drawing.Size(36, 23);
            this.pen_tool_button.TabIndex = 24;
            this.pen_tool_button.UseVisualStyleBackColor = true;
            // 
            // eraser_button
            // 
            this.eraser_button.Image = ((System.Drawing.Image)(resources.GetObject("eraser_button.Image")));
            this.eraser_button.Location = new System.Drawing.Point(26, 258);
            this.eraser_button.Name = "eraser_button";
            this.eraser_button.Size = new System.Drawing.Size(76, 23);
            this.eraser_button.TabIndex = 31;
            this.eraser_button.UseVisualStyleBackColor = true;
            this.eraser_button.Click += new System.EventHandler(this.eraser_button_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(26, 103);
            this.trackBar1.Minimum = 4;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(66, 45);
            this.trackBar1.TabIndex = 29;
            this.trackBar1.TickFrequency = 5;
            this.trackBar1.Value = 4;
            // 
            // circle_button
            // 
            this.circle_button.Image = ((System.Drawing.Image)(resources.GetObject("circle_button.Image")));
            this.circle_button.Location = new System.Drawing.Point(26, 200);
            this.circle_button.Name = "circle_button";
            this.circle_button.Size = new System.Drawing.Size(36, 23);
            this.circle_button.TabIndex = 26;
            this.circle_button.UseVisualStyleBackColor = true;
            this.circle_button.Click += new System.EventHandler(this.circle_button_Click);
            // 
            // clear_all_button
            // 
            this.clear_all_button.Location = new System.Drawing.Point(26, 473);
            this.clear_all_button.Name = "clear_all_button";
            this.clear_all_button.Size = new System.Drawing.Size(82, 38);
            this.clear_all_button.TabIndex = 25;
            this.clear_all_button.Text = "Clear All";
            this.clear_all_button.UseVisualStyleBackColor = true;
            this.clear_all_button.Click += new System.EventHandler(this.clear_all_button_Click);
            // 
            // rectangle_button
            // 
            this.rectangle_button.Image = ((System.Drawing.Image)(resources.GetObject("rectangle_button.Image")));
            this.rectangle_button.Location = new System.Drawing.Point(68, 171);
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
            this.line_button.Location = new System.Drawing.Point(68, 200);
            this.line_button.Name = "line_button";
            this.line_button.Size = new System.Drawing.Size(34, 23);
            this.line_button.TabIndex = 28;
            this.line_button.Text = "Line";
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
            // drawing_panel
            // 
            this.drawing_panel.BackColor = System.Drawing.Color.White;
            this.drawing_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawing_panel.Location = new System.Drawing.Point(126, 0);
            this.drawing_panel.Name = "drawing_panel";
            this.drawing_panel.Size = new System.Drawing.Size(488, 523);
            this.drawing_panel.TabIndex = 1;
            this.drawing_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.drawing_panel_Paint);
            this.drawing_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawing_panel_MouseDown);
            this.drawing_panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawing_panel_MouseMove);
            this.drawing_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawing_panel_MouseUp);
            // 
            // redoButton
            // 
            this.redoButton.Location = new System.Drawing.Point(27, 405);
            this.redoButton.Name = "redoButton";
            this.redoButton.Size = new System.Drawing.Size(75, 23);
            this.redoButton.TabIndex = 36;
            this.redoButton.Text = "Redo";
            this.redoButton.UseVisualStyleBackColor = true;
            this.redoButton.Click += new System.EventHandler(this.redoButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 523);
            this.Controls.Add(this.drawing_panel);
            this.Controls.Add(this.toolbar_panel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolbar_panel.ResumeLayout(false);
            this.toolbar_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel toolbar_panel;
        private System.Windows.Forms.Panel drawing_panel;
        private System.Windows.Forms.Button drawX_button;
        private System.Windows.Forms.Panel preview_panel;
        private System.Windows.Forms.Button text_button;
        private System.Windows.Forms.Button undo_button;
        private System.Windows.Forms.Button pen_tool_button;
        private System.Windows.Forms.Button eraser_button;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button circle_button;
        private System.Windows.Forms.Button clear_all_button;
        private System.Windows.Forms.Button rectangle_button;
        private System.Windows.Forms.Button blue_button;
        private System.Windows.Forms.Button line_button;
        private System.Windows.Forms.Button green_button;
        private System.Windows.Forms.Button red_button;
        private System.Windows.Forms.Button black_button;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button redoButton;
    }
}

