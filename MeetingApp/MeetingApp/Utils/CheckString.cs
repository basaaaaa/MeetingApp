using System.Text.RegularExpressions;

namespace MeetingApp.Utils
{
    class CheckString
    {
        public bool isAlphaNumericPlusAlphaOnly(string s)
        {
            //許可する文字以外のマッチがあるかどうかを判定
            if (Regex.IsMatch(s, @"[^a-zA-z0-9-_]"))
            {
                //マッチすれば使用不可能文字が含まれている
                return true;
            }

            return false;
        }
    }
}
