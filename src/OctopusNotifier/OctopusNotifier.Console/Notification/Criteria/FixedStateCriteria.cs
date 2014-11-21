using System;
using OctopusNotifier.Console.Domain;

namespace OctopusNotifier.Console.Notification.Criteria
{
    public class FixedStateCriteria : INotificationCriterion
    {
        private readonly string deploymentStateOfInterest;

        public FixedStateCriteria(string deploymentStateOfInterest)
        {
            this.deploymentStateOfInterest = deploymentStateOfInterest;
        }

        public bool IsSatisfiedBy(Deployment latestDeployment)
        {
            if (latestDeployment.State.Equals(deploymentStateOfInterest, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }
    }
}