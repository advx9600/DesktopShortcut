using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new FormSetKey(null));
        }
        /*static void Main(String[] args)
        {    // 调用bat测试
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "C:\\Users\\nwz\\Downloads\\jadx-0.7.1\\bin\\jadx-gui.bat";
            try
            {
                p.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }*/
        /*static void Main(String[] args)
        {
            // 测试NHibernate
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.TbConfig).Assembly);

            // Get ourselves an NHibernate Session
            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();
            var ttt =sess.QueryOver<Domain.TbConfig>().List();
            var tbConfig = new Domain.TbConfig();
            
            // And save it to the database
            sess.Save(tbConfig);
            sess.Flush();
        }*/
    }
}
