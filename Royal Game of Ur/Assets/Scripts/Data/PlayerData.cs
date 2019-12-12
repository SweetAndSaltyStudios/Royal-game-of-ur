using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class PlayerData
    {
        #region VARIABLES

        [HideInInspector]
        public string Name = "New Player";

        public PLAYER_TYPE Type;
        public LayerMask ContactLayer;
        public Tile[] Path;
        public Stone[] Stones;
        public Color32 StoneColor;

        #endregion VARIABLES

        #region PROPERTIES

        public Tile GetLastTile
        {
            get
            {
                return Path[Path.Length - 1];
            }
        }

        public void ChangePlayerType(PLAYER_TYPE newPlayerType)
        {
            Type = newPlayerType;
        }

        #endregion PROPERTIES
    }
}
