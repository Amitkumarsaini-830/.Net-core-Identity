using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace com.aadviktech.IMS.Constant
{
    public static class ApplicationGlobal
    {
        public static string AppName = "DrCrEntry";
        public static string AppEmailId = "ims.system@aadviktech.com";
        public static string AppWebsiteUrl = "www.ims.net.in";
        public static bool activeRecForword = false;
        public static decimal delayRecForword = 3;
        public static string activeSmsApi = string.Empty;

        public static decimal delayFactor = 0;
        public static string GetUniqueKey(int maxSize = 8)
        {
            // whatever length you want
            string a;
            a = "123456789";
            char[] chars = new char[a.Length];
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }

        //public static string GetUniqueKeyString(int maxSize = 20)
        //{
        //    char[] chars = new char[62];
        //    chars =
        //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        //    byte[] data = new byte[1];
        //    using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
        //    {
        //        crypto.GetNonZeroBytes(data);
        //        data = new byte[maxSize];
        //        crypto.GetNonZeroBytes(data);
        //    }
        //    StringBuilder result = new StringBuilder(maxSize);
        //    foreach (byte b in data)
        //    {
        //        result.Append(chars[b % (chars.Length)]);
        //    }
        //    return result.ToString();
        //}

        public static int GetNextDelayTime()
        {
            if (delayFactor > 10)
                delayFactor = 0;
            else delayFactor = delayFactor + Convert.ToDecimal(0.2);
            return Convert.ToInt32(100 * delayFactor);
        }

        //public static string AppUrl { get { return Configurationst.GetSection("ConnectionStrings").ToString(); } }
        //public static string OperatorImgPath { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["OperatorImgPath"].ToString(); } }
        //public static string NewsImgPath { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["NewsImgPath"].ToString(); } }
        //public static string TempFolderPath { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["TempFolderPath"].ToString(); } }
        //public static string HelpNumber { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["HelpMobileNo"].ToString(); } }

        //public static string ClientIPAddress
        //{
        //    get
        //    {
        //        //  return HttpContext.Current.Request.ServerVariables["Remote_Addr"].ToString();
        //        return HttpContext.Current.Request.UserHostAddress;
        //    }
        //}
        public static string Message_Warning(string Heading, string Message)
        {
            return "<div style='margin-bottom:0px;' class='alert alert-warning alert-dismissable'><button class='close' type='button' data-dismiss='alert' aria-hidden='true'>×</button>" + Message + "</div>";
        }
        public static string Message_Success(string Heading, string Message)
        {
            return "<div style='margin-bottom:0px;' class='alert alert-success alert-dismissable'><button class='close' type='button' data-dismiss='alert' aria-hidden='true'>×</button>" + Message + ".</div>";
        }
        public static string Message_Information(string Heading, string Message)
        {
            return "<div style='margin-bottom:0px;' class='alert alert-info alert-dismissable'><button class='close' type='button' data-dismiss='alert' aria-hidden='true'>×</button>" + Message + ". <span class='alert-link'>" + Heading + "</span>.</div>";
        }

        public static string GetRandomPassword(int maxSize = 8)
        {
            // whatever length you want
            string pwd = GetUniqueKeyString(2).ToUpper() + GetUniqueKey(4) + GetUniqueKeyString(2).ToLower() + GetUniqueKeySpecial(2);
            return StringMixer(pwd);
        }

        #region PasswordCreator
        static void Fisher_Yates(int[] array)
        {
            System.Random rnd = new System.Random();

            int arraysize = array.Length;
            int random;
            int temp;

            for (int i = 0; i < arraysize; i++)
            {
                random = i + (int)(rnd.NextDouble() * (arraysize - i));

                temp = array[random];
                array[random] = array[i];
                array[i] = temp;
            }
        }

        public static string StringMixer(string s)
        {
            string output = "";
            int arraysize = s.Length;
            int[] randomArray = new int[arraysize];

            for (int i = 0; i < arraysize; i++)
            {
                randomArray[i] = i;
            }

            Fisher_Yates(randomArray);

            for (int i = 0; i < arraysize; i++)
            {
                output += s[randomArray[i]];
            }

            return output;
        }

        public static string GetUniqueKeyString(int maxSize = 20)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public static string GetUniqueKeySpecial(int maxSize = 6)
        {
            char[] chars = new char[62];
            chars =
            "!@#$*&%".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        #endregion

        // create comseparate String
        //public static ReceiveUserString(string source)
        //{
        //    string[] UserString = source.Split(',');

        //    var User = new User();
        //    foreach (string u in UserString)
        //    {
        //        User.Username = u[0];
        //        User.Password = u[1];
        //    }
        //}
    }
}
