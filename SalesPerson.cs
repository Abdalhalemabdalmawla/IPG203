using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public delegate void PerformanceAlertHandler(SalesPerson sender, string message);

    public abstract class SalesPerson : IReportable
    {
        // Static counter for assigning IDs
        private static int _idCounter = 0;
        // Readonly ID assigned at construction
        private readonly int _id;
        private string _name = string.Empty;
        private List<double> _salesFigures = new List<double>();
        private readonly string _department = string.Empty;

        // Additional classic fields
        private string _email = string.Empty;
        private DateTime? _startDate = null;



        // Static property
        public static int TotalSalesPeople
        {
            get { return _idCounter; }
        }
        // Encapsulated fields exposed via properties
        public int Id
        {
            get
            {
                return _id;
            }
            // No Set Because it's readonly after initialization
        }
        //  email property with basic validation
        public string Email
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Email cannot be null or whitespace.");
                }
                if (!value.Contains("@"))
                {
                    throw new ArgumentException($"Invalid email format: '{value}'. Email must contain '@' symbol.");
                }
                _email = value;
            }
        }

        //  start date
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!DataValidator.IsValidName(value))
                {
                    throw new ArgumentException($"Invalid name: '{value}'. Name must be at least 2 characters long and not null or whitespace.");
                }
                _name = value;
            }
        }

        public List<double> SalesFigures
        {
            get { return _salesFigures; }
        }

        public string Department
        {
            get { return _department; }
        }


        protected SalesPerson(string name, string department)
        {
            try
            {
                Name = name; // This will now throw if invalid
                _department = department;
                _idCounter++;
                _id = _idCounter;
                _salesFigures = new List<double>();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Failed to create SalesPerson: {ex.Message}");
                throw; // Re-throw to prevent object creation with invalid state
            }
        }

        public void AddSalesFigure(double salesAmount)
        {
            if (!DataValidator.IsValidSalesAmount(salesAmount))
            {
                throw new ArgumentException($"Invalid sales amount: {salesAmount}. Sales amount must be non-negative.");
            }

            _salesFigures.Add(salesAmount);
        }
        // Abstract method to be implemented by subclasses (inheritance & polymorphism)
        public abstract double CalculatePerformanceRating();

        // Calculate a performance rating scaled to 5.0 based on average sales
       

        // Clear all sales figures for the sales person
        public void ClearSalesFigures()
        {
            _salesFigures.Clear();
        }

        // Number of sales figures currently recorded for the sales person
        public int SalesFigureCount
        {
            get { return SalesFigures == null ? 0 : SalesFigures.Count; }
        }

        // Experience computed from StartDate (returns null if StartDate not set)
        public int? Experience
        {
            get
            {
                if (!StartDate.HasValue) return null;
                var today = DateTime.Today;
                int experience = today.Year - StartDate.Value.Year;
                if (StartDate.Value.Date > today.AddYears(-experience)) experience--;
                return experience;
            }
        }

        // Classic helper: check if the sales person is experienced (>= 5 years)
        public bool IsExperienced()
        {
            var exp = Experience;
            return exp.HasValue && exp.Value >= 5;
        }

        public string GetInfo()
        {
            return "ID: " + Id + ", Name: " + Name + ", Department: " + Department;
        }

        public event PerformanceAlertHandler? PerformanceBelowThreshold;

        protected void CheckAndRaiseAlerts(double performanceRating)
        {
            if (performanceRating < 2.0)
            {
                if (PerformanceBelowThreshold != null)
                {
                    PerformanceBelowThreshold(this, "Performance rating " + performanceRating + " is below threshold for " + Name);
                }
            }
        }
       
    }
}



