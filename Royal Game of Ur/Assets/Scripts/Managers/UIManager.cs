using TMPro;
using UnityEngine;
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

        private const string defaultMessage = "No Legal Moves!";

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            SetRollTheDiceButtonInteractableState(false);
            MessageText.gameObject.SetActive(false);
        }
        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void RollTheDiceButton_OnClick()
        {
            GameManager.Instance.RollDice();
        }

        public void SetRollTheDiceButtonInteractableState(bool interactable)
        {
            RollTheDiceButton.interactable = interactable;
        }

        public void UpdateDiceTotalResult(int diceTotalValue = -1)
        {
            DiceResultText.text = diceTotalValue != -1 ? $"{diceTotalValue}" : "?";       
        }

        public void UpdateCurrentPlayerText(int playerIndex)
        {
            CurrentPlayerText.text = $"Current Player {playerIndex}"; 
        }

        public void UpdatePlayerScore(Player player)
        {
            if(player.Index == 1)
            {
                Player_1_ScoreText.text = player.Score.ToString();
            }
            else
            {
                Player_2_ScoreText.text = player.Score.ToString();
            }
        }

        public void ShowMessageText(string message)
        {
            MessageText.text = message;

            MessageText.gameObject.SetActive(true);

            LeanTween.scale(MessageText.gameObject, Vector2.one, 0.25f)
            .setFrom(Vector2.zero)
            .setOnComplete(() =>
            {
                LeanTween.scale(MessageText.gameObject, Vector2.one * 1.1f, 0.25f)
                .setLoopPingPong()
                .setOnStart(() =>
                {
                    LeanTween.delayedCall(1f, () =>
                    {
                        LeanTween.cancel(MessageText.gameObject);
                        LeanTween.scale(MessageText.gameObject, Vector2.zero, 0.25f)
                        .setOnComplete(() =>
                        {
                            MessageText.gameObject.SetActive(false);
                            MessageText.text = defaultMessage;
                        });                     
                    });                  
                });             
            });
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
