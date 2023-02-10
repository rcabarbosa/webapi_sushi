using Microsoft.EntityFrameworkCore;
using SushiStore.Models;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SushiDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
 c.SwaggerDoc("v1", new OpenApiInfo {
 Title = "SushiStore API",
 Description = "Making the Sushis you love",  Version = "v1" });
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
 c.SwaggerEndpoint("/swagger/v1/swagger.json", "SushiStore API V1");
});
app.MapGet("/", () => "Hello World!");
app.MapGet("/sushis", async (SushiDb db) => await db.Sushis.ToListAsync());
app.MapPost("/sushi", async (SushiDb db, Sushi sushi) =>
{
 await db.Sushis.AddAsync(sushi);
 await db.SaveChangesAsync();
 return Results.Created($"/sushi/{sushi.Id}", sushi);
});
app.MapGet("/sushi/{id}", async (SushiDb db, int id) => await 
db.Sushis.FindAsync(id));
app.MapPut("/sushi/{id}", async (SushiDb db, Sushi updatesushi, 
int id) =>
{
 var sushi = await db.Sushis.FindAsync(id);
 if (sushi is null) return Results.NotFound();
 sushi.Name = updatesushi.Name;
 sushi.Description = updatesushi.Description;
 await db.SaveChangesAsync();
 return Results.NoContent();
});
app.MapDelete("/sushi/{id}", async (SushiDb db, int id) =>
{
 var sushi = await db.Sushis.FindAsync(id);
 if (sushi is null)
 {
 return Results.NotFound();
 }
 db.Sushis.Remove(sushi);
 await db.SaveChangesAsync();
 return Results.Ok();
});
app.Run();