using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class FPSDisplay : MonoBehaviour
    {
        #region VARIABLES

        private float deltaTime = 0.0f;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            var width = Screen.width;
            var height = Screen.height;

            var style = new GUIStyle();

            var rect = new Rect(0, 0, width, height * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = height * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            var msec = deltaTime * 1000.0f;
            var fps = 1.0f / deltaTime;
            var text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}