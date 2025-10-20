using IPG203_TIC;

namespace MyIPG203_F24_HW
{
    internal class Program
    {
        static void Main(string[] args)
        {

                Console.WriteLine("Sales Management System :");

                SalesTeam team = new SalesTeam();

                // Regular sales rep: set email and start date, add sales figures
                RegularSalesRep s1 = new RegularSalesRep("Ali", "Electronics");
                s1.Email = "ali@salescompany.com";
                s1.StartDate = new DateTime(2020, 5, 12);
                s1.AddSalesFigure(50000);
                s1.AddSalesFigure(55000);

                // Premium sales rep: demonstrate EvaluateCommission and IsCommissionEligible
                PremiumSalesRep s2 = new PremiumSalesRep("Sara", "Software");
                s2.Email = "sara@salescompany.com";
                s2.StartDate = new DateTime(2019, 3, 2);
                s2.AddSalesFigure(80000);
                s2.AddSalesFigure(90000);
                s2.AddSalesFigure(85000);
                s2.AddSalesFigure(70000);

                // Contract sales rep: add several sales figures and show contract management
                ContractSalesRep s3 = new ContractSalesRep("Khaled", "Hardware", new DateTime(2018, 8, 20), TimeSpan.FromDays(90));
                s3.StartDate = new DateTime(2018, 8, 20);
                s3.AddSalesFigure(45000);
                s3.AddSalesFigure(48000);
                s3.AddSalesFigure(78000);
                s3.AddSalesFigure(88000);
                s3.AddSalesFigure(60000);
                s3.AddSalesFigure(90000);

                // Mark the contract as completed early (on time)
                s3.MarkAsCompleted(new DateTime(2018, 11, 15));

                // Directly attach an event handler to demonstrate events
                s1.PerformanceBelowThreshold += (sender, msg) => Console.WriteLine("[PERFORMANCE ALERT] " + msg);
               s2.PerformanceBelowThreshold += (sender, msg) => Console.WriteLine("[PERFORMANCE ALERT] " + msg);
               s3.PerformanceBelowThreshold += (sender, msg) => Console.WriteLine("[PERFORMANCE ALERT] " + msg);


            // Add to team (SalesTeam also wires events internally)
                team.AddSalesPerson(s1);
                team.AddSalesPerson(s2);
                team.AddSalesPerson(s3);

                // Use properties and helper methods to print details
                Console.WriteLine("\n-- Sales Person Details --");
                foreach (var sp in team.SalesPeople)
                {
                    string info = sp.GetInfo();
                    string salesSummary = sp.SalesFigureCount + " sales figures";
                    string performance = sp.CalculatePerformanceRating().ToString();
                    string experience = sp.StartDate.HasValue ? sp.Experience?.ToString() ?? "-" : "unknown";
                    string expStatus = sp.IsExperienced() ? "Experienced" : "Junior";
                    Console.WriteLine(info + " | " + salesSummary + " | Performance: " + performance + " | Experience: " + experience + " (" + expStatus + ")");
                }

                // Process all to compute and show final performance ratings (and trigger any alerts)
                Console.WriteLine("\n-- Processing performance reviews --");
                team.ProcessAllPerformanceReviews();

                // Commission evaluation example
                Console.WriteLine("\n-- Commission evaluation (Sara) --");
                bool awarded = s2.EvaluateCommission(75000);
                Console.WriteLine("Sara commission eligible: " + awarded + " | Amount: $" + s2.CommissionAmount.ToString("N2"));



                // Contract sales rep best performance periods
                Console.WriteLine("\n-- Contract sales rep best performance periods (Khaled) --");
                Console.WriteLine("Best performance count: " + s3.BestPerformanceCount());
                var top3 = s3.GetBestPerformancePeriods(3);
                Console.WriteLine("Top 3 performance periods: " + string.Join(", ", top3));

                // Demonstrate contract status
                Console.WriteLine("\n-- Contract Status (Khaled) --");
                Console.WriteLine("Contract status: " + s3.GetContractStatus());
                Console.WriteLine("Contract completed: " + s3.IsCompleted);
                Console.WriteLine("Completed on time: " + s3.IsCompletedOnTime);
                Console.WriteLine("Days overdue: " + s3.DaysOverdue);

                // Demonstrate team helpers: summary and remove by name
                Console.WriteLine("\nTeam summary: " + team.GetSummary());
                int removed = team.RemoveSalesPersonByName("Ali");
                Console.WriteLine("Removed by name 'Ali': " + removed + " | New summary: " + team.GetSummary());

                Console.WriteLine("Total Sales People in System: " + SalesTeam.TotalSalesPeopleAcrossSystem);

                Console.WriteLine("=== Program End ===");
            }
        }
    }
