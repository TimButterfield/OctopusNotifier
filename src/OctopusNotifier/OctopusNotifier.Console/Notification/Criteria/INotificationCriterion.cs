using OctopusNotifier.Console.Domain;

namespace OctopusNotifier.Console.Notification.Criteria
{
    public interface INotificationCriterion
    {
        bool IsSatisfiedBy(Deployment latestDeployment); 
    }
}