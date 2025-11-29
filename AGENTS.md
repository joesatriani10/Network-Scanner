# Repository Guidelines

## Project Structure & Module Organization
- Solution: `Network Scanner.sln`
- App code: `Network Scanner/` (WinForms UI in `Form1.*`, entry in `Program.cs`)
- Resources: `Form1.resx`; binaries build to `Network Scanner/bin/<Configuration>/net9.0-windows/`

## Build, Test, and Development Commands
- `dotnet restore` — restore NuGet packages
- `dotnet build "Network Scanner.sln"` — compile the solution (Debug by default)
- `dotnet run --project "Network Scanner/Network Scanner.csproj"` — launch the WinForms app

## Coding Style & Naming Conventions
- C# 9+ targeting .NET 9 for Windows; prefer explicit types unless `var` improves clarity
- Indentation: 4 spaces; braces on new lines (Allman style per existing code)
- UI code: keep layout changes in `Form1.Designer.cs`; logic and event handlers in `Form1.cs`
- Avoid non-ASCII unless required; favor short, descriptive method/variable names

## Testing Guidelines
- No automated tests are currently present; add unit tests under a `Tests/` project when introducing logic changes
- For manual checks: build, run, scan a small range (e.g., `192.168.1.1-20`), try cancel, toggle host resolution, and export CSV

## Commit & Pull Request Guidelines
- Commit messages: imperative mood, concise (e.g., `Add performance options for network scans`, `Restyle UI for a modern look`)
- Keep commits focused; separate formatting/normalization from feature changes when possible
- PRs should include: brief summary, screenshots/GIFs for UI tweaks, steps to reproduce issues fixed, and any known limitations

## Security & Configuration Tips
- Running requires Windows with .NET 9 SDK and appropriate network permissions; firewalls can block ICMP/DNS
- Avoid storing secrets in the repo; no external network dependencies beyond system DNS/ICMP
