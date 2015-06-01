namespace NHibernateGenDbSqlite
{
    partial class FormPop
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
            this.components = new System.ComponentModel.Container();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelDir = new System.Windows.Forms.Panel();
            this.contextMenuStripExeCon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDirCon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripExeCon.SuspendLayout();
            this.contextMenuStripDirCon.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AllowDrop = true;
            this.panelMain.AutoScroll = true;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Location = new System.Drawing.Point(12, 12);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(50, 64);
            this.panelMain.TabIndex = 2;
            this.panelMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelMain_DragDrop);
            this.panelMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelMain_DragEnter);
            this.panelMain.DragOver += new System.Windows.Forms.DragEventHandler(this.panelMain_DragOver);
            // 
            // panelDir
            // 
            this.panelDir.AllowDrop = true;
            this.panelDir.AutoScroll = true;
            this.panelDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDir.Location = new System.Drawing.Point(96, 12);
            this.panelDir.Name = "panelDir";
            this.panelDir.Size = new System.Drawing.Size(50, 64);
            this.panelDir.TabIndex = 3;
            this.panelDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelDir_DragDrop);
            this.panelDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelDir_DragEnter);
            this.panelDir.DragOver += new System.Windows.Forms.DragEventHandler(this.panelDir_DragOver);
            // 
            // contextMenuStripExeCon
            // 
            this.contextMenuStripExeCon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delToolStripMenuItem});
            this.contextMenuStripExeCon.Name = "contextMenuStripExeBtn";
            this.contextMenuStripExeCon.Size = new System.Drawing.Size(95, 26);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.delToolStripMenuItem.Text = "del";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // contextMenuStripDirCon
            // 
            this.contextMenuStripDirCon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStripDirCon.Name = "contextMenuStripExeBtn";
            this.contextMenuStripDirCon.Size = new System.Drawing.Size(95, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem1.Text = "del";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // FormPop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 213);
            this.Controls.Add(this.panelDir);
            this.Controls.Add(this.panelMain);
            this.Name = "FormPop";
            this.ShowInTaskbar = false;
            this.Text = "FormPop";
            this.Deactivate += new System.EventHandler(this.FormPop_Deactivate);
            this.Load += new System.EventHandler(this.FormPop_Load);
            this.ResizeEnd += new System.EventHandler(this.FormPop_ResizeEnd);
            this.Move += new System.EventHandler(this.FormPop_Move);
            this.Resize += new System.EventHandler(this.FormPop_Resize);
            this.contextMenuStripExeCon.ResumeLayout(false);
            this.contextMenuStripDirCon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelDir;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExeCon;
        private System.Windows.Forms.ToolStripMenuItem delToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDirCon;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;


    }
}