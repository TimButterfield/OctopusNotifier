# OctopusNotifier

###A customizable toast style notifier that informs you of Deployment state/changes. 

OctopusDeploy is a fantastic deployment tool. I've used it for approximately 8 months but one of it's short comings is the notification system. By comparison the JetBrains TeamCity ships with the option of using a system tray notifier to keep you up to speed with the state of one more configurable builds/deployments. OctopusNotifier is a very basic attempt at trying to create notification mechanism with some individual flexibility (i.e. user specific preferences) that is similar to the TeamCity tray notifier. 

It's currently vanilla, naive and ignores caching and performance considerations. It runs as a noddy console app. However it will poll your Octopus server, monitor changes in the deployment state for a given project and environment and when required produce a toast style notification. 

### Dependencies
1. Growl for windows

### Getting Started

1. Install Growl for Windows - http://www.growlforwindows.com/gfw/
2. Download and install one of the growl displays http://www.growlforwindows.com/gfw/displays.aspx (optional). 
3. Update the octopus.json file with the uri of your Octopus Server and the api key
4. Update the prefernces.json file with the appropriate polling (milliseconds), project, environment and notification settings. 
5. Compile and run


