using SirenaDataHarvesting;
using SirenaDataHarvesting.Services.GenericRepository;
using SirenaDataHarvesting.Services.ProductService;
using SirenaDataHarvesting.Services.ScraperService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IScraperService, ScraperService>();
    })
    .Build();

await host.RunAsync();
