cd Algorithms
dotnet pack -o ..\LocalNuGet
rm bin, obj -Recurse -Force
cd ..\ConsoleApplication
dotnet build