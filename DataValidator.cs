using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPG203_TIC
{
    public static class DataValidator
    {
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
                return false;
            else
                return true;
        }

        // Overload: allow spaces flag (classic implementation)
        public static bool IsValidName(string name, bool allowSpaces)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2) return false;
            if (!allowSpaces && name.Contains(" ")) return false;
            return true;
        }

        public static bool IsValidSalesAmount(double salesAmount)
        {
            if (salesAmount >= 0)
                return true;
            else
                return false;
        }
    }
}
