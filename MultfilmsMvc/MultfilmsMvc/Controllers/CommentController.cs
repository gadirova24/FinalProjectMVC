using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.UI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MultfilmsMvc.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

    
        [HttpPost]
        public async Task<IActionResult> Add(CommentCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                return BadRequest(error ?? "Ошибка валидации.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _commentService.AddCommentAsync(vm, userId);

            return Ok(); 
        }
    
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int cartoonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                bool isAdmin = User.IsInRole("Admin");
                await _commentService.DeleteCommentAsync(id, userId, isAdmin);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch
            {
                return BadRequest("Ошибка при удалении комментария.");
            }
        }
    }

}

