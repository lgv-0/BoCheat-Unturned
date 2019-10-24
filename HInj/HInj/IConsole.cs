using SDG.Unturned;
using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace HInj
{
    //tHIS Whole class is ugh. When the game upated unity I believe, the entire method for
    //grabbing console changed so you know, it's completely messed up now, but it still pops up so
    public class IConsole
    {
        //Called when console recieves a command
        public static void HandleCommand(string input)
        {
            string Send = "Command not found???";
            string[] breaks = input.Split(' ');

            foreach (string str in breaks)
                switch (str)
                {
                    case "exit":
                        Process.GetCurrentProcess().Close();
                        break;

                    default: break;
                }
            Console.WriteLine(Send);
        }

        static Thread ConsoleThread = new Thread(ExConsole);
        static ConsoleInputOutput s = new ConsoleInputOutput();
        static CommandWindow w = new CommandWindow();
        static CommandInputHandler x = new CommandInputHandler(HandleCommand);

        public static void Setup()
        {
            w.title = "BoCheat";
            s.initialize(w);
            s.inputCommitted += x;

            ConsoleThread.Start();
        }

        public static void ExConsole()
        {
            Console.WriteLine("CONCID::" + AppDomain.GetCurrentThreadId());
            while (true)
            {
                s.update();
            }
        }
    }
}
