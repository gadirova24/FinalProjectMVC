using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class CartonSliderRepository: BaseRepository<CartoonSlider>, ICartoonSliderRepository
    {
		public CartonSliderRepository(AppDbContext context) : base(context)
        {
		}
	}
}

