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
        public event Action<Player, Player> ScoreNeedsToBeUpdatedOnFormBecauseSomethingChangedLol;

        public const int WIDTH = 8;
        public const int HEIGHT = 8;

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
            RecalculateStones();
        }

        private void RecalculateStones() {
            gameLogic.ReadyPlayerOne.currentStoneCount = 0;
            gameLogic.ReadyPlayerTwo.currentStoneCount = 0;

            foreach (var tile in tiles) {
                if(tile.CurrentStone != null) {
                    if (tile.CurrentStone.IsWhite)
                        gameLogic.ReadyPlayerOne.currentStoneCount++; 
                    if (!tile.CurrentStone.IsWhite)
                        gameLogic.ReadyPlayerTwo.currentStoneCount++;
                }
            }
            ScoreNeedsToBeUpdatedOnFormBecauseSomethingChangedLol?.Invoke(gameLogic.ReadyPlayerOne, gameLogic.ReadyPlayerTwo);

            string winner = "";
            if(gameLogic.ReadyPlayerOne.currentStoneCount == 0) {
                winner = gameLogic.ReadyPlayerTwo.name;
            }
            if(gameLogic.ReadyPlayerTwo.currentStoneCount == 0) {
                winner = gameLogic.ReadyPlayerOne.name;
            }
            if(!string.IsNullOrEmpty(winner)) {
                Refresh();
               var res = MessageBox.Show("Vyhrál hráč " + winner + ". Chcete spustit novou hru?", "Konec hry!", MessageBoxButtons.YesNo);
                if(res == DialogResult.Yes) {
                    Application.Restart();
                } else {
                    Application.Exit();
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
                    tiles[x, y].CurrentStone = new Stone(tileSize, gameLogic.ReadyPlayerOne);
                    x = (j % 2 == 0) ? i * 2 : i * 2 + 1;
                    y = HEIGHT - y - 1;
                    tiles[x, y].CurrentStone = new Stone(tileSize, gameLogic.ReadyPlayerTwo);

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

            var selected = tiles[x, y];

            if (selected.HasStone(gameLogic.CurrentPlayer.hasWhite)) {
                Highlighter.Instance.UnHighlight();
                Highlighter.Instance.Highlight(selected);

                //MessageBox.Show($"{x}:{y}");

                gameLogic.CurrentTile = selected;

                var tilesToHighlight = gameLogic.CurrentTile.CurrentStone.GetPossibleTiles(tiles, selected);
                Highlighter.Instance.Highlight(tilesToHighlight);

            } else {
                if (gameLogic.CurrentTile != null) {
                    // Přesun kamene -> ukončení tahu
                    if (gameLogic.CurrentTile.IsHighlighted && selected.IsHighlighted) {
                        // TEST FOR KICKS
                        KickFromTo(selected, gameLogic.CurrentTile);
                        selected.CurrentStone = gameLogic.CurrentTile.CurrentStone;
                        gameLogic.CurrentTile.CurrentStone = null;
                        
                        TestForKitchenLady(selected);
                        gameLogic.SwitchPlayer();
                    }
                }
            }
            Refresh();
        }

        private void KickFromTo(Tile start, Tile end) {
            int startX = start.X;
            int startY = start.Y;
            Point dir = new Point(end.X - startX < 0 ? -1 : 1, end.Y - startY < 0 ? -1 : 1);
            Tile tempTile = null;
            for(int i = 0; i < Math.Abs(end.X - start.X)-1; i++) {

                startX += dir.X;
                startY += dir.Y;
                tempTile = GetTile(startX, startY);
                Console.WriteLine($"Testing: {startX}:{startY}");
                if (tempTile.CurrentStone != null)
                    tempTile.CurrentStone = null;
            }
        }

        private void TestForKitchenLady(Tile tile) {
            if(gameLogic.CurrentPlayer.hasWhite) {
                if (tile.Y == 7) {
                    KitchenLady lady = new KitchenLady(tile.CurrentStone);
                    tile.CurrentStone = lady;
                }
            } else {
                if(tile.Y == 0) {
                    KitchenLady lady = new KitchenLady(tile.CurrentStone);
                    tile.CurrentStone = lady;
                }
            }
        }

        private Tile GetPossibleTile(Tile t, int xDir, int yDir, List<Tile> enemyTiles, int remain) {
            if (remain == 0)
                return null;
            var next = GetTile(t.X + xDir, t.Y + yDir);
            if (next == null)
                return null;
            if (next.CurrentStone == null)
                return next;
            if (next.HasStone(gameLogic.CurrentPlayer.hasWhite))
                return null;

            //enemy stone is here!
            enemyTiles.Add(next);
            return GetPossibleTile(next, xDir, yDir, enemyTiles, remain - 1);
        }

        private Tile GetTile(int x, int y) {
            if (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT)
                return tiles[x, y];
            return null;
        }
    }
}
