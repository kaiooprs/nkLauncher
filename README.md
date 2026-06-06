# nkLauncher

A minimalistic, terminal-based Minecraft: Java Edition launcher built natively with C# (.NET).

## About The Project
This is an open-source project created for educational purposes (learning C# and OOP) and personal use. It focuses on being extremely fast, lightweight, and completely free of heavy graphical interfaces (like Electron). 

It runs seamlessly on both Linux and Windows terminals.

### Built With
* [.NET / C#](https://dotnet.microsoft.com/)
* [CmlLib.Core](https://github.com/CmlLib/CmlLib.Core) - For core Minecraft launching logic.
* [Spectre.Console](https://spectreconsole.net/) - For the beautiful CLI interface.
* [MSAL (Microsoft Authentication Library)](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) - For secure, interactive OAuth login.

---

**⚠️ Note for Microsoft App Reviewers:** 
If you are reviewing the Client ID application for this project, please note that this is a legitimate, non-commercial custom launcher. It strictly uses the OAuth Device/Interactive flow to allow legitimate owners of the game to securely authenticate directly from their terminal without exposing credentials. It is meant for personal use and close friends.
