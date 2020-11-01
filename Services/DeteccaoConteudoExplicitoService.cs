using DanielDevBot.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielDevBot.Services
{
    public static class DeteccaoConteudoExplicitoService
    {
        public static bool Detectar(string urlImagem)
        {

            var imagePostObject = new
            {
                DataRepresentation = "URL",
                Value = urlImagem,
            };

            var imagePostObjectJson = JsonConvert.SerializeObject(imagePostObject);

            var client = new RestClient(BotAppSettings.XRapidApiBaseUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-rapidapi-host", BotAppSettings.XRapidApiHost);
            request.AddHeader("x-rapidapi-key", BotAppSettings.XRapidApiKey);
            request.AddParameter("application/json", $"{imagePostObjectJson}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var detecctaoRespostaDefinition = new { AdultClassificationScore=0.0, IsImageAdultClassified=false, IsImageRacyClassified=false, RacyClassificationScore = 0.0 };

            var deteccaoDefinitionJson = JsonConvert.DeserializeAnonymousType(response.Content, detecctaoRespostaDefinition);

            var imagemPossuiConteudoExplicito = (deteccaoDefinitionJson.IsImageAdultClassified || detecctaoRespostaDefinition.IsImageRacyClassified);

            return imagemPossuiConteudoExplicito;
        }
    }


}
