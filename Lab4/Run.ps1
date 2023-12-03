Invoke-WebRequest -Uri https://github.com/loic-sharma/BaGet/releases/download/v0.4.0-preview2/BaGet.zip -OutFile BaGet.zip
Expand-Archive -Path BaGet.zip -DestinationPath BaGet
Remove-Item BaGet.zip
cd BaGet
Start-Process dotnet -ArgumentList BaGet.dll
cd ..\ConsoleApplication
dotnet pack -o .
dotnet nuget push AShevchenko.1.0.0.nupkg -s http://localhost:5000/v3/index.json
Remove-Item AShevchenko.1.0.0.nupkg, bin, obj -Recurse -Force
cd ..\Labs
Remove-Item bin, obj -Recurse -Force
cd ..
vagrant up mac
vagrant halt mac
vagrant up linux
vagrant halt linux
vagrant up windows