namespace Gameplay.World
{
    public static class Progression
    {
        public static int Floor { get; set; }
        public static float Health { get; set; }
        
        public static int Kills { get; set; }
        public static float RunTime { get; set; }

        public static float FloorDifficultiModifier => 1 + (Floor - 1) * 0.3f;

        public static void Reset()
        {
            Floor = 0;
            Health = 0;
            Kills = 0;
            RunTime = 0;
        }
    }
}