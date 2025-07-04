using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface ICommentRepository
	{
        Task<Comment> GetByIdAsync(int id);
        Task<List<Comment>> GetByCartoonIdAsync(int cartoonId);
        Task AddAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        Task<List<Comment>> GetAllAsync();
    }
}

