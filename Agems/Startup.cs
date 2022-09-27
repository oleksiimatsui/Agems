﻿using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Agems
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddRazorPages();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "GitHub";
            })
               .AddCookie()
               .AddOAuth("GitHub", options =>
               {
                   options.ClientId = Configuration["GitHub:ClientId"];
                   options.ClientSecret = Configuration["GitHub:ClientSecret"];
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
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };
            app.UseWebSockets(webSocketOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}