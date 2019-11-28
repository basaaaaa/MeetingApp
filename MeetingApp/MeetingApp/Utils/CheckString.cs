using System.Text.RegularExpressions;

namespace MeetingApp.Utils
{
    public class CheckString
    {
        /// <summary>
        /// ���p�p�����Ɣ��p�L���݂̂ō\������Ă��邩�`�F�b�N
        /// </summary>
        /// <param name="s">�`�F�b�N�Ώۂ̕�����</param>
        /// <returns>�����𖞂����Ă���ΐ^�A�ᔽ�������܂܂�Ă���΋U</returns>
        public bool isAlphaNumericPlusAlphaOnly(string s)
        {

            return Regex.IsMatch(s, @"^[0-9a-zA-Z]+[ -~]+$");
        }
    }
}
