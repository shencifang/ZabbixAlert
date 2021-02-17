using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
namespace _202012111347
{
    class sqllite
    {
        string connStr = ConfigurationManager.ConnectionStrings["itcastCater"].ConnectionString;
        public void Create()
        {
            SQLiteConnection cn = new SQLiteConnection(connStr);
            //按照路径创建数据库文件
            cn.Open();
            cn.Close();
        }
        //创建数据库表
        public void CreateTable()
        {
            SQLiteConnection cn = new SQLiteConnection(connStr);//建立数据库连接
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();//打开数据库
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;//把 SQLiteCommand的 Connection和SQLiteConnection 联系起来
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int)";//输入SQL语句
                cmd.ExecuteNonQuery();//调用此方法运行
            }
            cn.Close();
        }
        //删除数据库表
        public  void DeleteTable()
        {
            SQLiteConnection cn = new SQLiteConnection(connStr);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;
                cmd.CommandText = "DROP TABLE IF EXISTS t1";
                cmd.ExecuteNonQuery();
            }
            cn.Close();
        }
        //插入数据
        public void instert(string start, string hostname, string describe)
        {
            SQLiteConnection cn = new SQLiteConnection(connStr);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;
                cmd.CommandText = "INSERT INTO history(start,hostname,describe) VALUES('" + start + "','" + hostname + "','" + describe + "');";
                cmd.ExecuteNonQuery();
            }
            cn.Close();
        }
            //查询数据
        public string select(string hostname)
        {
            string retr="";
            string gm = "";
            SQLiteConnection cn = new SQLiteConnection(connStr);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM info WHERE hostname='" + hostname + "'";
                SQLiteDataReader sr = cmd.ExecuteReader();
                //while (sr.Read())
                //{
                    //retr += $"{sr.GetString(0)}{sr.GetInt32(1).ToString()}";
                    //Console.WriteLine($"{sr.GetString(0)} {sr.GetInt32(1).ToString()}");
                //}
                while (sr.Read())
                {
                    
                    retr = sr["gm"].ToString();
                }
                //retr = sr.GetString(1);
                sr.Close();
            }
            cn.Close();
            return retr;
        }
    }
}
