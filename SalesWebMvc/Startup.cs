using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo tempo de execução.
        // Use este método para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // Este lambda determina se o consentimento do usuário para cookies não essenciais é necessário para uma determinada solicitação.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Define o Microsoft.AspNetCore.Mvc.CompatibilityVersion para ASP.NET Core MVC para o aplicativo.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Definição do Banco de dados
            services.AddDbContext<SalesWebMvcContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                        builder.MigrationsAssembly("SalesWebMvc")));

            // Registro dos Services no Container
            services.AddScoped<SeedingService>();
            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordService>();
        }

        // Este método é chamado pelo tempo de execução.
        // Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            var enUS = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUS),
                SupportedCultures = new List<CultureInfo> { enUS },
                SupportedUICultures = new List<CultureInfo> { enUS }
            };

            // Definir automaticamente as informações de cultura para solicitações com base nas informações fornecidas pelo cliente.
            app.UseRequestLocalization(localizationOptions);

            // Verifica qual ambiente o App se encontra
            if (env.IsDevelopment())
            {
                // Middleware para capturar instâncias System.Exception síncronas e assíncronas do pipeline e gera respostas de erro HTML
                app.UseDeveloperExceptionPage();

                // Popula o banco de dados default
                seedingService.Seed();
            }
            else
            {
                // Middleware que captura exceções, registra-as, redefine o caminho da solicitação e reexecuta a solicitação.
                // A solicitação não será executada novamente se a resposta já tiver começado.
                app.UseExceptionHandler("/Home/Error");

                // Middleware para usar HSTS, que adiciona o cabeçalho Strict-Transport-Security.
                app.UseHsts();
            }

            // Middleware para redirecionar solicitações HTTP para HTTPS.
            app.UseHttpsRedirection();

            // Adiciona MVC ao pipeline de execução de solicitação Microsoft.AspNetCore.Builder.IApplicationBuilder
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
