/*using System.Text.RegularExpressions;
using WaterCity.Core.EnumCore;

namespace WaterCity.Core.Utils
{
    public class StrHelper
    {
        //only work with NMCT0000007 or NMCT0000007_1 format
        public static string IncrementNumericSuffix(string input)
        {
            // Split the input string by underscores
            string[] parts = input.Split('_');

            if (parts.Length == 1)
            {
                // Case 1: If there's no underscore in the input, add "_1" to the end
                return input + "_1";
            }
            else if (parts.Length == 2)
            {
                // Case 2: If there's one underscore in the input, try to parse and increment the numeric part
                if (int.TryParse(parts[1], out int numericPart))
                {
                    numericPart++; // Increment the numeric part by 1
                    return $"{parts[0]}_{numericPart}";
                }
            }

            // If the input doesn't match either case, return it as is
            return input;
        }

        public static string IncrementChiSoNumber(string input)
        {
            // Define a regular expression pattern to extract the numeric part
            Regex regex = new Regex(@"_(\d+)$");

            // Match the numeric part at the end of the input string
            Match match = regex.Match(input);

            if (match.Success)
            {
                // Extract the numeric part and convert it to an integer
                int currentNumber = int.Parse(match.Groups[1].Value);

                // Increment the numeric part
                int newNumber = currentNumber + 1;

                // Replace the old numeric part with the incremented value
                string newInput = regex.Replace(input, $"_{newNumber}");

                return newInput;
            }
            else
            {
                // If no numeric part found, append "_1" to the input string
                return input + "_1";
            }
        }

    }
}*/