using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RandTerm
{

    static class Init
    {

        public static void checkFiles()
        {
            string[] fileList = { "help.txt", "helpArguments.json" };

            foreach (string fileName in fileList)
                if (!File.Exists(fileName))
                    downloadFiles(fileName);

        }

        private static void downloadFiles(string fileName)
        {
            WebClient client = new WebClient();

            client.DownloadFile("https://raw.githubusercontent.com/DwayneDavies/curterm/blob/master/CurTerm/bin/Release/netcoreapp3.1/publish/" + fileName, fileName);
        }

    }

}
