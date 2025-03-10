using FluentValidation;
using Library.API.Controllers;
using Library.Services.DTOs;
using Library.Services.Interfaces;
using Library.Services.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.App.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController(IBookService bookService) : BaseController
    {
        private readonly IBookService _bookService = bookService;


        [HttpGet]
        public IActionResult GetBooks()
        {
            var result = _bookService.GetAll();
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetByIdAsync(id);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }


        [HttpPost]
        public async Task<IActionResult> Create(BookDto bookDto)
        {
            var validator = new BookDtoValidator();
            var validationResult = await validator.ValidateAsync(bookDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }
            var result = await _bookService.AddAsync(bookDto);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }


        [HttpPut]
        public async Task<IActionResult> Update(int id, BookDto bookDto)
        {
            var validator = new BookDtoValidator();
            var validationResult = await validator.ValidateAsync(bookDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }
            var result = await _bookService.UpdateAsync(id, bookDto);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooks(int id)
        {
            var result = await _bookService.DeleteAsync(id);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

    }
}
