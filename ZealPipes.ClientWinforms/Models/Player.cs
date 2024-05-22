using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZealPipes.Common.Models;
namespace ZealPipes.ClientWinforms.Models
{
    public class Player
    {
        public string Name { get; internal set; }
        public int Level { get; internal set; }
        public int HP { get; internal set; }
        public int Mana { get; internal set; }
        public ZealCharacter _zealCharacter { get; internal set; }
        public Player(ZealCharacter zealCharacter)
        {
            _zealCharacter = zealCharacter;
            Name = zealCharacter.Name;
            Debug.WriteLine(JsonSerializer.Serialize(zealCharacter));
        }
    }
}
