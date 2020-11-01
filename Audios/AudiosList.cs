using DanielDevBot.Utils;
using System.Collections.Generic;

namespace DanielDevBot.Audios
{
    public static class AudiosList
    {
        public static List<string> ObterReacoes()
        {
            return new List<string>
            {
                $@"{BotAppSettings.ReacoesPath}bem-bolada.wav",
                $@"{BotAppSettings.ReacoesPath}booing-crowd-2.wav",
                $@"{BotAppSettings.ReacoesPath}booing-crowd.wav",
                $@"{BotAppSettings.ReacoesPath}cggasa.wav",
                $@"{BotAppSettings.ReacoesPath}crickets.wav",
                $@"{BotAppSettings.ReacoesPath}laughter-short.wav",
                $@"{BotAppSettings.ReacoesPath}rca.wav",
                $@"{BotAppSettings.ReacoesPath}rimshot.wav",
                $@"{BotAppSettings.ReacoesPath}risada-de-zacarias.wav",
                $@"{BotAppSettings.ReacoesPath}risada_quico.wav"
            };
}

        public static List<string> ObterMemes()
        {
            return new List<string>
            {
                $@"{BotAppSettings.MemesPath}acabou.wav",
                $@"{BotAppSettings.MemesPath}agora_entendi.wav",
                $@"{BotAppSettings.MemesPath}aplausos.wav",
                $@"{BotAppSettings.MemesPath}arquivo_x.wav",
                $@"{BotAppSettings.MemesPath}cavalo.wav",
                $@"{BotAppSettings.MemesPath}chora_nao_bebe.wav",
                $@"{BotAppSettings.MemesPath}erro.wav",
                $@"{BotAppSettings.MemesPath}eu_nao_entendi.wav",
                $@"{BotAppSettings.MemesPath}faustao_errou.wav",
                $@"{BotAppSettings.MemesPath}foto_feia.wav",
                $@"{BotAppSettings.MemesPath}hora_alegria.wav",
                $@"{BotAppSettings.MemesPath}MLG_buzina.wav",
                $@"{BotAppSettings.MemesPath}nada_haver_irmao.wav",
                $@"{BotAppSettings.MemesPath}nao_faz_mal.wav",
                $@"{BotAppSettings.MemesPath}pede_pra_nerfar.wav",
                $@"{BotAppSettings.MemesPath}quero_cafe.wav",
                $@"{BotAppSettings.MemesPath}senna.wav",
                $@"{BotAppSettings.MemesPath}ta_bom.wav",
                $@"{BotAppSettings.MemesPath}ta_trollando.wav",
                $@"{BotAppSettings.MemesPath}ver_filme_pele.wav",
                $@"{BotAppSettings.MemesPath}voce_vergonha_profissao.wav",
                $@"{BotAppSettings.MemesPath}ce_e_o_bichao_hein.wav",
            };
        }
    }
}
