using System;

namespace DanielDevBot.Jogos
{
    public interface IJogo
    {
        bool IsJogoIniciado { get; set; }
        void Finalizar(Delegate notificarBotCallback = null, Delegate notificarErroBotCallback = null);
        void Jogada(string comando, string mensagem, string jogador, Delegate notificarBotCallback = null, Delegate notificarErroBotCallback = null);
    }
}
