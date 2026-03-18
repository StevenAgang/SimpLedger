using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpLedger.Middleware;
using SimpLedger.Repository;
using SimpLedger.Repository.Configuration.Helper;
using SimpLedger.Repository.Configurations;
using SimpLedger.Repository.Configurations.Validation.Account;
using SimpLedger.Repository.Interface.Inventory;
using SimpLedger.Repository.Interface.Sales;
using SimpLedger.Repository.Interfaces.Account;
using SimpLedger.Repository.Interfaces.Common;
using SimpLedger.Repository.Interfaces.Emailing;
using SimpLedger.Repository.Service.Inventory;
using SimpLedger.Repository.Service.Sales;
using SimpLedger.Repository.Services.Account;
using SimpLedger.Repository.Services.Common;
using SimpLedger.Repository.Services.Emailing;

namespace SimpLedger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("FrontEnd", policy =>
                {
                    policy.WithOrigins("https://localhost:4200")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            #region Scoped Services

            builder.Services.AddScoped<ISalesService, SalesService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<IUserAccountService, UserAccountService>();
            builder.Services.AddScoped<IEmailProviderService, EmailProviderService>();
            builder.Services.AddScoped<IManualAuthenticationService, ManualAuthenticationService>();

            #endregion

            #region Singleton Services

            builder.Services.AddSingleton<ResponseHelper>();
            builder.Services.AddSingleton<UserAccountValidation>();
            builder.Services.AddSingleton<ISetTokenSevice,SetTokenService>();

            #endregion

            #region Transient Services
            #endregion

            #region Hosted Services
            #endregion

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.HttpContext.Request.Cookies["accesstoken"];
                        if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie()
            .AddGoogleOpenIdConnect(options =>
            {
            options.ClientId = builder.Configuration["EmailProviderSetting:ClientId"];
            options.ClientSecret = builder.Configuration["EmailProviderSetting:ClientSecret"];
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.SaveTokens = true;
                options.ResponseType = "code";
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.SetParameter("access_type", "offline");
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            builder.Services.AddDbContext<DatabaseContext>(context => context.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("FrontEnd");

            //app.Map("/sales/push", EventsMiddleware.RunProcess);
            app.UseMiddleware<RequestMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
