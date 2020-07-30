using FluentValidation;
using Potter.Characters.Domain.Models;
using Potter.Characters.Utils.Messages;

namespace Potter.Characters.Domain.Validators
{
    public class CharacterValidator : AbstractValidator<Character>
    {
        public CharacterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(CharacterMessages.NameIsRequired);
            RuleFor(x => x.Name).MaximumLength(150).WithMessage(CharacterMessages.NameLessThen);

            RuleFor(x => x.Role).NotEmpty().WithMessage(CharacterMessages.RoleIsRequired);
            RuleFor(x => x.Role).MaximumLength(100).WithMessage(CharacterMessages.RoleLessThen);

            RuleFor(x => x.School).NotEmpty().WithMessage(CharacterMessages.SchoolIsRequired);
            RuleFor(x => x.School).MaximumLength(200).WithMessage(CharacterMessages.SchoolLessThen);

            RuleFor(x => x.House).NotEmpty().WithMessage(CharacterMessages.HouseIsRequired);
            RuleFor(x => x.House).MaximumLength(100).WithMessage(CharacterMessages.HouseLessThen);

            RuleFor(x => x.Patronus).NotEmpty().WithMessage(CharacterMessages.PatronusIsRequired);
            RuleFor(x => x.Patronus).MaximumLength(50).WithMessage(CharacterMessages.PatronusLessThen);
        }
    }
}
