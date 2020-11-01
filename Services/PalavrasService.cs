using DanielDevBot.Model;
using DanielDevBot.Utils;
using RestSharp;

namespace DanielDevBot.Services
{
    public class PalavrasService
    {
        public PalavraModel ObterPalavra()
        {
            var client = new RestClient(BotAppSettings.PalavraApi);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            IRestResponse response = client.Execute(request);

            var palavra = response.Content;

            var jsonDefinition = new { word = ""};
            var palavraJson = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(palavra, jsonDefinition);

            return new PalavraModel
            {
                QtdLetras = palavraJson.word.Length,
                Valor = palavraJson.word.RemoverAcentos()
            };
        }        
    }
}
