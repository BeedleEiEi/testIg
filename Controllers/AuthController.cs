using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using testIg.Models;

namespace testIg.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private IConfiguration _configuration;
        public AuthController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }
        [Route("signin")]
        public async Task<dynamic> auth([FromQuery] string code)
        {
            var token = await getToken(code);
            return token;
        }

        public async Task<dynamic> getToken(string code)
        {
            var client = new HttpClient();
            string app_id = _configuration.GetValue<string>("app_id");
            string app_secret = _configuration.GetValue<string>("app_secret");
            string grant_type = "authorization_code";
            string redirect_uri = "https://localhost:5001/auth/signin";

            string apiUrl = "https://api.instagram.com/oauth/access_token"; //?app_id=" + app_id + "&app_secret=" + app_secret + "&grant_type=" + grant_type + "&redirect_uri=" + redirect_uri + "&code=" + code;

            var values = new Dictionary<string, string> { { "app_id", app_id },
                        { "app_secret", app_secret },
                        { "grant_type", grant_type },
                        { "redirect_uri", redirect_uri },
                        { "code", code }
                    };

            var content = new FormUrlEncodedContent(values);


            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();

                dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(jsonResult);

                return jsonData;
            }
            var returnDict = new Dictionary<string, dynamic>(){
                {"success", false}
            };
            return JsonConvert.SerializeObject(returnDict);
        }

        //CURL
        // curl -X POST \ https://api.instagram.com/oauth/access_token \ -F app_id={app-id} \ -F app_secret={app-secret} \ -F grant_type=authorization_code \ -F redirect_uri={redirect-uri} \ -F code={code}



        // [Route("signin")]
        // public IActionResult SignIn() => View();

        // [Route("instagram")]
        // public dynamic callback([FromQuery] string scope, [FromQuery] string response_type){
        //     // string json = "";
        //     var returnDict = new Dictionary<string, dynamic>(){
        //         {"scope", scope},
        //         {"response_type", response_type}
        //     };
        //     return JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(returnDict));
        // }

        // [Route("signin/{provider}")]
        // public IActionResult SignIn(string provider, string returnUrl = null) =>
        //     Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, provider);

        // [Route("signout")]
        // public async Task<IActionResult> SignOut()
        // {
        //     await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //     return RedirectToAction("Index", "Home");
        // }
    }
}