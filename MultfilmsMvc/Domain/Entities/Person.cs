using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Person:BaseEntity
	{
		public string FullName { get; set; }
		public ICollection<CartoonActor> CartoonActors { get; set; }
        public ICollection<CartoonDirector> CartoonDirectors { get; set; }
        public ICollection<PersonRole> Roles { get; set; }
    }
}

