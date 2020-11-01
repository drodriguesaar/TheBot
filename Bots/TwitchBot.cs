using DanielDevBot.Fala;
using DanielDevBot.Jogos;
using DanielDevBot.Model;
using DanielDevBot.Repositorio;
using DanielDevBot.Services;
using DanielDevBot.Singleton;
using DanielDevBot.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;

namespace DanielDevBot.Bots
{
    internal class TwitchBot : BotBase, IBot, IBotJogo
    {
        readonly ConnectionCredentials _Credentials;
        readonly RankingRepositorio _RankingRepositorio;
        readonly ArquivoRepositorio _ArquivoRepositorio;

        TwitchClient _TwitchClient;

        public IJogo Jogo { get; set; }
        public IBotFala Piada { get; set; } = FalaFactory.GerarFala(FalaEnum.Piada);
        public IBotFala Comentario { get; set; } = FalaFactory.GerarFala(FalaEnum.Comentario);
        public IBotFala Memes { get; set; } = FalaFactory.GerarFala(FalaEnum.Memes);

        public TwitchBot()
        {
            _Credentials = new ConnectionCredentials(BotAppSettings.TwitchUserName, BotAppSettings.AccessToken);
            _Jogos = Extensions.TransformarEmLista<JogoEnum>();
            _RankingRepositorio = new RankingRepositorio(BotAppSettings.RankingDB);
            _ArquivoRepositorio = new ArquivoRepositorio(BotAppSettings.TreinamentoCSV);
        }

        public void Start()
        {
            //ModelBuilder.CreateModel();

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            var customClient = new WebSocketClient(clientOptions);
            _TwitchClient = new TwitchClient(customClient);
            _TwitchClient.Initialize(_Credentials, BotAppSettings.ChannelName);
            _TwitchClient.OnMessageReceived += _TwitchClient_OnMessageReceived;
            _TwitchClient.OnNewSubscriber += _TwitchClient_OnNewSubscriber;
            _TwitchClient.OnRaidNotification += _TwitchClient_OnRaidNotification;
            _TwitchClient.OnReSubscriber += _TwitchClient_OnReSubscriber;
            _TwitchClient.OnError += _TwitchClient_OnError;
            _TwitchClient.OnConnected += _TwitchClient_OnConnected;
            _TwitchClient.OnChatCommandReceived += _TwitchClient_OnChatCommandReceived;
            _TwitchClient.Connect();

        }

        #region Metodos Twitch
        private void _TwitchPubSub_OnBitsReceived(object sender, TwitchLib.PubSub.Events.OnBitsReceivedArgs e)
        {
            _TwitchClient.SendMessage(BotAppSettings.ChannelName, $"@{e.Username}, obrigado pelo apoio!!");
        }
        private void _TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
        {
            if (!e.Command.CommandIdentifier.Equals('!')) { return; }
            ValidarComando(Tuple.Create(e.Command.CommandText, e.Command.ChatMessage.Message, e.Command.ChatMessage.Username));
        }
        private void _TwitchClient_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            _TwitchClient.AddChatCommandIdentifier('!');
            _TwitchClient.SendMessage(BotAppSettings.ChannelName, $"O pai ta on! {EmotesConsts.PogChamp}");
            Console.WriteLine($"Twitch bot conectado às {DateTime.Now}");

        }
        private void _TwitchClient_OnError(object sender, TwitchLib.Communication.Events.OnErrorEventArgs e)
        {
            Console.WriteLine($"Erro ocorrido: {e.Exception.Message} {e.Exception.InnerException}");
            Console.WriteLine(Environment.NewLine);
        }
        private void _TwitchClient_OnReSubscriber(object sender, TwitchLib.Client.Events.OnReSubscriberArgs e)
        {
            _TwitchClient.SendMessage(e.Channel,
                $"@{e.ReSubscriber.DisplayName} obrigado por apoiar o @{e.Channel} por {e.ReSubscriber.Months} meses!");
        }
        private void _TwitchClient_OnRaidNotification(object sender, TwitchLib.Client.Events.OnRaidNotificationArgs e)
        {
            _TwitchClient.SendMessage(e.Channel,
                $"@{e.RaidNotification.DisplayName} e sua raid chegando! " +
                $"{EmotesConsts.PogChamp} {EmotesConsts.PogChamp} {EmotesConsts.PogChamp} {EmotesConsts.PogChamp} {EmotesConsts.PogChamp}");

        }
        private void _TwitchClient_OnNewSubscriber(object sender, TwitchLib.Client.Events.OnNewSubscriberArgs e)
        {
            _TwitchClient.SendMessage(e.Channel,
                $"@{e.Subscriber.DisplayName} obrigado por apoiar o @{e.Channel}!");
        }
        private void _TwitchClient_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith("!") || e.ChatMessage.Username.ToLower().Equals("streamlabs"))
            {
                return;
            }
        }
        #endregion

        #region Callback de eventos do TwitchBot
        void TerminadoCallback(CallbackModel model)
        {
            if (model.JogoEncerrado)
            {
                Jogo = null;
            }

            _TwitchClient.SendMessage(BotAppSettings.ChannelName, model.Mensagem);
        }
        void ErroCallback(CallbackModel model)
        {
            if (model.JogoEncerrado)
            {
                Jogo = null;
            }

            _TwitchClient.SendMessage(BotAppSettings.ChannelName, model.Mensagem);
        }
        #endregion

        #region Metodos de interação especificos com o TwitchBot
        void ValidarComando(Tuple<string, string, string> comando)
        {
            var botcomando = comando.Item1.ToLower();

            switch (botcomando)
            {
                case "rank":
                    VerificarRank(comando);
                    break;
                case "piada":
                    FazerPiada(comando);
                    break;
                case "coments":
                    FazerComentario(comando);
                    break;
                case "meme":
                    RodarMeme(comando);
                    break;
                case "iniciarjogo":
                case "participar":
                case "chute":
                case "resposta":
                    Jogar(comando);
                    break;
                case "imagem":
                    MostrarImagem(comando);
                    break;
                default:
                    break;
            }
        }


        public void Jogar(Tuple<string, string, string> comando)
        {
            if (Jogo == null)
            {
                var jogoAleatorio = new Random().Next(_Jogos.Count);
                Jogo = JogoFactory.ObterJogo(_Jogos[jogoAleatorio]);
            }

            CallbackTerminadoDelegate callbackTerminadoDelegate = new CallbackTerminadoDelegate(TerminadoCallback);
            CallbackErroDelegate callbackErroDelegate = new CallbackErroDelegate(ErroCallback);

            Jogo.Jogada(
                comando.Item1,
                comando.Item2,
                comando.Item3,
                notificarBotCallback: callbackTerminadoDelegate,
                notificarErroBotCallback: callbackErroDelegate);
        }
        void MostrarImagem(Tuple<string, string, string> comando)
        {
            var imagemUrl = comando.Item2.Replace("!imagem", "").Trim();

            var imgPossuiConteudoExplicito = DeteccaoConteudoExplicitoService.Detectar(imagemUrl);

            if (imgPossuiConteudoExplicito)
            {
                _TwitchClient.SendMessage(BotAppSettings.ChannelName, $"@{comando.Item3} sua imagem não foi considerada adequada");
            }
            else
            {
                HubSingleton.Instance.ImagemMemeHubContext.Clients.All.exibirimagem(imagemUrl);
            }

        }
        void FazerPiada(Tuple<string, string, string> comando)
        {
            CallbackTerminadoDelegate callbackTerminadoDelegate = new CallbackTerminadoDelegate(TerminadoCallback);
            CallbackErroDelegate callbackErroDelegate = new CallbackErroDelegate(ErroCallback);
            Piada.Falar(new FalaModel
            {
                Jogador = comando.Item3
            }, notificarBotCallback: callbackTerminadoDelegate, notificarErroBotCallback: callbackErroDelegate);
        }
        void FazerComentario(Tuple<string, string, string> comando)
        {
            CallbackTerminadoDelegate callbackTerminadoDelegate = new CallbackTerminadoDelegate(TerminadoCallback);
            CallbackErroDelegate callbackErroDelegate = new CallbackErroDelegate(ErroCallback);

            var texto = comando.Item2.Replace("!coments", "");
            Comentario.Falar(new FalaModel
            {
                Jogador = comando.Item3,
                TextoParaFala = texto,
            }, notificarErroBotCallback: callbackErroDelegate);
        }
        void RodarMeme(Tuple<string, string, string> comando)
        {
            CallbackTerminadoDelegate callbackTerminadoDelegate = new CallbackTerminadoDelegate(TerminadoCallback);
            CallbackErroDelegate callbackErroDelegate = new CallbackErroDelegate(ErroCallback);
            var texto = comando.Item2.Replace("!meme", "").Trim();
            Memes.Falar(new FalaModel
            {
                Jogador = comando.Item3,
                TextoParaFala = texto,
            }, notificarErroBotCallback: callbackErroDelegate);
        }
        void VerificarRank(Tuple<string, string, string> comando)
        {
            CallbackTerminadoDelegate callbackTerminadoDelegate = new CallbackTerminadoDelegate(TerminadoCallback);
            CallbackErroDelegate callbackErroDelegate = new CallbackErroDelegate(ErroCallback);
            Task.Run(async () =>
            {
                await _RankingRepositorio.ObterJogador(comando.Item3, callBackTerminado: callbackTerminadoDelegate, callBackErro: callbackErroDelegate);
            });
        }
        #endregion
    }
}
