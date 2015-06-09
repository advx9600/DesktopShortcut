using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using NHibernate.Cfg;
using NHibernate;
using NHibernateGenDbSqlite.Domain;

namespace NHibernateGenDbSqlite
{
    class MySave
    {
        private const string DATABASENAME = "my_desk_shortcut.db";
        private const string CONNECT_STRING = "Data Source=" + DATABASENAME + ";Version=3;";

        // table name
        private const string TB_CONFIG = "tb_config";
        private const String TB_APPS = "tb_apps";
        private static SQLiteConnection conn;


        private static void insertData()
        {
            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
            string sql = "CREATE TABLE " + TB_CONFIG + "(id INTEGER PRIMARY KEY AUTOINCREMENT,key varchar(50),val varchar(50))";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();

            {
                var list = TBConfigDao.getInitList();
                for (int i = 0; i < list.Count(); i++)
                {
                    var data = list.ElementAt(i);
                    sql = "insert into " + TB_CONFIG + "(key,val)values('" + data.key + "','" + data.val + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }

            sql = "CREATE TABLE " + TB_APPS + "(id INTEGER PRIMARY KEY AUTOINCREMENT,name varchar(100),type int default 0,path varchar(255))";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();            
        }
        public static void check()
        {
            String file = System.AppDomain.CurrentDomain.BaseDirectory +"\\"+ DATABASENAME;
            if (File.Exists(file))
            {
                return;
            }
            else
            {
                SQLiteConnection.CreateFile(file);
                conn = new System.Data.SQLite.SQLiteConnection(CONNECT_STRING);

                conn.Open();
                insertData();
                conn.Close();
            }
        }
    }
}
