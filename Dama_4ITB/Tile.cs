using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama_4ITB
{
    public class Tile
    {
        private int x;
        public int X => x;
        private int y;
        public int Y => y;

        private int tileSize;

        private Brush brush;
        private Stone currentStone;
        public Stone CurrentStone {
            get { return currentStone; }
            set { currentStone = value; }
        }

        private bool highlighted = false;

        public bool IsHighlighted => highlighted;

        public Tile(int x, int y, int tileSize, bool isWhite) {
            this.x = x;
            this.y = y;
            this.tileSize = tileSize;
            brush = isWhite ? Brushes.White : Brushes.Black;
        }

        public bool HasStone(bool white) {
            if (CurrentStone == null)
                return false;
            if (CurrentStone.IsWhite != white)
                return false;
            return true;
        }

        public void Draw(Graphics g) {
            
            g.FillRectangle(highlighted ? Brushes.Yellow : brush, x * tileSize, y * tileSize, tileSize, tileSize);
            
            if(currentStone != null) {
                currentStone.Draw(g, x, y);
            }
        }

        public void Highlight() {
            highlighted = true;
        }

        public void Unhighlight() {
            highlighted = false;
        }
    }
}

