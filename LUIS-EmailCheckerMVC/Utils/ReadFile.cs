using System;
using System.IO;
using System.Web;

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
    }
}