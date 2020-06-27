using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace RandTerm
{

    class OptionsEngine
    {
        protected string[] argList;
        string[] infoArgs;
        protected Dictionary<string, MethodInfo> supportedArgs;
        protected Dictionary<string, string> options;
        public bool PrematureTerm { get; }

        public OptionsEngine(string[] args, string[] argList, char divider, Dictionary<string, string> options)
        {
            // Don't forget to replace "/" and "--" charactes in our args with "-"! 
            string[] arguments = string.Join(" ", args).Replace('/', '-').Replace("--", "-").Split(divider);
            // Our supported arguments so far. Others are ignored.
            this.argList = argList;
            this.argList = argList;
            // Stores options, including some default ones.
            this.options = options;
            // The supported functions all have their own named methods.
            supportedArgs = new Dictionary<string, MethodInfo>();

            // Used by main to check if it should not do other stuff.
            PrematureTerm = false;

            foreach (string arg in argList)
                supportedArgs[arg] = this.GetType().GetTypeInfo().GetDeclaredMethod(arg);

            // Add option, but only if it has a named function that handles that that option does. If it is help, call that instead.
            for (int i = 1; i < arguments.Length; i++)
                if (arguments[i].StartsWith("help"))
                    help(arguments[i]);
                else
                    addOption(arguments[i]);

            // If only "help" and "list" options are given, lets not do other stuff.
            if (isInfoOnly(arguments))
                PrematureTerm = true;

        }

        protected void help(string argument)
        {
            string[] split = argument.Split(" ");
  
            if ((split.Length < 2))
                if (File.Exists("help.txt"))
                    Console.WriteLine(File.ReadAllText("help.txt") + "\n");
                else
                    Console.WriteLine("Help file has gone missing or is otherwise inaccessible!");
            else
                getHelpArgument(split[1]);

        }

        protected void addOption(string potentialOption)
        {
            string toSplit = potentialOption;

            // If option has no parameter, this will stop undesired behaviour later.
            if (toSplit.IndexOf(' ') < 0)
                toSplit += " ";

            string[] str = toSplit.Split(' ');

            if (argsContains(argList, str[0]))
                if ((bool)supportedArgs[str[0]].Invoke(this, new object[] { str[1] }))
                    options[str[0]] = str[1];

        }

        protected bool argsContains(string[] args, string toFind)
        {
            return (Array.FindIndex(args, x => x == toFind) > -1);
        }

        protected void getHelpArgument(string argument)
        {
            string jsonString = System.IO.File.ReadAllText("helpArguments.json");
            Dictionary<string, string> helpArguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

            if (helpArguments.ContainsKey(argument))
                Console.WriteLine("\nHelp for the " + argument + " argument: \n" + helpArguments[argument] + "\n");
            else
                Console.WriteLine("We do not have any help information for the " + argument + " argument\n");

        }

        protected bool isInfoOnly(string[] args)
        {

            if (infoArgs == null)
                return false;

            var items = argList.Except(infoArgs);

            foreach (var item in args)
                if (items.Contains(item.Split(" ")[0]))
                    return false;

            return true;
        }

        public string this[string key]
        {
            get { return options[key]; }
        }

    }


}
