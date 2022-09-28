using Agems;
using Agems.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AgemsSoundsContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
   .AddCookie()
   .AddOAuth("GitHub", options =>
   {
       options.ClientId = builder.Configuration["GitHub:ClientId"];
       options.ClientSecret = builder.Configuration["GitHub:ClientSecret"];
       options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/github-oauth");
       options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
       options.TokenEndpoint = "https://github.com/login/oauth/access_token";
       options.UserInformationEndpoint = "https://api.github.com/user";
       options.SaveTokens = true;
       options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
       options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
       options.ClaimActions.MapJsonKey("urn:github:login", "login");
       options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
       options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
       options.Events = new OAuthEvents
       {
           OnCreatingTicket = async context =>
           {
               var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
               request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
               var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
               response.EnsureSuccessStatusCode();
               var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
               context.RunClaimActions(json.RootElement);
           }
       };
   });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
});
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();
app.MapHub<CanvasHub>("/canvasHub");
app.Run();