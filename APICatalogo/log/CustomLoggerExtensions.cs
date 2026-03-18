namespace APICatalogo.log
{
    // A classe precisa ser estática
    public static class CustomLoggerExtensions
    {
        // O "this ILoggingBuilder builder" indica que estamos estendendo a funcionalidade de log do .NET
        public static ILoggingBuilder AddCustomLogger(
            this ILoggingBuilder builder,
            Action<CustomLoggerProviderConfiguration> configure)
        {
            // 1. Cria a configuração com os valores padrão
            var config = new CustomLoggerProviderConfiguration();

            // 2. Aplica as alterações que o usuário passar (ex: mudar o LogLevel)
            configure(config);

            // 3. Instancia o seu provedor e o adiciona ao contêiner de logs do .NET
            builder.AddProvider(new CustomLoggerProvider(config));

            return builder;
        }
    }
}