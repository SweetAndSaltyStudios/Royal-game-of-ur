using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public abstract class Model : MonoBehaviour
    {
        #region VARIABLES

        private bool hasInitialized;
        protected Color32 defaultColor;
        protected SpriteRenderer spriteRenderer;

        protected Color32 disabledColor = new Color32(255, 255, 255, 100);

        #endregion VARIABLES

        #region PROPERTIES

        public bool IsAnimating
        {
            get
            {
                return LeanTween.isTweening(gameObject);
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected void Base_Initialize()    
        {
            if(hasInitialized)
            {
                Debug.LogError("FUQ");
                return;
            }

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            defaultColor = spriteRenderer.color;

            hasInitialized = true;
        }

        public void AnimateScale(Vector2 newScale, float animateDuration, bool isLooping)
        {
            if(isLooping)
            {
                LeanTween.scale(
                    gameObject,
                    newScale,
                    animateDuration).setLoopPingPong();
                return;
            }

            LeanTween.scale(
                gameObject,
                newScale,
                animateDuration);
        }

        public void AnimateColor(Color32 newColor, float animateDuration, bool isLooping)
        {
            if(isLooping)
            {
                LeanTween.color(
                  gameObject,
                  newColor,
                  animateDuration).setLoopPingPong();
                return;
            }

            LeanTween.color(
                gameObject,
                newColor,
                animateDuration);
        }

        public void CancelAnimations()
        {
            LeanTween.cancel(gameObject);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
