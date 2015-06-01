using IWshRuntimeLibrary;
using NHibernateGenDbSqlite.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    public enum MOUSE_TYPE
    {
        DOWN, MOVE, UP, DROP
    }
    public enum MOUSE_DRAG
    {
        ENTER, HOVER, DROP
    }
    class MouseDataSave
    {
        public int downX;
        public int downY;
        public bool isDown;
        public bool isMove;
        public int upX;
        public int upY;

        public int downTop;
        public int downLeft;
        private object downsendA;
        public object downSender { get { return downsendA; } set { downsendA = value; downTop = ((Control)downsendA).Top; downLeft = ((Control)downsendA).Left; } }
        public object moveSender { get; set; }
        public List<Control> listConOrigin = new List<Control>();

        public bool isIgnore;

        public MouseDataSave()
        {
            isDown = false;
            isIgnore = false;
        }

        internal bool isClick()
        {
            if (downX == upX && downY == upY && !isMove)
            {
                return true;
            }
            return false;
        }

        internal bool isDownEquMove()
        {
            if (moveSender != null && moveSender == downSender)
            {
                return true;
            }
            return false;
        }

        internal void setMoveConTempInvisible()
        {
            if (moveSender != null)
            {
                var m = ((Control)moveSender);
                m.Left = -100;
                m.Top = -100;
            }
        }

        public bool ignoreMove { get; set; }

        private int mPreMoveX = 0;
        private int mPreMoveY = 0;
        internal bool needMove(MouseEventArgs e)
        {
            if (mPreMoveX != e.X || mPreMoveY != e.Y)
            {
                mPreMoveX = e.X;
                mPreMoveY = e.Y;
                return true;
            }
            return false;
        }

        internal bool isMoveAction()
        {
            return isMove;
        }

        internal void setOrginControlList(Control.ControlCollection cons)
        {
            listConOrigin.Clear();
            for (int i = 0; i < cons.Count; i++)
            {
                listConOrigin.Add(cons[i]);
            }
        }
        internal List<Control> getOrginConList()
        {
            return listConOrigin;
        }

        public int switch1 { get; set; }

        public int switch2 { get; set; }
        
    }
    class MyParams
    {
        public static TbApps getDataFromSender(object sender)
        {
            var data = ((Control)sender).Tag;
            return (TbApps)data;
        }
        private static String[] SupportExtention = { "exe" };

        public static bool isSupportedExeFile(String file)
        {
            var ext = Path.GetExtension(file).ToLower();
            if (ext != null && ext.Length > 0) ext = ext.Substring(1);
            for (int i = 0; i < SupportExtention.Count(); i++)
            {
                var data = SupportExtention[i];
                if (data.Equals(ext))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isSupportedDirFile(String file)
        {
            return Directory.Exists(file) ? true : false;
        }
    }
}
