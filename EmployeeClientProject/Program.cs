using EmployeeClientProject.Controllers;
using EmployeeClientProject.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); ;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "User_data"; // Set a custom cookie name
        options.Cookie.HttpOnly = true; // Ensure the cookie is HttpOnly
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Configure secure policy

        options.LoginPath = "/Account/Login"; // Set the login path
        options.LogoutPath = "/Account/Login"; // Set the logout path
        options.AccessDeniedPath = "/Account/AccessDenied"; // Set the access denied path

        options.SlidingExpiration = true; // Enable sliding expiration
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set the expiration time

        // Configure events for handling authentication events
        options.Events = new CookieAuthenticationEvents
        {
            // Handle events here if needed
        };
    });



// SESSION Authorization
//builder.Services.AddSingleton<IAuthorizationHandler, SessionHandler>();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Manager", policy =>
//    {
//        policy.Requirements.Add(new SessionRequirement("Manager"));
//    });
//    options.AddPolicy("Admin", policy =>
//    {
//        policy.Requirements.Add(new SessionRequirement("Admin"));
//    });

//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Manager", policy =>
        policy.RequireClaim("ClaimTypes.Role", "Manager"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
