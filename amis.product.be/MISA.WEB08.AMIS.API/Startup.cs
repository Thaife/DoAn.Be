using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MISA.WEB08.AMIS.API.Middleware;
using MISA.WEB08.AMIS.BL;
using MISA.WEB08.AMIS.DL;
using System.IO;
using System.Text;

namespace MISA.WEB08.AMIS.API
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddControllers().ConfigureApiBehaviorOptions(options => {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MISA.WEB08.AMIS.API", Version = "v1" });
            });
            //injiection
            services.AddScoped(typeof(IDatabaseHelper<>), typeof(DatabaseHelper<>));

            services.AddScoped<IEmployeeBL, EmployeeBL>();
            services.AddScoped<IEmployeeDL, EmployeeDL>();
            services.AddScoped<IBranchBL, BranchBL>();
            services.AddScoped<IBranchDL, BranchDL>();
            services.AddScoped<IDepotBL, DepotBL>();
            services.AddScoped<IDepotDL, DepotDL>();
            services.AddScoped<ILoginBL, LoginBL>();
            services.AddScoped<ILoginDL, LoginDL>();
            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<ICategoryDL, CategoryDL>();
            services.AddScoped<IOriginBL, OriginBL>();
            services.AddScoped<IOriginDL, OriginDL>();
            services.AddScoped<IProductBL, ProductBL>();
            services.AddScoped<IProductDL, ProductDL>();
            services.AddScoped<ITrademarkBL, TrademarkBL>();
            services.AddScoped<ITrademarkDL, TrademarkDL>();
            services.AddScoped<ICartBL, CartBL>();
            services.AddScoped<ICartDL, CartDL>();
            services.AddScoped<ICouponBL, CouponBL>();
            services.AddScoped<ICouponDL, CouponDL>();
            services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
            services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
            DataContext.MySqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            DataContext.Path_root = Configuration["AppSettings:Path_root"];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MISA.WEB08.AMIS.API v1"));
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), $@"{DataContext.Path_root}")),
                RequestPath = new PathString("/Assets")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), $@"{DataContext.Path_root}")),
                RequestPath = new PathString("/Assets")
            });
        }
    }
}
