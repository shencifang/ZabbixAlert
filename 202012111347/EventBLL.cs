using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;
using System.Data;

namespace _202012111347
{
    public partial class EventBLL
    {
        private readonly _202012111347.EventDAL dal = new _202012111347.EventDAL();
        public EventBLL()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(_202012111347.Event model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<_202012111347.Event> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<_202012111347.Event> DataTableToList(DataTable dt)
        {
            List<_202012111347.Event> modelList = new List<_202012111347.Event>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                _202012111347.Event model;
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
