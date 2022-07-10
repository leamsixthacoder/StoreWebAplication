using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
namespace BussinesLayer
{
    public class BL_Resources
    {

        public static string GeneratedPass()
        {
            string password = Guid.NewGuid().ToString("N").Substring(0,8);
            return password;
        }
        public static string ConvertSha256(string text)
        {
            StringBuilder SB = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach(byte b in result)
                    SB.Append(b.ToString("x2"));
            }
            return SB.ToString();
        }

        public static bool SendMail(string mail, string subject, string message)
        {
            bool result = false;

            try
            {

                MailMessage email = new MailMessage();
                email.To.Add(mail);
                email.From = new MailAddress("leamsi0735@gmail.com");
                email.Subject = subject;
                email.Body = message;
                email.IsBodyHtml = true;


                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("leamsi0735@gmail.com", "orivuqxvrtyqpffx"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(email);
                result = true;

            }catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public static string ConvertBase64(string route, out bool convertion)
        {
            string textBase64 = string.Empty;
            convertion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(route);
                textBase64 = Convert.ToBase64String(bytes);
            }

            catch
            {
                convertion = false;
            }

            return textBase64;
        }
      

    }
}
