using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ID4Test
{
    /// <summary>
    /// 客户端模式
    /// </summary>
    public class Config1
    {
        /// <summary>
        /// 配置Api范围集合
        /// 4.x版本新增的配置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("GoodsService"),
                new ApiScope("OrderService"),
                new ApiScope("ProductService")
             };
        }


        /// <summary>
        /// 需要保护的Api资源
        /// 4.x版本新增后续Scopes的配置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> resources = new List<ApiResource>();
            //ApiResource第一个参数是ServiceName，第二个参数是描述
            resources.Add(new ApiResource("GoodsService", "GoodsService服务需要保护哦") { Scopes = { "GoodsService" } });
            resources.Add(new ApiResource("OrderService", "OrderService服务需要保护哦") { Scopes = { "OrderService" } });
            resources.Add(new ApiResource("ProductService", "ProductService服务需要保护哦") { Scopes = { "ProductService" } });
            return resources;
        }


        /// <summary>
        /// 可以使用ID4 Server 客户端资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>() {
                new Client
                {
                    ClientId = "client1",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0001".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "GoodsService", "OrderService", "ProductService" }        //允许访问的api服务
                },
                new Client
                {
                    ClientId = "client2",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0002".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "GoodsService" , "ProductService" } //允许访问的api服务
                },
                 new Client
                {
                    ClientId = "client3",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0003".Sha256())},    //密钥和加密方式
                    AllowedScopes = {"OrderService" }        //允许访问的api服务
                },
                new Client
                {
                    ClientId = "client4",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0004".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ProductService" },        //允许访问的api服务
                    //基于角色授权
                   Claims=
                    {
                        new ClientClaim("role","ypfRole")
                    },
                    ClientClaimsPrefix="", //把前缀设置成空，就IDS4和Core MVC之间就不用转换了
                }
            };
            return clients;
        }
    }
}
