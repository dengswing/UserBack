using System;
using System.Security.Cryptography;
using System.Text;

namespace Networks.tool
{
    public class BaseBytes
    {
        public static string StringToBase64(string value)
        {
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(value);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            return strPath;
        }

        public static string Base64ToString(string strPath)
        {
            byte[] bpath = Convert.FromBase64String(strPath);
            strPath = System.Text.ASCIIEncoding.Default.GetString(bpath);
            return strPath;
        }

        public static Object HashHmac(string signatureString, string secretKey, bool raw_output = false)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(signatureString);
            if (raw_output)
            {
                return hmac.ComputeHash(buffer);
            }
            else
            {
                return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
            }
        }

        public static string ToBase64StringData(object value, bool raw_output = false)
        {
            if (raw_output)
            {
                return Convert.ToBase64String((byte[])value);
            }
            else
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes((String)value));
            }
        }

        public static string EscapeUriString(string str)
        {
            return Uri.EscapeUriString(str);
        }

        public static string EscapeDataString(string str)
        {
            return Uri.EscapeDataString(str);
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}