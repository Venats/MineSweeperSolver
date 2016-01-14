using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace MineSweeperSolver
{
    class ScreenShot
    {
        int screenLeft;
        int screenRight;
        int screenTop;
        int screenBottom;
        Bitmap playField;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public void UpdateField()
        {
            Thread.Sleep(100);
            playField = new Bitmap(playField.Size.Width, playField.Size.Height);
            Graphics g = Graphics.FromImage(playField);
            g.CopyFromScreen(new Point(screenLeft, screenTop), new Point(0, 0), playField.Size);
            Console.WriteLine("saving update");
            playField.Save("PlayField.jpg", ImageFormat.Jpeg);
        }

        public static bool PixelInRange(Color c, Bitmap bmp, int x1, int y1, int x2, int y2)
        {
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (bmp.GetPixel(i,j) == c)
                    {
                        return true;

                    }
                }
            }
            return false;
        }

        private Bitmap FindPlayField(Bitmap bmp)
        {
            Bitmap playField;
            Rectangle field;
            int left;
            int right;
            int top;
            int bottom;

            //find left edge
            int row = bmp.Size.Height / 2;
            int column = 0;
            Color edgeColor = Color.FromArgb(230,230,230);
            while (column < bmp.Size.Width && !(PixelInRange(edgeColor, bmp, column, row-3, column, row+3)))
            {
                column++;
            }
            if (column == bmp.Size.Width)
            {
                Console.WriteLine("Could not find left of play field");
                return null;
            }
            Console.WriteLine("left is: {0}", column);
            left = column;
            screenLeft += left;

            //find right edge
            column = bmp.Size.Width-1;
            edgeColor = Color.FromArgb(176,176,176);
            while (column > 0 && !(PixelInRange(edgeColor, bmp, column, row - 5, column, row + 5)))
            {
                column--;
            }
            if (column == 0)
            {
                Console.WriteLine("Could not find right of play field");
                return null;
            }
            Console.WriteLine("right is: {0}", column);
            right = column+1;
            screenRight += right;

            //find top edge
            row = 0;
            column = bmp.Size.Width / 3;
            edgeColor = Color.FromArgb(239, 239, 239);
            while (row < bmp.Size.Height && !(PixelInRange(edgeColor, bmp, column-3, row, column+3, row)))
            {
                row++;
            }
            if (row == bmp.Size.Height)
            {
                Console.WriteLine("Could not find top of play field");
                return null;
            }
            Console.WriteLine("top is: {0}",row);
            top = row;
            screenTop += top;

            //find bottom
            row = bmp.Size.Height-1;
            column = bmp.Size.Width / 2;
            edgeColor = Color.FromArgb(172, 172, 172);
            while (row > 0 && !(PixelInRange(edgeColor, bmp, column-7, row, column+7, row)))
            {
                row--;
            }
            if (row == 0)
            {
                Console.WriteLine("Could not find bottom of play field");
                return null;
            }
            Console.WriteLine("bottom is: {0}", row);
            bottom = row+2;
            screenBottom += bottom;

            field = new Rectangle(left, top, right - left, bottom - top);
            playField = bmp.Clone(field, bmp.PixelFormat);
            playField.Save("PlayField.jpg", ImageFormat.Jpeg);
            return playField;
        }

        public ScreenShot()
        {
            IntPtr hWnd = IntPtr.Zero;
            RECT rct;
            foreach (Process pList in Process.GetProcesses())
            {
                String title = pList.MainWindowTitle;
                if (title == "MineSweeper")
                {
                    hWnd = pList.MainWindowHandle;
                    if (!GetWindowRect(new HandleRef(this, hWnd), out rct))
                    {
                        Console.WriteLine("ERROR");
                    }
                    Console.WriteLine("Left: {0}, Top: {1}, Right:{2}, Bottom:{3}", rct.Left, rct.Top, rct.Right, rct.Bottom);

                    screenLeft = rct.Left;
                    screenRight = rct.Right;
                    screenTop = rct.Top;
                    screenBottom = rct.Bottom;

                    Bitmap bmp = new Bitmap(rct.Right - rct.Left, rct.Bottom - rct.Top);
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(new Point(rct.Left, rct.Top), new Point(0, 0), bmp.Size);
                    playField = FindPlayField(bmp);
                }
            }
        }

        public int GetScreenLeft()
        {
            return screenLeft;
        }
        public int GetScreenRight()
        {
            return screenRight;
        }

        public int GetScreenTop()
        {
            return screenTop;
        }

        public int GetScreenBottom()
        {
            return screenBottom;
        }

        public int GetScreenWidth()
        {
            return screenRight - screenLeft;
        }

        public int GetScreenHeight()
        {
            return screenBottom - screenTop;
        }

        public Bitmap GetPlayField()
        {
            return playField;
        }
    }
}
