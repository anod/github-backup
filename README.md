# github-backup

.NET 6 console app for backup GitHub repositories by cloning and fetching

Multi-platform: Tested with `win-arm64` and `linux-armv7l`

# How to use

1. Generate personal access token https://github.com/settings/tokens
2. Create appsettings.<env>.json with `BackupFolder` and `GitHubToken`
3. Execute `dotnet run --environment <env>`
