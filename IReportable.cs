using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public interface IReportable
    {
        double CalculatePerformanceRating();
        string GetInfo();
    }
}
