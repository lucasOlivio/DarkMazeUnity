using System.Collections.Generic;

namespace DM
{
    public class WorldMap
    {
        public int size { get; private set; }
        public int width => size;
        public int area => width * width;

        public List<Cell> cells;

        public WorldMap(int size_)
        {
            size = size_;
            cells = new List<Cell>(area);
        }
    }
}
