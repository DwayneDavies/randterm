using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RandTerm
{

    class Options : OptionsEngine
    {

        public Options(string[] args, string[] argList, char divider, Dictionary<string, string> options) : base(args, argList, divider, options)
        {

        }

        public static Options FactoryMethod(string[] args, char divider = '-')
        {
            string[] argList = new string[] {"list", "roll" };
            Dictionary<string, string> options = new Dictionary<string, string>();

            return new Options(args, argList, divider, options);
        }

        private object[] getXInts(string input)
        {
            string minimum = "1", sides, times;
            string pattern = @"[0-9]+";
            string [] mc = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[0].Value).ToArray();

            if (mc.Length == 2)
            {
                sides = mc[1];
                times = mc[0];
            }
            else if (mc.Length == 3)
            {
                sides = mc[1];
                times = mc[0];

                if (Int32.Parse(mc[2]) < Int32.Parse(mc[1]))
                    minimum = mc[2];

            }
            else if (mc.Length == 1)
            {
                sides = "6";
                times = mc[0];
            }
            else
            {
                sides = "6";
                times = "1";
            }

            WebClient client = new WebClient();
            string result = client.DownloadString(@"https://www.random.org/integers/?num=" + times + "&min=" + minimum + "&max=" + sides + "&col=1&base=10&format=plain&rnd=new");
            string[] nums = result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            return new object[] { nums, sides, times, minimum};
        }

        protected bool list(string input)
        {
            object[] returnedObject = getXInts(input);
            string min = (string) returnedObject[3];
            string[] nums = (string[])returnedObject[0];
            string sides = (string)returnedObject[1];
            string times = (string)returnedObject[2];
            
            Console.WriteLine("Your list of " + times +  " numbers from " + min + " to " + sides + ": ");

            foreach (string n in nums)
                Console.WriteLine(n);

            return true;
        }

        protected bool roll(string diceInput)
        {
            object[] returnedObject = getXInts(diceInput);
            string[] nums = (string[])returnedObject[0];
            string sides = (string)returnedObject[1];
            string times = (string)returnedObject[2];

            int total = 0;

            foreach (string n in nums)
                if (n.Trim().Length > 0)
                    total = total + Convert.ToInt32(n.Trim());

            Console.WriteLine("The result of rolling " + times + "d" + sides + " is : " + total);

            return true;
        }

    }

}
