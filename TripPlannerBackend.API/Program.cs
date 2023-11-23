using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Initializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<TripPlannerDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TripReadAccess", policy =>
                          policy.RequireClaim("permissions", "read:trip"));
    options.AddPolicy("TripWriteAccess", policy =>
                          policy.RequireClaim("permissions", "create:trip", "update:trip"));
    options.AddPolicy("TripDeleteAccess", policy =>
                      policy.RequireClaim("permissions", "delete:trip"));
});

builder.Services.AddControllers();
builder.Services.AddSwaggerService();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var tripContext = scope.ServiceProvider.GetRequiredService<TripPlannerDbContext>();
    DBInitializer.Initialize(tripContext);
}

app.Run();
