using Unity.Mathematics;

namespace DM
{
    public class Cell
    {
        public int id;
        public bool isWalkable;
        public int2 position;

        public GroundType groundType;
        public StructureType structureType;
    }
}
