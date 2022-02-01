using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama_4ITB
{
    public class GameLogic
    {
        public event Action<Player> PlayerChanged;

        Player p1;
        Player p2;

        public Player ReadyPlayerOne => p1;
        public Player ReadyPlayerTwo => p2;

        Player currentPlayer;
        public Player CurrentPlayer {
            get { return currentPlayer; }
            set {
                currentPlayer = value;
                PlayerChanged?.Invoke(currentPlayer);
            }
        }

        private Tile currentTile;
        public Tile CurrentTile {
            get { return currentTile; }
            set { currentTile = value; }
        }

        public GameLogic(string name1="Bob", string name2="Dean") {
            p1 = new Player() { name = name1, hasWhite = true, directionY = 1 };
            p2 = new Player() { name = name2, hasWhite = false, directionY = -1 };
        }

        public void StartGame() {
            CurrentPlayer = p1;
        }

        public void SwitchPlayer() {
            CurrentPlayer = CurrentPlayer == p1 ? p2 : p1;
        }
    }
}
