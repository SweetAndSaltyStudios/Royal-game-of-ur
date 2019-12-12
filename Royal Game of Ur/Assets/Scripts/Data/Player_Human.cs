using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Player_Human : Player
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region CONSTUCTORS

        public Player_Human(PlayerData playerData, int index) : base(playerData, index)
        {
            Index = index;
            path = playerData.Path;
            stones = playerData.Stones;
        }

        public override IEnumerator IHandleDiceRoll()
        {
            UIManager.Instance.SetRollTheDiceButton(true);

            yield return null;
        }

        protected override IEnumerator IGetSelectedStone()
        {
            while(selectedStone == null)
            {
                yield return new WaitUntil(() => InputManager.Instance.TouchDown);

                var result = InputManager.Instance.GetColliderFromTouchPosition(contactLayer.value);

                if(result == null)
                {
                    continue;
                }

                selectedStone = result.GetComponent<Stone>();

                if(selectedStone == null)
                {
                    continue;
                }
            }
        }

        #endregion PROPERTIES

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}
