using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Tile : MonoBehaviour
    {
        #region VARIABLES

        public TILE_TYPE TileType;

        public Color RollAgainColor;

        private Color defaultColor;

        private SpriteRenderer spriteRenderer;

        #endregion VARIABLES

        #region PROPERTIES

        public Stone OccupiedStone
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if(TileType != TILE_TYPE.DEFAULT)
            {
                spriteRenderer.color = RollAgainColor;
            }

            defaultColor = spriteRenderer.color;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void PlaceStone(Stone stone)
        {
            OccupiedStone = stone;

            spriteRenderer.color = Color.magenta;
        }

        public void ClearOccupiedStone()
        {         
            OccupiedStone = null;

            spriteRenderer.color = defaultColor;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
