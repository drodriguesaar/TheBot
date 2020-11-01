using System.Collections.Generic;

namespace DanielDevBot.Model
{
    public class ForcaModel
    {
        public ForcaModel()
        {
            Participantes = new List<JogadorModel>();
        }
        public string Palavra { get; set; }
        public string Segredo { get; set; }
        public string Resposta { get; set; }
        public string Jogador { get; set; }
        public List<JogadorModel> Participantes { get; set; }
    }
}
