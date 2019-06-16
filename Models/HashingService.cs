using System;
using System.Text;

namespace ScheduleBuilder.Models
{
    public class HashingService
    {
        //converts string passwords to appropriate hashed string
        //Developed using tutorial at https://youtu.be/AU-4oLUV5VU
        public String PasswordHashing(string password)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            System.Security.Cryptography.SHA256Managed sha256string =
                new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256string.ComputeHash(bytes);

            return ByteArrayToHexString(hash);
        }

        //converts hashed passwords to appropriate string
        //Developed using tutorial at https://youtu.be/AU-4oLUV5VU
        private string ByteArrayToHexString(byte[] ba)
        {
            System.Text.StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}