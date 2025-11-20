using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentManagement.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sessions for simple auth
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// register MongoContext
builder.Services.AddSingleton<MongoContext>();

var app = builder.Build();

// Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MongoContext>();
    var col = ctx.Students;
    var count = await col.CountDocumentsAsync(Builders<Student>.Filter.Empty);
    if (count == 0)
    {
        var demo = new[]
        {
            new Student { Name = "Hardik Parmar", EnrollmentNo = "ENR2023001", Semester = "6", Field = "CSE", GRNumber = "GR1001", Mobile = "9876543210", InstituteEmail = "hardik@marwadiuniversity.ac.in", PersonalEmail = "hardik.personal@example.com", Address = "Ahmedabad, Gujarat", DOB = new DateTime(2003,5,20) },
            new Student { Name = "Priya Mehta", EnrollmentNo = "ENR2023002", Semester = "4", Field = "IT", GRNumber = "GR1002", Mobile = "9123456780", InstituteEmail = "priya@marwadiuniversity.ac.in", PersonalEmail = "priya.personal@example.com", Address = "Rajkot, Gujarat", DOB = new DateTime(2004,9,11) }
        };
        await col.InsertManyAsync(demo);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();