using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transaction.Api.Middleware;
using Transaction.Data;
using Transaction.Services;

namespace Transaction.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddDbContext<TransactionContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("TransactionConnection")));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("TransactionApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "FinalProject - Transaction",
                        Version = "1",
                        Description = "Brix Final Project",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Name = "Yehudit Cohen & Batya Hartman",
                            Email = "cyehudit10@gmail.com"
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/TransactionApiSpecification/swagger.json",
                    "FinalProject - Transaction");
                setupAction.RoutePrefix = "";
            });
        }
    }
}
