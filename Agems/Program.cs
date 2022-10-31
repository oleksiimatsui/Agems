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
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllersWithViews();



string connection = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<AgemsSoundsContext>(option => option.UseSqlServer(
    connection
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
       options.ClientId = builder.Configuration["GitClientId"];
       options.ClientSecret = builder.Configuration["GitClientSecret"];
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
app.UseSession();
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
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
 //   DefaultContentType = "text/plain"
});
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();
app.MapHub<CanvasHub>("/canvasHub");
app.Run();