using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using OctopusNotifier.Console.Domain;
using OctopusNotifier.Console.Notification.Criteria;

namespace OctopusNotifier.Console
{
    public class NotificationFactory
    {
        private readonly Preferences preferences;

        public NotificationFactory(Preferences preferences)
        {
            this.preferences = preferences;
        }

        public IEnumerable<INotificationCriterion> GetNotificationCriteria(DashboardProjectResource project, DashboardEnvironmentResource environment)
        {
            //TODO : Needs to be a registry/cache put in place to ensure memory is not abused

            var envs = preferences.Projects
                .Where(x => x.Name == project.Name)
                .SelectMany(x => x.Environments)
                .Where(x => x.Name == environment.Name);

            var notfiicaitonsForCurrentProjectAndEnvironment = envs.SelectMany(x => x.Notifications); 

            foreach (var notification in notfiicaitonsForCurrentProjectAndEnvironment) 
            {
                if (!String.IsNullOrEmpty(notification.OnTransitionTo))
                    yield return new StateTransitionCriteria(notification.OnTransitionTo);

                if (!String.IsNullOrEmpty(notification.HasState))
                    yield return new FixedStateCriteria(notification.HasState); 
            }; 
        }
    }
}