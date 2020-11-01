using DanielDevBot.Hubs;
using Microsoft.AspNet.SignalR;

namespace DanielDevBot.Singleton
{
    public class HubSingleton
    {
        private static HubSingleton _instance;
        public static HubSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HubSingleton();
                }
                return _instance;
            }
        }

        private static IHubContext _ForcaHubContext;
        public IHubContext ForcaHubContext
        {
            get
            {
                if (_ForcaHubContext == null)
                {
                    _ForcaHubContext = GlobalHost.ConnectionManager.GetHubContext<ForcaHub>();
                }
                return _ForcaHubContext;
            }
        }


        private static IHubContext _ImagemMemeHubContext;
        public IHubContext ImagemMemeHubContext
        {
            get
            {
                if (_ImagemMemeHubContext == null)
                {
                    _ImagemMemeHubContext = GlobalHost.ConnectionManager.GetHubContext<ImagemMemeHub>();
                }
                return _ImagemMemeHubContext;
            }
        }
    }
}
