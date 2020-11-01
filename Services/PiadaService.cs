using DanielDevBot.Model;
using DanielDevBot.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielDevBot.Services
{
    public class PiadaService
    {
        public PiadaModel ObterPiada()
        {
            var client = new RestClient(BotAppSettings.PiadaApi);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);

            var palavra = response.Content;

            var jsonDefinition = new { pergunta = "", resposta = "" };
            var palavraJson = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(palavra, jsonDefinition);

            return new PiadaModel
            {
                Pergunta = palavraJson.pergunta,
                Resposta = palavraJson.resposta
            };
        }
    }
}
