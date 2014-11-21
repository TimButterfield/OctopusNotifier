using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusNotifier.Console
{
    public class OctopusServer
    {
        private readonly string uri;
        private readonly string apiKey;
        private static OctopusServerEndpoint endpoint;
        private static OctopusRepository repository;

       public OctopusServer(string uri, string apiKey)
       {
           this.uri = uri;
           this.apiKey = apiKey;
           repository = repository ?? (repository = GetOctopusRepository());
       }

        private OctopusRepository GetOctopusRepository()
        {
            if (endpoint == null) 
                endpoint = new OctopusServerEndpoint(uri, apiKey);

            return new OctopusRepository(endpoint);
        }

        public DashboardResource Dashboard
        {
            get { return repository.Dashboards.GetDashboard(); } 
        }
    }
}