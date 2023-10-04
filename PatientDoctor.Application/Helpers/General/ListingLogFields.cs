using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Helpers.General
{
    public class ListingLogFields
    {
        public int TotalCount { get; set; }
        public long SerialNo { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public int? CreatedOn { get; set; }
        public string CreatedDate
        {
            get
            {
                if (this.CreatedOn != null)
                {
                    Int32 dateTime = Convert.ToInt32(CreatedOn.Value);
                    DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(dateTime);
                    return dtDateTime.ToString("MMM dd, yyyy, hh:mm:ss tt", CultureInfo.InvariantCulture);
                }
                else
                {
                    return null;
                }

            }
        }
    }

}
