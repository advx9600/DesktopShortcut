using NHibernateGenDbSqlite.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    public partial class FormPop : Form
    {
        private FormPopManager mManager;
        public int mX;
        public int mY;
        public int mWidth;
        public int mHeight;
        public bool mIsAwlayShow = false;

        public IList<TbApps> mListTbApps;

        private const Int32 WM_SYSCOMMAND = 0x112;
        private const Int32 MF_BYPOSITION = 0x400;
        private const Int32 MYMENU_DISPLAY_NOFOCUS = 1000;
        //private const Int32 MUMENU2 = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public FormPop()
        {
            InitializeComponent();
        }

        private void insertTitleBarMenu()
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 5, MF_BYPOSITION, MYMENU_DISPLAY_NOFOCUS, "not close when unfocus");
        }
        private void FormPop_Load(object sender, EventArgs e)
        {
            insertTitleBarMenu();
            MySave.check();
            mIsAwlayShow = false;
            mManager = new FormPopManager(this);
            mManager.resizeListViews();
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case MYMENU_DISPLAY_NOFOCUS:
                        alwaysShowWin(true);
                        return;
                    default:
                        break;
                }
            }
            base.WndProc(ref msg);
        }

        public void alwaysShowWin(bool show)
        {
            mIsAwlayShow = show;
        }
        public Panel getMainPanel()
        {
            return this.panelMain;
        }
        public Panel getDirPanel()
        {
            return this.panelDir;
        }
        private void FormPop_ResizeEnd(object sender, EventArgs e)
        {
            if (mManager != null)
                mManager.resizeEnd();
        }

        private void FormPop_Move(object sender, EventArgs e)
        {
            if (mManager != null)
                mManager.move();
        }

        private void FormPop_Deactivate(object sender, EventArgs e)
        {
            if (mManager != null)
                mManager.deActivated();
        }

        private void FormPop_Resize(object sender, EventArgs e)
        {
            if (mManager != null)
            {
                mManager.resizeListViews();
            }
        }

        private void flowLayoutPanelMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        internal void ExeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            mManager.exeBtnAction(sender, e, MOUSE_TYPE.UP);
        }

        internal void exeBtn_MouseMove(object sender, MouseEventArgs e)
        {
            mManager.exeBtnAction(sender, e, MOUSE_TYPE.MOVE);
        }

        internal void exeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            mManager.exeBtnAction(sender, e, MOUSE_TYPE.DOWN);
        }

        internal void DirBtn_MouseUp(object sender, MouseEventArgs e)
        {
            mManager.dirBtnAction(sender, e, MOUSE_TYPE.UP);
        }

        internal void DirBtn_MouseMove(object sender, MouseEventArgs e)
        {
            mManager.dirBtnAction(sender, e, MOUSE_TYPE.MOVE);
        }

        internal void DirBtn_MouseDown(object sender, MouseEventArgs e)
        {
            mManager.dirBtnAction(sender, e, MOUSE_TYPE.DOWN);
        }

        public System.Windows.Forms.ContextMenuStrip exeCon_ContextMenuStrip { get { return contextMenuStripExeCon; } }
        public System.Windows.Forms.ContextMenuStrip dirCon_ContextMenuStrip { get { return contextMenuStripDirCon; } }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mManager.delExeConMenuClick(MyParams.getDataFromSender(contextMenuStripExeCon.SourceControl));
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mManager.delDirConMenuClick(MyParams.getDataFromSender(contextMenuStripDirCon.SourceControl));
        }
        private void openDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mManager.openDir(MyParams.getDataFromSender(contextMenuStripExeCon.SourceControl));
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            mManager.doKeyEvent(keyData);
            return true;
        }

        private void panelMain_DragEnter(object sender, DragEventArgs e)
        {
            mManager.panelMainDragEnter(sender, e);
        }
        private void panelMain_DragOver(object sender, DragEventArgs e)
        {
            mManager.panelMainDragOver(sender, e);
        }

        private void panelMain_DragDrop(object sender, DragEventArgs e)
        {
            mManager.panelMainDragDrop(sender, e);
        }
        private void panelDir_DragEnter(object sender, DragEventArgs e)
        {
            mManager.panelDirDragEnter(sender, e);
        }

        private void panelDir_DragOver(object sender, DragEventArgs e)
        {
            mManager.panelDirDragOver(sender, e);
        }

        private void panelDir_DragDrop(object sender, DragEventArgs e)
        {
            mManager.panelDirDragDrop(sender, e);
        }

    }
}
