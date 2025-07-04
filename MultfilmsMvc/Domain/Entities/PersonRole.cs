using System;
using Domain.Common;
using Domain.Helpers.Enums;

namespace Domain.Entities
{
	public class PersonRole:BaseEntity
	{
        public PersonRoleType RoleType { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}

