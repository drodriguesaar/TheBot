using DanielDevBot.Jogos.Forca;

namespace DanielDevBot.Jogos
{
    public static class JogoFactory
    {
        public static IJogo ObterJogo(JogoEnum jogoEnum)
        {
            switch (jogoEnum)
            {
                case JogoEnum.Forca:
                    return new ForcaJogo();
                default:
                    return new NullJogo();
            }
        }
    }
}
