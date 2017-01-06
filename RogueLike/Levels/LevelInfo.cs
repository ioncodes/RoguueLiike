using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Levels
{
    public abstract class LevelInfo
    {
        public abstract int Level { get; }
        public abstract string Name { get; }
        public abstract int XP { get; }
        public abstract int Health { get; }
        public abstract int Shield { get; }
        public abstract int Damage { get; }
    }
}
