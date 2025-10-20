using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public class PremiumSalesRep : SalesPerson
    {
        // Commission amount in dollars (simple field)
        public double CommissionAmount { get; private set; }

        public PremiumSalesRep(string name, string department) : base(name, department)
        {
            CommissionAmount = 0.0;
        }

        //  evaluation system for premium sales rep based on average sales with simple bonus
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

                //  premium employees: 20% performance bonus
                performance *= 1.2;
            }

            CheckAndRaiseAlerts(performance);
            return performance;
        }

        // Calculate commission based on percentage of each sales figure
        public bool EvaluateCommission(double threshold)
        {
            double performance = CalculatePerformanceRating();

            if (SalesFigures.Count == 0)
            {
                CommissionAmount = 0.0;
                return false;
            }

            double totalCommission = 0.0;
            double commissionRate;

            if (performance >= threshold)
            {
                commissionRate = 0.25; // 25% commission for high performers
            }
            else
            {
                commissionRate = 0.15; // 15% commission for others
            }

            // Calculate commission for each sales figure
            foreach (double salesFigure in SalesFigures)
            {
                totalCommission += salesFigure * commissionRate;
            }

            CommissionAmount = totalCommission;
            return performance >= threshold;
        }


        public string GetSalesType()
        {
            return "Premium";
        }
    }
}
