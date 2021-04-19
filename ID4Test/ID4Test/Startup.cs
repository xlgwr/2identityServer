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
            //1. 客户端模式
            services.AddIdentityServer()
                  .AddDeveloperSigningCredential()    //生成Token签名需要的公钥和私钥,存储在bin下tempkey.rsa(生产场景要用真实证书，此处改为AddSigningCredential)
                  .AddInMemoryApiResources(Config1.GetApiResources())  //存储需要保护api资源
                  .AddInMemoryApiScopes(Config1.GetApiScopes())        //配置api范围 4.x版本必须配置的
                  .AddInMemoryClients(Config1.GetClients()); //存储客户端模式(即哪些客户端可以用)

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

            //1.启用IdentityServe4
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
