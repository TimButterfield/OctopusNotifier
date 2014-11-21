using System;
using Growl.Connector;
using OctopusNotifier.Console.Domain;

namespace OctopusNotifier.Console
{
    public static class Notifier
    {
        private static readonly GrowlConnector GrowlConnector;

        static Notifier()
        {
            GrowlConnector = new GrowlConnector {EncryptionAlgorithm = Cryptography.SymmetricAlgorithmType.PlainText};
            var application = new Application("Octopus Deploy");
            GrowlConnector.Register(application, new[] { new NotificationType("Deployment Failure"), new NotificationType("Deployment Success")});
        }

        public static void Notify(Deployment deployment)
        {
            var messsage = string.Format("Deployment of {0} to environmnet {1} {2}", deployment.Project, deployment.Environment, GetStateForMessage(deployment));
            GrowlConnector.Notify(new Growl.Connector.Notification("Octopus Deploy", "Deployment Failure", Guid.NewGuid().ToString(), "Deployment Notification",  messsage));
        }

        private static string GetStateForMessage(Deployment deployment)
        {
            return deployment.State.Equals("Success", StringComparison.OrdinalIgnoreCase) ? "Succeeded" : deployment.State.Equals("Failure") ? "Failed" : deployment.State;
        }
    }
}