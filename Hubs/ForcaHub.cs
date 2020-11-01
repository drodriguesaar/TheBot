using Microsoft.AspNet.SignalR;

namespace DanielDevBot.Hubs
{
    public class ForcaHub : Hub
    {
        public void Atualizar(string palavra)
        {
            Clients.All.atualizarPalavra(palavra);
        }

        public void Encerrar(string palavra)
        {
            Clients.All.encerrarJogo(palavra);
        }
    }
}
