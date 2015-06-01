using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateGenDbSqlite
{
    class MyDao
    {
        public static void Add(Object obj)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(obj);                
                session.Flush();
                transaction.Commit();
            }
        }
        public static void Insert(Object obj)
        {
            Add(obj);
        }
        public static void Save(Object obj)
        {
            Add(obj);
        }

        public static void Update(Object obj)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(obj);
                session.Flush();
                transaction.Commit();
            }
        }

        public static void Remove(Object obj)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(obj);
                session.Flush();
                transaction.Commit();
            }
        }

        public static void ExecuteSQL(String  sql)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateQuery(sql).ExecuteUpdate();
                session.Flush();
                transaction.Commit();
            }
        }
    }
}
