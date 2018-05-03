using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateGenDbSqlite.Domain
{
     public class TbConfig
    {
        public virtual int Id { get; set; }
        public virtual string key { get; set; }
        public virtual string val { get; set; }
    }

     public class TbApps
     {
         public virtual int Id { get; set; }
         public virtual string name { get; set; }
         public virtual string path { get; set; }
         public virtual int type { get; set; }
     }

  
}
