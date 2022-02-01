using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama_4ITB
{
    public struct Turn
    {
        public Tile TargetTile;
        public List<Tile> EnemyTiles;

        public Turn(Tile target, List<Tile> enemies) {
            TargetTile = target;
            EnemyTiles = enemies;
        }
    }
}
