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
        private int rowCount = 1;
        private GameLogic gameLogic;

        Turn turn1;
        Turn turn2;

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
                Refresh();

                return;
                List<Tile> enTiles1 = new List<Tile>();
                List<Tile> enTiles2 = new List<Tile>();
                
                var t1 = GetPossibleTile(selected, 1, gameLogic.CurrentPlayer.directionY, enTiles1, 2);
                var t2 = GetPossibleTile(selected, -1, gameLogic.CurrentPlayer.directionY, enTiles2, 2);

                turn1 = new Turn(t1, enTiles1);
                turn2 = new Turn(t2, enTiles2);

                if (t1 != null)
                    Highlighter.Instance.Highlight(t1);
                if (t2 != null)
                    Highlighter.Instance.Highlight(t2);
            } else {
                if (gameLogic.CurrentTile != null) {
                    // Přesun kamene -> ukončení tahu
                    if (gameLogic.CurrentTile.IsHighlighted && selected.IsHighlighted) {
                        selected.CurrentStone = gameLogic.CurrentTile.CurrentStone;
                        gameLogic.CurrentTile.CurrentStone = null;
                        // skočil jsem kámen?
                        SolveTurn(selected, turn1);
                        SolveTurn(selected, turn2);
                        TestForKitchenLady(selected);
                        gameLogic.SwitchPlayer();
                    }
                }
            }
            Refresh();
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

        private void SolveTurn(Tile selected, Turn t) {
            if (selected == t.TargetTile) {
                for (int i = 0; i < t.EnemyTiles.Count; i++) {
                    t.EnemyTiles[i].CurrentStone = null;
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
