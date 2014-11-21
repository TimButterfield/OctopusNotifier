using System;
using System.Collections.Generic;
using System.Linq;
using OctopusNotifier.Console.Domain;

namespace OctopusNotifier.Console.Notification.Criteria
{
    public static class DeploymentCache
    {
        private static List<Deployment> deployments;

        static DeploymentCache()
        { 
            deployments = new List<Deployment>();
        }

        public static bool ExistsInCache(Deployment deployment)
        {
            return deployments.Any(x => x.Project.Equals(deployment.Project, StringComparison.InvariantCultureIgnoreCase)
                                        && x.Environment.Equals(deployment.Environment, StringComparison.InvariantCultureIgnoreCase)); 
        }

        public static Deployment GetDeployment(Deployment deployment)
        {
            return deployments == null ? null : deployments.FirstOrDefault(x => x.Project == deployment.Project && x.Environment == deployment.Environment);
        }


        public static void Cache(Deployment deployment)
        {
            if (deployments.Contains(deployment))
                deployments.Remove(deployment); 

            deployments.Add(deployment);
        }
    }
}