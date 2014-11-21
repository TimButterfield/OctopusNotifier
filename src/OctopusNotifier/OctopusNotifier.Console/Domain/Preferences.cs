using System.Collections.Generic;
using Newtonsoft.Json;

namespace OctopusNotifier.Console.Domain
{
    public class Preferences
    {
        [JsonProperty("pollingInterval")]
        public int PollingInterval { get; set; }
    
        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }
    }

    public class Project
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("environments")]
        public List<Environment> Environments { get; set; }
    }
    
    public class Environment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public List<Notification> Notifications { get; set; }
    }

    public class Notification
    {
        [JsonProperty("onTransitionTo")]
        public string OnTransitionTo { get; set; }

        [JsonProperty("hasState")]
        public string HasState { get; set; }
    }

    public class OctopusSettings
    {
        [JsonProperty("hostUri")]
        public string Uri { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}