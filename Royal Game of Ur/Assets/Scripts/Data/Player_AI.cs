using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Player_AI : Player
    {
        #region VARIABLES       

        private readonly WaitForSeconds ai_WaitForDiceRoll;
        private readonly WaitForSeconds ai_GetSelectedStone;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region CONSTRUCTOR

        public Player_AI(PlayerData playerData, int index) : base(playerData, index)
        {
            Index = index;
            path = playerData.Path;
            stones = playerData.Stones;

            ai_WaitForDiceRoll = new WaitForSeconds(GameManager.Instance.AI_WaitForDiceRoll_Delay);
            ai_GetSelectedStone = new WaitForSeconds(GameManager.Instance.AI_GetSelectedStone_Delay);
        }

        #endregion CONSTRUCTOR

        #region CUSTOM_FUNCTIONS

        public override IEnumerator IHandleDiceRoll()
        {
            GameManager.Instance.RollTheDice();

            yield return ai_WaitForDiceRoll;
        }

        protected override IEnumerator IGetSelectedStone()
        {
            selectedStone = selectableStones[Random.Range(0, selectableStones.Length - 1)];

            yield return ai_GetSelectedStone;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
