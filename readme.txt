#Octopus Notifier

# H5 Having worked with JetBrains TeamCity for a number of years I found the notification system in Octopus a little lacking. That is I found the tray notifier in TeamCity a great way to stay up to date with Build/Deployment state. 

OctopusNotifier is a very basic attempt at trying to create notification mechanism with some individual flexibility (i.e. user specific prefernces) that is similar to the TeamCity tray notifier. 

It's currently a vanilla, naive and ignores caching and performance considerations. But it can poll your Octopus server, monitor changes in the deployment state for a given project and environment and when required produce a toast style notification. 

# H3 Getting Started

Install Growl for Windows - http://www.growlforwindows.com/gfw/
Download and install one of the growl displays http://www.growlforwindows.com/gfw/displays.aspx (optional). 
Update the octopus.json file with the uri of your Octopus Server instance and api key
Update the prefernces.json file with the appropriate polling (milliseconds), project, environment and notification settings. 


