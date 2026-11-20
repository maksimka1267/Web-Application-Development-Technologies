var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTTP-клиенты для взаимодействия между микросервисами

builder.Services.AddHttpClient("Products", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:Products"]!
    );
});

builder.Services.AddHttpClient("Customers", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:Customers"]!
    );
});

builder.Services.AddHttpClient("Orders", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:Orders"]!
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();