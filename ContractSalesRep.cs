using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public class ContractSalesRep : SalesPerson
    {
        // Contract-related properties integrated directly into ContractSalesRep
        public DateTime ReceiptDate { get; set; }
        public TimeSpan ExpectedDuration { get; set; }
        public DateTime? CompletionDate { get; set; }

        // Constructor - delegates initialization to base SalesPerson and initializes contract properties
        public ContractSalesRep(string name, string department, DateTime receiptDate, TimeSpan expectedDuration) : base(name, department)
        {
            ReceiptDate = receiptDate;
            ExpectedDuration = expectedDuration;
            CompletionDate = null;
        }

        // Contract-related properties
        public bool IsCompleted => CompletionDate.HasValue;

        public DateTime ExpectedCompletionDate => ReceiptDate + ExpectedDuration;

        public bool IsCompletedOnTime
        {
            get
            {
                if (!IsCompleted) return false;
                return CompletionDate.HasValue && CompletionDate.Value.Date <= ExpectedCompletionDate.Date;
            }
        }

        public int DaysOverdue
        {
            get
            {
                if (!IsCompleted || IsCompletedOnTime) return 0;
                return CompletionDate.HasValue ? (CompletionDate.Value - ExpectedCompletionDate).Days : 0;
            }
        }

        public void MarkAsCompleted(DateTime completionDate)
        {
            CompletionDate = completionDate;
        }

        // For contract sales reps, performance rating is based on contract completion and sales figures
        public override double CalculatePerformanceRating()
        {
            double performance = 0;

            if (SalesFigures.Count > 0)
            {
                // Sort descending and average the top two sales figures
                var sortedFigures = new List<double>(SalesFigures);
                sortedFigures.Sort();
                sortedFigures.Reverse();

                int count = sortedFigures.Count >= 2 ? 2 : sortedFigures.Count;
                double sum = 0;

                for (int i = 0; i < count; i++)
                {
                    sum += sortedFigures[i];
                }

                CheckAndRaiseAlerts(performance);
                performance = sum / count;
            }

            // Apply contract completion bonus/penalty
            if (IsCompleted)
            {
                if (IsCompletedOnTime)
                {
                    performance *= 1.2; // 12% bonus for on-time completion
                }
                else
                {
                    double penaltyFactor = 1.0 - (DaysOverdue * 0.05); // 5% penalty per day overdue
                    performance *= Math.Max(0.5, penaltyFactor); // Minimum 50% of original performance
                }
            }
            else
            {
                performance *= 0.8; // 8% penalty if contract not completed
            }

            CheckAndRaiseAlerts(performance);
            return performance;
        }

        // Return the top N sales figures for this sales rep
        public List<double> GetBestPerformancePeriods(int n)
        {
            var copy = new List<double>(SalesFigures);
            copy.Sort();
            copy.Reverse();
            if (n <= 0 || copy.Count == 0) return new List<double>();
            if (n >= copy.Count) return copy;
            return copy.GetRange(0, n);
        }

        // Simple helper that identifies the sales type
        public string GetSalesType()
        {
            return "Contract";
        }

        // Count how many sales figures are currently recorded
        public int BestPerformanceCount()
        {
            return SalesFigures == null ? 0 : SalesFigures.Count;
        }

        // Get contract status information
        public string GetContractStatus()
        {
            if (!IsCompleted)
                return $"In Progress - Expected completion: {ExpectedCompletionDate.ToShortDateString()}";
            else if (IsCompletedOnTime)
                return $"Completed On Time - Completed: {CompletionDate!.Value.ToShortDateString()}";
            else
                return $"Completed Late - {DaysOverdue} days overdue, Completed: {CompletionDate!.Value.ToShortDateString()}";
        }
    }
}