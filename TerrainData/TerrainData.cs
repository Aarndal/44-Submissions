namespace TerrainData
{
    public class TerrainData
    {
        // ---------------------------------------
        // Terrain Variables
        // ---------------------------------------
        public float MapSize;
        public float MaxTerrainHeight;
        
        // Relative Transition Heights
        public float BaseHeight;
        public float TerrainLayerHeight01;
        public float TerrainLayerHeight02;
        public float TerrainLayerHeight03;

        public float TransitionSharpness; // Range between 0 and 1 | in percent

        // ---------------------------------------
        // Tree Variables
        // ---------------------------------------
        //public string[] TerrainLayer = { "Base Layer", "Layer 01", "Layer 02", "Layer 03" };

        public int Density;
        public float VerticalOffset; // positive value will set the prefab under the terrain

        public float MaxRotationTowardsTerrainNormal; // Range between 0 and 1 | in percent

        // Random Rotation Range around local Y-Axis
        public float MinRotation; // Range between 0 and 360 | in degrees
        public float MaxRotation; // Range between 0 and 360 | in degrees

        // Random Scaling Ranges
        public float MinXScale;
        public float MinYScale;
        public float MinZScale;

        public float MaxXScale;
        public float MaxYScale;
        public float MaxZScale;
    }
}
