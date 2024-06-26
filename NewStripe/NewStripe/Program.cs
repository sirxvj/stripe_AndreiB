using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

StripeConfiguration.ApiKey = "sk_test_51PVu8U2MQA9DJDKIdgZibGUcfghhxkqZHMzIfjfvxIAyJY4lFGWGbw0H95fHYDReIFdXKftkwl97ynleYHqmTC9d00J6q4ibIh";
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

