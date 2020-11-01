using DanielDevBot.Model;

namespace DanielDevBot.Jogos.Forca
{
    public interface IForcaStrategy
    {
        ForcaRetornoJogadaModel Realizar(ForcaModel forcaModel, string Jogador = "", string Mensagem = "");
    }
}
