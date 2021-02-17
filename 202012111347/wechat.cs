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
    public partial class wechat : Form
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
        public wechat()
        {
            InitializeComponent();
        }

        private void wechat_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.AppSettings["corpid"];
            textBox2.Text = ConfigurationManager.AppSettings["secret"];
            log.log("初始WECHAT为" + ConfigurationManager.AppSettings["corpid"] + "+" + ConfigurationManager.AppSettings["secret"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("corpid");
            config.AppSettings.Settings.Add("corpid", textBox1.Text);
            config.AppSettings.Settings.Remove("secret");
            config.AppSettings.Settings.Add("secret", textBox2.Text);
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
            log.log("WECHAT修改为" + textBox1.Text + "+" + textBox2.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
