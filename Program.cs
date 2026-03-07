using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using SimpLedger.Repository.Service.Inventory;
using SimpLedger.Repository.Service.Sales;
using SimpLedger.Repository.Services.Account;

namespace SimpLedger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Scoped Services

            builder.Services.AddScoped<ISalesService, SalesService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<IUserAccountService, UserAccountService>();

            #endregion

            #region Singleton Services

            builder.Services.AddSingleton<ResponseHelper>();
            builder.Services.AddSingleton<LoginValidation>();

            #endregion

            #region Hosted Services
            #endregion

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
            });

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            builder.Services.AddDbContext<DatabaseContext>(context => context.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
