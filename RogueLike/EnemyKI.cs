using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike
{
    public class EnemyKI
    {
        public int[] Decide(MapTile[,] map, Enemy enemy, string name, Player player)
        {
            int[] paths = GetPaths(map, enemy, name, player);
            int[] way = {0,0};
            if (paths[0] == 1)
            {
                way[0] = paths[0]*32;
                way[1] = 0;
            }
            if (paths[1] == -1)
            {
                way[0] = paths[1]*32;
                way[1] = 0;
            }
            if (paths[2] == 1)
            {
                way[0] = 0;
                way[1] = paths[2]*32;
            }
            if (paths[3] == -1)
            {
                way[0] = 0;
                way[1] = paths[3]*32;
            }
            return way;
        }

        private int[] GetPaths(MapTile[,] map, Enemy enemy, string name, Player player)
        {
            var pos = CoordinatesOf(map, enemy, name);
            int[] paths = {1,-1,1,-1};
            if (map[pos.Item1 + 1, pos.Item2].Texture.Name == "wall/vines0" || map[pos.Item1 + 1, pos.Item2].EntityTexture == player.Texture) // +x
                paths[0] = 0;
            if (map[pos.Item1 - 1, pos.Item2].Texture.Name == "wall/vines0" || map[pos.Item1 - 1, pos.Item2].EntityTexture == player.Texture) // -x
                paths[1] = 0;
            if (map[pos.Item1, pos.Item2+1].Texture.Name == "wall/vines0" || map[pos.Item1, pos.Item2 + 1].EntityTexture == player.Texture) // +y
                paths[2] = 0;
            if (map[pos.Item1, pos.Item2-1].Texture.Name == "wall/vines0" || map[pos.Item1, pos.Item2 - 1].EntityTexture == player.Texture) // -y
                paths[3] = 0;
            return paths;
        }

        private Tuple<int, int> CoordinatesOf(MapTile[,] matrix, Enemy value, string name)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].EntityTexture != null && matrix[x, y].EntityTexture == value.Texture && matrix[x, y].EntityName == name)
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }
    }
}
