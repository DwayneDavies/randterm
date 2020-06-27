using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.NetworkInformation;

namespace RandTerm
{
    class Program
    {

        static void Main(string[] args)
        {

            if (new Ping().Send("www.google.com.mx").Status != IPStatus.Success)
            {
                Console.WriteLine("You do not seem to be connected to the internet!");

                return;
            }

            Init.checkFiles();

            Options options = Options.FactoryMethod(args);

        }

    }

}
