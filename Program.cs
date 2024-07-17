using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using ocp_blog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Parse PostgreSQL URL and configure DbContext to use PostgreSQL
var databaseUrl = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = ConvertPostgresUrlToConnectionString(databaseUrl);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static string ConvertPostgresUrlToConnectionString(string url)
{
    var uri = new Uri(url);
    var userInfo = uri.UserInfo.Split(':');
    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.IsDefaultPort ? 5432 : uri.Port,
        Database = uri.AbsolutePath.Trim('/'),
        Username = userInfo[0],
        Password = userInfo[1],
        SslMode = SslMode.Require, // Adjust based on your requirements
        TrustServerCertificate = true // Adjust based on your requirements
    };
    return builder.ToString();
}
