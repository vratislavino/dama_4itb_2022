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

        Tile[,] tiles;
        private int rowCount = 3;

        public Board() {
            InitializeComponent();

            tileSize = this.Width / WIDTH;

            tiles = new Tile[WIDTH, HEIGHT];
            
            CreateCheckboard();
            CreateBaseGame(rowCount);

        }

        private void CreateCheckboard() {
            bool isWhite = true;
            for (int i = 0; i < WIDTH; i++) {
                for (int j = 0; j < HEIGHT; j++) {
                    tiles[i, j] = new Tile(i, j, tileSize, isWhite);
                    isWhite = !isWhite;
                }
                isWhite = !isWhite;
            }
        } 

        private void CreateBaseGame(int count) {
            int x, y;
            for (int i = 0; i < WIDTH / 2; i++) {
                for (int j = 0; j < count; j++) {
                    x = (j % 2 == 1) ? i * 2 : i * 2 + 1;
                    y = j;
                    tiles[x, y].CurrentStone = new Stone(tileSize, true);
                    x = (j % 2 == 0) ? i * 2 : i * 2 + 1;
                    y = HEIGHT - y - 1;
                    tiles[x, y].CurrentStone = new Stone(tileSize, false);

                }
            }
        }

        private void Board_Paint(object sender, PaintEventArgs e) {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawCheckboard(e.Graphics);
        }

        private void DrawCheckboard(Graphics g) {
            for(int i = 0; i < WIDTH; i++) {
                for(int j = 0; j < HEIGHT; j++) {
                    tiles[i, j].Draw(g);
                }
            }
        }

        private void Board_MouseClick(object sender, MouseEventArgs e) {
            int x = e.X / tileSize;
            int y = e.Y / tileSize;

            // TODO: PODMÍNKY PRO HIGHLIGHT A PRAVIDLA
            var selected = tiles[x, y];
            Highlighter.Instance.UnHighlight();
            Highlighter.Instance.Highlight(selected);
            Refresh();
        }
    }
}
