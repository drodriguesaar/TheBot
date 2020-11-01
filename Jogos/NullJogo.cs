using System;

namespace DanielDevBot.Jogos
{
    internal class NullJogo : IJogo
    {
        public bool IsJogoFinalizado { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsJogoIniciado { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsJogoEmAndamento { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Finalizar(Delegate onTerminadoCallBack = null, Delegate onErroCallback = null)
        {
            throw new NotImplementedException();
        }

        public void Iniciar(Delegate onTerminadoCallBack = null, Delegate onErroCallback = null)
        {
            throw new NotImplementedException();
        }

        public void Jogada(string comando, string mensagem, string jogador, Delegate onTerminadoCallBack = null, Delegate onErroCallback = null)
        {
            throw new NotImplementedException();
        }
    }
}
