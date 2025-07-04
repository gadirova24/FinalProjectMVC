using System;
namespace Service.ViewModels.Admin.Studio
{
	public class StudioDetailVM
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public int CartoonCount { get; set; }
        public List<string> CartoonNames { get; set; } = new();
    }
}

