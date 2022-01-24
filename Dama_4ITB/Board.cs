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
        private GameLogic gameLogic;

        public Board() {
            InitializeComponent();
            gameLogic = new GameLogic();

            gameLogic.PlayerChanged += OnPlayerChanged;
            tileSize = this.Width / WIDTH;
            tiles = new Tile[WIDTH, HEIGHT];
            
            CreateCheckboard();
            CreateBaseGame(rowCount);

            gameLogic.StartGame();
        }

        private void OnPlayerChanged(Player obj) {
            Highlighter.Instance.UnHighlight();
            foreach(var tile in tiles) {
                if(tile.HasStone(obj.hasWhite)) {
                    Highlighter.Instance.Highlight(tile);
                }
            }
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

            // TODO: ODSTRAŇOVAT PŘESKOČENÝ KÁMEN
            var selected = tiles[x, y];

            if (selected.HasStone(gameLogic.CurrentPlayer.hasWhite)) {
                Highlighter.Instance.UnHighlight();
                Highlighter.Instance.Highlight(selected);

                gameLogic.CurrentTile = selected;


                var t1 = GetPossibleTile(selected, 1, gameLogic.CurrentPlayer.directionY, 2);
                var t2 = GetPossibleTile(selected, -1, gameLogic.CurrentPlayer.directionY, 2);
                //var t1 = GetTile(x + 1, y + gameLogic.CurrentPlayer.directionY);
                //var t2 = GetTile(x - 1, y + gameLogic.CurrentPlayer.directionY);
                if (t1 != null)
                    Highlighter.Instance.Highlight(t1);
                if (t2 != null)
                    Highlighter.Instance.Highlight(t2);
            } else {
                if (gameLogic.CurrentTile != null) {
                    if (gameLogic.CurrentTile.IsHighlighted && selected.IsHighlighted) {
                        selected.CurrentStone = gameLogic.CurrentTile.CurrentStone;
                        gameLogic.CurrentTile.CurrentStone = null;
                        gameLogic.SwitchPlayer();
                    }
                }
            }
            Refresh();
        }

        private Tile GetPossibleTile(Tile t, int xDir, int yDir, int remain) {
            if (remain == 0)
                return null;
            var next = GetTile(t.X + xDir, t.Y + yDir);
            if (next == null)
                return null;
            if (next.CurrentStone == null)
                return next;
            if (next.HasStone(gameLogic.CurrentPlayer.hasWhite))
                return null;

            return GetPossibleTile(next, xDir, yDir, remain - 1);
        }

        private Tile GetTile(int x, int y) {
            if (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT)
                return tiles[x, y];
            return null;
        }
    }
}
