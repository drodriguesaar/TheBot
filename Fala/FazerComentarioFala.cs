using DanielDevBot.Bots;
using DanielDevBot.Model;
using DanielDevBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DanielDevBot.Fala
{
    internal class FazerComentarioFala : FalaBase, IBotFala
    {
        public FazerComentarioFala()
        {
            this.DefinirTimerFilaDeExecucaoAudio();
        }

        public bool IsExecutandoAudio { get; set; }
        public List<Tuple<FalaModel, Delegate, Delegate>> Falas { get; set; } = new List<Tuple<FalaModel, Delegate, Delegate>>();

        void DefinirTimerFilaDeExecucaoAudio()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (this.Falas.Any() && !this.IsExecutandoAudio)
                    {
                        this.IsExecutandoAudio = true;
                        var falaModel = this.Falas.First();
                        ExecutarFala(falaModel);
                    }
                    Thread.Sleep(3000);
                }
            });
        }

        public void Falar(FalaModel model, Delegate notificarBotCallback = null, Delegate notificarErroBotCallback = null)
        {
            this.Falas.Add(Tuple.Create(model, notificarBotCallback, notificarErroBotCallback));
        }

        void ExecutarFala(Tuple<FalaModel, Delegate, Delegate> falaTuple)
        {
            var model = falaTuple.Item1;
            var notificarBotCallback = falaTuple.Item2;
            var notificarErroBotCallback = falaTuple.Item3;

            var _CallbackModel = new CallbackModel
            {
                ChannelName = BotAppSettings.ChannelName,
                Mensagem = string.Empty
            };

            try
            {
                base.EncadeiaReacao = false;
                this.IsExecutandoAudio = true;
                CallBackFalaFinalizada callback = new CallBackFalaFinalizada(FinalizarComentarioCallback);
                base.ExecutarAudioSync(model, callbackFalaFinalizada: callback);
            }
            catch
            {
                this.IsExecutandoAudio = false;
                this.Falas.Remove(falaTuple);
                _CallbackModel.Mensagem = $"@{model.Jogador} ruim ao fazer o comentário {EmotesConsts.NotLikeThis}";
                notificarErroBotCallback?.DynamicInvoke(_CallbackModel);
            }
        }

        void FinalizarComentarioCallback(FalaModel model)
        {
            if (model != null)
            {
                var tuppleLookUp = this.Falas.ToLookup(fala => fala.Item1.ID);
                if (tuppleLookUp.Any())
                {
                    var tupple = tuppleLookUp[model.ID];
                    this.Falas.Remove(tupple.First());
                }
            }
            this.IsExecutandoAudio = false;
        }
    }
}
