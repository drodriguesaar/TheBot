using DanielDevBot.Model;
using DanielDevBot.Utils;
using System;

namespace DanielDevBot.Jogos.Forca
{
    public class ForcaJogo : IJogo
    {
        public ForcaJogo()
        { }

        ForcaModel Forca;
        CallbackModel CallbackModel;
        ForcaContextStrategy _ForcaContextStrategy;

        public bool IsJogoIniciado { get; set; }

        public void Jogada(string comando, string mensagem, string jogador, Delegate onTerminadoCallBack = null, Delegate onErroCallback = null)
        {
            try
            {
                _ForcaContextStrategy = new ForcaContextStrategy(comando);

                var feedBack = _ForcaContextStrategy.ExecutarAcao(Forca, Jogador: jogador, Mensagem: mensagem);

                Forca = feedBack.Jogo;

                CallbackModel = new CallbackModel
                {
                    UserName = jogador,
                    Mensagem = feedBack.Mensagem,
                    ChannelName = BotAppSettings.ChannelName,
                    JogoEncerrado = (Forca == null)
                };

                if (CallbackModel.JogoEncerrado)
                {
                    this.Finalizar(onErroCallback: onErroCallback);
                }

                onTerminadoCallBack?.DynamicInvoke(CallbackModel);
            }
            catch (Exception e)
            {
                onErroCallback?.DynamicInvoke(new CallbackModel
                {
                    Mensagem = e.Message,
                    ChannelName = BotAppSettings.ChannelName
                });
            }
        }

        public void Finalizar(Delegate onTerminadoCallBack = null, Delegate onErroCallback = null)
        {
            try
            {
                Forca = null;
                this.IsJogoIniciado = false;
                this._ForcaContextStrategy = null;
            }
            catch (Exception e)
            {
                onErroCallback?.DynamicInvoke(new CallbackModel
                {
                    Mensagem = e.Message,
                    ChannelName = BotAppSettings.ChannelName
                });
            }
        }
    }
}
