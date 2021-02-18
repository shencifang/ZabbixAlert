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
    public partial class InfoDAL
    {
        //#region Singleton Pattern 单例模式
        public InfoDAL()
        { }
        //#endregion

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(_202012111347.Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into info(");
            strSql.Append("hostname,gm)");
            strSql.Append(" values (");
            strSql.Append("@hostname,@gm)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@hostname", MySqlDbType.VarChar,255),
                    new MySqlParameter("@gm", MySqlDbType.VarChar,255)};
            parameters[0].Value = model.hostname;
            parameters[1].Value = model.gm;

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
        public _202012111347.Info GetModel(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select hostname,gm from info ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            MySqlParameter[] parameters = {
            };

            _202012111347.Info model = new _202012111347.Info();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public _202012111347.Info DataRowToModel(DataRow row)
        {
            _202012111347.Info model = new _202012111347.Info();
            if (row != null)
            {
                if (row["hostname"] != null && row["hostname"].ToString() != "")
                {
                    model.hostname = row["hostname"].ToString();
                }
                if (row["gm"] != null && row["gm"].ToString() != "")
                {
                    model.gm = row["gm"].ToString();
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
            strSql.Append("select hostname,gm ");
            strSql.Append(" FROM info ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }


        #endregion  成员方法

       
    }
}