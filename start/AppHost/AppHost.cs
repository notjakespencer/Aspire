using Aspire.Hosting.GitHub;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight();

var githubModel = builder.AddGitHubModel("chat-model", GitHubModel.OpenAI.OpenAIGpt4oMini);

var weatherApi = builder.AddExternalService("weather-api", "https://api.weather.gov");

var keyVault = builder.AddAzureKeyVault("key-vault");

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false);
    // .WithInitFiles("./database-init")
    // .WithLifetime(ContainerLifetime.Persistent);

var weatherDb = postgres.AddDatabase("weatherDb");

var api = builder.AddProject<Projects.Api>("api")
    .WithReference(weatherApi)
    .WithReference(cache)
    .WithReference(keyVault);

var web = builder.AddProject<Projects.MyWeatherHub>("myweatherhub")
    .WithReference(api)
    .WithExternalHttpEndpoints()
    .WithReference(keyVault)
    .WithReference(githubModel)
    .WithReference(weatherDb);

builder.Build().Run();


// vckv25
// rg_func_vc
// 22ce4ac0-aacc-4579-b287-49c8335d9e16