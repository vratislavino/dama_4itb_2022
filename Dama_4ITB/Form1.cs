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
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
            board1.ScoreNeedsToBeUpdatedOnFormBecauseSomethingChangedLol += OnScoreNeedsToBeUpdatedOnFormBecauseSomethingChangedLol;
        }

        private void OnScoreNeedsToBeUpdatedOnFormBecauseSomethingChangedLol(Player theFirstPlayerInOurGame, Player theSecondPlayerInOurGame) {
            this.Text = theFirstPlayerInOurGame.name + ": " + theFirstPlayerInOurGame.currentStoneCount + " ; " + theSecondPlayerInOurGame.name + ": " + theSecondPlayerInOurGame.currentStoneCount;
        }
    }
}
