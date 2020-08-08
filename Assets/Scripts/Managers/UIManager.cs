using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sweet_And_Salty_Studios
{
    public class UIManager : Singelton<UIManager>
    {
        #region VARIABLES

        [Space]
        [Header("References")]
        public Image DiceResultImage;
        public Button RollTheDiceButton;
        public TextMeshProUGUI DiceResultText;
        public TextMeshProUGUI CurrentPlayerText;
        public TextMeshProUGUI Player_1_ScoreText;
        public TextMeshProUGUI Player_2_ScoreText;
        public TextMeshProUGUI MessageText;
        public RectTransform MainMenuPanel;
        public RectTransform SelectionPanel;
        public RectTransform GamePanel;
        public PlayerInfoDisplay PlayerInfoDisplayPrefab;

        private const string defaultPlayerText = "Current Player: ";
        private const string defaultMessage = "No Legal Moves!";

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            SetRollTheDiceButton(false);

            UpdateMessageText("");

            SelectionPanel.gameObject.SetActive(false);
            GamePanel.gameObject.SetActive(false);
            MainMenuPanel.gameObject.SetActive(true);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void CreatePlayerInfoDisplay(PlayerData playerData, int index)
        {
            var selectionPanelContent = SelectionPanel.GetChild(0).GetChild(1).GetChild(1);
            var playerInfoDisplay = Instantiate(PlayerInfoDisplayPrefab, selectionPanelContent);
            playerInfoDisplay.Initialize(playerData, index);

      
        }

        public void SetRollTheDiceButton(bool interactable)
        {
            RollTheDiceButton.interactable = interactable;
        }

        public void RollTheDiceButton_OnClick()
        {
            SetRollTheDiceButton(false);

            GameManager.Instance.RollTheDice();
        }

        public void UpdateCurrentPlayerText(int index)
        {
            CurrentPlayerText.text = $"{defaultPlayerText} {index}";
        }

        public void UpdateDiceRollText(int totalDiceRoll)
        {
            DiceResultText.text = totalDiceRoll > -1 ? totalDiceRoll.ToString() : "?";
        }

        public void UpdateMessageText(string newMessage)
        {
            MessageText.text = newMessage;
        }

        public void UpdatePlayerScore(int playerIndex, int score)
        {
            switch(playerIndex)
            {
                case 1:

                    Player_1_ScoreText.text = score.ToString();

                    break;

                case 2:

                    Player_2_ScoreText.text = score.ToString();

                    break;

                default:

                    break;
            }
        }

        public void ChangePlayerTypeToggle(PlayerInfoDisplay playerInfoDisplay)
        {
            var isHumanPlayer = playerInfoDisplay.PlayerData.Type == PLAYER_TYPE.HUMAN;

            playerInfoDisplay.PlayerData.ChangePlayerType(isHumanPlayer ? PLAYER_TYPE.AI : PLAYER_TYPE.HUMAN);

            //playerInfoDisplay.ChangePlayerTpeButton.isOn = isHumanPlayer;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
