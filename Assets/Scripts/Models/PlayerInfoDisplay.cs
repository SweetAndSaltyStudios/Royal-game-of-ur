using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sweet_And_Salty_Studios
{
    public class PlayerInfoDisplay : MonoBehaviour
    {
        #region VARIABLES

        public Sprite Human_Player_Icon;
        public Sprite AI_Player_Icon;

        private Image icon;
        private TextMeshProUGUI playerIndexText;
        private Image buttonImage;

        #endregion VARIABLES

        #region PROPERTIES

        public PlayerData PlayerData
        {
            get;
            private set;
        }

        public Button ChangePlayerTypeButton
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Initialize(PlayerData playerData, int index)
        {
            icon = transform.Find("Icon").GetComponent<Image>();

            playerIndexText = GetComponentInChildren<TextMeshProUGUI>();
            ChangePlayerTypeButton = GetComponentInChildren<Button>();
            buttonImage = ChangePlayerTypeButton.transform.GetChild(0).GetComponent<Image>();

            PlayerData = playerData;
            playerIndexText.text = $"Player: {index}";

            ChangePlayerTypeButton.onClick.AddListener(() =>
            {
                playerData.ChangePlayerType(PlayerData.Type == PLAYER_TYPE.HUMAN ? PLAYER_TYPE.AI : PLAYER_TYPE.HUMAN);

                buttonImage.sprite = PlayerData.Type == PLAYER_TYPE.HUMAN
                        ? Human_Player_Icon
                        :  AI_Player_Icon;
            });

            buttonImage.sprite = PlayerData.Type == PLAYER_TYPE.HUMAN
                       ? Human_Player_Icon
                       : AI_Player_Icon;

            //ChangePlayerTypeButton.onClick.Invoke();        
        }

        #endregion CUSTOM_FUNCTIONS      
    }
}
