using APICatalogo.Context;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using APICatalogo.log;

namespace ApiCatalogoUnitTests.UnitTests
{
    public class ProdutoUnitTestController
    {
        public IUnitOfWork repository;
        public IMapper mapper;
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "User ID=postgres;Password=12345678;Host=localhost;Port=5432;Database=CatalogoDB;";

        static ProdutoUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options;
        }

        public ProdutoUnitTestController()
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();

                builder.AddCustomLogger(config =>
                {
                    config.LogLevel = LogLevel.Information;
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new APICatalogo.DTO.Mappings.ProdutoDTOMappingProfile());
            }, loggerFactory);

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);
            repository = new UnitOfWork(context);
        }
    }
}