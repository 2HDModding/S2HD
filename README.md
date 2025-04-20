# S2HD

* A decompilation of Sonic 2 HD.

* Runs on x64 only (sadly)

# Tools used

I used dotpeek to export the project and dnSpy to reference the scripts while fixing them, I also used [this](https://github.com/maybekoi/FSNSFix) to turn "namespace S2HD;" to "namespace S2HD {".

## How to build

Install the following dependencies:
- [.NET Framework 4.5.2](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net452-developer-pack-offline-installer)
- [Microsoft XNA Framework Redistributable 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=20914)

Then clone the project, open the Visual Studio solution file, and you should be good to go!