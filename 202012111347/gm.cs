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
    public partial class gm : Form
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
        public gm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("gm");
            config.AppSettings.Settings.Add("gm",textBox1.Text );
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
            log.log("GM修改为" + textBox1.Text);
            this.Close();
        }

        private void gm_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.AppSettings["gm"];
            log.log("初始GM为" + ConfigurationManager.AppSettings["gm"]);
        }
    }
}
