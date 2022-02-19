using BookMVC.DTOs;
using BookMVC.DTOs.AuthorDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookMVC.Controllers
{
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            AuthorListDto listDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44366/admin/api/authors");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listDto = JsonConvert.DeserializeObject<AuthorListDto>(responseStr);
                    return View(listDto);
                }
            }
            return RedirectToAction("Index", "home");

        }

        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(AuthorPostDto authorDto)
        {
            string endpoint = "https://localhost:44366/admin/api/authors";

            using (HttpClient client = new HttpClient())
            {
                var multipartContent = new MultipartFormDataContent();

                byte[] byteArr = null;

                if (authorDto.ImageFile!=null)
                {
                    using (var ms = new MemoryStream())
                    {
                        authorDto.ImageFile.CopyTo(ms);
                        byteArr = ms.ToArray();
                        var byteArrContent = new ByteArrayContent(byteArr);
                        byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(authorDto.ImageFile.ContentType);
                        multipartContent.Add(byteArrContent, "ImageFile", authorDto.ImageFile.FileName);
                    }
                }
              
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(authorDto.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(authorDto.Surname), Encoding.UTF8, "application/json"), "Surname");
                



                using (var Response = await client.PostAsync(endpoint, multipartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "Author");
                    }
                    else
                    {
                        return Json(Response.StatusCode);
                    }
                }
            }
            
            
        }
        public async Task<IActionResult> Edit(int id)
        {
            AuthorPostDto authorPost;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:44366/admin/api/authors/{id}");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorPost = JsonConvert.DeserializeObject<AuthorPostDto>(responseStr);
                    return View(authorPost);
                }
                else
                {
                    return NotFound();
                }
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AuthorPostDto authorPost)
        {
           
            string endpoints = $"https://localhost:44366/admin/api/authors/{authorPost.Id}";
            using (HttpClient client = new HttpClient())
            {
                var multipartContent = new MultipartFormDataContent();

                byte[] byteArr = null;

                if (authorPost.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        authorPost.ImageFile.CopyTo(ms);
                        byteArr = ms.ToArray();
                        var byteArrContent = new ByteArrayContent(byteArr);
                        byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(authorPost.ImageFile.ContentType);
                        multipartContent.Add(byteArrContent, "ImageFile", authorPost.ImageFile.FileName);
                    }
                }

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(authorPost.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(authorPost.Surname), Encoding.UTF8, "application/json"), "Surname");
               
                using (var response = await client.PutAsync(endpoints, multipartContent))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index", "author");
                    }
                    else
                    {
                        return Json(response.StatusCode);
                    }

                }

            }


        }

        public async Task<IActionResult> Delete(int id)
        {
            
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:44366/admin/api/authors/{id}");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index", "Author");
                }
            }
            return RedirectToAction("Index", "Author");
        }
    }
}
