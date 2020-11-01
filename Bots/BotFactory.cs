using DanielDevBot.Hubs;
using Microsoft.AspNet.SignalR;

namespace DanielDevBot.Bots
{
    public static class BotFactory
    {
        public static IBot FabricarBot(BotEnum botEnum)
        {
            switch (botEnum)
            {
                case BotEnum.Twitch:
                    return new TwitchBot();
                default:
                    return new NullBot();
            }
        }
    }
}
