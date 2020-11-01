using System.ComponentModel.DataAnnotations.Schema;

namespace DanielDevBot.Model
{
    public class JogadorModel
    {
        public JogadorModel()
        {
            Tentativas = 10;
        }

        public string Nome { get; set; }
        public int Moedas { get; set; }

        /// <summary>
        /// Valor dinamico de posição no ranking, calculado em tempo real.
        /// </summary>
        [NotMapped]
        public int Posicao { get; set; }

        [NotMapped]
        /// <summary>
        /// Valor default para tentativas de jogo
        /// </summary>
        public int Tentativas { get; set; }
    }
}
