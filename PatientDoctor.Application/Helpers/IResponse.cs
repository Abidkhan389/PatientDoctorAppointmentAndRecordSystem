using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Helpers
{
    public interface IResponse
    {
        bool Success { get; set; }
        string Message { get; set; }
        object Data { get; set; }
    }
    public interface ICountResponse
    {
        int TotalCount { get; set; }
        object DataList { get; set; }
    }
}
