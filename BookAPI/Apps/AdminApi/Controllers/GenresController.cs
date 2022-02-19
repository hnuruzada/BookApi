using AutoMapper;
using BookAPI.Apps.AdminApi.DTOs;
using BookAPI.Apps.AdminApi.DTOs.GenreDtos;
using BookAPI.Data.DAL;
using BookAPI.Data.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookAPI.Apps.AdminApi.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public GenresController(BookDbContext context,IWebHostEnvironment env,IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }
        [HttpPost("")]
        public IActionResult Create(GenrePostDto genrePostDto)
        {
            if (_context.Genres.Any(g => g.Name.ToUpper().Trim() == genrePostDto.Name.Trim().ToUpper()))
                return StatusCode(409);
            Genre genre = new Genre
            {
                Name = genrePostDto.Name
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return StatusCode(201, genre);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(g => g.Id == id && !g.IsDeleted);
            if(genre==null) return NotFound();

            GenreGetDto genreDto = _mapper.Map<GenreGetDto>(genre);

            return Ok(genre);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var query=_context.Genres.Where(x => !x.IsDeleted);

            ListDto<GenreListItemDto> listDto = new ListDto<GenreListItemDto>
            {
                TotalCount=query.Count(),
                Items=query.Skip((page-1)*6).Take(6).Select(x=> new GenreListItemDto { Id=x.Id,Name=x.Name}).ToList()
            };
            return Ok(listDto);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id,GenrePostDto genrePostDto)
        {
            Genre genre= _context.Genres.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (genre == null) return NotFound();

            if (_context.Genres.Any(x => x.Id == id && x.Name.ToUpper().Trim() == genrePostDto.Name.ToUpper().Trim()))
                return StatusCode(409);

            genre.Name = genrePostDto.Name;
            genre.ModifiedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(x=>x.Id == id);
            if(genre == null) return NotFound();
            genre.IsDeleted = true;
            genre.ModifiedAt= DateTime.UtcNow;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
