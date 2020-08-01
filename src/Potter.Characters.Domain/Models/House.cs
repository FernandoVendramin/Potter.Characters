using MongoDB.Bson.Serialization.Attributes;
using Potter.Characters.Domain.Models.Base;

namespace Potter.Characters.Domain.Models
{
    public class House : ModelBase
    {
        public House(string id, string name, string mascot, string headOfHouse, string houseGhost, string founder)
        {
            Id = id;
            Name = name;
            Mascot = mascot;
            HeadOfHouse = headOfHouse;
            HouseGhost = houseGhost;
            Founder = founder;
        }

        [BsonRequired]
        public string Name { get; private set; }
        [BsonRequired]
        public string Mascot { get; private set; }
        [BsonRequired]
        public string HeadOfHouse { get; private set; }
        [BsonRequired]
        public string HouseGhost { get; private set; }
        [BsonRequired]
        public string Founder { get; private set; }
    }
}
