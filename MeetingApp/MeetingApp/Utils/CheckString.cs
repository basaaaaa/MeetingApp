using System.Text.RegularExpressions;

namespace MeetingApp.Utils
{
    public class CheckString
    {
        /// <summary>
        /// 半角英数字と半角記号のみで構成されているかチェック
        /// </summary>
        /// <param name="s">チェック対象の文字列</param>
        /// <returns>条件を満たしていれば真、違反文字が含まれていれば偽</returns>
        public bool isAlphaNumericPlusAlphaOnly(string s)
        {

            return Regex.IsMatch(s, @"^[0-9a-zA-Z]+[ -~]+$");
        }
    }
}
