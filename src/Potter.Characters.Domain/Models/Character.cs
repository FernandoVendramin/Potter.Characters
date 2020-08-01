using FluentValidation.Results;
using MongoDB.Bson.Serialization.Attributes;
using Potter.Characters.Domain.Models.Base;
using Potter.Characters.Domain.Validators;
using System;

namespace Potter.Characters.Domain.Models
{
    public class Character : ModelBase
    {
        public Character(
            string id, 
            string name, 
            string role, 
            string school, 
            House house, 
            string patronus, 
            DateTime editionDateTime, 
            DateTime creationDateTime)
        {
            Id = id;
            Name = name;
            Role = role;
            School = school;
            House = house;
            Patronus = patronus;
            EditionDateTime = editionDateTime;
            CreationDateTime = creationDateTime;
        }

        [BsonRequired()]
        public string Name { get; private set; }

        public string Role { get; private set; }

        public string School { get; private set; }

        public House House { get; private set; }

        public string Patronus { get; private set; }

        [BsonRequired()]
        public DateTime EditionDateTime { get; set; }

        [BsonRequired()]
        public DateTime CreationDateTime { get; set; }

        public ValidationResult Validate()
        {
            var validator = new CharacterValidator();
            return validator.Validate(this);
        }
    }
}
