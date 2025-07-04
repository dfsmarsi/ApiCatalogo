
namespace APICatalogo.log
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId} - {formatter(state, exception)}";

            EscreverLogNoArquivo(mensagem);
        }

        private void EscreverLogNoArquivo(string mensagem)
        {
            try
            {
                string caminhoArquivoLog = Path.Combine(AppContext.BaseDirectory, "logs", "log.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivoLog));

                using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
                {
                    streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao escrever log: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
