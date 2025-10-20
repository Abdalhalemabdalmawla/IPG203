using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public class RegularSalesRep : SalesPerson
    {
        // Simple sales target achievement percentage field (0-100)
        public double SalesTargetAchievement { get; set; }

        public RegularSalesRep(string name, string department) : base(name, department)
        {
            SalesTargetAchievement = 100.0;
        }

        // Regular sales reps: performance rating is the simple average of sales figures
        public override double CalculatePerformanceRating()
        {
            double performance = 0;

            if (SalesFigures.Count > 0)
            {
                double sum = 0;
                for (int i = 0; i < SalesFigures.Count; i++)
                {
                    sum += SalesFigures[i];
                }
                performance = sum / SalesFigures.Count;
            }

            CheckAndRaiseAlerts(performance);
            return performance;
        }

        public string GetSalesType()
        {
            return "Regular";
        }
    }
}
