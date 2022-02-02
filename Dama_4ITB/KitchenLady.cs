using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama_4ITB
{
    public class KitchenLady : Stone
    {
        public KitchenLady(int tileSize, Player player) : base(tileSize, player) {
        }

        public KitchenLady(Stone stone) : base(stone.tileSize, stone.Player) {
        }

        public override void Draw(Graphics g, int x, int y) {
            base.Draw(g, x, y);
            g.FillEllipse(Brushes.RoyalBlue, x * tileSize + 3 * innerOffset,
                    y * tileSize + 3 * innerOffset,
                    tileSize - 6 * innerOffset,
                    tileSize - 6 * innerOffset);
            //g.DrawString("D", new Font(FontFamily, Brushes.Red, x * tileSize + tileSize / 2, y * tileSize + tileSize / 2);
        }

        public override List<Tile> GetPossibleTiles(Tile[,] tiles, Tile t) {
            List<Tile> possibleTiles = new List<Tile>();

            GetPossibleTile(tiles, t, possibleTiles, 1, Player.directionY, 8);
            GetPossibleTile(tiles, t, possibleTiles, -1, Player.directionY, 8);
            GetPossibleTile(tiles, t, possibleTiles, 1, -Player.directionY, 8);
            GetPossibleTile(tiles, t, possibleTiles, -1, -Player.directionY, 8);

            return possibleTiles;
        }
        protected override Tile GetPossibleTile(Tile[,] tiles, Tile t, List<Tile> possibleTiles, int xDir, int yDir, int remain) {
            if (remain == 0)
                return null;
            var next = GetTile(tiles, t.X + xDir, t.Y + yDir);
            if (next == null)
                return null;
            if (next.CurrentStone == null) {
                possibleTiles.Add(next);
                return GetPossibleTile(tiles, next, possibleTiles, xDir, yDir, remain - 1); ;
            }
            if (next.HasStone(Player.hasWhite))
                return null;

            //enemy stone is here!
            return GetPossibleTile(tiles, next, possibleTiles, xDir, yDir, remain - 1);

        }
    }
}
