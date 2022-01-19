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

        protected bool isWhite;
        public bool IsWhite => isWhite;

        protected int x;
        public int X => x;

        protected int y;
        public int Y => y;

        protected int tileSize;

        static int innerOffset = 12;
        static Pen outlinePenBlack = new Pen(Color.Black, 4);
        static Pen outlinePenWhite = new Pen(Color.White, 4);

        #endregion

        public Stone(int tileSize, bool isWhite, int x, int y) {
            this.tileSize = tileSize;
            this.isWhite = isWhite;
            this.x = x;
            this.y = y;
        }

        public virtual void Draw(Graphics g) {
            if (isWhite) {
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

    }
}
