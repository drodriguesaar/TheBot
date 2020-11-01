using System.Threading;

namespace DanielDevBot.Bots
{
    public interface IBotTimer
    {
        Timer BotTimer { get; set; }
        void DefinirTimers();
    }
}
