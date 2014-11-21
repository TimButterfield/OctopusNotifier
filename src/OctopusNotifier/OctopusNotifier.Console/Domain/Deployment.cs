using System;
using Newtonsoft.Json;

namespace OctopusNotifier.Console.Domain
{
    public class Deployment
    {
        public int Id { get; private set; }
        public string Project { get; private set; }
        public string Environment { get; private set; }
        public string State { get; private set; }

        [JsonConstructor]
        public Deployment(string project, string environment)
        {
            Project = project;
            Environment = environment;
        }

        public static Deployment Of(string project, string environment)
        {
            return new Deployment(project, environment);
        }

        public Deployment HasState(string state)
        {
            State = state;
            return this; 
        }

        public bool HasChangedState(string latestState)
        {
            var hasChanged = !State.Equals(latestState, StringComparison.InvariantCultureIgnoreCase);
            return hasChanged; 
        }

        protected bool Equals(Deployment other)
        {
            return string.Equals(Project, other.Project) && string.Equals(Environment, other.Environment) && string.Equals(State, other.State);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Deployment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Project != null ? Project.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Environment != null ? Environment.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (State != null ? State.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool IsNotEqualTo(Deployment mostRecentDeployment)
        {
            return !Equals((object)mostRecentDeployment);
        }

    }
}