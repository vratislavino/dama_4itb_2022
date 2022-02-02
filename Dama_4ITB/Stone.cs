using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama_4ITB
{
    public class Stone
    {
        // direktivy preprocesoru

        #region Variables

        private Player player;
        public Player Player => player; 
        
        //protected bool isWhite;
        public bool IsWhite => player.hasWhite;

        public int tileSize;

        static protected int innerOffset = 12;
        static Pen outlinePenBlack = new Pen(Color.Black, 4);
        static Pen outlinePenWhite = new Pen(Color.White, 4);

        #endregion

        public Stone(int tileSize, Player player) {
            this.tileSize = tileSize;
            this.player = player;
        }

        public virtual void Draw(Graphics g, int x, int y) {
            if (IsWhite) {
                g.FillEllipse(
                    Brushes.SandyBrown,
                    x * tileSize + innerOffset,
                    y * tileSize + innerOffset,
                    tileSize - 2 * innerOffset,
                    tileSize - 2 * innerOffset);

                g.DrawEllipse(outlinePenBlack, x * tileSize + innerOffset,
                    y * tileSize + innerOffset,
                    tileSize - 2 * innerOffset,
                    tileSize - 2 * innerOffset);
                g.DrawEllipse(outlinePenBlack, x * tileSize + 3*innerOffset,
                    y * tileSize + 3*innerOffset,
                    tileSize - 6 * innerOffset,
                    tileSize - 6 * innerOffset);
            } else {
                g.FillEllipse(
                    Brushes.DarkSlateGray,
                    x * tileSize + innerOffset,
                    y * tileSize + innerOffset,
                    tileSize - 2 * innerOffset,
                    tileSize - 2 * innerOffset);

                g.DrawEllipse(outlinePenWhite, x * tileSize + innerOffset,
                    y * tileSize + innerOffset,
                    tileSize - 2 * innerOffset,
                    tileSize - 2 * innerOffset);
                g.DrawEllipse(outlinePenWhite, x * tileSize + 3 * innerOffset,
                    y * tileSize + 3 * innerOffset,
                    tileSize - 6 * innerOffset,
                    tileSize - 6 * innerOffset);
            }
        }

        public virtual List<Tile> GetPossibleTiles(Tile[,] tiles, Tile t) {
            List<Tile> possibleTiles = new List<Tile>();
            
            GetPossibleTile(tiles, t, possibleTiles, 1, player.directionY, 2);
            GetPossibleTile(tiles, t, possibleTiles, -1, player.directionY, 2);

            return possibleTiles;
        }

        protected virtual Tile GetPossibleTile(Tile[,] tiles, Tile t, List<Tile> possibleTiles, int xDir, int yDir, int remain) {
            if (remain == 0)
                return null;
            var next = GetTile(tiles, t.X + xDir, t.Y + yDir);
            if (next == null)
                return null;
            if (next.CurrentStone == null) {
                possibleTiles.Add(next);
                return next;
            }
            if (next.HasStone(player.hasWhite))
                return null;

            //enemy stone is here!
            return GetPossibleTile(tiles, next, possibleTiles, xDir, yDir, remain - 1);
        }

        protected Tile GetTile(Tile[,] tiles, int x, int y) {
            if (x >= 0 && x < Board.WIDTH && y >= 0 && y < Board.HEIGHT)
                return tiles[x, y];
            return null;
        }
    }
}
