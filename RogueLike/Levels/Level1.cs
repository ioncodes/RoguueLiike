using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Levels
{
    class Level1 : LevelInfo
    {
        public override int Level { get; } = 1;
        public override string Name { get; } = "N00b";
        public override int XP { get; } = 100;
        public override int Health { get; } = 30;
        public override int Shield { get; } = 5;
        public override int Damage { get; } = 20;
    }
}
