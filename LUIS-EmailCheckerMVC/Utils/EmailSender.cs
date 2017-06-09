using System;
using System.IO;
using System.Net.Mail;

namespace LUIS_EmailCheckerMVC.Utils
{
    public class EmailSender
    {
        public static bool SendEmail(string email, string password, string name, string subject, string emailBody)
        {
            try
            {
                //porta 465 perchè gmail usa SSL
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(email, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                //Setting From, To and CC
                mail.From = new MailAddress(email, name);
                mail.To.Add(new MailAddress("geppi.alessandro@gmail.com"));
                mail.Body = emailBody;
                mail.Subject = subject;
                //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool DeleteFileAfterSend(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}