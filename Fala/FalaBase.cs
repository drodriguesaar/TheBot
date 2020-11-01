using DanielDevBot.Audios;
using DanielDevBot.Model;
using System;
using System.Media;
using System.Speech.Synthesis;

namespace DanielDevBot.Fala
{
    internal class FalaBase
    {
        protected bool EncadeiaReacao { get; set; }

        protected delegate void CallBackFalaFinalizada(FalaModel model);

        internal virtual void ExecutarAudioAsync(FalaModel model, Delegate callbackFalaFinalizada = null)
        {
            if (model.TextoPrompFala == null && string.IsNullOrEmpty(model.TextoParaFala))
            { return; }

            var sintetizador = new SpeechSynthesizer
            {
                Volume = 100
            };

            sintetizador.SelectVoice("Microsoft Maria Desktop");
            sintetizador.SetOutputToDefaultAudioDevice();
            sintetizador.SpeakCompleted += (speakersender, se) =>
            {
                if (EncadeiaReacao)
                {
                    var player = new SoundPlayer();
                    player.LoadCompleted += (playersender, pe) =>
                        {
                            var sp = (SoundPlayer)playersender;
                            if (sp.IsLoadCompleted)
                            {
                                sp.Play();
                                callbackFalaFinalizada?.DynamicInvoke(model);
                            }
                        };
                    var audios = AudiosList.ObterReacoes();
                    var audioAleatorio = new Random().Next(audios.Count);
                    var audio = audios[audioAleatorio];
                    player.SoundLocation = audio;
                    player.LoadAsync();
                }
                else
                {
                    callbackFalaFinalizada?.DynamicInvoke(model);
                }
            };

            if (!string.IsNullOrEmpty(model.TextoParaFala))
            {
                sintetizador.SpeakAsync(model.TextoParaFala);
            }
            else if (model.TextoPrompFala != null)
            {
                sintetizador.SpeakAsync(model.TextoPrompFala);
            }
        }

        internal virtual void ExecutarAudioSync(FalaModel model, Delegate callbackFalaFinalizada = null)
        {
            if (model.TextoPrompFala == null && string.IsNullOrEmpty(model.TextoParaFala))
            {
                callbackFalaFinalizada?.DynamicInvoke(model);
            }

            var sintetizador = new SpeechSynthesizer
            {
                Volume = 100
            };

            sintetizador.SelectVoice("Microsoft Maria Desktop");
            sintetizador.SetOutputToDefaultAudioDevice();

            if (!string.IsNullOrEmpty(model.TextoParaFala))
            {
                sintetizador.Speak(model.TextoParaFala);
            }
            else if (model.TextoPrompFala != null)
            {
                sintetizador.Speak(model.TextoPrompFala);
            }

            if (EncadeiaReacao)
            {
                var audios = AudiosList.ObterReacoes();
                var audioAleatorio = new Random().Next(audios.Count);
                var audio = audios[audioAleatorio];

                var player = new SoundPlayer
                {
                    SoundLocation = audio
                };
                player.Load();
                player.Play();
                callbackFalaFinalizada?.DynamicInvoke(model);
            }
            else
            {
                callbackFalaFinalizada?.DynamicInvoke(model);
            }
        }

        internal virtual void ExecutarWAVAsync(FalaModel model, Delegate callbackFalaFinalizada = null)
        {
            var player = new SoundPlayer();
            player.LoadCompleted += (playersender, pe) =>
            {
                var sp = (SoundPlayer)playersender;
                if (sp.IsLoadCompleted)
                {
                    sp.Play();
                    callbackFalaFinalizada?.DynamicInvoke(model);
                }
            };
            player.SoundLocation = model.TextoParaFala;
            player.LoadAsync();
        }

        internal virtual void ExecutarWAVSync(FalaModel model, Delegate callbackFalaFinalizada = null)
        {
            
            var player = new SoundPlayer
            {
                SoundLocation = model.TextoParaFala
            };
            player.Load();
            player.PlaySync();
            callbackFalaFinalizada?.DynamicInvoke(model);
        }
    }
}
