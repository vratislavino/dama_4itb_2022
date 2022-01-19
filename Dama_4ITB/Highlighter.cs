using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama_4ITB
{
    public class Highlighter
    {
        private static Highlighter instance = new Highlighter();
        public static Highlighter Instance => instance;
        private Highlighter() { }

        public List<Tile> tiles = new List<Tile>();

        public void Highlight(List<Tile> newTiles) {
            for(int i = 0; i < newTiles.Count; i++) {
                Highlight(newTiles[i]);
            }
        }

        public void Highlight(Tile newTile) {
            tiles.Add(newTile);

            tiles.ForEach(x => x.Highlight());
        }

        public void UnHighlight() {
            tiles.ForEach(x => x.Unhighlight());
            tiles.Clear();
        }


    }
}
