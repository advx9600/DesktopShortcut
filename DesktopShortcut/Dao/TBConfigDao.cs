using NHibernate;
using NHibernateGenDbSqlite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{

    class TBConfigDao : MyDao
    {
        private const String SEG_WIDTH = "width";
        private const String SEG_HEIGHT = "height";
        private const String SEG_X = "x";
        private const String SEG_Y = "y";
        // pop window
        private const String SEG_POP_WIDTH = "pop_width";
        private const String SEG_POP_HEIGHT = "pop_height";
        private const String SEG_POP_X = "pop_x";
        private const String SEG_POP_Y = "pop_y";
        // hot key
        private const String SET_POP_HOTKEY = "pop_hotkey";

        public static List<Data> getInitList()
        {
            List<Data> list = new List<Data>();

            list.Add(new Data(SEG_WIDTH, "40"));
            list.Add(new Data(SEG_HEIGHT, "40"));
            list.Add(new Data(SEG_X, "100"));
            list.Add(new Data(SEG_Y, "100"));

            list.Add(new Data(SEG_POP_WIDTH, "80"));
            list.Add(new Data(SEG_POP_HEIGHT, "400"));
            list.Add(new Data(SEG_POP_X, "100"));
            list.Add(new Data(SEG_POP_Y, "100"));
            list.Add(new Data(SET_POP_HOTKEY, "F")); // F key


            // check
            for (int i = 0; i < list.Count(); i++)
            {
                var dataI = list.ElementAt(i);
                for (int j = i + 1; j < list.Count; j++)
                {
                    var dataJ = list.ElementAt(j);
                    if (dataI.key.Equals(dataJ.key))
                    {
                        MessageBox.Show("tb_config name:" + dataI.key + " conflict");
                        return null;
                    }
                }
            }
            return list;
        }
        public class Data
        {
            public String key;
            public String val;
            public Data(String key, String val)
            {
                this.key = key;
                this.val = val;
            }
        }

        public static void setFiled(String name, String val)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var list = session.QueryOver<TbConfig>().Where(c => c.key == name).List();
                var entry = list.ElementAt(0);
                entry.val = val;
                Update(entry);
            }
        }
        public static String getFiled(String key)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var list = session.QueryOver<TbConfig>().Where(c => c.key == key).List();
                if (list.Count() == 0 || list.ElementAt(0).val == null) return "";
                return list.ElementAt(0).val.ToString();
            }
        }

        public static int getPopHotkey()
        {
            String key = getFiled(SET_POP_HOTKEY);
            try
            {
                Keys getKey;
                Enum.TryParse<Keys>(key, out getKey);
                return (int)getKey;
            }
            catch { }
            return 0x46;
        }
        public static void setPopHotKey(String val)
        {
            setFiled(SET_POP_HOTKEY, val);
        }
        public static String getHeight()
        {
            return getFiled(SEG_HEIGHT);
        }
        public static void setHeight(String val)
        {
            setFiled(SEG_HEIGHT, val);
        }

        public static String getWidth()
        {
            return getFiled(SEG_WIDTH);
        }
        public static void setWidth(String val)
        {
            setFiled(SEG_WIDTH, val);
        }
        public static String getX()
        {
            return getFiled(SEG_X);
        }
        public static void setX(String val)
        {
            setFiled(SEG_X, val);
        }
        public static String getY()
        {
            return getFiled(SEG_Y);
        }
        public static void setY(String val)
        {
            setFiled(SEG_Y, val);
        }
        public static String getPopWidth()
        {
            return getFiled(SEG_POP_WIDTH);
        }
        public static void setPopWidth(String val)
        {
            setFiled(SEG_POP_WIDTH, val);
        }
        public static String getPopHeight()
        {
            return getFiled(SEG_POP_HEIGHT);
        }
        public static void setPopHeight(String val)
        {
            setFiled(SEG_POP_HEIGHT, val);
        }
        public static String getPopX()
        {
            return getFiled(SEG_POP_X);
        }
        public static void setPopX(String val)
        {
            setFiled(SEG_POP_X, val);
        }
        public static String getPopY()
        {
            return getFiled(SEG_POP_Y);
        }
        public static void setPopY(String val)
        {
            setFiled(SEG_POP_Y, val);
        }
    }
}
