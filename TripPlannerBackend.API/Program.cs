using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TripPlannerBackend.API.Services;
using TripPlannerBackend.DAL;
using TripPlannerBackend.DAL.Initializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var connectionString =
    $"Server={dbServer};Initial Catalog={dbName};User ID={dbUser};Password={dbPassword};";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
Console.WriteLine(connectionString);
builder.Services.AddDbContext<TripPlannerDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminRights", policy =>
                      policy.RequireClaim("permissions", "admin:crud"));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddSwaggerService();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://angular-23-team-02-git-bm-hosting-branch-bmelis-projects.vercel.app", "https://angular-23-team-02.vercel.app")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddScoped<TripAuthorizationService>();
builder.Services.AddScoped<PublicImageManagementService>();

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

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