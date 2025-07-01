using Unity.Mathematics;

namespace DM
{
    public class Enemy
    {
        private static int nextEnemyId = 1;

        public int id { get; private set; }
        public EnemyType type { get; set; }
        public int2 position { get; set; }
        public Direction direction { get; set; }

        public Enemy()
        {
            id = nextEnemyId++;
        }

        public void Tick()
        {
            
        }
    }
}
