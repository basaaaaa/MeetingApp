using MeetingApp.Models.Param;
using MeetingApp.Utils;

namespace MeetingApp.Models.Validate
{
    public class SignUpValidation
    {
        CheckString _checkString = new CheckString();
        public SignUpParam InputValidate(string userId, string password)
        {
            SignUpParam signUpParam = new SignUpParam();

            //入力されたuserIdが空かどうかチェック
            if (string.IsNullOrEmpty(userId))
            {
                //存在していた場合POSTを失敗で終了
                signUpParam.HasError = true;
                signUpParam.BlankUserId = true;
                return signUpParam;
            }

            //入力されたpasswordが空かどうかチェック
            if (string.IsNullOrEmpty(password))
            {
                //存在していた場合POSTを失敗で終了
                signUpParam.HasError = true;
                signUpParam.BlankPassword = true;
                return signUpParam;
            }

            //入力されたパスワードが指定文字数を満たしているかどうかチェック
            if (password.Length < 6)

            {
                //存在していた場合POSTを失敗で終了
                signUpParam.HasError = true;
                signUpParam.ShortPassword = true;
                return signUpParam;
            }

            //入力されたuserIdが半角英数字のみで構成されているかチェック
            if (!_checkString.isAlphaNumericPlusAlphaOnly(userId))
            {
                signUpParam.HasError = true;
                signUpParam.UnSpecifiedUserId = true;
                return signUpParam;
            }

            //passwordポリシーに適しているかどうかチェック
            if (!_checkString.isAlphaNumericPlusAlphaOnly(password))
            {
                signUpParam.HasError = true;
                signUpParam.UnSpecifiedPassword = true;
                return signUpParam;
            }

            signUpParam.IsSuccessed = true;

            return signUpParam;
        }
    }
}
