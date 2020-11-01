using DanielDevBot.Model;
using DanielDevBot.Repositorio;
using DanielDevBot.Singleton;
using DanielDevBot.Utils;
using System.Linq;

namespace DanielDevBot.Jogos.Forca
{
    internal class ForcaChutarStrategy : IForcaStrategy
    {
        ForcaRetornoJogadaModel _RetornoJogadaModel;
        RankingRepositorio _RankingRepositorio;
        public ForcaChutarStrategy()
        {
            _RetornoJogadaModel = new ForcaRetornoJogadaModel();
            _RankingRepositorio = new RankingRepositorio(BotAppSettings.RankingDB);
        }

        public ForcaRetornoJogadaModel Realizar(ForcaModel _Forca, string Jogador = "", string Mensagem = "")
        {
            if (_Forca == null)
            {
                _RetornoJogadaModel.Mensagem = $"Nenhum jogo iniciado, use !iniciarjogo para criar um novo.";
                return _RetornoJogadaModel;
            }

            _RetornoJogadaModel.Jogo = _Forca;

            //Existe jogador?
            if (!_Forca.Participantes.Any(p => p.Nome.ToLower() == Jogador.ToLower()))
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} use o comando !participar para jogar.";
                return _RetornoJogadaModel;
            }

            //Jogador não possui tentativas?
            if (_Forca.Participantes.Any(p => p.Nome.ToLower() == Jogador.ToLower() && p.Tentativas == 0))
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} suas tentativas acabaram, aguarde o próximo jogo.";
                return _RetornoJogadaModel;
            }

            //Valida formato de resposta
            var chute = Mensagem.Split(' ');
            if (!chute.Length.Equals(2))
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} para chutar use o comando !chute <palavra>!";
                return _RetornoJogadaModel;
            }

            //Valida se chute corresponde a palavra
            var resposta = chute[1].ToLower();
            var isChuteCorreto = _Forca.Palavra.ToLower().Equals(resposta);
            if (!isChuteCorreto)
            {
                //_RetornoJogadaModel.Mensagem = $"@{Jogador} seu chute está errado!";
                return _RetornoJogadaModel;
            }

            _RetornoJogadaModel.Mensagem = $"@{Jogador} chutou corretamente! A palavra é {_Forca.Palavra.ToUpper()}";
            HubSingleton.Instance.ForcaHubContext.Clients.All.encerradocomvencedor(_RetornoJogadaModel.Mensagem);
            _RetornoJogadaModel.Jogo = null;

            var moedasGanhas = _Forca.Palavra.Length * 10;

            var jogador = new JogadorModel { Nome = Jogador, Moedas = moedasGanhas };

            _RankingRepositorio.AtualizarRanking(jogador);

            return _RetornoJogadaModel;
        }
    }
}
