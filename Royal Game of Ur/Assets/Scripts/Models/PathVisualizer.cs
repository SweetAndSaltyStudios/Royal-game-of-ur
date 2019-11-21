using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class PathVisualizer : MonoBehaviour
    {
        #region VARIABLES

        private LineRenderer lineRenderer;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void SetPathPositions(Vector3[] positions, Color pathColor)
        {
            lineRenderer.startColor = pathColor;
            lineRenderer.endColor = pathColor;

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
