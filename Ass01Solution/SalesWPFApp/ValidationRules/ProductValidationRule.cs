using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DataAccess.Models;
namespace SalesWPFApp.ValidationRules
{
    public class ProductValidationRule
    {
        public static List<string> ValidateProductName(string value)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(value)) errors.Add("Please enter Product Name field.");
            else if (value.Length > 40) errors.Add("length of product name doesn't over 40 character.");

            return errors;
        }

        public static List<string> ValidateWeigt(string value)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(value)) errors.Add("Please enter Weight.");
            else if (value.Length > 20) errors.Add("length of weight doesn't over 20 character.");

            return errors;
        }
        public static List<string> ValidateUnitPrice(decimal? value)
        {
            var errors = new List<string>();
            if (value == null) errors.Add("Please enter Price field");
            else if (value <= 0) errors.Add("Price is not in correct format.");

            return errors;
        }

        public static List<string> ValidateUnitsInStockValidate(int? value)
        {
            var errors = new List<string>();
            if (value == null) errors.Add("Please enter Units In Stock");
            else if (value < 0) errors.Add("Price is not in correct format.");

            return errors;
        }

    }
}
