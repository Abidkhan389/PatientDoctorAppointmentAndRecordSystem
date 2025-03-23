using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class VM_PatientCountYearWise
    {
        public string Name { get; set; } // Represents the year
        public List<decimal> Data { get; set; } // Represents the series data for the year
    }
    public class RevenueForCastMonthly
    {
        public int Month { get; set; } // Month number (1 = January, 12 = December)
        public decimal TotalAmount { get; set; } // Sum of the Amount for the month
    }
}
