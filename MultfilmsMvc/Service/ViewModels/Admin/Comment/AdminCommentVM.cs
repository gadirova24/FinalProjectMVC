using System;
namespace Service.ViewModels.Admin.Comment
{
	public class AdminCommentVM
	{
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public int CartoonId { get; set; }
        public string CartoonName { get; set; }
    }
}

