using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperSolver
{
    enum TileType
    {
        Unknown,
        Mine,
        Clear,
    }
    class Tile
    {
        int number;
        TileType type;
        int xpos;
        int ypos;
        List<Tile> Neighbourhood;
        int colPos;

        public Tile(TileType type, int xpos, int ypos)
        {
            this.type = type;
            this.xpos = xpos;
            this.ypos = ypos;
        }
        public Tile(int number, TileType type, int xpos, int ypos)
        {
            this.number = number;
            this.type = type;
            this.xpos = xpos;
            this.ypos = ypos;
            Neighbourhood = new List<Tile>();
        }
        public void AddNeighbour(Tile Neighbour)
        {
            Neighbourhood.Add(Neighbour);
        }
        public List<Tile> GetNeighbourhood()
        {
            return Neighbourhood;
        }
        
        public TileType GetTileType()
        {
            return type;
        }
        public void SetTileType(TileType type)
        {
            this.type = type;
        }
        public void DecrNumber()
        {
            number--;
        }
        public int GetXpos()
        {
            return xpos;
        }
        public int GetYpos()
        {
            return ypos;
        }
        public int GetNumber()
        {
            if (type.Equals(TileType.Clear))
            {
                return number;
            }
            return -1;
        }
        public override string ToString()
        {
            string writeBoard = "";
            if (type == TileType.Clear)
            {
                writeBoard = "[" + number + "]";
            }
            if (type == TileType.Unknown)
            {
                writeBoard = "[U]";
            }
            if (type == TileType.Mine)
            {
                writeBoard = "[M]";
            }
            return writeBoard;
        }
    }
}
