using System;
using System.Collections.Generic;
using System.Text;

namespace Potter.Characters.Application.DTOs.Character
{
    public class CharacterRequestFilter
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string School { get; set; }
        public string House { get; set; }
        public string Patronus { get; set; }
    }
}
