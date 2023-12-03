using Labs;
using McMaster.Extensions.CommandLineUtils;

namespace ConsoleApplication;


internal static class Program
{
    private static readonly string[] _labNames = ["lab1", "lab2", "lab3"];


    private static int Main(string[] args)
    {
        var app = new CommandLineApplication
        {
            UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw
        };
        app.HelpOption("-? | --help");

        app.Command("version", versionCommand =>
        {
            versionCommand.Description = "Displays information about the program";
            versionCommand.OnExecute(() =>
            {
                DisplayVersion();
                return 0;
            });
        });

        app.Command("run", runCommand =>
        {
            runCommand.Description = "Runs a lab";

            foreach (var labName in _labNames)
            {
                runCommand.Command(labName, labCommand =>
                {
                    var inputOption = labCommand.Option(
                        "-i | --input",
                        "Input file",
                        CommandOptionType.SingleValue
                    );
                    var outputOption = labCommand.Option(
                        "-o | --output",
                        "Output file",
                        CommandOptionType.SingleValue
                    );

                    labCommand.OnExecute(() =>
                    {
                        return RunLab(labName, inputOption.Value(), outputOption.Value());
                    });
                });
            }

            runCommand.OnExecute(() =>
            {
                Console.WriteLine("Error: Unknown command.");
                runCommand.ShowHelp();
            });
        });

        app.Command("set-path", setPathCommand =>
        {
            setPathCommand.Description =
                "Sets the path to the folder with input and output files";
            var pathOption = setPathCommand.Option(
                "-p | --path",
                "Path to the folder",
                CommandOptionType.SingleValue
            )
                .IsRequired();

            setPathCommand.OnExecute(() =>
            {
                SetPath(pathOption.Value()!);
                return 0;
            });
        });

        app.OnExecute(() =>
        {
            Console.WriteLine("Error: Unknown command.");
            app.ShowHelp();
            return 1;
        });


        try
        {
            return app.Execute(args);
        }
        catch (CommandParsingException exception)
        {
            Console.WriteLine(exception.Message);
            app.ShowHelp();
        }
        catch
        {
            app.ShowHelp();
        }
        return 1;
    }


    private static void DisplayVersion()
    {
        Console.WriteLine("Author: Andrey Shevchenko");
        Console.WriteLine("Version: 1.0.0");
    }


    private static int RunLab(string labName, string? inputPath, string? outputPath)
    {
        inputPath = ResolvePath(inputPath, "INPUT.TXT");
        outputPath = ResolvePath(outputPath, "OUTPUT.TXT");

        Console.WriteLine($"""
            Running practical {labName}
            with input file {inputPath}
            and output file {outputPath}
            """);

        string text;
        try
        {
            text = File.ReadAllText(inputPath);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error reading file. {exception.Message}");
            return 1;
        }

        string result;
        try
        {
            switch (labName.ToLower())
            {
                case "lab1":
                    result = Lab1.Run(text);
                    break;
                case "lab2":
                    result = Lab2.Run(text);
                    break;
                case "lab3":
                    result = Lab3.Run(text);
                    break;
                default:
                    result = null!;
                    break;
            }
        }
        catch (Exception exception)
        {
            result = exception.Message;
        }

        try
        {
            File.WriteAllLines(outputPath, [result]);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error writing file. {exception.Message}");
            return 1;
        }

        return 0;
    }


    private static string ResolvePath(string? path, string defaultName)
    {
        if (path is not null)
        {
            return path;
        }

        var directoryPath =
            Environment.GetEnvironmentVariable("LAB_PATH")
            ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Join(directoryPath, defaultName);
    }


    private static void SetPath(string path)
    {
        Environment.SetEnvironmentVariable(
            "LAB_PATH",
            path,
            EnvironmentVariableTarget.User
        );
        Console.WriteLine($"Path to the folder with input and output files set: {path}");
    }
}