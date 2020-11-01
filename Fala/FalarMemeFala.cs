using DanielDevBot.Audios;
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
    class FalarMemeFala : FalaBase, IBotFala
    {
        public bool IsExecutandoAudio { get; set; }

        public List<Tuple<FalaModel, Delegate, Delegate>> Falas { get; set; } = new List<Tuple<FalaModel, Delegate, Delegate>>();

        public FalarMemeFala()
        {
            this.DefinirTimerFilaDeExecucaoAudio();
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
                ChannelName = BotAppSettings.ChannelName
            };

            try
            {
                int idMeme = 0;
                int.TryParse(model.TextoParaFala, out idMeme);

                var memeAudio = ObterMeme(idMeme);

                if (string.IsNullOrEmpty(memeAudio))
                {
                    _CallbackModel.Mensagem = $"Não achei o audio.... {EmotesConsts.NotLikeThis}";
                    FinalizarMemeCallback(model);
                    return;
                }

                model.TextoParaFala = memeAudio;

                CallBackFalaFinalizada callback = new CallBackFalaFinalizada(FinalizarMemeCallback);
                this.ExecutarWAVSync(model, callbackFalaFinalizada: callback);

            }
            catch 
            {
                this.IsExecutandoAudio = false;
                this.Falas.Remove(falaTuple);
                _CallbackModel.Mensagem = $"@{model.Jogador} Deu ruim ao rodar o meme {EmotesConsts.NotLikeThis}";
                notificarErroBotCallback?.DynamicInvoke(_CallbackModel);
            }
        }

        string ObterMeme(int idMeme)
        {
            var memes = AudiosList.ObterMemes();
            var memeSaida = string.Empty;
            switch (idMeme)
            {
                case 1:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("acabou.wav"));
                    break;
                case 2:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("agora_entendi.wav"));
                    break;
                case 3:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("arquivo_x.wav"));
                    break;
                case 4:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("chora_nao_bebe.wav"));
                    break;
                case 5:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("erro.wav"));
                    break;
                case 6:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("eu_nao_entendi.wav"));
                    break;
                case 7:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("faustao_errou.wav"));
                    break;
                case 8:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("foto_feia.wav"));
                    break;
                case 9:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("hora_alegria.wav"));
                    break;
                case 10:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("MLG_buzina.wav"));
                    break;
                case 11:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("nada_haver_irmao.wav"));
                    break;
                case 12:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("nao_faz_mal.wav"));
                    break;
                case 13:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("pede_pra_nerfar.wav"));
                    break;
                case 14:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("quero_cafe.wav"));
                    break;
                case 15:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("senna.wav"));
                    break;
                case 16:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("ta_bom.wav"));
                    break;
                case 17:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("ver_filme_pele.wav"));
                    break;
                case 18:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("voce_vergonha_profissao.wav"));
                    break;
                case 19:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("cavalo.wav"));
                    break;
                case 20:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("aplausos.wav"));
                    break;
                case 21:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("ta_trollando.wav"));
                    break;
                case 22:
                    memeSaida = memes.SingleOrDefault(a => a.Contains("ce_e_o_bichao_hein.wav"));
                    break;
                default:
                    memeSaida = string.Empty;
                    break;
            }
            return memeSaida;
        }

        void FinalizarMemeCallback(FalaModel model)
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
