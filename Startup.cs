using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.Swagger;
using ticketmaster.Models;
using ticketmaster.Services;
using Microsoft.AspNetCore.Cors;
using ticketmaster.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ticketmaster.Helpers;

namespace ticketmaster
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("My_Api", new OpenApiInfo{ Version = "1.0", Description = "my api" });
            });
            services.AddCors();

            services.Configure<FormsDatabaseSettings>(
        Configuration.GetSection(nameof(FormsDatabaseSettings)));

            services.AddSingleton<IFormsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<FormsDatabaseSettings>>().Value);

            services.AddSingleton<FormsService>(); /* FORMS COLLECTION IMPORTED */

            services.Configure<TeamsDataBaseSettings>(
       Configuration.GetSection(nameof(TeamsDataBaseSettings)));

            services.AddSingleton<ITeamsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TeamsDataBaseSettings>>().Value);

            services.AddSingleton<TeamsService>(); /* TEAMS COLELCTION IMPORTED */


            services.Configure<MatchesDataBaseSettings>(
        Configuration.GetSection(nameof(MatchesDataBaseSettings)));

            services.AddSingleton<IMatchesDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MatchesDataBaseSettings>>().Value);

            services.AddSingleton<MatchesService>(); /*MATCHES COLLECTION IMPORTED */

            services.Configure<TicketsDataBaseSettings>(
       Configuration.GetSection(nameof(TicketsDataBaseSettings)));

            services.AddSingleton<ITicketsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TicketsDataBaseSettings>>().Value);

            services.AddSingleton<TicketsService>(); /*TICKETS COLLECTION IMPORTED */

            services.Configure<UsersDataBaseSettings>(
      Configuration.GetSection(nameof(UsersDataBaseSettings)));

            services.AddSingleton<IUsersDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UsersDataBaseSettings>>().Value);

            services.AddSingleton<UsersService>(); /*USERS COLLECTION IMPORTED */

            services.Configure<CreditCardsDataBaseSettings>(
      Configuration.GetSection(nameof(CreditCardsDataBaseSettings)));

            services.AddSingleton<ICreditCardsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CreditCardsDataBaseSettings>>().Value);

            services.AddSingleton<CreditCardsService>(); /*CREDIT CARDS COLLECTION */

            services.AddScoped<IAuthRepository, AuthRepository>(); /*ADDING THE AUTH REPOSITORY*/

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                         .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if(error !=null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseAuthentication();

            app.UseCors(options =>
            options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger","Test");
            });
        }
    }
}
