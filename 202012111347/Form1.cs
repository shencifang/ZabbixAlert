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

        public _202012111347.Info InfoModule = new _202012111347.Info();
        public _202012111347.InfoBLL InfoBLL = new _202012111347.InfoBLL();

        public _202012111347.Event eventModule = new _202012111347.Event();
        public _202012111347.EventBLL eventBLL = new _202012111347.EventBLL();

        public Form1()
        {
            InitializeComponent();
            TopMost = true;
            log.log("----------------------------------");
            log.log("程序开始");
            access_token();
            fill();
        }
        //填充数据
        private void fill()
        {
            textBox2.Text += "工程师A关闭了交换机X（10.0.0.1）网络连通性检查失败事件\r\n----------------------\r\n";
            textBox2.Text += "工程师B正在处理设备Y(10.0.0.2)网络连通性检查失败事件\r\n----------------------\r\n";
            textBox2.Text += "系统Z（10.0.0.3）网络连通性检查失败事件推送至工程师A超过30分钟未接收，推送至备角工程师C\r\n----------------------\r\n";
            label7.Text = "1";
            radPageView2.SelectedPage = radPageViewPage6;

            this.dataGridView1.Rows.Clear();
            List<_202012111347.Event> eventlist = eventBLL.GetModelList("");
            foreach (_202012111347.Event item in eventlist)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                cell.Value = item.id;
                DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                cell2.Value = item.time;
                DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                cell3.Value = item.ip;
                DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                cell4.Value = item.content;
                DataGridViewTextBoxCell cell5 = new DataGridViewTextBoxCell();
                cell5.Value = item.gm;
                DataGridViewTextBoxCell cell6 = new DataGridViewTextBoxCell();
                cell6.Value = item.recetime;
                DataGridViewTextBoxCell cell7 = new DataGridViewTextBoxCell();
                cell7.Value = item.close;
                DataGridViewTextBoxCell cell8 = new DataGridViewTextBoxCell();
                cell8.Value = item.closetime;
                row.Cells.Add(cell);
                row.Cells.Add(cell2);
                row.Cells.Add(cell3);
                row.Cells.Add(cell4);
                row.Cells.Add(cell5);
                row.Cells.Add(cell6);
                row.Cells.Add(cell7);
                row.Cells.Add(cell8);

                this.dataGridView1.Rows.Add(row);
            }
            this.label6.Text = eventlist.Count.ToString();
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
                strpost = "{\"touser\":\"" + "默认负责人|默认备用负责人手机号|" + gm + "\",\"msgtype\":\"text\",\"agentid\":1000002,\"text\":{\"content\":\"" + mess + "\"},\"safe\":0}";
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
                //声音告警
                sp.SoundLocation = "music\\15758094501719.wav";
                sp.Load();
                sp.Play();
                //显示日志
                textBox1.Text += info;
                //发送通知
                //send(gm, info);
                //存储事件
                eventModule.id = "4";
                eventModule.time = DateTimeUtil.TimeStampToDateTime(long.Parse(data.lastchange));
                eventModule.ip = data.hostname;
                eventModule.content = data.description;
                eventModule.gm = gm;
                eventModule.recetime = null;
                eventModule.close = Convert.ToDecimal(false);
                eventModule.closetime = null;
                bool isAdd = eventBLL.Add(eventModule);
                if (isAdd)
                   log.log("增加数据成功");
                //记录
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

        

        zabbixuser form2 = null;
        
        wechat form3 = null;
        
        gm form4 = null;
        private void 通知人员设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            send("默认负责人","测试");
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            fill();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void radPageViewPage9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dispose()
        {

        }

        private void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (radPageView1.SelectedPage == radPageViewPage4)
            {
                this.dataGridView2.Rows.Clear();
                List<_202012111347.Info> infolist = InfoBLL.GetModelList("");
                foreach (_202012111347.Info item in infolist)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = item.hostname;
                    comboBox3.Items.Add(item.hostname);
                    comboBox6.Items.Add(item.hostname);
                    comboBox7.Items.Add(item.hostname);
                    DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                    cell2.Value = item.gm;

                    row.Cells.Add(cell);
                    row.Cells.Add(cell2);

                    this.dataGridView2.Rows.Add(row);
                }
                //
                comboBox2.Items.Clear();
                List<_202012111347.Info> infolist2 = InfoBLL.GetModelList("gm is not null group by gm");
                foreach (_202012111347.Info item in infolist2)
                {
                    comboBox2.Items.Add(item.gm);
                    comboBox5.Items.Add(item.gm);
                }
            }
            if (radPageView1.SelectedPage == radPageViewPage10)
            {
                textBox3.Text = ConfigurationManager.AppSettings["gm"];
            }
        }


        private void radPageView2_SelectedPageChanged(object sender, EventArgs e)
        {
            if (radPageView2.SelectedPage == radPageViewPage6)
            {
                fill();
            }
            if (radPageView2.SelectedPage == radPageViewPage7)
            {
                //MessageBox.Show("34");
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                this.dataGridView2.Rows.Clear();
                List<_202012111347.Info> infolist = InfoBLL.GetModelList("gm='" + comboBox2.Text + "'");
                foreach (_202012111347.Info item in infolist)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = item.hostname;
                    DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                    cell2.Value = item.gm;

                    row.Cells.Add(cell);
                    row.Cells.Add(cell2);

                    this.dataGridView2.Rows.Add(row);
                }
            }
            else MessageBox.Show("请选择查询条件");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sp.Stop();
            sp.Dispose();
        }
    }
}
