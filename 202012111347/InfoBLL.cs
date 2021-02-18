using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;
using System.Data;

namespace _202012111347
{
    public partial class InfoBLL
    {
        private readonly _202012111347.InfoDAL dal = new _202012111347.InfoDAL();
        public InfoBLL()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(_202012111347.Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public _202012111347.Info GetModel(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.GetModel(strWhere);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<_202012111347.Info> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<_202012111347.Info> DataTableToList(DataTable dt)
        {
            List<_202012111347.Info> modelList = new List<_202012111347.Info>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                _202012111347.Info model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        #endregion  成员方法
    }
}
