using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Levels
{
    class Level0 : LevelInfo
    {
        public override int Level { get; } = 0;
        public override string Name { get; } = "Noob";
        public override int XP { get; } = 50;
        public override int Health { get; } = 20;
        public override int Shield { get; } = 0;
        public override int Damage { get; } = 10;
    }
}
