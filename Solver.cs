using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace MineSweeperSolver
{
    class Solver
    {
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        //This hide the console
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        public static void RightMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
        }

        public static void RemoveMouseFromPlay(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
        }


        public static void UpdateBoard(Bitmap playField, Tile[ , ] board)
        {
            int tileWidth = 30;
            int tileHeight = 30;

            Color blue = Color.FromArgb(0,52,222);       // Colour of the one
            Color green = Color.FromArgb(15,149,13);     // Colour of the two
            Color red = Color.FromArgb(193,16,16);       // Colour of the three
            Color orange = Color.FromArgb(255,108,0);    // Colour of the four
            Color black = Color.FromArgb(0,0,0);         // Colour of bomb
            Color grey = Color.FromArgb(233,233,233);


            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (ScreenShot.PixelInRange(black, playField, (i * tileWidth) + 10, (j * tileHeight) + 10, (i * tileWidth) + 20, (j * tileHeight) + 20) )
                    {
                        Console.WriteLine("hit a bomb");
                        Console.ReadLine();
                    }

                    else if (ScreenShot.PixelInRange(blue, playField, (i * tileWidth) + 10, (j * tileHeight) + 10, (i * tileWidth) + 20, (j * tileHeight) + 20) && board[i, j].GetTileType() == TileType.Unknown)
                    {
                        board[i,j] = new Tile(1,TileType.Clear,i,j);
                    }

                    else if (ScreenShot.PixelInRange(green, playField, (i * tileWidth) + 10, (j * tileHeight) + 10, (i * tileWidth) + 20, (j * tileHeight) + 20) && board[i, j].GetTileType() == TileType.Unknown)
                    {
                        board[i, j] = new Tile(2, TileType.Clear, i, j);
                    }

                    else if (ScreenShot.PixelInRange(red, playField, (i * tileWidth) + 10, (j * tileHeight) + 10, (i * tileWidth) + 20, (j * tileHeight) + 20) && board[i, j].GetTileType() == TileType.Unknown)
                    {
                        board[i, j] = new Tile(3, TileType.Clear, i, j);
                    }

                    else if (ScreenShot.PixelInRange(orange, playField, (i * tileWidth) + 10, (j * tileHeight) + 10, (i * tileWidth) + 20, (j * tileHeight) + 20) && board[i, j].GetTileType() == TileType.Unknown)
                    {
                        board[i, j] = new Tile(4, TileType.Clear, i, j);
                    }

                    else if (ScreenShot.PixelInRange(grey, playField, (i * tileWidth) + 10, (j * tileHeight) + 10, (i * tileWidth) + 20, (j * tileHeight) + 20) && board[i, j].GetTileType() == TileType.Unknown)
                    {
                        board[i, j] = new Tile(0, TileType.Clear, i, j);
                    }
                    //else
                    //{
                    //    board[i, j] = new Tile(TileType.Unknown, i, j);
                    //}
                }
            }

        }

        public static void FindAndAddNeighbours(Tile numberedTile, Tile[,] board){
            int xpos = numberedTile.GetXpos();
            int ypos = numberedTile.GetYpos();

            if (xpos != 0)
            {
                if (board[xpos - 1, ypos].GetTileType().Equals(TileType.Unknown))
                {
                    numberedTile.AddNeighbour(board[xpos-1, ypos]);
                }
                if (ypos != 0)
                {
                    if (board[xpos - 1, ypos-1].GetTileType().Equals(TileType.Unknown))
                    {
                        numberedTile.AddNeighbour(board[xpos-1, ypos-1]);
                    }
                }
                if (ypos != board.GetLength(1) - 1)
                {
                    if (board[xpos - 1, ypos + 1].GetTileType().Equals(TileType.Unknown))
                    {
                        numberedTile.AddNeighbour(board[xpos - 1, ypos+1]);
                    }
                }
            }
            if (ypos != 0)
            {
                if (board[xpos, ypos-1].GetTileType().Equals(TileType.Unknown))
                {
                    numberedTile.AddNeighbour(board[xpos, ypos-1]);
                }
            }
            if (ypos != board.GetLength(1)-1)
            {
                if (board[xpos, ypos + 1].GetTileType().Equals(TileType.Unknown))
                {
                    numberedTile.AddNeighbour(board[xpos, ypos + 1]);
                }
            }
            if (xpos != board.GetLength(0))
            {
                if (board[xpos + 1, ypos].GetTileType().Equals(TileType.Unknown))
                {
                    numberedTile.AddNeighbour(board[xpos + 1, ypos]);
                }
                if (ypos != 0)
                {
                    if (board[xpos + 1, ypos - 1].GetTileType().Equals(TileType.Unknown))
                    {
                        numberedTile.AddNeighbour(board[xpos + 1, ypos - 1]);
                    }
                }
                if (ypos != board.GetLength(1) - 1)
                {
                    if (board[xpos + 1, ypos + 1].GetTileType().Equals(TileType.Unknown))
                    {
                        numberedTile.AddNeighbour(board[xpos + 1, ypos + 1]);
                    }
                }
            }
            return;
        }
        public static void FindNumberedTiles(Tile[,] board, List<Tile> numberedTiles)
        {
            numberedTiles.Clear();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].GetTileType().Equals(TileType.Clear) && board[i,j].GetNumber() > 0)
                    {
                        numberedTiles.Add(board[i, j]);
                        FindAndAddNeighbours(board[i,j], board);
                    }
                }
            }
            return;
        }
        public static List<List<int>> BuildSolvingMatrix(List<Tile> columns,  List<Tile> numberedTiles)
        {
            int colPos = 0;
            int columnIndex;
            List<List<int>> solvingMatrix = new List<List<int>>();
            for (int i = 0; i < numberedTiles.Count; i++)
            {
                List<Tile> Neighbourhood = numberedTiles[i].GetNeighbourhood();
                for (int j = 0; j < Neighbourhood.Count; j++ )
                {
                    if (!(columns.Contains(Neighbourhood[j])))
                    {
                        solvingMatrix.Add(new List<int>());
                        columns.Add(Neighbourhood[j]);
                        while (solvingMatrix[colPos].Count() <= i)
                        {
                            solvingMatrix[colPos].Add(0);
                        }
                        solvingMatrix[colPos][i]++;
                        colPos++;
                    }
                    else
                    {
                        columnIndex = columns.IndexOf(Neighbourhood[j]);
                        while (solvingMatrix[columnIndex].Count() <= i)
                        {
                            solvingMatrix[columnIndex].Add(0);
                        }
                        solvingMatrix[columnIndex][i]++;
                    }
                }
            }
            for (int i = 0; i < solvingMatrix.Count(); i++)
            {
                if (solvingMatrix[i].Count() < numberedTiles.Count())
                {
                    for (int j = solvingMatrix[i].Count(); j < numberedTiles.Count(); j++)
                    {
                        solvingMatrix[i].Add(0);
                    }
                }
            }
            solvingMatrix.Add(new List<int>());
            for (int i = 0; i < numberedTiles.Count(); i++)
            {
                solvingMatrix[solvingMatrix.Count() -1 ].Add(numberedTiles[i].GetNumber());
            }
                
            return solvingMatrix;
        }

        // turn solvingMatrix into a matrix????
        public static void rref(List<List<int>> solvingMatrix)
        {
            int lead = 0;
            int rowCount = solvingMatrix[0].Count();
            int columnCount = solvingMatrix.Count();
            for (int r = 0; r < rowCount; r++)
            {
                if (columnCount <= lead) break;
                int i = r;
                while (solvingMatrix[lead][i] == 0)
                {
                    i++;
                    if (i == rowCount)
                    {
                        i = r;
                        lead++;
                        if (columnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                for (int j = 0; j < columnCount; j++)
                {
                    int temp = solvingMatrix[j][r];
                    solvingMatrix[j][r] = solvingMatrix[j][i];
                    solvingMatrix[j][i] = temp;
                }
                int div = solvingMatrix[lead][r];
                if (div != 0)
                    for (int j = 0; j < columnCount; j++) solvingMatrix[j][r] /= div;
                for (int j = 0; j < rowCount; j++)
                {
                    if (j != r)
                    {
                        int sub = solvingMatrix[lead][j];
                        for (int k = 0; k < columnCount; k++) solvingMatrix[k][j] -= (sub * solvingMatrix[k][r]);
                    }
                }
                lead++;
            }
            return;
        }

        public static void FindBombs(List<List<int>> solvingMatrix, List<Tile> columns,List<Tile> numberedTiles, ScreenShot screen)
        {
            int rowSum = 0;
            bool negativeNumber = false;
            List<Tile> tilesInRow = new List<Tile>();
            for (int i = 0; i < solvingMatrix[0].Count(); i++)
            {
                for (int j = 0; j < solvingMatrix.Count() - 1; j++)
                {
                    rowSum += solvingMatrix[j][i];
                    if (solvingMatrix[j][i] < 0)
                    {
                        negativeNumber = true;
                    }
                    if (solvingMatrix[j][i] != 0)
                    {
                        tilesInRow.Add(columns[j]);
                    }
                }
                if (rowSum == solvingMatrix[solvingMatrix.Count()-1][i] && !negativeNumber)
                {
                    for (int j = 0; j < tilesInRow.Count(); j++) 
                    {
                        tilesInRow[j].SetTileType(TileType.Mine);
                        RightMouseClick(screen.GetScreenLeft() + 15 + 30 * tilesInRow[j].GetXpos(), screen.GetScreenTop() + 15 + 30 * tilesInRow[j].GetYpos());
                        numberedTiles[i].DecrNumber();
                        if (numberedTiles[i].GetNumber() == 0)
                        {

                        }
                    }
                        
                }
                if(solvingMatrix[solvingMatrix.Count()-1][i] == 0 && tilesInRow.Count() == 1)
                {
                    LeftMouseClick(screen.GetScreenLeft() + 15 + 30 * tilesInRow[0].GetXpos(), screen.GetScreenTop() + 15 + 30*tilesInRow[0].GetYpos());

                }

                tilesInRow.Clear();
                negativeNumber = false;
                rowSum = 0;
            }
        }
        static void Main(string[] args)
        {
            int tileWidth = 30;
            int tileHeight = 30;
            var handle = GetConsoleWindow();

            Console.SetWindowSize(100, 40);
            ShowWindow(handle, SW_HIDE);
            Thread.Sleep(300);
            
            ScreenShot screen = new ScreenShot();
            Bitmap playField = screen.GetPlayField();
            try
            {
                int boardWidth = playField.Size.Width / tileWidth;
                int boardHeight = playField.Size.Height / tileHeight;
                Console.WriteLine("Screen width: {0} Screen height: {1}", boardWidth, boardHeight);
                Tile[,] board = new Tile[boardWidth, boardHeight];
                List<Tile> numberedTiles = new List<Tile>();
                List<Tile> columns = new List<Tile>();

                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        board[i, j] = new Tile(TileType.Unknown, i, j);
                    }
                }

                LeftMouseClick(screen.GetScreenLeft() + 15, screen.GetScreenTop() + 15);

                while (true)
                {
                    RemoveMouseFromPlay(screen.GetScreenLeft() - 30, screen.GetScreenTop() - 30);
                    screen.UpdateField();
                    UpdateBoard(screen.GetPlayField(), board);
                    ShowWindow(handle, SW_SHOW);
                    for (int i = 0; i < board.GetLength(1); i++)
                    {
                        for (int j = 0; j < board.GetLength(0); j++)
                        {
                            Console.Write(board[j, i]);
                        }
                        Console.Write("\n");
                    }
                    FindNumberedTiles(board, numberedTiles);
                    List<List<int>> solvingMatrix = BuildSolvingMatrix(columns, numberedTiles);
                    rref(solvingMatrix);
                    FindBombs(solvingMatrix, columns,numberedTiles, screen);

                    numberedTiles.Clear();
                    solvingMatrix.Clear();
                    columns.Clear();
                    //Console.ReadLine();
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("play field not found");
            }
            Console.ReadLine();   
        }
    }
}
