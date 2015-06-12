using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    class MyUtils : MyUtilsBase
    {

        public static String getRealPath(String file)
        {
            if (file.ToLower().EndsWith(".lnk"))
            {
                return getShortCutRealPath(file);
            }
            return file;
        }
        public static String getShortCutRealPath(String file)
        {
            WshShell shell = new WshShell();
            var realExe = (IWshShortcut)shell.CreateShortcut(file);
            return realExe.TargetPath;
        }
        public static Icon GetIconByFileName(string fileName, bool isLarge = true)
        {
            int[] phiconLarge = new int[1];
            int[] phiconSmall = new int[1];
            //文件名 图标索引 
            Win32.ExtractIconEx(fileName, 0, phiconLarge, phiconSmall, 1);
            IntPtr IconHnd = new IntPtr(isLarge ? phiconLarge[0] : phiconSmall[0]);
            return Icon.FromHandle(IconHnd);
        }

        public static void startExe(string exeFile)
        {
            if (!System.IO.File.Exists(exeFile))
            {
                MessageBox.Show("file not exist:" + exeFile);
                return;
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = exeFile;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }
        public static void openFolder(String fileFullName, String arg = null)
        {
            if (!System.IO.Directory.Exists(fileFullName))
            {
                MessageBox.Show("direct not exist");
                return;
            }
            if (arg == null)
                System.Diagnostics.Process.Start(fileFullName);
            else
                System.Diagnostics.Process.Start("explorer.exe", arg);
        }

        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtCursor(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(Cursor.Position));
            return null;
        }

        internal static int getFormTitleBarHeight(Form form)
        {
            return form.Height - form.ClientRectangle.Height - getFormBorderWidth(form);
        }

        internal static int getFormBorderWidth(Form form)
        {
            return (form.Width - form.ClientRectangle.Width) / 2;
        }
        public static Control getChildAtPosition(Control panel, int x, int y, bool isGlobalPosition = false, Form globalForm = null)
        {
            if (isGlobalPosition && globalForm != null)
            {
                var f = globalForm;
                x -= f.Left + panel.Left + getFormBorderWidth(f);
                y -= f.Top + panel.Top + getFormTitleBarHeight(f);
            }
            return getChildAtPosition(panel, null, x, y);
        }
        public static Control getChildAtPosition(Control panel, Control exceptCon, int x, int y)
        {
            var cons = panel.Controls;
            for (int i = 0; i < cons.Count; i++)
            {
                var con = cons[i];
                if (con != exceptCon)
                {
                    if (x > con.Left && x < con.Left + con.Width && y > con.Top && y < con.Top + con.Height)
                    {
                        return con;
                    }
                }
            }
            return null;
        }
    }
}
