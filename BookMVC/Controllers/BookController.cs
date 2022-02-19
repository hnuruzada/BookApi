using BookMVC.DTOs.BookDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookMVC.Controllers
{
    public class BookController : Controller
    {
        public async Task<IActionResult> Index()
        {
            BookListDto listDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44366/admin/api/books");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listDto = JsonConvert.DeserializeObject<BookListDto>(responseStr);
                    return View(listDto);
                }
            }
            return RedirectToAction("Index", "home");
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Create(BookPostDto bookPost)
        {
            string endpoint = "https://localhost:44366/admin/api/books";

            using(HttpClient client = new HttpClient())
            {
                var multipartContent = new MultipartFormDataContent();

                byte[] byteArr = null;

                if (bookPost.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        bookPost.ImageFile.CopyTo(ms);
                        byteArr = ms.ToArray();
                        var byteArrContent = new ByteArrayContent(byteArr);
                        byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(bookPost.ImageFile.ContentType);
                        multipartContent.Add(byteArrContent, "ImageFile", bookPost.ImageFile.FileName);
                    }
                }
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookPost.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookPost.Language), Encoding.UTF8, "application/json"), "Language");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookPost.SalePrice), Encoding.UTF8, "application/json"), "SalePrice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookPost.CostPrice), Encoding.UTF8, "application/json"), "CostPrice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookPost.PageCount), Encoding.UTF8, "application/json"), "PageCount");

            }
        } 
    }
}
