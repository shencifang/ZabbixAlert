using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Net.NetworkInformation;


namespace _202012111347
{
    public partial class Form1 : Form
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
        private SoundPlayer sp = new SoundPlayer();
        public Form1()
        {
            InitializeComponent();
            log.log("程序开始");
            log.log("----------------------------------");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zabbix zabbix = new Zabbix("账号(如:Admin)", "密码", "Zabbix API URL(如:https://wearm.xin/zabbix/api_jsonrpc.php)");
            zabbix.login();
            Response responseObj = zabbix.objectResponse("trigger.get", new
            {
                output = new string[] { "hostname", "description", "lastchange", "priority", "value", "status", "triggerid" },
                min_severity = 3,
                expandData = true,
                expandDescription = true,
                expandExpression = true,
                selectHosts = "extend",
                selectGroups = "extend",
                monitored = true,
                sortfield = "hostname",
                skipDependent = true,
                filter = new { value = 1 }
            });
            zabbix.logout();
            log.log("输出开始");
            foreach (dynamic data in responseObj.result)
            {
                textBox1.Text += data.hostname + "\r\n";
                textBox1.Text += data.description + "\r\n";
                textBox1.Text += data.lastchange + "\r\n";
                textBox1.Text += DateTimeUtil.TimeStampToDateTime(long.Parse(data.lastchange));
            }
            textBox1.Text += "----------------------------------";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zabbix zabbix = new Zabbix("账号(如:mt)", "密码", "Zabbix API URL(如:http://10.135.4.68/zabbix/api_jsonrpc.php)");
            zabbix.login();
            Response responseObj = zabbix.objectResponse("trigger.get", new
            {
                output = new string[] { "hostname", "description", "lastchange", "priority", "value", "status", "triggerid" },
                min_severity = 1,
                expandData = true,
                expandDescription = true,
                expandExpression = true,
                selectHosts = "extend",
                selectGroups = "extend",
                monitored = true,
                sortfield = "hostname",
                skipDependent = true,
                lastChangeSince = 1606705350,
                filter = new { value = 1 }
            });
            zabbix.logout();
            foreach (dynamic data in responseObj.result)
            {
                textBox1.Text += data.hostname + "\r\n";
                textBox1.Text += data.description + "\r\n";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            string host = "";
            Zabbix zabbix = new Zabbix("账号(如:mt)", "密码", "Zabbix API URL(如:http://10.135.4.68/zabbix/api_jsonrpc.php)");
            zabbix.login();
            long lastChangeSinceTime = DateTimeUtil.DateTimeToTimeStamp(DateTime.Now.AddSeconds(-1200));
            log.log("取数" + DateTime.Now.AddSeconds(-1200).ToString() + "至" + DateTime.Now.ToString());
            textBox1.Text += "-------------------" + "\r\n";
            textBox1.Text += "取数" + DateTime.Now.AddSeconds(-1200).ToString() + "至" + DateTime.Now.ToString() + "\r\n";
            Response responseObj = zabbix.objectResponse("trigger.get", new
            {
                output = new string[] { "hostname", "description", "lastchange", "priority", "value", "status", "triggerid" },
                min_severity = 1,
                expandData = true,
                expandDescription = true,
                expandExpression = true,
                selectHosts = "extend",
                selectGroups = "extend",
                monitored = true,
                sortfield = "hostname",
                skipDependent = true,
                lastChangeSince = lastChangeSinceTime,
                filter = new { value = 1 }
            });
            zabbix.logout();
            foreach (dynamic data in responseObj.result)
            {
                textBox1.Text += "告警设备" + data.hostname + "\r\n";
                textBox1.Text += "告警描述" + data.description + "\r\n";
                //textBox1.Text += data.lastchange + "\r\n";
                textBox1.Text += "告警时间" + DateTimeUtil.TimeStampToDateTime(long.Parse(data.lastchange)) + "\r\n";
                log.log("告警设备" + data.hostname);
                log.log("告警描述" + data.description);
                //log.log(data.lastchange);
                log.log("告警时间" + DateTimeUtil.TimeStampToDateTime(long.Parse(data.lastchange)).ToString());
                host = data.hostname;
            }
            if (host == "")
            {
                textBox1.Text += "无告警" + "\r\n";
            }
            else
            {
                sp.SoundLocation = "music\\15758094501719.wav";
                sp.Load();
                sp.Play();
            }
            textBox1.Text += DateTime.Now.ToString() + "本次取数结束" + "\r\n";
            log.log("本次取数结束");
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            sp.SoundLocation = "music\\15758094501719.wav";
            sp.Load();
            sp.Play();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sp.Stop();
            sp.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1_Tick(timer1, EventArgs.Empty);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
