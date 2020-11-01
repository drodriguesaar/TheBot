using DanielDevBot.Bots;
using DanielDevBot.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DanielDevBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                
                BotFactory.FabricarBot(BotEnum.Twitch).Start();
            });
            Task.Run(() =>
            {
                string url = "http://localhost:8080";
                using (WebApp.Start(url))
                {
                    Console.WriteLine("Server running on {0}", url);
                    Console.ReadLine();                    
                }
            });
            //Console.ReadKey();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
