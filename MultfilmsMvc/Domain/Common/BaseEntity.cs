﻿using System;
namespace Domain.Common
{
	public abstract class BaseEntity
	{
		public int Id  { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}

