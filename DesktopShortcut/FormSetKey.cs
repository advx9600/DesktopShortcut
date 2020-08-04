using NHibernateGenDbSqlite.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernateGenDbSqlite
{
    public partial class FormSetKey : Form
    {
        private TbApps app;
        private Action<TbApps , string> callback;

        public FormSetKey()
        {
            InitializeComponent();
        }

        public FormSetKey(TbApps app, Action<TbApps, string> callback)
        {
            InitializeComponent();
            this.app = app;
            this.callback = callback;
        }



        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            Keys[] ignoreKeys = { Keys.Alt, Keys.Shift, Keys.Space,Keys.Control };
            foreach (var ignoreKey in ignoreKeys)
            {
                if (ignoreKey == e.KeyCode)
                    return;
            }

            if (e.KeyCode == Keys.Escape) Close();
            else if (MessageBox.Show(e.KeyCode == Keys.Delete ? string.Format("确认删除快捷键{0}吗?", app.hotkey) : string.Format("确认快捷键是 {0} 吗", e.KeyCode), "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                callback.Invoke(app, e.KeyCode.ToString());
                Close();
            };
        }

    }
}
