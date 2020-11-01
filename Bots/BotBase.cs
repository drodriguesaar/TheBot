using DanielDevBot.Jogos;
using DanielDevBot.Model;
using System.Collections.Generic;

namespace DanielDevBot.Bots
{
    internal class BotBase
    {
        protected delegate void CallbackTerminadoDelegate(CallbackModel model);

        protected delegate void CallbackErroDelegate(CallbackModel model);
        
        protected List<JogoEnum> _Jogos { get; set; }
    }
}
