using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dama_4ITB
{
    public partial class Board : UserControl
    {
        const int WIDTH = 8;
        const int HEIGHT = 8;

        private int tileSize;

        Stone[,] stones;

        public Board() {
            InitializeComponent();

            tileSize = this.Width / WIDTH;

            stones = new Stone[WIDTH, HEIGHT];
            stones[3, 3] = new Stone(tileSize, true, 3, 3);
            stones[3, 4] = new Stone(tileSize, true, 3, 4);

            stones[4, 3] = new Stone(tileSize, false, 4, 3);
            stones[4, 4] = new Stone(tileSize, false, 4, 4);
        }

        private void Board_Paint(object sender, PaintEventArgs e) {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawCheckBoard(e.Graphics);
            DrawStones(e.Graphics);
        }

        private void DrawStones(Graphics g) {
            for (int i = 0; i < stones.GetLength(0); i++) {
                for (int j = 0; j < stones.GetLength(1); j++) {
                    if(stones[i,j] != null) {
                        stones[i, j].Draw(g);
                    }
                }
            }
        }

        private void DrawCheckBoard(Graphics g) {
            bool isWhite = true;
            for (int i = 0; i < WIDTH; i++) {
                for (int j = 0; j < HEIGHT; j++) {
                    g.FillRectangle((isWhite ? Brushes.White : Brushes.Black), i * tileSize, j * tileSize, tileSize, tileSize);
                    isWhite = !isWhite;
                }
                isWhite = !isWhite;
            }
        }
    }
}
