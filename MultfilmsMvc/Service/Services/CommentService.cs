    using System;
    using AutoMapper;
    using Domain.Entities;
    using Repository.Repositories.Interfaces;
    using Service.Helpers.Exceptions;
    using Service.Services.Interfaces;
using Service.ViewModels.Admin.Comment;
using Service.ViewModels.UI;

    namespace Service.Services
    {
        public class CommentService : ICommentService
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IMapper _mapper;

            public CommentService(ICommentRepository commentRepository, IMapper mapper)
            {
                _commentRepository = commentRepository;
                _mapper = mapper;
            }

            public async Task<List<CommentVM>> GetCommentsAsync(int cartoonId)
            {
                var comments = await _commentRepository.GetByCartoonIdAsync(cartoonId);
                return _mapper.Map<List<CommentVM>>(comments);
            }
        public async Task<List<AdminCommentVM>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllAsync(); 
            return _mapper.Map<List<AdminCommentVM>>(comments);
        }

        public async Task AddCommentAsync(CommentCreateVM vm, string userId)
            {
                var comment = new Comment
                {
                    Text = vm.Text,
                    CreatedAt = DateTime.UtcNow,
                    CartoonId = vm.CartoonId,
                    UserId = userId
                };

                await _commentRepository.AddAsync(comment);
            }

            public async Task DeleteCommentAsync(int commentId, string userId, bool isAdmin = false)
            {
                var comment = await _commentRepository.GetByIdAsync(commentId);
                if (comment == null)
                    throw new NotFoundException("Комментарий не найден.");

                if (!isAdmin && comment.UserId != userId)
                    throw new UnauthorizedAccessException("Вы не можете удалить этот комментарий.");

                await _commentRepository.DeleteAsync(comment);
            }
        }

    }

