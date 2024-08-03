using System.Text.Json.Serialization;
using Dometrain.EFCore.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        // for age enum
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add a DbContext here
builder.Services.AddDbContext<MoviesContext>();


var app = builder.Build();


// FIXME: dirty hack
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
// context.Database.EnsureDeleted(); // nuke everything
// context.Database.EnsureCreated(); // generate the Movies table using the model as schema


// Apply any pending migration, BUT this gives this application Db privileges to modify the schema
// and that violates the principle of least privilege
// await context.Database.MigrateAsync(); // apply any pending migration


// Check if there are any pending migrations
var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
if (pendingMigrations.Any())
{
    throw new Exception("There are pending migrations. Please apply them before running the application.");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();