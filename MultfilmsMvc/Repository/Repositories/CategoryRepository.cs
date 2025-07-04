using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class CategoryRepository:BaseRepository<Category>,ICategoryRepository
	{
		public CategoryRepository(AppDbContext context):base(context)
		{
				
		}
	}
}

