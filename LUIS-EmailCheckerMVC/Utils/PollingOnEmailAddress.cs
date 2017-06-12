using EAGetMail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LUIS_EmailCheckerMVC.Utils
{
    public class PollingOnEmailAddress
    {
        public static string[] DownloadEmail(string pop3Server, string email, string password, bool needSSL)
        {
            string[] retString = new string[2];
            // Create a folder named "inbox" under current directory
            // to save the email retrieved.
            string curpath = HttpContext.Current.Server.MapPath("~");
            string mailbox = string.Format("{0}inbox", curpath);

            // If the folder is not existed, create it.
            if (!Directory.Exists(mailbox))
                Directory.CreateDirectory(mailbox);

            MailServer mailServer = new MailServer(pop3Server,
                email, password, ServerProtocol.Pop3);
            MailClient mailClient = new MailClient("TryIt");

            // If your POP3 server requires SSL connection,
            // Please add the following codes:
            if (needSSL)
            {
                mailServer.SSLConnection = true;
                mailServer.Port = 995;
            }

            try
            {
                mailClient.Connect(mailServer);
                MailInfo[] infos = mailClient.GetMailInfos();

                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];

                    // Receive email from POP3 server
                    Mail mail = mailClient.GetMail(info);

                    // Generate an email file name based on date time.
                    System.Globalization.CultureInfo cur = new
                        System.Globalization.CultureInfo("it-IT");
                    string subject = mail.Subject;

                    if (subject.Length > 70)
                        subject = subject.Substring(0, 70);

                    string fileName = string.Format("{0}\\{1}.eml",
                        mailbox, subject);

                    fileName = fileName.Replace("(Trial Version)", "");

                    // Save email to local disk
                    mail.SaveAs(fileName.Trim(), true);

                    // Mark email as deleted from POP3 server.
                    mailClient.Delete(info);

                    retString[0] = mailbox;
                    retString[1] = subject.Replace("(Trial Version)", "");
                }

                // Quit and purge emails marked as deleted from POP3 server.
                mailClient.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retString;
        }
    }
}