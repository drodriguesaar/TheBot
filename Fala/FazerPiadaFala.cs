using DanielDevBot.Bots;
using DanielDevBot.Fala;
using DanielDevBot.Model;
using DanielDevBot.Repositorio;
using DanielDevBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace DanielDevBot.Acoes
{
    internal class FazerPiadaFala : FalaBase, IBotFala
    {
        PiadaRepositorio _PiadaService;

        CallbackModel _CallbackModel;

        public bool IsExecutandoAudio { get; set; }

        public List<Tuple<FalaModel, Delegate, Delegate>> Falas { get; set; } = new List<Tuple<FalaModel, Delegate, Delegate>>();

        public FazerPiadaFala()
        {
            _PiadaService = new PiadaRepositorio();
            DefinirTimerFilaDeExecucaoAudio();
        }

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
                    Thread.Sleep(5000);
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

            _CallbackModel = new CallbackModel
            {
                ChannelName = BotAppSettings.ChannelName
            };
            try
            {
                if (model == null)
                {
                    _CallbackModel.Mensagem = $"Não consegui realizar a ação {EmotesConsts.NotLikeThis}";
                    notificarBotCallback?.DynamicInvoke(_CallbackModel);
                    this.Falas.Remove(falaTuple);
                    return;
                }

                var piada = _PiadaService.ObterPiadaAleatoria();
                var promptBuilder = new PromptBuilder(new System.Globalization.CultureInfo("pt-BR"));

                promptBuilder.AppendText(piada.Pergunta);
                promptBuilder.AppendBreak(PromptBreak.Small);
                promptBuilder.AppendText(piada.Resposta);
                promptBuilder.AppendBreak(PromptBreak.Small);

                CallBackFalaFinalizada callback = new CallBackFalaFinalizada(FinalizarPiadaCallback);

                base.EncadeiaReacao = true;

                model.TextoPrompFala = promptBuilder;

                _CallbackModel.Mensagem = $"{piada.Pergunta} {piada.Resposta} {EmotesConsts.LUL} {EmotesConsts.Kappa}";
                notificarBotCallback?.DynamicInvoke(_CallbackModel);

                base.ExecutarAudioSync(model, callbackFalaFinalizada: callback);
            }
            catch
            {
                this.IsExecutandoAudio = false;
                this.Falas.Remove(falaTuple);
                _CallbackModel.Mensagem = $"@daniel_dev Deu ruim ao fazer o comentário, ve o log aí {EmotesConsts.NotLikeThis}";
                notificarErroBotCallback?.DynamicInvoke(_CallbackModel);
            }
        }

        void FinalizarPiadaCallback(FalaModel model)
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
