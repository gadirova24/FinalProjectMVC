using System;
namespace Service.ViewModels.UI
{
	public class CommentVM
	{
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
    }
}

