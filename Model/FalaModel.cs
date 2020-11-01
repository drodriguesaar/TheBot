using System;
using System.Speech.Synthesis;

namespace DanielDevBot.Model
{
    public class FalaModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Jogador { get; set; }
        public string TextoParaFala { get; set; }
        public PromptBuilder TextoPrompFala { get; set; }

    }
}
