using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 资源服务器的api
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string GetMsg()
        {
            //快速获取token的方式
            string token = HttpContext.GetTokenAsync("access_token").Result;

            return $"ypf";
        }
    }
}
