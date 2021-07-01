using System.Text.RegularExpressions;


namespace TestForItMonitoring.Helpers
{
    public class Validation
    {
        static public bool IsNumber(string num)
        {
            bool result = Regex.IsMatch(num, @"\-?[0-9]+$");            
            return result;
        }
    }
}
