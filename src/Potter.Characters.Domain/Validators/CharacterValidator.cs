using FluentValidation;
using Potter.Characters.Domain.Models;
using Potter.Characters.Utils.Messages;

namespace Potter.Characters.Domain.Validators
{
    public class CharacterValidator : AbstractValidator<Character>
    {
        public CharacterValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(CommonMessages.IdIsRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(CharacterMessages.NameIsRequired);

            RuleFor(x => x.Id).MaximumLength(50).WithMessage(CharacterMessages.IdLessThen);
            RuleFor(x => x.Name).MaximumLength(150).WithMessage(CharacterMessages.NameLessThen);
            RuleFor(x => x.Role).MaximumLength(100).WithMessage(CharacterMessages.RoleLessThen);
            RuleFor(x => x.School).MaximumLength(200).WithMessage(CharacterMessages.SchoolLessThen);
            RuleFor(x => x.Patronus).MaximumLength(50).WithMessage(CharacterMessages.PatronusLessThen);
        }
    }
}
