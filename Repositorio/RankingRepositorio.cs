using DanielDevBot.Model;
using DanielDevBot.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielDevBot.Repositorio
{
    public class RankingRepositorio
    {
        readonly string _PathArquivo;

        public RankingRepositorio(string PathArquivo)
        {
            _PathArquivo = PathArquivo;
        }

        public void AtualizarRanking(JogadorModel Jogador)
        {
            var jsonData = File.ReadAllText(_PathArquivo, Encoding.UTF8).Replace("\0", "");

            var Ranking = JsonConvert.DeserializeObject<RankingModel>(jsonData);

            if (Ranking.Jogadores == null)
            {
                Ranking.Jogadores = new List<JogadorModel>();
            }

            if (Ranking.Jogadores.Any(j => j.Nome.ToLower() == Jogador.Nome.ToLower()))
            {
                Ranking.Jogadores.SingleOrDefault(j => j.Nome.ToLower() == Jogador.Nome.ToLower()).Moedas += Jogador.Moedas;
            }
            else
            {
                Ranking.Jogadores.Add(Jogador);
            }

            jsonData = JsonConvert.SerializeObject(Ranking, new JsonSerializerSettings { Formatting = Formatting.None });

            Task.Run(async () => { await EscreverTexto(jsonData); });
        }

        public async Task ObterJogador(string Jogador, Delegate callBackTerminado = null, Delegate callBackErro = null)
        {
            var _CallbackModel = new CallbackModel
            {
                ChannelName = BotAppSettings.ChannelName
            };

            try
            {
                var jsonData = await LerTexto();
                var Ranking = JsonConvert.DeserializeObject<RankingModel>(jsonData);
                var resultado = Ranking.Jogadores.SingleOrDefault(j => j.Nome.ToLower() == Jogador.ToLower());

                _CallbackModel.Mensagem = (resultado != null) ? $"@{Jogador} você tem {resultado.Moedas} moeda(s)" : $"@{Jogador} você ainda não conquistou nenhuma moeda!";
                callBackTerminado?.DynamicInvoke(_CallbackModel);
            }
            catch
            {
                _CallbackModel.Mensagem = $"@{Jogador} não consegui ver a quantidade de moedas que você tem {EmotesConsts.NotLikeThis}";
                callBackErro?.DynamicInvoke(_CallbackModel);
            }
        }

        public async Task<JogadorModel> AtualizarJogador(JogadorModel jogadorNew)
        {
            var jsonData = File.ReadAllText(_PathArquivo, Encoding.UTF8).Replace("\0", "");

            var Ranking = JsonConvert.DeserializeObject<RankingModel>(jsonData);

            if (Ranking.Jogadores == null)
            {
                Ranking.Jogadores = new List<JogadorModel>();
            }

            if (Ranking.Jogadores.Any(j => j.Nome.ToLower() == jogadorNew.Nome.ToLower()))
            {
                var jogadorOld = Ranking.Jogadores.SingleOrDefault(j => j.Nome.ToLower() == jogadorNew.Nome.ToLower());

                var indexJogadorOld = Ranking.Jogadores.IndexOf(jogadorOld);

                Ranking.Jogadores[indexJogadorOld] = jogadorNew;

                jsonData = JsonConvert.SerializeObject(Ranking, new JsonSerializerSettings { Formatting = Formatting.None });

                await EscreverTexto(jsonData);
            }

            return jogadorNew;
        }

        async Task EscreverTexto(string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);

            using (var sourceStream =
                new FileStream(
                    _PathArquivo,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }

        }

        async Task<string> LerTexto()
        {
            var buffer = new byte[] { };

            using (var source = new FileStream(_PathArquivo,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true))
            {

                buffer = new byte[source.Length];
                await source.ReadAsync(buffer, 0, (int)source.Length);
                var txt = Encoding.UTF8.GetString(buffer);
                return txt;
            }
        }
    }
}
