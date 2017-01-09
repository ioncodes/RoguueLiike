using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    public class Equipment
    {
        public class EThumbnails
        {
            public Texture2D Helmet { get; set; }
            public Texture2D Mail { get; set; }
            public Texture2D Boots { get; set; }
            public Texture2D Weapon { get; set; }
            public Texture2D Shield { get; set; }
            public List<Texture2D> EquipmentSet { get; } = new List<Texture2D>();

            public void Update()
            {
                EquipmentSet.Clear();
                if (Helmet != null)
                    EquipmentSet.Add(Helmet);
                if (Mail != null)
                    EquipmentSet.Add(Mail);
                if (Boots != null)
                    EquipmentSet.Add(Boots);
                if (Weapon != null)
                    EquipmentSet.Add(Weapon);
                if (Shield != null)
                    EquipmentSet.Add(Shield);
            }
        }

        public class ESkins
        {
            public Texture2D Helmet { get; set; }
            public Texture2D Mail { get; set; }
            public Texture2D Boots { get; set; }
            public Texture2D Weapon { get; set; }
            public Texture2D Shield { get; set; }
            public List<Texture2D> EquipmentSet { get; } = new List<Texture2D>();

            public void Update()
            {
                EquipmentSet.Clear();
                if (Helmet != null)
                    EquipmentSet.Add(Helmet);
                if (Mail != null)
                    EquipmentSet.Add(Mail);
                if (Boots != null)
                    EquipmentSet.Add(Boots);
                if (Weapon != null)
                    EquipmentSet.Add(Weapon);
                if (Shield != null)
                    EquipmentSet.Add(Shield);
            }
        }

        public EThumbnails Thumbnails { get; } = new EThumbnails();
        public ESkins Skins { get; } = new ESkins();
    }
}
