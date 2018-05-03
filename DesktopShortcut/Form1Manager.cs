using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    public class Form1Manager
    {
        private Form1 mForm;

        public static void setWindow(Form1 f)
        {
            f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            f.ShowInTaskbar = false;
            f.AllowDrop = true;
            f.BackColor = Color.Black;
            f.Width = int.Parse(TBConfigDao.getWidth());
            f.Height = int.Parse(TBConfigDao.getHeight());
            f.Left = int.Parse(TBConfigDao.getX());
            f.Top = int.Parse(TBConfigDao.getY());

            f.BackgroundImageLayout = ImageLayout.Stretch;

            Image img = Properties.Resources.default_img;
            f.BackgroundImage = img;
        }
        public Form1Manager(Form1 f)
        {
            mForm = f;
        }

        private bool mIsMouseDown = false;
        private bool mIsMoveWin = false;
        internal void setMouseAction(MouseEventArgs e, MOUSE_TYPE mOUSE_TYPE)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (mOUSE_TYPE)
                {
                    case MOUSE_TYPE.DOWN:
                        mIsMouseDown = true; mIsMoveWin = false; mForm.mXDown = e.X; mForm.mYDown = e.Y; break;
                    case MOUSE_TYPE.MOVE:
                        if (mIsMouseDown && !(mForm.mXDown == e.X && mForm.mYDown == e.Y))
                        {
                            mIsMoveWin = true;
                            doWindowMove(e.X, e.Y);
                        }; break;
                    case MOUSE_TYPE.UP:
                        mIsMouseDown = false;
                        if (mIsMoveWin)
                        {
                            saveWinLocatoin();
                        }
                        else
                        {
                            mForm.mFormPop.ShowDialog();
                        }
                        break;
                    case MOUSE_TYPE.DROP:
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (mOUSE_TYPE)
                {
                    case MOUSE_TYPE.UP:
                        mForm.Close();
                        break;
                }
            }
        }
        internal void setMouseAction(DragEventArgs e, MOUSE_TYPE mOUSE_TYPE, String setFile = null)
        {
            string file = setFile == null ? ((string[])e.Data.GetData(DataFormats.FileDrop))[0] : setFile;
            int addType = 0;// 1 exe, 2 dir
            String realPath = null;
            if (file.ToLower().EndsWith(".lnk"))
            {
                realPath = MyUtils.getShortCutRealPath(file);
                if (Directory.Exists(realPath) || File.Exists(realPath))
                {
                    setMouseAction(e, mOUSE_TYPE, realPath);
                }
                return;
            }
            else if (file.ToLower().EndsWith(".exe") || file.ToLower().EndsWith(".txt") || file.ToLower().EndsWith(".bat"))
            {
                /* 处理各种扩展名文件拖载过来后的处理 */
                realPath = file;
                addType = 1;
            }
            else if (Directory.Exists(file))
            {
                realPath = file;
                addType = 2;
            }

            if (addType == 1)
            {
                /* 是否显示快捷方式 */
                if (!MyUtils.isBatFile(realPath) && !MyUtils.isTxtFile(realPath) && MyUtils.GetIconByFileName(realPath) == null)
                {
                    return;
                }
                TBAppsDao.addShortCut(realPath);
                showAnimaSuccess();
            }
            else if (addType == 2)
            {
                TBAppsDao.addShortCut(realPath, null, TBAppsDao.TYPE_DIR);
                showAnimaSuccess();
            }
        }

        private void showAnimaSuccess()
        {
            Thread oThread = new Thread(new ThreadStart(threadShowAminateAddSuccess));
            oThread.Start();
        }

        private void threadShowAminateAddSuccess()
        {
            var w = mForm.Width;
            var h = mForm.Height;
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                mForm.Width = i;
                mForm.Height = i;
            }
            mForm.Width = w;
            mForm.Height = h;
        }
        private void doWindowMove(int x, int y)
        {
            mForm.Left += x - mForm.mXDown;
            mForm.Top += y - mForm.mYDown;
        }
        private void saveWinLocatoin()
        {
            TBConfigDao.setX(mForm.Left + "");
            TBConfigDao.setY(mForm.Top + "");
        }
    }
}
