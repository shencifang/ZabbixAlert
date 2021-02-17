using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace _202012111347
{
    public partial class zabbixuser : Form
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
        public zabbixuser()
        {
            InitializeComponent();
        }

        private void zabbixuser_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.AppSettings["user"];
            textBox2.Text = ConfigurationManager.AppSettings["passw"];
            textBox3.Text = ConfigurationManager.AppSettings["url"];
            log.log("初始ZABBIX为" + ConfigurationManager.AppSettings["user"] + "+" + ConfigurationManager.AppSettings["passw"] + "+" + ConfigurationManager.AppSettings["url"]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("user");
            config.AppSettings.Settings.Add("user", textBox1.Text);
            config.AppSettings.Settings.Remove("passw");
            config.AppSettings.Settings.Add("passw", textBox2.Text);
            config.AppSettings.Settings.Remove("url");
            config.AppSettings.Settings.Add("url", textBox3.Text);
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
            log.log("ZABBIX修改为" + textBox1.Text + "+" + textBox2.Text + "+" + textBox3.Text);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
