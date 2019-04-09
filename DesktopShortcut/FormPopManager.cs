using NHibernateGenDbSqlite.Dao;
using NHibernateGenDbSqlite.Domain;
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
    class FormPopManager
    {
        private FormPop mForm;
        public FormPopManager(FormPop form)
        {
            mForm = form;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            setFormSize();
            refreshAppsData();
        }
        private void setFormSize()
        {
            mForm.Width = mForm.mWidth = int.Parse(TBConfigDao.getPopWidth());
            mForm.Height = mForm.mHeight = int.Parse(TBConfigDao.getPopHeight());
            mForm.Left = mForm.mX = int.Parse(TBConfigDao.getPopX());
            mForm.Top = mForm.mY = int.Parse(TBConfigDao.getPopY());
        }
        public void resizeListViews()
        {
            int w = mForm.Width;
            int h = mForm.Height;

            int topTitle = MyUtils.getFormTitleBarHeight(mForm) + MyUtils.getFormBorderWidth(mForm);
            int widthTitle = MyUtils.getFormBorderWidth(mForm) * 2;

            var panel = mForm.getMainPanel();
            panel.Width = (w - widthTitle) / 2;
            panel.Height = (h - topTitle);
            panel.Top = panel.Left = 0;
            panelControlReset(panel);

            var panelDir = mForm.getDirPanel();
            panelDir.Width = (w - widthTitle) / 2;
            panelDir.Height = (h - topTitle);
            panelDir.Top = 0;
            panelDir.Left = (w - widthTitle) / 2;
            panelControlReset(panelDir);
        }
        private void panelControlReset(Control panel, Object except = null, Control preAddControlParam = null)
        {
            int count = panel.Controls.Count;
            int w = panel.Width;
            int h = panel.Height;
            if (count == 0)
            {
                preAddControlParam = null;
                return;
            }
            int conW = panel.Controls[0].Width;
            int conH = panel.Controls[0].Height;

            const int marginTop = 5;
            const int marginLeft = 0;
            const int marginRight = 0;
            const int everDurLeft = 5;
            const int everDurTop = 5;

            int widthNum = (w - conW - marginLeft - marginRight) / (conW + everDurLeft) + 1;
            for (int i = 0; i < count; i++)
            {
                var con = panel.Controls[i];
                preAddControlParamGetPosition:
                int lines = i / widthNum;
                int cols = i % widthNum;
                if (except != null)
                {
                    if (con == except) continue;
                }
                con.Top = (marginTop) + lines * (everDurTop + conH);
                if (widthNum == 1)
                {
                    con.Left = (w - conW) / 2;
                }
                else
                {
                    con.Left = marginLeft + cols * (everDurLeft + conW);
                }
                if (preAddControlParam != null && i == count - 1)
                {
                    con = preAddControlParam;
                    i++;
                    goto preAddControlParamGetPosition;
                }
            }
        }
        public void refreshAppsData()
        {
            mForm.mListTbApps = TBAppsDao.getAllData();
            addExeData();
            addDirData();
        }
        private void addExeData()
        {
            addData(mForm.getMainPanel(), 32, 32, TBAppsDao.TYPE_EXE);
        }
        private void addDirData()
        {
            addData(mForm.getDirPanel(), 50, 32, TBAppsDao.TYPE_DIR);
        }

        private void setBtnBackggroundAsync(object obj)
        {
            var btn = (Button)obj;
            var data = (TbApps)btn.Tag;
            btn.BackgroundImage = MyUtils.GetIconByFileName(data.path).ToBitmap();
        }

        private void addData(Control panel, int width, int height, int type)
        {
            var list = mForm.mListTbApps;
            delControls(panel);
            for (int i = 0; i < list.Count; i++)
            {
                var data = list.ElementAt(i);
                if (!TBAppsDao.isTypeEqual(data, type))
                {
                    continue;
                }
                Button btn = new Button();
                btn.Tag = data;
                btn.Width = width;
                btn.Height = height;
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                bool check = false;
                switch (type)
                {
                    case TBAppsDao.TYPE_EXE: // include txt
                        if (TBAppsDaoCheck.isExe(data) && TBAppsDaoCheck.isOk(data))
                        {
                            check = true;
                            btn.MouseUp += mForm.ExeBtn_MouseUp;
                            btn.MouseMove += mForm.exeBtn_MouseMove;
                            btn.MouseDown += mForm.exeBtn_MouseDown;
                            btn.ContextMenuStrip = mForm.exeCon_ContextMenuStrip;
                            if (MyUtils.isTxtFile(data.path) || MyUtils.isBatFile(data.path))
                            {
                                btn.Text = Path.GetFileNameWithoutExtension(data.path);
                            }
                            else
                            {
                                btn.Tag = data;
                                Thread t = new Thread(new ParameterizedThreadStart(setBtnBackggroundAsync));
                                t.Start(btn);
                            }
                        }
                        break;
                    case TBAppsDao.TYPE_DIR:
                        if (TBAppsDaoCheck.isDir(data) && TBAppsDaoCheck.isOk(data))
                        {
                            check = true;
                            btn.MouseUp += mForm.DirBtn_MouseUp;
                            btn.MouseMove += mForm.DirBtn_MouseMove;
                            btn.MouseDown += mForm.DirBtn_MouseDown;
                            btn.ContextMenuStrip = mForm.dirCon_ContextMenuStrip;
                            btn.Text = data.name;
                        }
                        break;
                }
                if (check)
                {
                    panel.Controls.Add(btn);
                }
            }
            panelControlReset(panel);
        }

        private void SetHotKeyBack(TbApps app, string key)
        {
            app.hotkey = key;
            MyDao.Update(app);
            if (!string.IsNullOrEmpty(key))
            {
                foreach (var item in this.mForm.mListTbApps)
                {
                    if (key.Equals(item.hotkey) && item.Id != app.Id)
                    {
                        item.hotkey = "";
                        MyDao.Update(item);
                    }
                }
            }

            refreshAppsData();
        }
        internal void setHotKey(TbApps app, EventArgs e)
        {
            var callback = new FormSetKey.SetHotKeyCallback(SetHotKeyBack);
            var form = new FormSetKey(app, callback);
            form.Show();
        }

        // 快捷键显示窗口
        internal void showFront()
        {
            mForm.TopMost = true;
            mForm.Show();
            if (!MyUtils.setForegroundWin(mForm.Handle))
            {
                mForm.Hide();
            }
        }

        private void delControls(Control panel)
        {
            int count = panel.Controls.Count;
            for (int i = 0; i < count; i++) panel.Controls.RemoveAt(0);
        }
        internal void resizeEnd()
        {
            saveSize();
        }
        private void saveSize()
        {
            mForm.mWidth = mForm.Width;
            mForm.mHeight = mForm.Height;
            TBConfigDao.setPopWidth("" + mForm.mWidth);
            TBConfigDao.setPopHeight("" + mForm.mHeight);
        }
        private void saveLocation()
        {
            mForm.mX = mForm.Left;
            mForm.mY = mForm.Top;
            TBConfigDao.setPopX("" + mForm.mX);
            TBConfigDao.setPopY("" + mForm.mY);
        }

        private bool mIsHasMoreMove;
        private bool mIsThreadMoveJudgeStarted = false;
        private void thread_move_judge()
        {
            Thread.Sleep(300);
            if (mIsHasMoreMove)
            {
                mIsHasMoreMove = false;
                mIsThreadMoveJudgeStarted = false;
                move();
            }
            else
            {
                mIsThreadMoveJudgeStarted = false;
                saveLocation();
            }
        }
        internal void move()
        {
            if (!mIsThreadMoveJudgeStarted)
            {

                mIsHasMoreMove = false;
                mIsThreadMoveJudgeStarted = true;
                Thread oThread = new Thread(new ThreadStart(thread_move_judge));
                oThread.Start();
            }
            else
            {
                mIsHasMoreMove = true;
            }
        }

        internal void deActivated()
        {
            if (!mForm.mIsAwlayShow)
                mForm.Hide();
        }
        internal void appBtnClick(object sender)
        {
            var data = (TbApps)((Button)sender).Tag;
            startApp(data);
        }

        private void startApp(TbApps data)
        {
            switch (data.type)
            {
                case TBAppsDao.TYPE_EXE: MyUtils.startExeAsync(data.path); mForm.Hide(); break;
                case TBAppsDao.TYPE_DIR: openDir(data); break;
            }
        }

        internal void exeBtnAction(object sender, MouseEventArgs e, MOUSE_TYPE mOUSE_TYPE)
        {
            formControlAction(sender, e, mOUSE_TYPE, TBAppsDao.TYPE_EXE);
        }
        internal void dirBtnAction(object sender, MouseEventArgs e, MOUSE_TYPE mOUSE_TYPE)
        {
            formControlAction(sender, e, mOUSE_TYPE, TBAppsDao.TYPE_DIR);
        }
        private MouseDataSave mMouseData = new MouseDataSave();
        private MouseDataSave mMouseDrag = new MouseDataSave();
        private void formControlAction(object sender, MouseEventArgs e, MOUSE_TYPE mOUSE_TYPE, int type)
        {
            MouseDataSave mouseData = mMouseData;
            var panel = type == TBAppsDao.TYPE_EXE ? mForm.getMainPanel() : mForm.getDirPanel();
            if (mouseData.isIgnore)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                switch (mOUSE_TYPE)
                {
                    case MOUSE_TYPE.DOWN:
                        mouseData.isDown = true;
                        mouseData.isMove = false;
                        mouseData.downX = e.X;
                        mouseData.downY = e.Y;
                        mouseData.downSender = sender;
                        mouseData.moveSender = null;
                        mouseData.switch1 = -1;
                        mouseData.switch2 = -1;
                        mouseData.setOrginControlList(panel.Controls);
                        //((Control)sender).BringToFront();
                        break;
                    case MOUSE_TYPE.MOVE:
                        if (mouseData.isDown && mouseData.downX != e.X && mouseData.downY != e.Y)
                        {
                            mouseData.isMove = true;
                            var downCon = (Control)mouseData.downSender;

                            downCon.Left += e.X - mouseData.downX;
                            downCon.Top += e.Y - mouseData.downY;

                            var moveCon = MyUtils.getChildAtPosition(panel, downCon, e.X + ((Control)mouseData.downSender).Left, e.Y + ((Control)mouseData.downSender).Top);
                            if (moveCon != mouseData.moveSender)
                            {
                                mouseData.moveSender = moveCon;
                                reOrderPanel(panel, mouseData);
                            }
                        }
                        break;
                    case MOUSE_TYPE.UP:
                        mouseData.isDown = false;
                        mouseData.upX = e.X;
                        mouseData.upY = e.Y;

                        if (mouseData.isClick())
                        {
                            appBtnClick(sender);
                        }
                        else if (mouseData.isMoveAction())
                        {
                            if (mouseData.switch2 > -1)
                            {
                                var con = mouseData.getOrginConList()[mouseData.switch2];
                                TBAppsDao.reOrderApps(MyParams.getDataFromSender(mouseData.downSender), MyParams.getDataFromSender(con));
                            }
                            refreshAppsData();
                        }
                        break;
                }
            }
        }
        private void panelDragFromOutside(object sender, DragEventArgs e, MOUSE_DRAG drag, int type)
        {
            var mouseData = mMouseDrag;
            var con = MyUtils.getChildAtPosition((Control)sender, e.X, e.Y, true, mForm);
            var panel = type == TBAppsDao.TYPE_EXE ? mForm.getMainPanel() : mForm.getDirPanel();
            var file = MyUtils.getRealPath(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
            switch (drag)
            {
                case MOUSE_DRAG.ENTER:
                    bool isSupported = false;
                    switch (type)
                    {
                        case TBAppsDao.TYPE_EXE:
                            if (MyParams.isSupportedExeFile(file)) isSupported = true;
                            break;
                        case TBAppsDao.TYPE_DIR:
                            if (MyParams.isSupportedDirFile(file)) isSupported = true;
                            break;
                    }
                    if (isSupported)
                    {
                        e.Effect = DragDropEffects.Move;
                        mouseData.isDown = true;
                        mouseData.setOrginControlList(panel.Controls);
                        mouseData.switch2 = -1;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                        mouseData.isDown = false;
                    }
                    break;
                case MOUSE_DRAG.HOVER:
                    if (mouseData.isDown)
                    {
                        if (mouseData.moveSender != con)
                        {
                            mouseData.moveSender = con;
                            reOrderPanel(panel, mouseData);
                        }
                    }
                    break;
                case MOUSE_DRAG.DROP:
                    TbApps data = null;
                    if (mouseData.switch2 > -1)
                    {
                        data = MyParams.getDataFromSender(mouseData.getOrginConList()[mouseData.switch2]);
                    }
                    TBAppsDaoCheck.reOrderApps(file, data);
                    refreshAppsData();
                    break;
            }
        }
        private void reOrderPanel(Panel panel, MouseDataSave mouseData)
        {
            var p1 = mouseData.downSender;
            var p2 = mouseData.moveSender;
            var cons = panel.Controls;
            // 为单线程，不用担心重新reset之后马上有touch事件
            var addCon = new Control();
            panelControlReset(panel, p1, addCon);

            var list = mouseData.getOrginConList();
            int index1 = -1, index2 = -1;
            for (int i = 0; i < list.Count; i++)
            {
                var element = list.ElementAt(i);
                if (element == p1)
                {
                    index1 = i;
                }
                else if (element == p2)
                {
                    index2 = i;
                }
            }
            if (index1 < 0)// 从外部拖放文件
            {
                if (mouseData.switch2 != index2)
                {
                    mouseData.switch2 = index2;
                    if (addCon != null && mouseData.switch2 > -1)
                    {
                        reOrderPanelB(mouseData.getOrginConList(), mouseData.switch2, addCon);
                        mouseData.moveSender = null;
                    }
                }
            }
            else if (mouseData.switch1 != index1 || mouseData.switch2 != index2) // 内部拖放
            {
                mouseData.switch1 = index1;
                mouseData.switch2 = index2;
                if (index2 > -1)
                {
                    reOrderPanelA(mouseData.getOrginConList(), mouseData.switch1, mouseData.switch2, mouseData.downTop, mouseData.downLeft);
                    mouseData.moveSender = null;
                }
            }

        }
        private void reOrderPanelA(List<Control> cons, int index1, int index2, int top, int left)
        {
            if (index1 < index2)
            {
                for (int i = index2; i > index1 + 1; i--)
                {
                    cons[i].Top = cons[i - 1].Top;
                    cons[i].Left = cons[i - 1].Left;
                }
                cons[index1 + 1].Top = top;
                cons[index1 + 1].Left = left;
            }
            else if (index1 > index2)
            {
                for (int i = index2; i < index1; i++)
                {
                    cons[i].Top = cons[i + 1].Top;
                    cons[i].Left = cons[i + 1].Left;
                }
                cons[index1 - 1].Top = top;
                cons[index1 - 1].Left = left;
            }
        }
        private void reOrderPanelB(List<Control> list, int index, Control addCon)
        {
            for (int i = index; i < list.Count; i++)
            {
                var now = list[i];
                var next = i == list.Count - 1 ? addCon : list[i + 1];
                now.Top = next.Top;
                now.Left = next.Left;
            }
        }



        internal void delExeConMenuClick(TbApps tbApp)
        {
            delApp(tbApp);
        }

        internal void delDirConMenuClick(TbApps tbApp)
        {
            delApp(tbApp);
        }
        private void delApp(TbApps tbApp)
        {
            TBAppsDao.Remove(tbApp);
            refreshAppsData();
        }
        internal void openDir(TbApps tbApps)
        {
            var path = tbApps.path;
            switch (tbApps.type)
            {
                case TBAppsDao.TYPE_DIR:
                    MyUtils.openFolder(path);
                    break;
                case TBAppsDao.TYPE_EXE:
                    MyUtils.openFolder(Path.GetDirectoryName(path), @"/select," + path);
                    break;
                default:
                    MessageBox.Show("not support type");
                    break;
            }
        }
        internal void doKeyEvent(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape: mForm.Hide(); break;
            }

            foreach (var item in mForm.mListTbApps)
            {
                if (keyData.ToString().Equals(item.hotkey))
                {
                    startApp(item);
                    break;
                }
            }
        }
        internal void panelMainDragEnter(object sender, DragEventArgs e)
        {
            panelDragFromOutside(sender, e, MOUSE_DRAG.ENTER, TBAppsDao.TYPE_EXE);
        }

        internal void panelDirDragEnter(object sender, DragEventArgs e)
        {
            panelDragFromOutside(sender, e, MOUSE_DRAG.ENTER, TBAppsDao.TYPE_DIR);
        }

        internal void panelDirDragDrop(object sender, DragEventArgs e)
        {
            panelDragFromOutside(sender, e, MOUSE_DRAG.DROP, TBAppsDao.TYPE_DIR);
        }
        internal void panelMainDragOver(object sender, DragEventArgs e)
        {
            panelDragFromOutside(sender, e, MOUSE_DRAG.HOVER, TBAppsDao.TYPE_EXE);
        }
        internal void panelDirDragOver(object sender, DragEventArgs e)
        {
            panelDragFromOutside(sender, e, MOUSE_DRAG.HOVER, TBAppsDao.TYPE_DIR);
        }
        internal void panelMainDragDrop(object sender, DragEventArgs e)
        {
            panelDragFromOutside(sender, e, MOUSE_DRAG.DROP, TBAppsDao.TYPE_EXE);
        }
    }// end class    
}
