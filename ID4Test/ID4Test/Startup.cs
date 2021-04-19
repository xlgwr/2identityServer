using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ID4Test
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
            //1. �ͻ���ģʽ
            services.AddIdentityServer()
                  .AddDeveloperSigningCredential()    //����Tokenǩ����Ҫ�Ĺ�Կ��˽Կ,�洢��bin��tempkey.rsa(��������Ҫ����ʵ֤�飬�˴���ΪAddSigningCredential)
                  .AddInMemoryApiResources(Config1.GetApiResources())  //�洢��Ҫ����api��Դ
                  .AddInMemoryApiScopes(Config1.GetApiScopes())        //����api��Χ 4.x�汾�������õ�
                  .AddInMemoryClients(Config1.GetClients()); //�洢�ͻ���ģʽ(����Щ�ͻ��˿�����)

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ID4Test", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ID4Test v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            //1.����IdentityServe4
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
