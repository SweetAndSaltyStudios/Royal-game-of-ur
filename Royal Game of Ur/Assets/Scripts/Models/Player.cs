using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class Player
    {
        #region VARIABLES

        public PLAYER_TYPE Type;
        public Tile[] Path;
        public Stone[] Stones;

        public Color StoneColor;

        private bool initialized;

        #endregion VARIABLES

        #region PROPERTIES

        public int Index
        {
            get;
            private set;
        }
        public int LayerMaskIndex
        {
            get;
            private set;          
        }
        public int Score
        {
            get;
            private set;
        }
        public bool ShouldRollAgain
        {
            get;
            set;
        } = false;

        #endregion PROPERTIES

        #region CUSTOM_FUNCTIONS

        public void Initialize(int index)
        {
            if(initialized)
            {
                return;
            }

            LeanTween.alpha(Path[Path.Length - 1].gameObject, 0, 0.25f);

            Index = index;
            LayerMaskIndex = LayerMask.GetMask($"Player {Index}");

            SetPlayerStones(Index == 1 ? 8 : 9);

            initialized = true;
        }

        private void SetPlayerStones(int stoneLayerIndex)
        {
            foreach(var stone in Stones)
            {
                stone.Initialize(this);
                stone.gameObject.layer = stoneLayerIndex;
            }
        }

        public void AddScore()
        {
            Score++;

            UIManager.Instance.UpdatePlayerScore(this);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
