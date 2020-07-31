namespace Potter.Characters.Application.DTOs.House
{
    public class HouseResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mascot { get; set; }
        public string HeadOfHouse { get; set; }
        public string HouseGhost { get; set; }
        public string Founder { get; set; }
        public string School { get; set; }

        public static explicit operator HouseResponse(Domain.Models.House house)
        {
            return house != null ? new HouseResponse()
            {
                Id = house.Id,
                Name = house.Name,
                Mascot = house.Mascot,
                HeadOfHouse = house.School,
                HouseGhost = house.HouseGhost,
                Founder = house.Founder,
                School = house.School
            } : null;
        }
    }
}
