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
using System.Data.SQLite;
using System.Configuration;

namespace _202012111347
{
    public partial class Form1 : Form
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
        sqllite sqllite = new sqllite();
        private SoundPlayer sp = new SoundPlayer();
        string token = "";
        public Form1()
        {
            InitializeComponent();
            TopMost = true;
            log.log("----------------------------------");
            log.log("程序开始");
            access_token();
        }

        private void access_token()
        {
            HttpGet HttpGet = new HttpGet();
            string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + ConfigurationManager.AppSettings["corpid"] + "&corpsecret=" + ConfigurationManager.AppSettings["secret"] + "";
            string os = HttpGet.Get(url);
            log.log(os);
            string[] strArray = os.Split('"');
            if (strArray[5] == "ok")
                token = strArray[9];
            else MessageBox.Show("获取token失败");
            log.log("取得access_token:" + token);
        }

        private void send(string gm,string mess)
        {
            
            HttpPost HttpPost = new HttpPost();
            string url = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + token;
            string strpost ="";
            if (gm == "负责人1" || gm == "负责人2" || gm == "负责人3" || gm == "负责人4")
                strpost = "{\"touser\":\"" + "默认负责人1|默认负责人2手机号|" + gm + "\",\"msgtype\":\"text\",\"agentid\":1000002,\"text\":{\"content\":\"" + mess + "\"},\"safe\":0}";
            else
            {
                if (gm == "")
                {
                    strpost = "{\"touser\":\"" + ConfigurationManager.AppSettings["gm"] + "\",\"msgtype\":\"text\",\"agentid\":1000002,\"text\":{\"content\":\"" + mess + "\"},\"safe\":0}";
                }
                else
                strpost = "{\"touser\":\"" + ConfigurationManager.AppSettings["gm"] + "\",\"msgtype\":\"text\",\"agentid\":1000002,\"text\":{\"content\":\"" + mess + "\"},\"safe\":0}";
            }
            string os = HttpPost.Postdata(strpost, url);
            log.log(strpost);
            log.log(os);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zabbix zabbix = new Zabbix(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["passw"], "http://" + ConfigurationManager.AppSettings["url"] + "/zabbix/api_jsonrpc.php");
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
            Zabbix zabbix = new Zabbix(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["passw"], "http://" + ConfigurationManager.AppSettings["url"] + "/zabbix/api_jsonrpc.php");
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
            string gm = "";
            string host = "";
            //登陆
            Zabbix zabbix = new Zabbix(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["passw"], "http://" + ConfigurationManager.AppSettings["url"] + "/zabbix/api_jsonrpc.php");
            zabbix.login();
            log.log("取数" + DateTime.Now.AddSeconds(-630).ToString() + "至" + DateTime.Now.ToString());
            textBox1.Text += "-------------------" + "\r\n";
            textBox1.Text += "取数" + DateTime.Now.AddSeconds(-630).ToString() + "至" + DateTime.Now.ToString() + "\r\n";
            //取数
            long lastChangeSinceTime = DateTimeUtil.DateTimeToTimeStamp(DateTime.Now.AddSeconds(-630));
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
                lastChangeSince = lastChangeSinceTime,
                filter = new { value = 1 }
            });
            zabbix.logout();
            foreach (dynamic data in responseObj.result)
            {
                string info = "";
                    gm = sqllite.select(data.hostname);
                    info = "告警设备：" + data.hostname + "\r\n" + "告警描述：" + data.description + "\r\n" + "告警时间：" + DateTimeUtil.TimeStampToDateTime(long.Parse(data.lastchange)) + "\r\n" + "负责人：" + gm + "\r\n";

                    sp.SoundLocation = "music\\15758094501719.wav";
                    sp.Load();
                    sp.Play();

                    textBox1.Text += info;
                    send(gm, info);
                    // sqllite.instert(DateTimeUtil.TimeStampToDateTime(long.Parse(data.lastchange)).ToString(), data.hostname, data.description);
                    log.log(info);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        zabbixuser form2 = null;
        
        wechat form3 = null;
        
        gm form4 = null;
        private void 通知人员设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            send("默认负责人1","测试");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            access_token();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            TopMost = false;
            form2 = new zabbixuser();
            form2.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            TopMost = false;
            form3 = new wechat();
            form3.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            TopMost = false;
            form4 = new gm();
            form4.ShowDialog();
        }
    }
}
