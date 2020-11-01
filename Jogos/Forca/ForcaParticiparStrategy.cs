using DanielDevBot.Model;
using DanielDevBot.Singleton;
using System.Linq;

namespace DanielDevBot.Jogos.Forca
{
    internal class ForcaParticiparStrategy : IForcaStrategy
    {
        ForcaRetornoJogadaModel _RetornoJogadaModel;

        public ForcaParticiparStrategy()
        {
            _RetornoJogadaModel = new ForcaRetornoJogadaModel();
        }

        public ForcaRetornoJogadaModel Realizar(ForcaModel _Forca, string Jogador = "", string Mensagem = "")
        {
            if (_Forca == null)
            {
                _RetornoJogadaModel.Mensagem = $"Nenhum jogo iniciado, use !iniciarjogo para criar um novo.";
                return _RetornoJogadaModel;
            }

            _RetornoJogadaModel.Jogo = _Forca;

            var returnfeedback = string.Empty;
            var textoJogadores = _Forca.Participantes.Count > 1 ? "jogadores" : "jogador";

            var jogadorJaParticipando = _Forca
                                            .Participantes
                                            .Any(p => p.Nome.ToLower() == Jogador.ToLower());

            //Se o jogador já entrou no jogo, avisar ele.
            if (jogadorJaParticipando)
            {
                _RetornoJogadaModel.Mensagem = $"@{Jogador} você já entrou no jogo. Use !resposta <letra>, !chute <palavra> para jogar. Até o momento temos {_Forca.Participantes.Count} {textoJogadores}.";
                return _RetornoJogadaModel;
            }

            var novoJogador = new JogadorModel
            {
                Nome = Jogador
            };

            _Forca.Participantes.Add(novoJogador);

            HubSingleton.Instance.ForcaHubContext.Clients.All.adicionajogador(novoJogador);

            _RetornoJogadaModel.Mensagem = $"@{Jogador} você entrou no jogo. Use !resposta <letra>, !chute <palavra> para jogar. Até o momento temos {_Forca.Participantes.Count} {textoJogadores}.";
            _RetornoJogadaModel.Jogo = _Forca;

            return _RetornoJogadaModel;
        }
    }
}

