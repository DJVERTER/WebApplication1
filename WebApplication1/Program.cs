using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibrarieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Librarie")));

builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = "AdministratorAuth"; // Sau se poate doar asa: builder.Services.AddAuthentication("AdministratorAuth")
})
    .AddCookie("AdministratorAuth", option =>
    {
        option.Cookie.Name = "AdministratorAuth"; // Linie de cod redundanta. Am pus-o doar ca sa vezi ca e posibil si asa.
        option.LoginPath = "/User/Inregistrare";
        option.LogoutPath = "/Account/LogOut";
        option.AccessDeniedPath = "/Error/UnAuthorized"; // Daca vei face LogIn la un User cu alt rol decat cel indicat la eticheta [Authorize] de pe
                                                         // controlerul "Home", atunci cand vei accesa acest controller vei fi redirectionat catre "/Error/UnAuthorized"
                                                         // care nu este implimenatta la moment.
    })
    .AddCookie("UsersAuth", option => // Aceasta schema nu este necesara. Am adaugat-o doar pentru ca sa vezi ca poti adauga mai multe scheme.
    {
        option.Cookie.Name = "UsersAuth";
        option.LoginPath = new PathString("/Account/Login/");
        option.ExpireTimeSpan = TimeSpan.FromMinutes(144000);
        option.AccessDeniedPath = "/Error/UnAuthorized";
        option.LogoutPath = "/Security/LogoutUser";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

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
