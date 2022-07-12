#nullable disable
namespace AllSopFoodService
{
    using Constants;
    using Model;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using Npgsql;

    /// <summary>
    /// The main start-up class for the application.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html</param>
        /// <param name="webHostEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default. See http://docs.asp.net/en/latest/fundamentals/environments.html</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Configures the services to add to the ASP.NET Core Injection of Control (IoC) container. This method gets
        /// called by the ASP.NET runtime. See
        /// http://blogs.msdn.com/b/webdev/archive/2014/06/17/dependency-injection-in-asp-net-vnext.aspx
        /// </summary>
        /// <param name="services">The services.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomCaching()
                    .AddCustomCors()
                    .AddCustomOptions(this.configuration)
                    .AddCustomRouting()
                    .AddResponseCaching()
                    .AddCustomResponseCompression(this.configuration)
                    .AddCustomHealthChecks()
                    .AddCustomSwagger()
                    .AddHttpContextAccessor()
                    // Add useful interface for accessing the ActionContext outside a controller.
                    .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                    .AddCustomApiVersioning()
                    .AddServerTiming()
                    .AddControllers()
                    .AddCustomJsonOptions(this.webHostEnvironment)
                    .AddCustomMvcOptions(this.configuration)
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.AddAutoMapper(typeof(Startup))
                    .AddProjectCommands()
                    .AddProjectMappers()
                    .AddProjectRepositories()
                    .AddProjectServices()
                    .AddDistributedMemoryCache().AddSession().AddMvc();

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                //services.AddDbContext<FoodDbContext>(options => options.UseSqlServer(connectionString: this.configuration.GetConnectionString("DefaultConnection")));
                services.AddEntityFrameworkNpgsql().AddDbContext<FoodDbContext>(options =>
                    options.UseNpgsql(this.configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                //Prepare Heroku PostgreSQL credentials according to postgreSQL format
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                Console.WriteLine("Hello, Vu Vo is debugging here...." + connUrl);

                var uri = new Uri(connUrl);
                var userInfo = uri.UserInfo.Split(':');

                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = uri.LocalPath.TrimStart('/'),
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true
                };

                services.AddEntityFrameworkNpgsql().AddDbContext<FoodDbContext>(options => options.UseNpgsql(connectionStringBuilder.ToString()));
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(this.configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    });
        }

        /// <summary>
        /// Configures the application and HTTP request pipeline. Configure is called after ConfigureServices is
        /// called by the ASP.NET runtime.
        /// </summary>
        /// <param name="application">The application builder.</param>
        public virtual void Configure(IApplicationBuilder application)
        {
            application.UseIf(
                        this.webHostEnvironment.IsDevelopment(),
                        x => x.UseServerTiming())
                    .UseForwardedHeaders()
                    .UseRouting();


            application.UseAuthentication();
            application.UseAuthorization();

            application.UseCors(CorsPolicyName.AllowAny)
                    .UseResponseCaching()
                    .UseResponseCompression()
                    .UseIf(
                        this.webHostEnvironment.IsDevelopment(),
                        x => x.UseDeveloperExceptionPage())
                    .UseStaticFilesWithCacheControl()
                    .UseCustomSerilogRequestLogging()
                    .UseEndpoints(
                        builder =>
                        {
                            builder.MapControllers().RequireCors(CorsPolicyName.AllowAny);
                            builder
                                .MapHealthChecks("/status")
                                .RequireCors(CorsPolicyName.AllowAny);
                            builder
                                .MapHealthChecks("/status/self", new HealthCheckOptions() { Predicate = _ => false })
                                .RequireCors(CorsPolicyName.AllowAny);
                        })
                    .UseSwagger()
                    .UseCustomSwaggerUI()
                    .UseSession();

            AppDbInitializer.Seed(application);
        }

    }
}
