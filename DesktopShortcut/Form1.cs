using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    public partial class Form1 : Form
    {
        public int mXDown;
        public int mXUp;
        public int mYDown;
        public int mYUp;
        private Form1Manager mFMager;
        public FormPop mFormPop;
        public static Form1Manager S_MANAGER;
        public Form1()
        {
            InitializeComponent();
            mFMager = new Form1Manager(this);
            S_MANAGER = mFMager;
            mFormPop = new FormPop();            
            CheckForIllegalCrossThreadCalls = false; // 允许多进程调用控件
            //this.TopMost = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySave.check();
            Form1Manager.setWindow(this);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mFMager.setMouseAction(e, MOUSE_TYPE.MOVE);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mFMager.setMouseAction(e, MOUSE_TYPE.DOWN);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mFMager.setMouseAction(e, MOUSE_TYPE.UP);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private void Form1_DragDrop_1(object sender, DragEventArgs e)
        {
            mFMager.setMouseAction(e, MOUSE_TYPE.DROP);
        }

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle &= 0x00080000; // WS_EX_LAYERED
                cp.Style = 0x40000000 | 0x4000000; // WS_CHILD | WS_CLIPSIBLINGS
                cp.Parent = GetDesktopWindow();
                return cp;
            }
        }

    }
}
