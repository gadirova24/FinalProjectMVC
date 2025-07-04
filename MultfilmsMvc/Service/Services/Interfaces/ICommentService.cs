using System;
using Domain.Entities;
using Service.ViewModels.Admin.Comment;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface ICommentService
	{
        Task<List<CommentVM>> GetCommentsAsync(int cartoonId);
        Task AddCommentAsync(CommentCreateVM vm, string userId);
        Task DeleteCommentAsync(int commentId, string userId, bool isAdmin = false);
        Task<List<AdminCommentVM>> GetAllCommentsAsync();
    }
}

