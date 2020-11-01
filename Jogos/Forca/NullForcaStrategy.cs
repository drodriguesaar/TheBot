using DanielDevBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielDevBot.Jogos.Forca
{
    public class NullForcaStrategy : IForcaStrategy
    {
        public ForcaRetornoJogadaModel Realizar(ForcaModel forcaModel, string Jogador = "", string Mensagem = "")
        {
            return new ForcaRetornoJogadaModel
            {
                Mensagem = "Comando não reconhecido",
                Jogo = forcaModel
            };
        }
    }
}
