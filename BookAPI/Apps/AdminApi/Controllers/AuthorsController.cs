using AutoMapper;
using BookAPI.Apps.AdminApi.DTOs;
using BookAPI.Apps.AdminApi.DTOs.AuthorDtos;
using BookAPI.Data.DAL;
using BookAPI.Data.Entity;
using BookAPI.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookAPI.Apps.AdminApi.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public AuthorsController( BookDbContext context,IWebHostEnvironment env,IMapper mapper)
        {
            _context = context; 
            _env = env;
            _mapper = mapper;
        }
        [HttpPost("")]
        public IActionResult Create([FromForm] AuthorPostDto authorPostDto)
        {
            if (_context.Authors.Any(x => x.Name.ToUpper().Trim() == authorPostDto.Name.ToUpper().Trim()))
                return StatusCode(409);
            Author author = new Author
            {
                Name = authorPostDto.Name,
                Surname = authorPostDto.Surname,
            };
            author.Image = authorPostDto.ImageFile.SaveImg(_env.WebRootPath, "AuthorImg");
            _context.Authors.Add(author);
            _context.SaveChanges();

            return StatusCode(201, author);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id==id && !x.IsDeleted);

            if(author==null) return NotFound();
            AuthorGetDto authorGetDto = _mapper.Map<AuthorGetDto>(author);
            return Ok(authorGetDto);

        }

        [HttpGet("")]
        public IActionResult GetAll(int page=1)
        {
            var query = _context.Authors.Where(x => !x.IsDeleted);

            ListDto<AuthorListItemDto> listDto = new ListDto<AuthorListItemDto>
            {
               TotalCount=query.Count(),
               Items=query.Skip((page - 1) * 8).Take(8).Select(x => new AuthorListItemDto { Id = x.Id, Name = x.Name,Surname=x.Surname, Image = x.Image }).ToList()

            };
            return Ok(listDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,[FromForm] AuthorPostDto authorPostDto)
        {
            Author author= _context.Authors.FirstOrDefault(x=>x.Id==id && !x.IsDeleted);
            if( author==null) return NotFound();
            if (author.Image != null)
            {
                if (authorPostDto.ImageFile != null)
                {

                    Helpers.Helper.DeleteImg(_env.WebRootPath, "AuthorImg", author.Image);
                    author.Image = authorPostDto.ImageFile.SaveImg(_env.WebRootPath, "AuthorImg");
                }
                
            }
            if (_context.Authors.Any(x => x.Id != id && x.Name.ToUpper() == authorPostDto.Name.Trim().ToUpper()))
                return StatusCode(409);
            author.Name = authorPostDto.Name;
            author.Surname = authorPostDto.Surname;
            author.ModifiedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (author == null) return NotFound();
            Helpers.Helper.DeleteImg(_env.WebRootPath, "AuthorImg", author.Image);

            author.IsDeleted = true;
            author.ModifiedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
