using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    static class DamageDescriber
    {
        public static Texture2D AlmostDead { get; set; }
        public static Texture2D SeverelyDamaged { get; set; }
        public static Texture2D HeavilyDamaged { get; set; }
        public static Texture2D ModeratelyDamaged { get; set; }
        public static Texture2D LightlyDamaged { get; set; }
    }
}
