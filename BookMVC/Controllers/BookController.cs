using BookMVC.DTOs;
using BookMVC.DTOs.AuthorDtos;
using BookMVC.DTOs.BookDtos;
using BookMVC.DTOs.GenreDtos;
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
        public async Task<IActionResult> Create()
        {
            //AuthorInBookListItem authorInBook;
            //GenreInBookListItemDto genreInBook;
            ListDto<AuthorListItemDto> authorListDtos;
            ListDto<GenreListItemDto> genreListDtos;
            var EndpointAuthor = "https://localhost:44366/admin/api/authors";
            var EndpointGenre = "https://localhost:44366/admin/api/genres";
            using (HttpClient client = new HttpClient())
            {
                var AuthorResponse = await client.GetAsync(EndpointAuthor);
                var authorResponseStr = await AuthorResponse.Content.ReadAsStringAsync();


                var GenreResponse = await client.GetAsync(EndpointGenre);
                var GenreResponseStr = await GenreResponse.Content.ReadAsStringAsync();
                if (GenreResponse.StatusCode == System.Net.HttpStatusCode.OK && AuthorResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                   authorListDtos = JsonConvert.DeserializeObject<ListDto<AuthorListItemDto>>(authorResponseStr);
                  genreListDtos = JsonConvert.DeserializeObject<ListDto<GenreListItemDto>>(GenreResponseStr);
                    ListBookDto listDtos = new ListBookDto
                    {
                        Authors = authorListDtos,
                        Genres = genreListDtos
                    };
                    //BookListItemDto bookList = new BookListItemDto
                    //{
                    //    Author = authorInBook,
                    //    Genre = genreInBook,
                    //};
                    return View(listDtos);
                }
                else
                {
                    return Json(GenreResponse.StatusCode);
                }

            }


        }

        [HttpPost]
        public async Task<IActionResult> Create(ListBookDto listBook)
        {
            string endpoint = "https://localhost:44366/admin/api/books";
            
            using (HttpClient client = new HttpClient())
            {
                var multipartContent = new MultipartFormDataContent();

                byte[] byteArr = null;

                if (listBook.postDtos.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        listBook.postDtos.ImageFile.CopyTo(ms);
                        byteArr = ms.ToArray();
                        var byteArrContent = new ByteArrayContent(byteArr);
                        byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(listBook.postDtos.ImageFile.ContentType);
                        multipartContent.Add(byteArrContent, "ImageFile", listBook.postDtos.ImageFile.FileName);
                    }
                }
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.Language), Encoding.UTF8, "application/json"), "Language");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.SalePrice), Encoding.UTF8, "application/json"), "SalePrice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.CostPrice), Encoding.UTF8, "application/json"), "CostPrice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.PageCount), Encoding.UTF8, "application/json"), "PageCount");

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.AuthorId), Encoding.UTF8, "application/json"), "authorid");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.GenreId), Encoding.UTF8, "application/json"), "genreid");
                using (var Response = await client.PostAsync(endpoint, multipartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "book");
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
            ListDto<AuthorListItemDto> authorListItem;
            ListDto<GenreListItemDto> genreListItem;
            BookPostDto bookPost;
            var EndpointAuthor = "https://localhost:44366/admin/api/authors";
            var EndpointJanr = "https://localhost:44366/admin/api/genres";
            var EndpointBook = $"https://localhost:44366/admin/api/books/{id}";
            using (HttpClient client = new HttpClient())
            {
                var AuthorResponse = await client.GetAsync(EndpointAuthor);
                var authorResponStr = await AuthorResponse.Content.ReadAsStringAsync();
                var bookResponse = await client.GetAsync(EndpointBook);
                var bookResponsestr = await bookResponse.Content.ReadAsStringAsync();


                var GenreResponse = await client.GetAsync(EndpointJanr);
                var GenreResponseStr = await GenreResponse.Content.ReadAsStringAsync();
                if (GenreResponse.StatusCode == System.Net.HttpStatusCode.OK && AuthorResponse.StatusCode == System.Net.HttpStatusCode.OK && bookResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorListItem = JsonConvert.DeserializeObject<ListDto<AuthorListItemDto>>(authorResponStr);
                    genreListItem = JsonConvert.DeserializeObject<ListDto<GenreListItemDto>>(GenreResponseStr);
                    bookPost = JsonConvert.DeserializeObject<BookPostDto>(bookResponsestr);
                    ListBookDto listBook = new ListBookDto
                    {
                        Authors = authorListItem,
                        Genres = genreListItem,
                        postDtos = bookPost
                    };
                    return View(listBook);
                }
                else
                {
                    return Json(bookResponse.StatusCode);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ListBookDto listBook)
        {
            var endpoints = $"https://localhost:44366/admin/api/books/{id}";
            using (HttpClient client = new HttpClient())
            {
                var multipartContent = new MultipartFormDataContent();
                byte[] bytes = null;
                if (listBook.postDtos.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        listBook.postDtos.ImageFile.CopyTo(ms);
                        bytes = ms.ToArray();
                        var bytearrcontent = new ByteArrayContent(bytes);
                        bytearrcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(listBook.postDtos.ImageFile.ContentType);
                        multipartContent.Add(bytearrcontent, "Imagefile", listBook.postDtos.ImageFile.FileName);
                    }
                }

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.Language), Encoding.UTF8, "application/json"), "Language");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.SalePrice), Encoding.UTF8, "application/json"), "SalePrice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.CostPrice), Encoding.UTF8, "application/json"), "CostPrice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.PageCount), Encoding.UTF8, "application/json"), "PageCount");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.AuthorId), Encoding.UTF8, "application/json"), "authorid");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(listBook.postDtos.GenreId), Encoding.UTF8, "application/json"), "genreid");

                using (var response = await client.PutAsync(endpoints, multipartContent))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "book");
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
                var uri = Path.Combine($"https://localhost:44366/admin/api/books/{id}");
                await client.DeleteAsync(uri);
                return RedirectToAction("index", "book");
            }

        }
    }
}

