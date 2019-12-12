using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Tile : Model
    {
        #region VARIABLES

        public TILE_TYPE TileType;

        #endregion VARIABLES

        #region PROPERTIES

        public Stone OccupiedStone
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Initialize(Color32 color)
        {
            Base_Initialize();

            defaultColor = color;

            AnimateColor(color, 0.25f, false);
        }

        public void PlaceStone(Stone stone)
        {
            if(OccupiedStone)
            {
                Debug.LogError("!!!", gameObject);
                return;
            }

            OccupiedStone = stone;

            AnimateColor(Color.grey, 0.15f, false);
        }

        public void ClearOccupiedStone()
        {         
            OccupiedStone = null;

            AnimateColor(defaultColor, 0.15f, false);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
