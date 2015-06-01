using NHibernateGenDbSqlite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateGenDbSqlite.Dao
{
    class TBAppsDaoCheck
    {
        public static bool isOk(TbApps app)
        {
            if (app == null) return false;
            if (app.path == null || app.path.Length == 0) return false;
            if (!System.IO.Directory.Exists(app.path))
            {
                if (!System.IO.File.Exists(app.path)) return false;
            }
            return true;
        }

        internal static bool isExe(TbApps data)
        {
            if (data.type == TBAppsDao.TYPE_EXE)
            {
                return true;
            }
            return false;
        }

        internal static bool isDir(TbApps data)
        {
            if (data.type == TBAppsDao.TYPE_DIR)
            {
                return true;
            }
            return false;
        }

        public static void reOrderApps(string file, TbApps data2)
        {
            Form1.S_MANAGER.setMouseAction(null, MOUSE_TYPE.DROP, file);
            if (data2 != null)
            {
                TbApps data1 = TBAppsDao.getByPath(file);
                TBAppsDao.reOrderApps(data1, data2);
            }            
        }
    }
}
