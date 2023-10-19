using SirenaDataHarvesting;
using SirenaDataHarvesting.Services.CategoryService;
using SirenaDataHarvesting.Services.GenericRepository;
using SirenaDataHarvesting.Services.ProductService;
using SirenaDataHarvesting.Services.ScraperService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IScraperService, ScraperService>();
    })
    .Build();

await host.RunAsync();
