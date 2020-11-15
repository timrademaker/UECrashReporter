# UECrashReporter
 Custom crash reporter, aimed at games made in Unreal Engine.
 
 Instead of sending crashdumps to a server ([like you would with the original crash reporter](https://docs.unrealengine.com/en-US/Engine/Tools/CrashReporter/index.html)), this crash reporter sends the generated files to a Discord webhook.
 
 To use this tool for your own project, open `Discord.cs`, replace the value of `s_WebhookUrl` with your own webhook URL and build the project. Retrieve the generated `CrashReportClient.exe` file, and place this in your game build's folder under `Engine/Binaries/[Platform]`.
 

 Inspiration taken from [Teal](http://www.teal-game.com/blog/customcrashreporter/)
