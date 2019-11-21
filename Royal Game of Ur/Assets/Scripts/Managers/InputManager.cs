using UnityEngine;
using UnityEngine.EventSystems;

namespace Sweet_And_Salty_Studios
{
    public class InputManager : Singelton<InputManager>
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        public bool IsOverUI
        {
            get
            {
                return EventSystem.current.IsPointerOverGameObject();
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}
