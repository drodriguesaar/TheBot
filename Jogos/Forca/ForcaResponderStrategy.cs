using DanielDevBot.Model;
using DanielDevBot.Repositorio;
using DanielDevBot.Singleton;
using DanielDevBot.Utils;
using System.Linq;
using System.Text;

namespace DanielDevBot.Jogos.Forca
{
    internal class ForcaResponderStrategy : IForcaStrategy
    {
        ForcaRetornoJogadaModel _RetornoJogadaModel;
        RankingRepositorio _RankingRepositorio;

        public ForcaResponderStrategy()
        {
            _RetornoJogadaModel = new ForcaRetornoJogadaModel();
            _RankingRepositorio = new RankingRepositorio(BotAppSettings.RankingDB);
        }

        public ForcaRetornoJogadaModel Realizar(ForcaModel _Forca, string Jogador = null, string Mensagem = "")
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

            //Jogador mandou mensagem?
            if (string.IsNullOrEmpty(Mensagem))
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} não tenho bola de cristal, jogue usando !resposta <letra> ou chute uma palavra usando !chute <palavra>.";
                return _RetornoJogadaModel;
            }

            //Tentativa está correta?
            var mensagemArray = Mensagem.ToLower().Split(' ');
            if (!mensagemArray.Length.Equals(2))
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} jogue usando !resposta <letra> ou chute uma palavra usando !chute <palavra>.";
                return _RetornoJogadaModel;

            }

            //Tentativa está correta?
            var resposta = mensagemArray.Last();
            if (!resposta.Length.Equals(1))
            {
                _RetornoJogadaModel.Mensagem = $"@{_RetornoJogadaModel.Mensagem} jogue usando !resposta <letra> ou chute uma palavra usando !chute <palavra>.";
                return _RetornoJogadaModel;
            }

            var sbResposta = new StringBuilder(_Forca.Segredo);
            var letras = _Forca.Palavra.Select(c => c).ToList();
            var icExisteOcorrenciaDaLetra = false;
            for (int index = 0; index < letras.Count; index++)
            {
                var letra = letras[index].ToString();
                if (!resposta.Equals(letra))
                { continue; }
                sbResposta[index] = resposta.ToCharArray()[0];
                icExisteOcorrenciaDaLetra = true;
            }
            _Forca.Segredo = sbResposta.ToString().ToUpper();

            if (icExisteOcorrenciaDaLetra)
            {
                //_RetornoJogadaModel.Mensagem = $"@{Jogador} você acertou uma letra da palavra: {_Forca.Segredo}.";

                HubSingleton.Instance.ForcaHubContext.Clients.All.respostacerta(_Forca.Segredo);

                if (!_Forca.Segredo.Contains("#"))
                {
                    _RetornoJogadaModel.Mensagem = $"@{Jogador} venceu! A palavra é {_Forca.Palavra}!";
                    HubSingleton.Instance.ForcaHubContext.Clients.All.encerradocomvencedor(_RetornoJogadaModel.Mensagem);
                    _RetornoJogadaModel.Jogo = null;

                    var moedasGanhas = _Forca.Palavra.Length * 10;

                    var jogador = new JogadorModel { Nome = Jogador, Moedas = moedasGanhas };

                    _RankingRepositorio.AtualizarRanking(jogador);

                    return _RetornoJogadaModel;
                }

                _RetornoJogadaModel.Jogo = _Forca;
                return _RetornoJogadaModel;
            }

            if (!icExisteOcorrenciaDaLetra)
            {
                _Forca.Participantes.SingleOrDefault(p => p.Nome.ToLower() == Jogador.ToLower()).Tentativas -= 1;
                var jogadorParticipante = _Forca.Participantes.SingleOrDefault(p => p.Nome.ToLower() == Jogador.ToLower());
                //_RetornoJogadaModel.Mensagem = $"@{Jogador} você não acertou, segue a palavra: {_Forca.Segredo}! Tentativas restantes: {jogadorParticipante.Tentativas}.";
                _RetornoJogadaModel.Jogo = _Forca;

                HubSingleton.Instance.ForcaHubContext.Clients.All.respostaErrada(jogadorParticipante);

                if (!_Forca.Participantes.Any(p => p.Tentativas > 0))
                {
                    _RetornoJogadaModel.Mensagem = $"Ninguém acertou, a palavra era {_Forca.Palavra}!";
                    HubSingleton.Instance.ForcaHubContext.Clients.All.encerradosemvencedor(_RetornoJogadaModel.Mensagem);
                    _RetornoJogadaModel.Jogo = null;
                    return _RetornoJogadaModel;
                }

                return _RetornoJogadaModel;
            }

            return _RetornoJogadaModel;
        }
    }
}
