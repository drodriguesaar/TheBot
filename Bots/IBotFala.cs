using DanielDevBot.Model;
using System;
using System.Collections.Generic;

namespace DanielDevBot.Bots
{
    public interface IBotFala
    {
        /// <summary>
        /// Propriedade de controle de ação de executar áudio.
        /// </summary>
        bool IsExecutandoAudio { get; set; }

        /// <summary>
        /// Fila de falas para execução em sequencia.
        /// </summary>
        List<Tuple<FalaModel, Delegate, Delegate>> Falas { get; set; }

        /// <summary>
        /// Método de execução do audio desejado.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="notificarBotCallback"></param>
        /// <param name="notificarErroBotCallback"></param>
        void Falar(FalaModel model,
            Delegate notificarBotCallback = null,
            Delegate notificarErroBotCallback = null);
    }
}
