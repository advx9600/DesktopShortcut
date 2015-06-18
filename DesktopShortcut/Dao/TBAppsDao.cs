using NHibernate;
using NHibernate.Criterion;
using NHibernateGenDbSqlite.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    class TBAppsDao : MyDao
    {
        public const int TYPE_EXE = 0;
        public const int TYPE_DIR = 1;
        public static int getCount()
        {
            using (var ses = NHibernateHelper.OpenSession())
            {
                return ses.QueryOver<TbApps>().Select(Projections.RowCount()).FutureValue<int>().Value;
            }
        }
        public static bool addShortCut(String realPath)
        {
            return addShortCut(realPath, null);
        }
        public static bool addShortCut(String path, String name, int type = TYPE_EXE)
        {
            bool isNeedAdd = false;
            using (var se = NHibernateHelper.OpenSession())
            {
                if (se.QueryOver<TbApps>().Where(c => c.path == path).RowCount() == 0)
                {
                    isNeedAdd = true;
                }
            }
            if (isNeedAdd)
            {
                if (name == null)
                {
                    if (type == TYPE_EXE)
                        name = Path.GetFileNameWithoutExtension(path);
                    else if (type == TYPE_DIR)
                        name = Path.GetFileName(path);
                }
                var data = new TbApps
                {
                    type = type,
                    name = name,
                    path = path,
                };
                Insert(data);
            }
            return isNeedAdd;
        }

        internal static IList<TbApps> getAllData()
        {
            using (var se = NHibernateHelper.OpenSession())
            {
                return se.QueryOver<TbApps>().List();
            }
        }

        public static void copyMyAppData(TbApps tbSrc, TbApps tbSource)
        {
            tbSrc.name = tbSource.name;
            tbSrc.path = tbSource.path;
            tbSrc.type = tbSource.type;
        }

        private static IList<TbApps> queryDataBetwenId(TbApps app, int id1, int id2)
        {
            using (var se = NHibernateHelper.OpenSession())
            {
                if (id1 > id2)
                {
                    int temp = id1;
                    id1 = id2;
                    id2 = temp;
                }
                var list = se.QueryOver<TbApps>().Where(c => c.type == app.type).And(c => c.Id >= id1).And(c => c.Id <= id2).List();
                return list;
            }
        }
        internal static void reOrderApps(TbApps tbApps1, TbApps tbApps2)
        {
            var tbName = typeof(TbApps).Name;
            int id1 = tbApps1.Id;
            if (tbApps2 == null)
            {
                return;
            }
            int id2 = tbApps2.Id;
            if (id1 < id2)
            {
                using (var se = NHibernateHelper.OpenSession())
                using (ITransaction trans = se.BeginTransaction())
                {
                    var list = queryDataBetwenId(tbApps1, id1, id2);
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        var data = list.ElementAt(i);
                        copyMyAppData(data, list.ElementAt(i + 1));
                    }
                    copyMyAppData(list.ElementAt(list.Count - 1), tbApps1);
                    for (int i = 0; i < list.Count; i++)
                    {
                        se.Update(list.ElementAt(i));
                    }
                    se.Flush();
                    trans.Commit();
                }
            }
            else if (id1 > id2)
            {
                using (var se = NHibernateHelper.OpenSession())
                using (ITransaction trans = se.BeginTransaction())
                {
                    var list = queryDataBetwenId(tbApps1, id1, id2);
                    for (int i = list.Count - 1; i > 0; i--)
                    {
                        var data = list.ElementAt(i);
                        copyMyAppData(data, list.ElementAt(i - 1));
                    }
                    copyMyAppData(list.ElementAt(0), tbApps1);
                    for (int i = 0; i < list.Count; i++)
                    {
                        se.Update(list.ElementAt(i));
                    }
                    se.Flush();
                    trans.Commit();
                }
            }

        }

        internal static bool isTypeEqual(TbApps data, int type)
        {
            return data.type == type ? true : false;
        }

        internal static TbApps getByPath(string file)
        {
            using (var se = NHibernateHelper.OpenSession())
            {
                var list = se.QueryOver<TbApps>().Where(c => c.path == file).List();
                if (list.Count > 0) return list[0];
            }
            return null;
        }
    }
}
