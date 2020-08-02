using Potter.Characters.Domain.Models;
using Potter.Characters.Utils.Messages;
using System.Linq;
using Xunit;

namespace Potter.Characters.Domain.Test.Models
{
    public class CharacterTest
    {
        [Fact]
        public void Character_WithHouse_IsValid()
        {
            var house = new House("id", "name", "mascot", "hof", "hog", "founder");
            var character = new Character(
                "id",
                "name",
                null,
                null,
                house,
                null,
                new System.DateTime(),
                new System.DateTime());

            Assert.True(character.Validate().IsValid);
            Assert.Equal(house, character.House);
        }

        [Fact]
        public void Character_WithoutHouse_IsValid()
        {
            var character = new Character(
                "id",
                "name",
                null,
                null,
                null,
                null,
                new System.DateTime(),
                new System.DateTime());

            Assert.True(character.Validate().IsValid);
            Assert.Null(character.House);
        }

        [Fact]
        public void Character_NullFields_IsNotValid()
        {
            var character = new Character(
                null,
                null,
                null,
                null,
                null,
                null,
                new System.DateTime(),
                new System.DateTime());

            var validate = character.Validate();

            Assert.False(validate.IsValid);
            Assert.Equal(2, validate.Errors.Count());
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CommonMessages.IdIsRequired);
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CharacterMessages.NameIsRequired);
        }

        [Fact]
        public void Character_LessThenFields_IsNotValid()
        {
            var character = new Character(
                new string('*', 51), // id
                new string('*', 151), // name
                new string('*', 101), // role
                new string('*', 201), // school
                null,
                new string('*', 51), // patronus
                new System.DateTime(),
                new System.DateTime());

            var validate = character.Validate();

            Assert.False(validate.IsValid);
            Assert.Equal(5, validate.Errors.Count());
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CharacterMessages.IdLessThen);
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CharacterMessages.NameLessThen);
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CharacterMessages.RoleLessThen);
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CharacterMessages.SchoolLessThen);
            Assert.Contains(validate.Errors, x => x.ErrorMessage == CharacterMessages.PatronusLessThen);
        }
    }
}
