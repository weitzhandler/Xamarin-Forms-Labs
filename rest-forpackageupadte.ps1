gci "C:\Users\raven\.nuget\packages\XLabs*" | Remove-Items -Recurse -Force
gci .\Samples\XLabs.Test.NuGet\packages\XLabs* | Remove-Item -Recurse -Force
gci .\Samples\XLabs.Test.Upgrades.NuGet\packages\XLabs* | Remove-Item -Recurse -Force
.\tools\nuget.exe locals temp -clear