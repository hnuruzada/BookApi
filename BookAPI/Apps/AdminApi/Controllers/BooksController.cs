using AutoMapper;
using BookAPI.Apps.AdminApi.DTOs;
using BookAPI.Apps.AdminApi.DTOs.BookDtos;
using BookAPI.Data.DAL;
using BookAPI.Data.Entity;
using BookAPI.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookAPI.Apps.AdminApi.Controllers
{
    [ApiExplorerSettings(GroupName = "admin_v1")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public BooksController(BookDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Book book = _context.Books.Include(x => x.Author).ThenInclude(a => a.Books).Include(x=>x.Genre).ThenInclude(g=>g.Books).FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (book == null) return NotFound();

            BookGetDto bookGetDto = _mapper.Map<BookGetDto>(book);

            return Ok(bookGetDto);
        }
        
        [HttpGet("")]
        public IActionResult GetAll(int page = 1, string search = null)
        {
            var query = _context.Books.Include(x => x.Author).Include(x=>x.Genre).Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Name.Contains(search));

            ListDto<BookListItemDto> listDto = new ListDto<BookListItemDto>
            {
                Items = query.Skip((page - 1) * 6).Take(6).Select(x =>
                    new BookListItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Image=x.Image,
                        Language=x.Language,
                        PageCount=x.PageCount,
                        SalePrice = x.SalePrice,
                        CostPrice = x.CostPrice,
                        Author = new AuthorInBookListItem
                        {

                            Id = x.AuthorId,
                            Name = x.Author.Name,
                            Surname=x.Author.Surname
                            


                        },
                        Genre=new GenreInBookListItemDto
                        {
                            Id=x.GenreId,
                            Name = x.Genre.Name
                        }


                    }).ToList(),
                TotalCount = query.Count()
            };




            return Ok(listDto);
        }
        
        [HttpPost("")]
        public IActionResult Create([FromForm] BookPostDto bookPostDto)
        {
            if (!_context.Authors.Any(x => x.Id == bookPostDto.AuthorId && !x.IsDeleted) && !_context.Genres.Any(x => x.Id == bookPostDto.GenreId && !x.IsDeleted))
                return NotFound();
            
            Book book = new Book
            {
                Name = bookPostDto.Name,
                SalePrice = bookPostDto.SalePrice,
                CostPrice = bookPostDto.CostPrice,
                Language= bookPostDto.Language,
                PageCount = bookPostDto.PageCount,
                GenreId = bookPostDto.GenreId,
                AuthorId = bookPostDto.AuthorId
            };

            book.Image = bookPostDto.ImageFile.SaveImg(_env.WebRootPath, "BookImg");
            _context.Books.Add(book);
            _context.SaveChanges();

            return StatusCode(201, book);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, BookPostDto bookPostDto)
        {
            Book existBook = _context.Books.FirstOrDefault(x => x.Id == id);

            if (existBook == null)
                return NotFound();
            if (bookPostDto.ImageFile != null)
            {


                Helpers.Helper.DeleteImg(_env.WebRootPath, "BookImg", existBook.Image);
                existBook.Image = bookPostDto.ImageFile.SaveImg(_env.WebRootPath, "BookImg");
            }


            if (existBook.AuthorId != bookPostDto.AuthorId && !_context.Authors.Any(c => c.Id == bookPostDto.AuthorId && !c.IsDeleted))
                return NotFound();
            if (existBook.GenreId != bookPostDto.GenreId && !_context.Genres.Any(c => c.Id == bookPostDto.GenreId && !c.IsDeleted))
                return NotFound();
            existBook.AuthorId = bookPostDto.AuthorId;
            existBook.GenreId = bookPostDto.GenreId;

            existBook.Name = bookPostDto.Name;
            existBook.SalePrice = bookPostDto.SalePrice;
            existBook.CostPrice = bookPostDto.CostPrice;


            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            Helpers.Helper.DeleteImg(_env.WebRootPath, "BookImg", book.Image);
            book.IsDeleted = true;
            book.ModifiedAt = DateTime.UtcNow;
            _context.SaveChanges();


            return NoContent();
        }
    }
}
