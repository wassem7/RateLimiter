using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", opts =>
    {
        opts.Window = TimeSpan.FromSeconds(5);
        opts.PermitLimit = 3;
        opts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opts.QueueLimit = 10;

    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddSlidingWindowLimiter("sliding", option =>
    {
        option.Window = TimeSpan.FromSeconds(10);
        option.PermitLimit = 1;
        option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        option.SegmentsPerWindow = 3;
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddConcurrencyLimiter("concurrency", option =>
    {
        option.PermitLimit = 2;
        option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        option.QueueLimit = 2;
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddTokenBucketLimiter("tokenbucket", option =>
    {
        option.QueueLimit = 2;
        option.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
        option.ReplenishmentPeriod = TimeSpan.FromSeconds(20);
        option.TokensPerPeriod = 3;
        option.AutoReplenishment = true;
        option.TokenLimit = 5;
        
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();
app.MapControllers();

app.Run();