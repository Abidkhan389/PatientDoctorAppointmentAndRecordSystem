using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Helpers.General
{
    public class TableParam
    {
        public int Start { get; set; }
        public int Limit { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public int LimitEx
        {
            get
            {
                return this.Limit == 0 ? 10 : this.Limit;
            }
        }
        public string SortEx
        {
            get
            {
                string ret = this.Sort;
                if (String.IsNullOrEmpty(ret) || String.IsNullOrEmpty(ret))
                {
                    ret = "CreatedOn";
                }
                ret = ret == "createdDate" || ret == "serialNo" ? "CreatedOn" : ret;
                ret = $"{ret[0].ToString().ToUpper()}{ret.Substring(1)}";
                return ret;
            }
        }
        public string OrderEx
        {
            get
            {
                return String.IsNullOrEmpty(this.Order) || String.IsNullOrEmpty(this.Sort) ? "desc" : this.Order;
            }
        }
    }
    
}
