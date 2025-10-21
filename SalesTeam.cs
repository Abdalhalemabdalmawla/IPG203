using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public class SalesTeam
    {
        private List<SalesPerson> _salesPeople = new List<SalesPerson>();

        public List<SalesPerson> SalesPeople
        {
            get { return _salesPeople; }
        }

        public void AddSalesPerson(SalesPerson s)
        {
            _salesPeople.Add(s);
            s.PerformanceBelowThreshold += OnPerformanceAlert;
        }

        // Remove sales person by ID, returns true if removed
        public bool RemoveSalesPersonById(int id)
        {
            for (int i = 0; i < _salesPeople.Count; i++)
            {
                if (_salesPeople[i].Id == id)
                {
                    _salesPeople[i].PerformanceBelowThreshold -= OnPerformanceAlert;
                    _salesPeople.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        // Remove sales people by exact name, returns how many were removed
        public int RemoveSalesPersonByName(string name)
        {
            int removed = 0;
            for (int i = _salesPeople.Count - 1; i >= 0; i--)
            {
                if (_salesPeople[i].Name == name)
                {
                    _salesPeople[i].PerformanceBelowThreshold -= OnPerformanceAlert;
                    _salesPeople.RemoveAt(i);
                    removed++;
                }
            }
            return removed;
        }

        // Return a simple summary string for the sales team
        public string GetSummary()
        {
            return "Sales People: " + Count + ", Average Performance: " + GetAveragePerformance().ToString("0.00");
        }

        private double GetAveragePerformance()
        {
            if (_salesPeople.Count == 0) return 0.0;
            double sum = 0;
            for (int i = 0; i < _salesPeople.Count; i++) sum += _salesPeople[i].CalculatePerformanceRating();
            return sum / _salesPeople.Count;
        }

        // Find sales people by name (exact match) and return list
        public List<SalesPerson> FindSalesPersonByName(string name)
        {
            var result = new List<SalesPerson>();
            for (int i = 0; i < _salesPeople.Count; i++)
            {
                if (_salesPeople[i].Name == name)
                {
                    result.Add(_salesPeople[i]);
                }
            }
            return result;
        }

        // Simple count property
        public int Count
        {
            get { return _salesPeople.Count; }
        }

        private void OnPerformanceAlert(SalesPerson sender, string message)
        {
            Console.WriteLine("PERFORMANCE ALERT: " + message);
        }

        public void ProcessAllPerformanceReviews()
        {
            for (int i = 0; i < _salesPeople.Count; i++)
            {
                SalesPerson s = _salesPeople[i];
                double performanceRating = s.CalculatePerformanceRating();
                Console.WriteLine(s.GetInfo() + " | Performance Rating: " + performanceRating);
            }
        }

        public static int TotalSalesPeopleAcrossSystem
        {
            get { return SalesPerson.TotalSalesPeople; }
        }
    }
}
