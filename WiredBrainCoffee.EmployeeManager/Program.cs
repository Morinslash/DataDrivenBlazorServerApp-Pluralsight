using Microsoft.EntityFrameworkCore;
using WiredBrainCoffee.EmployeeManager.Data;
using WiredBrainCoffee.EmployeeManager.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<EmployeeManagerDbContext>(
    opt => opt.UseSqlServer(
        builder.Configuration.GetConnectionString("EmployeeManagerDb")));
builder.Services.AddScoped<StateContainer>();


var app = builder.Build();

//development only
await EnsureDatabaseIsMigrated(app.Services);

async Task EnsureDatabaseIsMigrated(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    using var context = scope.ServiceProvider.GetService<EmployeeManagerDbContext>();
    if (context is not  null)
    {
         await context.Database.MigrateAsync();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();