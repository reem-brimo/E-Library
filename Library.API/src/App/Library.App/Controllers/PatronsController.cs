using Library.API.Controllers;
using Library.Services.DTOs;
using Library.Services.Interfaces;
using Library.Services.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.App.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PatronsController(IPatronService patronService) : BaseController
    {
        private readonly IPatronService _patronService = patronService;

      
        [HttpGet]
        public IActionResult GetPatrons()
        {
            var result = _patronService.GetAll();
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _patronService.GetByIdAsync(id);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

       
        [HttpPost]
        public async Task<IActionResult> Create(PatronDto patronDto)
        {
            var validator = new PatronDtoValidator();
            var validationResult = await validator.ValidateAsync(patronDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var result =  await _patronService.AddAsync(patronDto);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

       
        [HttpPut]
        public async Task<IActionResult> Update(int id,PatronDto patronDto)
        {
            var validator = new PatronDtoValidator();
            var validationResult = await validator.ValidateAsync(patronDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }
            var result = await _patronService.UpdateAsync(id, patronDto);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatron(int id)
        {
            var result = await _patronService.DeleteAsync(id);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);

        }

    }
}
