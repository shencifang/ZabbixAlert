using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _202012111347
{
    /// <summary>
    /// 实体类Info (属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Info
    {
        public Info()
        { }
        #region Model
        private string _hostname = null;
        private string _gm = null;

        /// <summary>
        /// 
        /// </summary>
        public string hostname
        {
            set { _hostname = value; }
            get { return _hostname; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string gm
        {
            set { _gm = value; }
            get { return _gm; }
        }
        #endregion Model

    }
}