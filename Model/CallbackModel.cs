using DanielDevBot.Jogos;

namespace DanielDevBot.Model
{
    public class CallbackModel
    {
        public string Mensagem { get; set; }
        public string UserName { get; set; }
        public string ChannelName { get; set; }
        public bool JogoEncerrado { get; set; }
    }
}
