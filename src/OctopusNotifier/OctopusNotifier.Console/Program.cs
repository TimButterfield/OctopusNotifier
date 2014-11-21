using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using Octopus.Client.Model;
using OctopusNotifier.Console.Domain;
using OctopusNotifier.Console.Notification.Criteria;
using Timer = System.Timers.Timer;

namespace OctopusNotifier.Console
{
    class Program
    {
        private static Preferences preferences;
        private static OctopusSettings octopusSettings; 
        private static NotificationFactory notificationFactory;
        private static DashboardResource dashboard;
        private static OctopusServer octopusServer;

        static void Main(string[] args)
        {
       
            using (var fileReader = new StreamReader("Config/preferences.json"))
            {
                var configuration = fileReader.ReadToEnd();
                preferences = JsonConvert.DeserializeObject<Preferences>(configuration);             
            }

            using (var fileReader = new StreamReader("Config/octopus.json"))
            {
                var configuration = fileReader.ReadToEnd();
                octopusSettings = JsonConvert.DeserializeObject<OctopusSettings>(configuration);
            }

            notificationFactory = new NotificationFactory(preferences);
            octopusServer = new OctopusServer(octopusSettings.Uri, octopusSettings.ApiKey);

            var timer = new Timer();
            timer.Elapsed += ProcessDeployments;
            timer.Interval = preferences.PollingInterval;
            timer.Enabled = true; 

            timer.Start();

            while (System.Console.ReadLine() != "q"); 
        }


        private static void ProcessDeployments(object source, ElapsedEventArgs e)
        {
            dashboard = octopusServer.Dashboard; 
   
            var environments = GetMatchingEnvironments(dashboard);
            var projects = GetMatchingProjects(dashboard); 
            
            var filteredDeployments = FilterDeploymentsByPreference(dashboard, projects, environments); ;

            ProcessDeployments(filteredDeployments, environments, projects);
        }


        private static IEnumerable<DashboardProjectResource> GetMatchingProjects(DashboardResource dashboard)
        {
            return dashboard.Projects.Where(x => preferences.Projects.Select(project => project.Name).Contains(x.Name));
        }


        private static IEnumerable<DashboardEnvironmentResource> GetMatchingEnvironments(DashboardResource dashboard)
        {
            var environmentsOfInterest = preferences.Projects.SelectMany(project => project.Environments).Select(x => x.Name);

            var environments = dashboard.Environments.Where(environmet => environmentsOfInterest.Contains(environmet.Name));
            return environments;
        }
        

        private static IEnumerable<DashboardItemResource> FilterDeploymentsByPreference(DashboardResource dashboard, IEnumerable<DashboardProjectResource> projects, IEnumerable<DashboardEnvironmentResource> environments)
        {
            var matches = dashboard.Items.Where(x => projects.Any(project => project.Id == x.ProjectId));
            matches = matches.Where(x => environments.Any(environment => environment.Id == x.EnvironmentId));
            System.Console.WriteLine("Found {0} matching projects", matches.Count());

            return matches.OrderBy(project => project.ProjectId);
        }


        private static void ProcessDeployments(IEnumerable<DashboardItemResource> deploymentsOfInterest, IEnumerable<DashboardEnvironmentResource> environments, IEnumerable<DashboardProjectResource> projects)
        {
            foreach (var deploymentItem in deploymentsOfInterest)
            {
                IEnumerable<DashboardEnvironmentResource> dashboardEnvironmentResources = environments as DashboardEnvironmentResource[] ?? environments.ToArray();
                var environment = dashboardEnvironmentResources.FirstOrDefault(x => x.Id == deploymentItem.EnvironmentId);
                
                IEnumerable<DashboardProjectResource> dashboardProjectResources = projects as DashboardProjectResource[] ?? projects.ToArray();
                var project = dashboardProjectResources.FirstOrDefault(x => x.Id == deploymentItem.ProjectId);

                var deployment = Deployment.Of(project.Name, environment.Name)
                                    .HasState(deploymentItem.State.ToString());
                

                var cachedInstance = DeploymentCache.GetDeployment(deployment);
                if (cachedInstance == null)
                {
                    DeploymentCache.Cache(deployment);
                    continue;
                }

                var notificationSpecs = notificationFactory.GetNotificationCriteria(project, environment);
                
                foreach (var notificationSpec in notificationSpecs.Where(notificationSpec => notificationSpec.IsSatisfiedBy(deployment)))
                {
                    Notifier.Notify(deployment);
                }

                if (cachedInstance.HasChangedState(deployment.State))
                {
                    DeploymentCache.Cache(deployment);
                }
            }
        }
    }
}
