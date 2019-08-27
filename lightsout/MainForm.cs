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
        private const int GridOffset = 25;  // Distance from upper left side of window
        private const int GridLength = 200;
        private int NumCells = NumCellsx3;
        private const int NumCellsx3 = 3;
        private const int NumCellsx4 = 4;
        private const int NumCellsx5 = 5;

        private bool[,] grid;
        private Random rand;

        public MainForm() {
            InitializeComponent();

            rand = new Random();
            grid = new bool[NumCells, NumCells];
            for (int r = 0; r < NumCells; r++) {
                for (int c = 0; c < NumCells; c++) {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            newGameButton_Click(sender, e);
        }

        private bool PlayerWon() {
            bool won = true;
            for (int r = 0; r < NumCells; r++) {
                for (int c = 0; c < NumCells; c++) {
                    if (grid[r, c]) {
                        won = false;
                    }
                }
            }
            return won;
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void newGameButton_Click(object sender, EventArgs e) {
            for (int r = 0; r < NumCells; r++) {
                for (int c = 0; c < NumCells; c++) {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }

            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;

            for (int r = 0; r < NumCells; r++) {
                for (int c = 0; c < NumCells; c++) {
                    Brush brush;
                    Pen pen;

                    if (grid[r, c]) {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    } else {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = c * (GridLength/NumCells) + GridOffset;
                    int y = r * (GridLength / NumCells) + GridOffset;

                    g.DrawRectangle(pen, x, y, (GridLength / NumCells), (GridLength / NumCells));
                    g.FillRectangle(brush, x + 1, y + 1, (GridLength / NumCells) - 1, (GridLength / NumCells) - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e) {
            if (e.X < GridOffset || e.X > (GridLength / NumCells) * NumCells + GridOffset || e.Y < GridOffset || e.Y > (GridLength / NumCells) * NumCells + GridOffset) {
                return;
            }

            int r = (e.Y - GridOffset) / (GridLength / NumCells);
            int c = (e.X - GridOffset) / (GridLength / NumCells);

            for (int i = r-1; i <= r+1; i++) {
                for (int j = c-1; j <= c+1; j++) {
                    if (i >= 0 && i < NumCells && j >= 0 && j < NumCells) {
                        grid[i, j] = !grid[i, j];
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
            NumCells = NumCellsx3;
            grid = new bool[NumCells, NumCells];
            newGameButton_Click(sender, e);
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumCells = NumCellsx4;
            grid = new bool[NumCells, NumCells];
            newGameButton_Click(sender, e);
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumCells = NumCellsx5;
            grid = new bool[NumCells, NumCells];
            newGameButton_Click(sender, e);
        }
    }
}
