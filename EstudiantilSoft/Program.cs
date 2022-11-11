using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EstudiantilSoft.Data;
using EstudiantilSoft.SeedData;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EstudiantilSoftContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EstudiantilSoftContext") ?? throw new InvalidOperationException("Connection string 'EstudiantilSoftContext' not found.")));



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.Services.GetService<IMemoryCache>();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Index}/{id?}");

app.Run();
