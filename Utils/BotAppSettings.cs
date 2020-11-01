using System.Configuration;

namespace DanielDevBot.Utils
{
    public static class BotAppSettings
    {
        public static string TwitchUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["twitch_username"];
            }
        }
        public static string AccessToken
        {
            get
            {
                return ConfigurationManager.AppSettings["access_token"];
            }
        }

        public static string ChannelName
        {
            get
            {
                return ConfigurationManager.AppSettings["channel_name"];
            }
        }

        public static string PalavraApi
        {
            get
            {
                return ConfigurationManager.AppSettings["palavra_api"];
            }
        }

        public static string RankingDB
        {
            get
            {
                return ConfigurationManager.AppSettings["ranking_json"];
            }
        }

        public static string TreinamentoCSV
        {
            get
            {
                return ConfigurationManager.AppSettings["treinamento_csv"];
            }
        }


        public static string PiadaApi
        {
            get
            {
                return ConfigurationManager.AppSettings["piada_api"];
            }
        }

        public static string XRapidApiBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["x_rapid_api_base_url"];
            }
        }

        public static string XRapidApiHost
        {
            get
            {
                return ConfigurationManager.AppSettings["x_rapid_api_host"];
            }
        }

        public static string XRapidApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["x_rapid_api_key"];
            }
        }
        
         public static string ReacoesPath
        {
            get
            {
                return ConfigurationManager.AppSettings["reacoes_path"];
            }
        }

        public static string MemesPath
        {
            get
            {
                return ConfigurationManager.AppSettings["memes_path"];
            }
        }
        public static string DBPath
        {
            get
            {
                return ConfigurationManager.AppSettings["db_path"];
            }
        }
    }
}
