using DanielDevBot.Jogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielDevBot.Bots
{
    public interface IBotJogo
    {
        IJogo Jogo { get; set; }

        void Jogar(Tuple<string, string, string> comando);
    }
}
