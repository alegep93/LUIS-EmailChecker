using LUIS_EmailCheckerMVC.Models;
using LUIS_EmailCheckerMVC.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace LUIS_EmailCheckerMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(string email, string password)
        {
            int counterEthic, counterGucci;
            Query ret = new Query();
            string line = "";
            try
            {
                if (email != null)
                {
                    int fileNumber = 0;
                    string[] emailReturn = PollingOnEmailAddress.DownloadEmail("pop.gmail.com", email, password, true);
                    string directory = emailReturn[0];
                    string subject = emailReturn[1];

                    while (fileNumber < ReadFile.NumberOfFiles(directory))
                    {
                        counterEthic = counterGucci = 0;
                        string path = ReadFile.ChooseFile(directory, fileNumber);

                        if (path != null)
                        {
                            StreamReader sr = new StreamReader(path);

                            while (!sr.EndOfStream)
                            {
                                line = "";
                                for (int i = 0; i < 10; i++)
                                    line += sr.ReadLine();

                                EmailChecker objLUISResult = await QueryLUIS(line);
                                if (objLUISResult.entities != null)
                                {
                                    foreach (var item in objLUISResult.entities)
                                    {
                                        if (item.type.Contains("Gucci"))
                                            counterGucci++;
                                        else if (item.type.Contains("Ethic") || item.type.Contains("Hotel"))
                                            counterEthic++;
                                        else
                                            break;
                                    }
                                }
                                else
                                    continue;
                            }

                            bool isSend = false;
                            sr.Close();
                            StreamReader sr2 = new StreamReader(path);
                            string emailBody = sr2.ReadToEnd();

                            if (counterGucci > counterEthic)
                            {
                                ret.FirmaCalce = "Gucci, girare a Dimitri";
                                isSend = EmailSender.SendEmail(email, password, "Gucci", subject, emailBody);
                            }
                            else if (counterGucci < counterEthic)
                            {
                                ret.FirmaCalce = "EthicHotel, girare a Antonio";
                                isSend = EmailSender.SendEmail(email, password, "EthicHotel", subject, emailBody);
                            }
                            else
                            {
                                ret.FirmaCalce = "Mail non proveninente da Gucci o da Ethic";
                                EmailSender.DeleteFileAfterSend(path);
                            }

                            sr2.Close();

                            if (isSend)
                                EmailSender.DeleteFileAfterSend(path);

                            fileNumber++;
                        }else
                        {
                            ModelState.AddModelError(string.Empty, "Non è stato possibile trovare il file della mail");
                        }
                    }
                }
                return View(ret);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View(ret);
            }
        }

        private static async Task<EmailChecker> QueryLUIS(string Query)
        {
            EmailChecker LUISResult = new EmailChecker();
            var LUISQuery = Uri.EscapeDataString(Query);
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                // Get key values from the web.config
                string LUIS_Url = WebConfigurationManager.AppSettings["LUIS_Url"];
                string LUIS_Id = WebConfigurationManager.AppSettings["LUIS_Id"];
                string LUIS_Subscription_Key = WebConfigurationManager.AppSettings["LUIS_Subscription_Key"];
                string RequestURI = string.Format("{0}/{1}?subscription-key={2}&verbose=true&timezoneOffset=0&q={3}",
                                    LUIS_Url, LUIS_Id, LUIS_Subscription_Key, LUISQuery);

                System.Net.Http.HttpResponseMessage msg = await client.GetAsync(RequestURI);
                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    LUISResult = JsonConvert.DeserializeObject<EmailChecker>(JsonDataResponse);
                }
            }
            return LUISResult;
        }
    }
}