using DanielDevBot.Model;

namespace DanielDevBot.Jogos.Forca
{
    public class ForcaContextStrategy
    {
        private IForcaStrategy _ForcaStrategy;

        public ForcaContextStrategy(string forcaJogadas)
        {
            var jogadaEnum = AvaliarJogada(forcaJogadas);

            switch (jogadaEnum)
            {
                case ForcaJogadasEnum.IniciarJogo:
                    _ForcaStrategy = new ForcaIniciarJogoStrategy();
                    break;
                case ForcaJogadasEnum.Resposta:
                    _ForcaStrategy = new ForcaResponderStrategy();
                    break;
                case ForcaJogadasEnum.Chute:
                    _ForcaStrategy = new ForcaChutarStrategy();
                    break;
                case ForcaJogadasEnum.Participar:
                    _ForcaStrategy = new ForcaParticiparStrategy();
                    break;
                default:
                    _ForcaStrategy = new NullForcaStrategy();
                    break;
            }
        }

        public ForcaRetornoJogadaModel ExecutarAcao(ForcaModel forcaModel, string Jogador = null, string Mensagem = null)
        {
            return _ForcaStrategy.Realizar(forcaModel, Jogador, Mensagem);
        }
        private static ForcaJogadasEnum AvaliarJogada(string comando)
        {
            switch (comando)
            {
                case "iniciarjogo":
                    return ForcaJogadasEnum.IniciarJogo;
                case "participar":
                    return ForcaJogadasEnum.Participar;
                case "resposta":
                    return ForcaJogadasEnum.Resposta;
                case "chute":
                    return ForcaJogadasEnum.Chute;
                default:
                    return ForcaJogadasEnum.Null;
            }
        }
    }
}
