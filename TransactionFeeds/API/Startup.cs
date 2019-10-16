using Abstracts;
using Abstracts.FileReader;
using API.Options;
using infranstructure;
using infranstructure.FileReader;
using infranstructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
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
            services.Configure<ServiceSetting>(Configuration.GetSection("ServiceSetting"));
            services.Configure<TransactionRepositoryOptions>(Configuration.GetSection("TransactionRepository"));

            services.AddSingleton(Configuration);

            services.AddScoped<IFileReaderChain, FileReaderChain>();
            services.AddScoped<IFileStaging, FileStaging>();
            services.AddScoped<IRepository, TransactionRepository>();

            services.AddScoped<ICsvReader, CsvReader>();
            services.AddScoped<IXmlReader, XmlReader>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
