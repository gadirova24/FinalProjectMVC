using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class PersonRepository:BaseRepository<Person>,IPersonRepository
	{
		public PersonRepository(AppDbContext context):base(context)
		{
		}
	}
}

