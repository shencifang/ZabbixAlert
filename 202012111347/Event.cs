using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _202012111347
{
    public class Event
    {
        public Event()
        { }
        #region Model

        private string _id = null;
        private DateTime? _time = null;
        private string _ip = null;
        private string _content = null;
        private string _gm = null;
        private DateTime? _recetime = null;
        private decimal? _close = null;
        private DateTime? _closetime = null;

        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? time
        {
            set { _time = value; }
            get { return _time; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ip
        {
            set { _ip = value; }
            get { return _ip; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string content
        {
            set { _content = value; }
            get { return _content; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string gm
        {
            set { _gm = value; }
            get { return _gm; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? recetime
        {
            set { _recetime = value; }
            get { return _recetime; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal? close
        {
            set { _close = value; }
            get { return _close; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? closetime
        {
            set { _closetime = value; }
            get { return _closetime; }
        }


        #endregion Model


    }
}
