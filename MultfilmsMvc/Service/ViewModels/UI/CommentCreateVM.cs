using System;
using FluentValidation;

namespace Service.ViewModels.UI
{
	public class CommentCreateVM
	{
        public string Text { get; set; }

        public int CartoonId { get; set; }
    }
    public class CommentCreateVMValidator : AbstractValidator<CommentCreateVM>
    {
        public CommentCreateVMValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MinimumLength(10);
        }
    }
}

