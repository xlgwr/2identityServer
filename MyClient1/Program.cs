using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyClient1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //认证服务器地址
            string rzAddress = "http://127.0.0.1:7040";
            //资源服务器api地址
            string resAddress = "http://127.0.0.1:7007/api/Home/GetMsg";
            //资源服务器api地址（基于角色授权）
            string resAddress2 = "http://127.0.0.1:7007/api/Home/GetMsg2";

            #region 一.客户端模式
            {
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync(rzAddress);
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }
                //向认证服务器发送请求,要求获得令牌
                Console.WriteLine("---------------------------- 一.向认证服务器发送请求,要求获得令牌-----------------------------------");
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    //在上面的地址上拼接：/connect/token，最终：http://127.0.0.1:7040/connect/token
                    Address = disco.TokenEndpoint,
                    ClientId = "client1",
                    ClientSecret = "0001",
                    //空格分隔的请求范围列表,省略的话是默认配置的所有api资源，如: client1对应的是：{ "GoodsService", "OrderService", "ProductService" }  
                    //这里填写的范围可以和配置的相同或者比配置的少，比如{ "GoodsService OrderService"},这里只是一个范围列表,并不是请求哪个api资源必须要  写在里面
                    //但是如果配置的和默认配置出现不同，则认证不能通过 比如{ "ProductService OrderService111"},
                    //综上所述：此处可以不必配置
                    //Scope = "ProductService OrderService111"
                });
                if (tokenResponse.IsError)
                {
                    Console.WriteLine($"认证错误：{tokenResponse.Error}");
                    Console.ReadKey();
                }
                Console.WriteLine(tokenResponse.Json);

                //携带token向资源服务器发送请求
                Console.WriteLine("----------------------------二.携带token向资源服务器发送请求-----------------------------------");
                var apiClient = new HttpClient();
                apiClient.SetBearerToken(tokenResponse.AccessToken);   //设置Token格式  【Bear xxxxxx】
                var response = await apiClient.GetAsync(resAddress);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                    Console.ReadKey();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"请求资源服务器的结果为：{content}");
                }
            }
            #endregion

            Console.ReadLine();
        }
    }
}
