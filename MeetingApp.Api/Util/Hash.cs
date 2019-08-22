using System;

namespace MeetingApp.Api.Util
{
    public class Hash
    {
        public String encrypt(string password)
        {
            byte[] bytedPassword = System.Text.Encoding.UTF8.GetBytes(password);
            var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            byte[] sha256edPassword = sha256.ComputeHash(bytedPassword);

            sha256.Clear();

            // byte型配列を16進数の文字列に変換
            System.Text.StringBuilder hashedPassword = new System.Text.StringBuilder();

            foreach (byte b in sha256edPassword)
            {
                hashedPassword.Append(b.ToString("x2"));
            }

            password = hashedPassword.ToString();

            return password;
        }
    }
}
