using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using _2iDashApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.Cookie.SameSite = SameSiteMode.None;
        // Allow the cookie to be written over HTTP in development
        // so the login loop does not occur when HTTPS is unavailable.
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Events.OnValidatePrincipal = async ctx =>
        {
            var email = ctx.Principal?.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email) || !email.EndsWith("@2iltd.com", StringComparison.OrdinalIgnoreCase))
            {
                ctx.RejectPrincipal();
                await ctx.HttpContext.SignOutAsync();
            }
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? string.Empty;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? string.Empty;
        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Events.OnCreatingTicket = ctx =>
        {
            var email = ctx.Principal?.FindFirstValue(ClaimTypes.Email);
            if (email is null || !email.EndsWith("@2iltd.com", StringComparison.OrdinalIgnoreCase))
            {
                ctx.Fail("Only 2iltd.com accounts are allowed");
            }
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapGet("/login", async context =>
{
    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = "/" });
    }
    else
    {
        context.Response.Redirect("/");
    }
});

app.MapGet("/logout", async context =>
{
    await context.SignOutAsync();
    context.Response.Redirect("/");
});
app.MapFallbackToPage("/_Host");

app.Run();
