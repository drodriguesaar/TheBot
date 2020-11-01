using DanielDevBot.Acoes;
using DanielDevBot.Bots;

namespace DanielDevBot.Fala
{
    public static class FalaFactory
    {
        public static IBotFala GerarFala(FalaEnum fala)
        {
            switch (fala)
            {
                case FalaEnum.Piada:
                    return new FazerPiadaFala();
                case FalaEnum.Comentario:
                    return new FazerComentarioFala();
                case FalaEnum.Memes:
                    return new FalarMemeFala();
                default:
                    return new NullFala();
            }
        }
    }
}
