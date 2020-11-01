using DanielDevBot.Model;
using DanielDevBot.Services;
using DanielDevBot.Singleton;
using DanielDevBot.Utils;
using System.Collections.Generic;
using System.Linq;

namespace DanielDevBot.Jogos.Forca
{
    internal class ForcaIniciarJogoStrategy : IForcaStrategy
    {
        PalavrasService _PalavrasService;
        ForcaRetornoJogadaModel _RetornoJogadaModel;
        
        public ForcaIniciarJogoStrategy()
        {
            _PalavrasService = new PalavrasService();
            _RetornoJogadaModel = new ForcaRetornoJogadaModel();
        }

        public ForcaRetornoJogadaModel Realizar(ForcaModel _Forca, string Jogador = null, string Mensagem = "")
        {
            if (_Forca != null)
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} já existe um jogo em andamento";
                _RetornoJogadaModel.Jogo = _Forca;
                return _RetornoJogadaModel;
            }

            HubSingleton.Instance.ForcaHubContext.Clients.All.iniciando();

            var novaPalavra = _PalavrasService.ObterPalavra();

            if (string.IsNullOrEmpty(novaPalavra.Valor))
            {
                _RetornoJogadaModel.Mensagem = "Não foi possível iniciar o jogo";
                return _RetornoJogadaModel;
            }


            _Forca = new ForcaModel
            {
                Palavra = novaPalavra.Valor
            };

            var listSegredo = new List<string>();
            foreach (var item in Enumerable.Range(0, novaPalavra.QtdLetras))
            {
                listSegredo.Add("#");
            }

            var segredo = string.Join("", listSegredo);

            _Forca.Segredo = segredo;

            _RetornoJogadaModel.Mensagem =
                $"A palavra contem {novaPalavra.QtdLetras} letras. A palavra é: '{_Forca.Segredo}', que os jogos começem! Usem !participar para entrar no jogo {EmotesConsts.PogChamp}";

            _RetornoJogadaModel.Jogo = _Forca;

            HubSingleton.Instance.ForcaHubContext.Clients.All.iniciado(_Forca);

            return _RetornoJogadaModel;
        }
    }
}
