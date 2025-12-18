using MyWeatherHub;
using MyWeatherHub.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<NwsManager>(
    static client => client.BaseAddress = new("https+http://api"));

// Add GitHub Models chat client
builder.AddAzureChatCompletionsClient("chat-model")
       .AddChatClient();


builder.Services.AddScoped<ForecastSummarizer>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMemoryCache();

builder.AddNpgsqlDbContext<MyWeatherContext>(connectionName: "weatherDb");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MyWeatherContext>();
        await context.Database.EnsureCreatedAsync();
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
