namespace IslamiTextsProcessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"A path to config file was not provided.");
                Console.WriteLine();
                DisplayUsage();
                return;
            }

            string configPath = args[0];
            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Error: Config file not found: {configPath}");
                Console.WriteLine();
                DisplayUsage();
                return;
            }

            IslamiTextsProcessorConfig config = GetAndValidateConfig(configPath);
            ConfigRuntime.ConvertVerses(config);
        }

        public static IslamiTextsProcessorConfig GetAndValidateConfig(string configPath)
        {
            IslamiTextsProcessorConfig config = IslamiTextsProcessorConfig.Deserialize(configPath);
            if (config.Operation == null || !config.Operation.Equals("Convert-Verses", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Invalid operation specified in the config: {config.Operation}");
            }

            if (config.Input == null)
            {
                throw new InvalidOperationException("InputSources are not specified.");
            }

            if (config.Output == null)
            {
                throw new InvalidOperationException("OutputSources are not specified.");
            }

            if (!Directory.Exists(config.Input.Path))
            {
                throw new InvalidOperationException(
                    $"The following Path specified as InputSource does not exist: {config.Input.Path}");
            }

            if (!Directory.Exists(config.Output.Path))
            {
                throw new InvalidOperationException(
                    $"The following Path specified in OutputSources does not exist: {config.Output.Path}");
            }

            return config;
        }

        private static void DisplayUsage()
        {
            string HelpText = @"Provide only one parameter to itp.exe which is the path to the config file.";
            Console.WriteLine(HelpText);
        }
    }
}