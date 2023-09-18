using System.Globalization;
using DotNet6TestCacheRedis.Application;
using DotNet6TestCacheRedis.Domain;
using DotNet6TestCacheRedis.Repository;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepPerson, RepPerson>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonApplication, PersonApplication>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
    options.InstanceName = "SampleInstance";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.Lifetime.ApplicationStarted.Register(() =>
{
    var currentTimeUtc = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
    byte[] encodedCurrentTimeUtc = System.Text.Encoding.UTF8.GetBytes(currentTimeUtc);
    var options = new DistributedCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromSeconds(200));
    app.Services.GetService<IDistributedCache>()
        ?.Set("cachedTimeUTC", encodedCurrentTimeUtc, options);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
