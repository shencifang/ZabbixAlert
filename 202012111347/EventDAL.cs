using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Maticsoft.DBUtility;
using MySql.Data.MySqlClient;

namespace _202012111347
{
    public partial class EventDAL
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");

        public EventDAL()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(_202012111347.Event model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into event(");
            strSql.Append("id,time,ip,content,gm,recetime,close,closetime)");
            strSql.Append(" values (");
            strSql.Append("@id,@time,@ip,@content,@gm,@recetime,@close,@closetime)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@id", MySqlDbType.Int32,8),
                    new MySqlParameter("@time", MySqlDbType.DateTime,255),
                    new MySqlParameter("@ip", MySqlDbType.VarChar,255),
                    new MySqlParameter("@content", MySqlDbType.VarChar,255),
                    new MySqlParameter("@gm", MySqlDbType.VarChar,255),
                    new MySqlParameter("@recetime", MySqlDbType.DateTime,255),
                    new MySqlParameter("@close", MySqlDbType.Double,255),
                    new MySqlParameter("@closetime", MySqlDbType.DateTime,255)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.time;
            parameters[2].Value = model.ip;
            parameters[3].Value = model.content;
            parameters[4].Value = model.gm;
            parameters[5].Value = model.recetime;
            parameters[6].Value = model.close;
            parameters[7].Value = model.closetime;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public _202012111347.Event DataRowToModel(DataRow row)
        {
            _202012111347.Event model = new _202012111347.Event();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row["time"] != null && row["time"].ToString() != "")
                {
                    model.time = Convert.ToDateTime(row["time"]);
                }
                if (row["content"] != null && row["content"].ToString() != "")
                {
                    model.content = row["content"].ToString();
                }
                if (row["ip"] != null)
                {
                    model.ip = row["ip"].ToString();
                }
                if (row["gm"] != null && row["gm"].ToString() != "")
                {
                    model.gm = row["gm"].ToString();
                }
                if (row["recetime"] != null && row["recetime"].ToString() != "")
                {
                    model.recetime = Convert.ToDateTime(row["recetime"]);
                }
                if (row["close"] != null && row["close"].ToString() != "")
                {
                    model.close = Convert.ToDecimal(row["close"].ToString());
                }
                if (row["closetime"] != null && row["closetime"].ToString() != "")
                {
                    model.closetime = Convert.ToDateTime(row["closetime"]);
                }
            }
            return model;
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,time,ip,content,gm,recetime,close,closetime ");
            strSql.Append(" FROM event ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            log.log(strSql.ToString());
            return DbHelperMySQL.Query(strSql.ToString());
        }


        #endregion  成员方法

    }
}