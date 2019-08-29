using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lightsout {
    


    public partial class MainForm : Form {
        LightsOutGame game = new LightsOutGame();
        private const int GridOffset = 25;  // Distance from upper left side of window
        private const int GridLength = 200;

        public MainForm() {
            InitializeComponent();
            game.NewGame();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            newGameButton_Click(sender, e);
        }

        private bool PlayerWon() {
            return game.IsGameOver();
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void newGameButton_Click(object sender, EventArgs e) {
            game.NewGame();
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;

            for (int r = 0; r < game.GridSize; r++) {
                for (int c = 0; c < game.GridSize; c++) {
                    Brush brush;
                    Pen pen;

                    if (game.GetGridValue(r, c)) {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    } else {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = c * (GridLength/game.GridSize) + GridOffset;
                    int y = r * (GridLength / game.GridSize) + GridOffset;

                    g.DrawRectangle(pen, x, y, (GridLength / game.GridSize), (GridLength / game.GridSize));
                    g.FillRectangle(brush, x + 1, y + 1, (GridLength / game.GridSize) - 1, (GridLength / game.GridSize) - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e) {
            if (e.X < GridOffset || e.X > (GridLength / game.GridSize) * game.GridSize + GridOffset || e.Y < GridOffset || e.Y > (GridLength / game.GridSize) * game.GridSize + GridOffset) {
                return;
            }

            int r = (e.Y - GridOffset) / (GridLength / game.GridSize);
            int c = (e.X - GridOffset) / (GridLength / game.GridSize);

            for (int i = r-1; i <= r+1; i++) {
                for (int j = c-1; j <= c+1; j++) {
                    if (i >= 0 && i < game.GridSize && j >= 0 && j < game.GridSize) {
                       game.SetGridValue(i, j, !game.GetGridValue(i,j));
                    }
                }
            }

            this.Invalidate();

            if (PlayerWon()) {
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.GridSize = 3;
            newGameButton_Click(sender, e);
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.GridSize = 4;
            newGameButton_Click(sender, e);
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.GridSize = 5;
            newGameButton_Click(sender, e);
        }
    }

    class LightsOutGame
    {
        private int gridSize = 3;
        private bool[,] grid;
        private Random rand;
        public const int MaxGridSize = 7;
        public const int MinGridSize = 3;

        public int GridSize
        {
            get
            {
                return gridSize;
            }
            set
            {
                if (value >= MinGridSize && value <= MaxGridSize)
                {
                    gridSize = value;
                    grid = new bool[gridSize, gridSize];
                }
                else
                {
                    throw new ArgumentOutOfRangeException("NumCells must be between " + MinGridSize + " and " + MaxGridSize + ".");
                }
            }
        }

        public LightsOutGame()
        {
            rand = new Random();

            GridSize = MinGridSize;
        }

        public bool GetGridValue(int row, int col)
        {
            return grid[row, col];
        }

        public void SetGridValue(int row, int col, bool value)
        {
            grid[row, col] = value;
        }

        public void NewGame()
        {
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }
        }

        public void Move(int row, int col)
        {
            if (row < 0 || row >= gridSize || col < 0 || col >= gridSize)
            {
                throw new ArgumentException("Row or column is outsize the legal range of 0 to " + (gridSize - 1));
            }
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
                    {
                        grid[i, j] = !grid[i, j];
                    }
                }
            }
        }

        public bool IsGameOver()
        {
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    if (grid[r, c])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
