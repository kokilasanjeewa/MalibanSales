using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.DummyData;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Helpers;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using NLog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Linq;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Maliban.Pos.Api
{
    public class Startup
    {
        [Obsolete]
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            //auto reload appsetting.json
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                              Configuration = builder.Build();
                              Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Base64UrlEncoder.DecodeBytes(Configuration["JwtToken:SecretKey"]);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(key);

            services.AddHttpClient();

            IdentityBuilder builder = services.AddIdentityCore<ApplicationUser>(opt =>
            {
                // Password settings
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = false;
                // Lockout settings
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail = true;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<ApplicationRole>>();
            builder.AddRoleManager<RoleManager<ApplicationRole>>();
            builder.AddSignInManager<SignInManager<ApplicationUser>>();
            builder.AddDefaultTokenProviders();

            //sqlserver configure and setting automatic fainer detecting without user secret user id or password

            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

            }));

            services.AddScoped<Sale>(s => SessionSale.GetSale(s));
            services.AddScoped<GoodReceptNote>(grn => SessionGoodReceptNote.GetGoodReceptNote(grn));
            services.AddScoped<PurchaseRequest>(pr => SessionPurchaseRequest.GetPurchaseRequest(pr));
            services.AddScoped<PurchaseOrder>(or => SessionPurchaseOrder.GetPurchaseOrder(or));
            services.AddScoped<PurchaseReturn>(pr => SessionPurchaseReturn.GetPurchaseReturn(pr));
            services.AddScoped<StockAdjustment>(sta => SessionStockAdjustment.GetStockAdjustment(sta));
            services.AddScoped<IOperationGroupRepository, OperationGroupRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<DapperContext>();

            services.AddSession(options =>
            {
                options.Cookie.Name = "MalibanPos.Session";
                options.IdleTimeout = System.TimeSpan.FromHours(48);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                //  options.Cookie.Name = "cart";

            });

            // reference loop handling 
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(Options =>
            {
                Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                Options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });

            //lnink https://github.com/blowdart/AspNetAuthorizationWorkshop
            // add policy for authorization methods and controllers
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerAccess", policy => policy.RequireAssertion(context =>
                context.User.IsInRole("Administrator") || context.User.IsInRole("Manager")));
                options.AddPolicy("AdminAccess", policy => policy.RequireRole("Administrator", "Manager", "User"));
                options.AddPolicy("UserAccess", policy => policy.RequireAssertion(context =>
                context.User.IsInRole("Manager") || context.User.IsInRole("User")));
                options.AddPolicy("RequiredUser", policy => policy.RequireRole("User"));

            });

            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                // cfg.ApiVersionReader = new QueryStringApiVersionReader
            }

            );
            // CORS Access Config
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                      builder => builder.WithOrigins("").
                                         AllowAnyOrigin()  // single domain .WithOrigins("https://www.yogihosting.com")
                                        .AllowAnyMethod()  // multiple domain .WithOrigins(new string[] { "https://www.yogihosting.com", "https://example1.com", "https://example2.com" })
                                        .AllowAnyHeader().SetIsOriginAllowed(origin => true)); // allow any origin;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Maliban Pos API",
                    Description = "A Maliban Pos API Version 1",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kokila Sanjeewa",
                        Email = string.Empty,
                        Url = new Uri("https://www.facebook.com/kokila.sanjeewa"),
                    },

                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                // add basic instead of Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,// add basic instead of Bearer SecuritySchemeType.Http
                    Scheme = "Bearer",        // add basic instead of Bearer
                    BearerFormat = "JWT",          // add basic instead of Bearer remove this line
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
             {
               new OpenApiSecurityScheme
               {
                   Reference = new OpenApiReference
                         {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer" // add basic instead of Bearer
                         }
               },
                 new string[] {}
             }
               });
                //Set the comments path for the Swagger JSON and UI.
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //   var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //   c.IncludeXmlComments(xmlPath);
            });

            //https://dev.to/thisdotmedia/angular-and-the-rest-authentication-with-jwt-5em0

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              //  option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the JWT Issuer (iss) claim

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtToken:Issuer"],
                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtToken:Issuer"],
                    // Validate the token expiry
                    ValidateLifetime = true,
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    //  ValidIssuer = Configuration["JwtToken:Issuer"],
                    //  ValidAudience = Configuration["JwtToken:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)), //Configuration["JwtToken:SecretKey"]
                                                                                                                                               // TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                                                                                                                               //  ClockSkew = TimeSpan.FromMinutes(0),
                };
                // custom token with CreateJwt() in Authcontroller
                // https://dzone.com/articles/json-web-encryption-jwe-in-net-core
            });
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            services.AddAutoMapper(typeof(Startup));

            //Same instance same instance
            services.AddSingleton<ILog, LogNLog>();

            //same instance new instance AddScoped
            services.ConfigureRepositoryWrapper();

            services.AddHealthChecks()
                    .AddSqlServer(connectionString: Configuration.GetConnectionString("DefaultConnection"))
                    .AddCheck<DatabaseConnectionCheck>(name: "DataBaseConnectionCheck", tags: new[] { "database." })
                    //.AddCheck<ApiHealthCheck>(name: "ApiHealthCheck", tags: new[] { "api." })
                    .AddDiskStorageHealthCheck(x => x.AddDrive("C:\\", 7_000), "Check primary disk - warning", HealthStatus.Degraded, tags: new[] { "primary disk." })
                    .AddDiskStorageHealthCheck(x => x.AddDrive("C:\\", 2_000), "Check primary disk - error", HealthStatus.Unhealthy, tags: new[] { "primary disk." })
                    .AddDiskStorageHealthCheck(setup =>
                    {
                        DriveInfo.GetDrives().ToList().ForEach(di =>
                        {
                            setup.AddDrive(di.Name, 1024);
                        });
                    })
                    .AddPrivateMemoryHealthCheck(4000000000, "Check memory - error", HealthStatus.Unhealthy)
                    .AddProcessAllocatedMemoryHealthCheck(700, "Check process allocated memory - error", HealthStatus.Unhealthy, tags: new[] { "allocated memory." })
                    .AddCheck("Foo", () => HealthCheckResult.Healthy("Foo is OK!"), tags: new[] { "foo_tag" });
            services.AddHealthChecksUI().AddInMemoryStorage();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext _context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IMapper map, IConfiguration _config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //  app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //app.UseHttpsRedirection();
            //app.UseRouting(); core 3.1 version session managment

            app.UseSession();

            // app.UseEndpoints(); core 3.1 version session managment
            //app.UseResponseCaching(); DISABLE

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Maliban Pos API V1");
                c.DisplayOperationId();
            });
            //please provide the front end address ex: https;//localhost:4200 
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();                                                                                                                     //app.UseCors("AllowLimited");  //apply AllowLimited policy from cors 
            app.UseDefaultFiles();
            app.UseSpaStaticFiles();
            // angular application root folder
            //https://weblog.west-wind.com/posts/2020/Jul/12/Handling-SPA-Fallback-Paths-in-a-Generic-ASPNET-Core-Server
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/index.html");
                // Mapping a new endpoint to see the health
                // endpoints.MapHealthChecks("/health");
                endpoints.MapHealthChecksUI();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                // endpoints.MapHealthChecks("/healthcheck",new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }).RequireCors("CorsPolicy");
                // endpoints.MapHealthChecksUI(options => options.UIPath = "/dashboard").RequireCors("CorsPolicy");
            });
            app.UseSpa(
             spa =>
             {
                 spa.Options.SourcePath = "../MalibanPos-Spa";
                 // spa.Options.SourcePath = "wwwroot";

                 if (env.IsDevelopment())
                 {
                     spa.UseAngularCliServer("start");
                 }
             });
            SeedData.Initialize(userManager, _context, roleManager, map).Wait();
            // add districts provinces Cities and Countries data to sql db 
            // SeedData.InitializeDb(_context, _config, map).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            IdentityResult roleResult;
            //Adding Addmin Role  
            var roleCheck = await RoleManager.RoleExistsAsync("Administrator");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await RoleManager.CreateAsync(new ApplicationRole("Administrator"));
            }

            roleCheck = await RoleManager.RoleExistsAsync("Manager");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await RoleManager.CreateAsync(new ApplicationRole("Manager"));
            }
            //Adding User Role  
            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await RoleManager.CreateAsync(new ApplicationRole("User"));
            }
            //Assign Admin role to the main User here we have given our newly registered login id for Admin management  
            ApplicationUser user = await UserManager.FindByEmailAsync("kokila.sanjeewa@gmail.com");
            var User = new ApplicationUser();
            await UserManager.AddToRoleAsync(user, "Administrator");
            await UserManager.AddToRoleAsync(user, "Manager");
            user = await UserManager.FindByEmailAsync("kokila.sanjeewa@maliban.com");
            await UserManager.AddToRoleAsync(user, "User");

        }
    }

}
#region
/* https://scottsauber.com/2017/04/10/how-to-troubleshoot-an-error-occurred-while-starting-the-application-in-asp-net-core-on-iis/ */
/* https://code-maze.com/net-core-web-development-part16/ */
/* https://www.includehelp.com/dot-net/print-the-ip-address-of-the-computer.aspx */
#endregion