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

        public bool TouchDown
        {
            get
            {
                return Input.GetMouseButtonDown(0);
            }
        }

        public bool TouchUp
        {
            get
            {
                return Input.GetMouseButtonUp(0);
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public Collider2D GetColliderFromTouchPosition(int layerValue)
        {
            var hitResult = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), layerValue);

            return hitResult;
        }

#endregion CUSTOM_FUNCTIONS
    }
}
