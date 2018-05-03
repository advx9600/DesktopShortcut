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
        }
        /*static void Main(String[] args)
        {
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
