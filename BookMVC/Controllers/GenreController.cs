using BookMVC.DTOs;
using BookMVC.DTOs.GenreDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookMVC.Controllers
{
    public class GenreController : Controller
    {

        public async Task<IActionResult> Index(int page=1)
        {
            
           
            GenreListDto listDto;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44366/admin/api/genres");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listDto = JsonConvert.DeserializeObject<GenreListDto>(responseStr);
                    return View(listDto);
                }
            }
            return RedirectToAction("Index", "Home");

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(GenrePostDto genreDto)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(genreDto), Encoding.UTF8, "application/json");
                string endpoint = "https://localhost:44366/admin/api/genres";


                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "Genre");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            GenreListItemDto genrePost;
            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:44366/admin/api/genres/{id}");
                var responseStr=await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    genrePost=JsonConvert.DeserializeObject<GenreListItemDto>(responseStr);
                    return View(genrePost);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GenrePostDto genrePost)
        {
            string endpoints= $"https://localhost:44366/admin/api/genres/{genrePost.Id}";
            using(HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(genrePost),Encoding.UTF8,"application/json");
                using(var response=await client.PutAsync(endpoints, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index", "genre");
                    }
                    else
                    {
                        return BadRequest();
                    }
                   
                }
              
            }


        }

        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync("https://localhost:44366/admin/api/genres/" + id.ToString());
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index", "Genre");
                }
            }
            return RedirectToAction("Index", "Genre");
        }
    }
}
