using System.Text.RegularExpressions;

namespace MeetingApp.Utils
{
    public class CheckString
    {
        public bool isAlphaNumericPlusAlphaOnly(string s)
        {

            return Regex.IsMatch(s, @"^[0-9a-zA-Z]+[ -~]+$");
        }
    }
}
