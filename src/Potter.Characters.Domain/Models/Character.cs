using Potter.Characters.Domain.Models.Base;
using System;

namespace Potter.Characters.Domain.Models
{
    public class Character : ModelBase
    {
        public Character(Guid id, string name, string role, string school, string house, string patronus)
        {
            Id = id;
            Name = name;
            Role = role;
            School = school;
            House = house;
            Patronus = patronus;
        }

        public Character(string name, string role, string school, string house, string patronus)
        {
            Id = Guid.NewGuid();
            Name = name;
            Role = role;
            School = school;
            House = house;
            Patronus = patronus;
        }

        public string Name { get; private set; }
        public string Role { get; private set; }
        public string School { get; private set; }
        public string House { get; private set; }
        public string Patronus { get; private set; }
    }
}
