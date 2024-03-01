using ServiceContracts;
using Services;
using StocksApp.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
