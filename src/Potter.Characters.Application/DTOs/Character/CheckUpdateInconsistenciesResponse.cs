namespace Potter.Characters.Application.DTOs.Character
{
    public class CheckUpdateInconsistenciesResponse
    {
        public Domain.Models.Character Character { get; set; }
        public Domain.Models.House House { get; set; }
    }
}
