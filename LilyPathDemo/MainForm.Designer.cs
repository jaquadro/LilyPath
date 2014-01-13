namespace LilyPathDemo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.multisampleAAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingControl1 = new LilyPathDemo.DrawingControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._tbFPS = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.drawingControl1);
            this.splitContainer1.Size = new System.Drawing.Size(814, 567);
            this.splitContainer1.SplitterDistance = 177;
            this.splitContainer1.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(177, 567);
            this.listBox1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(814, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillModeToolStripMenuItem,
            this.toolStripSeparator1,
            this.multisampleAAToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // fillModeToolStripMenuItem
            // 
            this.fillModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solidToolStripMenuItem,
            this.wireframeToolStripMenuItem});
            this.fillModeToolStripMenuItem.Name = "fillModeToolStripMenuItem";
            this.fillModeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.fillModeToolStripMenuItem.Text = "&Fill Mode";
            // 
            // solidToolStripMenuItem
            // 
            this.solidToolStripMenuItem.Checked = true;
            this.solidToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.solidToolStripMenuItem.Name = "solidToolStripMenuItem";
            this.solidToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.solidToolStripMenuItem.Text = "&Solid";
            this.solidToolStripMenuItem.Click += new System.EventHandler(this.solidToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.wireframeToolStripMenuItem.Text = "&WireFrame";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // multisampleAAToolStripMenuItem
            // 
            this.multisampleAAToolStripMenuItem.Enabled = false;
            this.multisampleAAToolStripMenuItem.Name = "multisampleAAToolStripMenuItem";
            this.multisampleAAToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.multisampleAAToolStripMenuItem.Text = "Multisample &AA";
            this.multisampleAAToolStripMenuItem.Click += new System.EventHandler(this.multisampleAAToolStripMenuItem_Click);
            // 
            // drawingControl1
            // 
            this.drawingControl1.BackColor = System.Drawing.Color.Black;
            this.drawingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingControl1.DrawAction = null;
            this.drawingControl1.Location = new System.Drawing.Point(0, 0);
            this.drawingControl1.Name = "drawingControl1";
            this.drawingControl1.Size = new System.Drawing.Size(633, 541);
            this.drawingControl1.TabIndex = 0;
            this.drawingControl1.VSync = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tbFPS});
            this.statusStrip1.Location = new System.Drawing.Point(0, 594);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(814, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _tbFPS
            // 
            this._tbFPS.Name = "_tbFPS";
            this._tbFPS.Size = new System.Drawing.Size(38, 17);
            this._tbFPS.Text = "FPS: 0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 616);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
        private DrawingControl drawingControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem multisampleAAToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _tbFPS;
    }
}

