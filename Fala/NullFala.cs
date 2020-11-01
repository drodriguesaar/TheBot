using DanielDevBot.Bots;
using DanielDevBot.Model;
using DanielDevBot.Utils;
using System;
using System.Collections.Generic;

namespace DanielDevBot.Fala
{
    public class NullFala : IBotFala
    {
        public bool IsExecutandoAudio { get; set; }
        public List<Tuple<FalaModel, Delegate, Delegate>> Falas { get; set; } = new List<Tuple<FalaModel, Delegate, Delegate>>();

        CallbackModel CallbackModel;

        public void Falar(FalaModel model, Delegate notificarBotCallback = null, Delegate notificarErroBotCallback = null)
        {
            CallbackModel = new CallbackModel
            {
                ChannelName = BotAppSettings.ChannelName,
                Mensagem = "Eu não sei falar isso ainda"
            };
            try
            {
                notificarBotCallback?.DynamicInvoke(CallbackModel);
            }
            catch 
            {
                CallbackModel.Mensagem = $"deu pau... {EmotesConsts.NotLikeThis}";
                notificarErroBotCallback?.DynamicInvoke(CallbackModel);
            }
        }
    }
}
