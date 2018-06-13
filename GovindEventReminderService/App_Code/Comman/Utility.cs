using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Text;
using System.Security.Cryptography;

using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace GovindEventReminderService.App_Code.Comman
{

    public class Utility
    {
        
        #region Encription
        public static string Encript(string plainText, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[]
                {
                    0x49,0x76,0x61,0x6e,0x20,0x4d,0x65,0x64,0x76,0x65,0x64,0x65,0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    plainText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return plainText;
        }
        #endregion

        #region' decription
        public static string Decript(string encriptText, string decriptionKey)
        {
            //byte[] encriptBytes = Encoding.Unicode.GetBytes(encriptText);
            byte[] encriptBytes = Convert.FromBase64String(encriptText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(decriptionKey, new byte[]
                {
                    0x49,0x76,0x61,0x6e,0x20,0x4d,0x65,0x64,0x76,0x65,0x64,0x65,0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encriptBytes, 0, encriptBytes.Length);
                        cs.Close();
                    }
                    encriptText =Encoding.Unicode.GetString(ms.ToArray()); 
                }
            }
            return encriptText;
        }

        #endregion

        #region Mailing code
        public static void SendMail(string subject,string to,string body,bool isBodyHtml)
        {
            MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
            string[] toMails = to.Split(',');
            foreach(string toMail in toMails)
            {
                objMailMessage.To.Add(new MailAddress(toMail));
            }
            objMailMessage.Subject = subject;
            objMailMessage.Body = body;
            objMailMessage.IsBodyHtml = isBodyHtml;
            objMailMessage.Priority = MailPriority.High;

            SmtpClient objSmtpClient = new SmtpClient();
            objSmtpClient.Host = ConfigurationManager.AppSettings["Host"].ToString();
            objSmtpClient.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            NetworkCredential objNetworkCredential = new NetworkCredential(ConfigurationManager.AppSettings["From"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());

            objSmtpClient.Credentials = objNetworkCredential;
            objSmtpClient.EnableSsl = true;
            objSmtpClient.Send(objMailMessage);
        }
        public static void SendEmail(string displayName ,string subject, string to, string body, bool isBodyHtml,HttpPostedFileBase[] fileattaches)
        {
            MailMessage objMailMessage = new MailMessage();
        objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
            string[] toMails = to.Split(',');
            foreach(string toMail in toMails)
            {
                objMailMessage.To.Add(new MailAddress(toMail));
            }
    objMailMessage.Subject = subject;
            objMailMessage.Body = body;
            objMailMessage.IsBodyHtml = isBodyHtml;
            objMailMessage.Priority = MailPriority.High;
            if (fileattaches != null)
            {
                foreach (HttpPostedFileBase file in fileattaches)
                { 
                    if (file != null)
                    {
                        objMailMessage.Attachments.Add(new Attachment(file.InputStream, file.FileName));
                    }
            }
            }

            SmtpClient objSmtpClient = new SmtpClient();
    objSmtpClient.Host = ConfigurationManager.AppSettings["Host"].ToString();
    objSmtpClient.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            NetworkCredential objNetworkCredential = new NetworkCredential(ConfigurationManager.AppSettings["From"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());

          
    objSmtpClient.Credentials = objNetworkCredential;
            objSmtpClient.EnableSsl = true;
            objSmtpClient.Send(objMailMessage);
        }
        #endregion
    }
}