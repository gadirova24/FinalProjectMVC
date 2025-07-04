using System;
using Domain.Entities;

namespace Service.ViewModels.UI
{
	public class PersonVM
	{
		public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public ICollection<CartoonActor> CartoonActors { get; set; }
        public ICollection<CartoonDirector> CartoonDirectors { get; set; }
    }
}

