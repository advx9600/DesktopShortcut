﻿using NHibernate.Cfg;
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
        private KeyboardHook mKeyHook; // 监听按键，用于键盘模拟鼠标滚动
        public Form1()
        {
            InitializeComponent();
            mFMager = new Form1Manager(this);
            S_MANAGER = mFMager;
            mFormPop = new FormPop();
            CheckForIllegalCrossThreadCalls = false; // 允许多进程调用控件
            //this.TopMost = true;
            /********key hook init*********/
            mKeyHook = new KeyboardHook();
            mKeyHook.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            mKeyHook.KeyUp += new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyUp);
            mKeyHook.Install();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySave.check();
            Form1Manager.setWindow(this);
            mPopHotkey = mFMager.GetPopHotkey();
            MyUtils.RegisterHotKey(Handle, HotKeyID, MyUtils.KeyModifiers.Ctrl | MyUtils.KeyModifiers.Shift, (Keys)mPopHotkey);
        }

        private Boolean FnIsDown = false;
        private Boolean CtryIsDown = false;
        private Boolean ShiftIsDown = false;
        private int mPopHotkey;

        private void keyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
            if (key == KeyboardHook.VKeys.Fn)
            {
                FnIsDown = true;
            }
            else if (key == KeyboardHook.VKeys.LCONTROL)
            {
                CtryIsDown = true;
            }
            else if (key == KeyboardHook.VKeys.LSHIFT)
            {
                ShiftIsDown = true;
            }
            //Console.WriteLine("key is " + key);
            /******
             listen ctry +up and ctry + down
             and simulate mouse wheel scroll
             * ********/
            if (FnIsDown && key == KeyboardHook.VKeys.UP)
            {
                Console.WriteLine("ctry+ up " + DateTime.Now.ToString());
                mKeyHook.setTempIgnoreKey();
                MyUtils.mouseScrollEvent(100);
            }
            else if (FnIsDown && key == KeyboardHook.VKeys.DOWN)
            {
                Console.WriteLine("ctry+ down " + DateTime.Now.ToString());
                mKeyHook.setTempIgnoreKey();
                MyUtils.mouseScrollEvent(-100);
            }

            if (CtryIsDown && ShiftIsDown && (int)key == mPopHotkey)
            {
                //mFormPop.ShowFront();
            }
        }
        private void keyboardHook_KeyUp(KeyboardHook.VKeys key)
        {
            if (key == KeyboardHook.VKeys.Fn)
            {
                FnIsDown = false;
            }
            else if (key == KeyboardHook.VKeys.LCONTROL)
            {
                CtryIsDown = false;
            }
            else if (key == KeyboardHook.VKeys.LSHIFT)
            {
                ShiftIsDown = false;
            }
        }

        private const int WM_HOTKEY = 0x312; //窗口消息：热键
        private const int WM_CREATE = 0x1; //窗口消息：创建
        private const int WM_DESTROY = 0x2; //窗口消息：销毁

        private const int HotKeyID = 1; //热键ID（自定义）

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            switch (msg.Msg)
            {
                case WM_HOTKEY: //窗口消息：热键
                    int tmpWParam = msg.WParam.ToInt32();
                    if (tmpWParam == HotKeyID)
                    {
                        mFormPop.ShowFront();
                    }
                    break;
                case WM_DESTROY: //窗口消息：销毁
                    MyUtils.UnregisterHotKey(this.Handle, HotKeyID); //销毁热键
                    break;
            }
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
