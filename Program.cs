using System.IO;
using Microsoft.EntityFrameworkCore;
using SimpleDataEntryForm.Data;
using SimpleDataEntryForm.Models;

var builder = WebApplication.CreateBuilder(args);

// Build an absolute path
var dbDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
var dbPath = Path.Combine(dbDir, "data.db");
Directory.CreateDirectory(dbDir);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}")
       .EnableDetailedErrors());

var app = builder.Build();

// Apply migrations at runtime
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/submit", async (HttpRequest req, AppDbContext db) =>
{
    var form = await req.ReadFormAsync();
    var name = form["name"].ToString().Trim();
    var title = form["title"].ToString().Trim();
    var ageStr = form["age"].ToString().Trim();
    var hometown = form["hometown"].ToString().Trim();

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(title))
        return Results.BadRequest("Name and Title are required.");

    int? age = int.TryParse(ageStr, out var a) ? a : null;

    db.DataEntries.Add(new DataEntry
    {
        Name = name,
        Title = title,
        Age = age,
        Hometown = string.IsNullOrWhiteSpace(hometown) ? null : hometown
    });
    await db.SaveChangesAsync();

    return Results.Redirect("/confirmation/index.html");
});

app.MapGet("/api/entries/recent", async (AppDbContext db) =>
    await db.DataEntries
        .OrderByDescending(e => e.Id)
        .Take(10)
        .Select(e => new {
            e.Id,
            e.Name,
            e.Title,
            e.Age,
            e.Hometown
        })
        .ToListAsync()
);

app.Run();
