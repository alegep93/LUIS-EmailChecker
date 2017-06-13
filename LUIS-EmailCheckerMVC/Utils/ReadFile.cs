using CDO;
using ADODB;
using System.IO;
using System;

namespace LUIS_EmailCheckerMVC.Utils
{
    public class ReadFile
    {
        public static string ChooseFile(string directory, int fileNumber)
        {
            string dirPath = directory;
            string[] fileNames = Directory.GetFiles(dirPath);
            return fileNames[fileNumber];
        }
        public static int NumberOfFiles(string directory)
        {
            string dirPath = directory;
            string[] fileNames = Directory.GetFiles(dirPath);
            return fileNames.Length;
        }

        //Useless -> NON ho idea di come usarla correttamente
        public static Message ReadMessage(string emlFileName)
        {
            Message msg = new Message();
            ADODB.Stream stream = new ADODB.Stream();
            stream.Open(Type.Missing, ConnectModeEnum.adModeUnknown, StreamOpenOptionsEnum.adOpenStreamUnspecified, string.Empty, string.Empty);
            stream.LoadFromFile(emlFileName);
            stream.Flush();
            msg.DataSource.OpenObject(stream, "_Stream");
            msg.DataSource.Save();
            return msg;
        }
    }
}