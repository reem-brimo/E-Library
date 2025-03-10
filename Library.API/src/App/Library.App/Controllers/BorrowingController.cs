using Library.API.Controllers;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.App.Controllers
{
    [Route("api")]
    [Authorize]
    public class BorrowingController(IBorrowingService borrowingService) : BaseController
    {
      
        private readonly IBorrowingService _borrowingService = borrowingService;


        [HttpPost("borrow/{bookId}/patron/{patronId}")]
        public async Task<IActionResult> BorrowBook(int bookId, int patronId)
        {
           
            var result = await _borrowingService.AddBorrowAsync(bookId, patronId);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

        [HttpPut("return/{bookId}/patron/{patronId}")]
        public async Task<IActionResult> ReturnBook(int bookId, int patronId)
        {
            var result = await _borrowingService.UpdateBorrowAsync(bookId, patronId);
            return GetResult(result.ErrorMessages, result.EnumResult, result.Result);
        }

    }
}
