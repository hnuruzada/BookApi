using BookMVC.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginDto loginDto)
        {
            if (!ModelState.IsValid) return View();

            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient clinet = new HttpClient())
            {
                response = await clinet.PostAsync("https://localhost:44397/admin/api/Accounts/login", requestContent);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                AdminLoginResponseDto responseDto = JsonConvert.DeserializeObject<AdminLoginResponseDto>(responseContent);

                Response.Cookies.Append("AuthToken", responseDto.Token);

                return RedirectToAction("index", "category");
            }

            return NotFound();
        }
    }
}
