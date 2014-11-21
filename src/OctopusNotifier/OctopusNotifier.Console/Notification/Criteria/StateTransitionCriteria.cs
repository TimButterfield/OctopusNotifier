using System;
using OctopusNotifier.Console.Domain;

namespace OctopusNotifier.Console.Notification.Criteria
{
    public class StateTransitionCriteria : INotificationCriterion
    {
        private readonly string newDeploymentState;

        public StateTransitionCriteria(string newDeploymentState)
        {
            this.newDeploymentState = newDeploymentState;
        }

        public bool IsSatisfiedBy(Deployment latestDeployment)
        {
            return DeploymentCache.GetDeployment(latestDeployment).HasChangedState(latestDeployment.State) && latestDeployment.State.Equals(newDeploymentState, StringComparison.InvariantCultureIgnoreCase); 
        }
    }
}