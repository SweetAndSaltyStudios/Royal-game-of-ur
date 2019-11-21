using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class GameManager : Singelton<GameManager>
    {
        #region VARIABLES

        public Player[] Players;

        private Stone selectedStone;

        private List<Stone> selectableStones = new List<Stone>();

        private Camera mainCamera;

        private int[] diceValues = new int[4];
        private int diceTotalValue;
        private bool isDiceRolled;

        #endregion VARIABLES

        #region PROPERTIES

        public Player CurrentPlayer
        {
            get;
            private set;
        }

        public bool IsAnimating
        {
            get;
            set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            StartCoroutine(IOnGameStart());
        }

        private void Update()
        {
            if(CurrentPlayer == null)
            {
                return;
            }

            if(CurrentPlayer.Type == PLAYER_TYPE.HUMAN)
            {
                if(InputManager.Instance.IsOverUI || IsAnimating)
                {
                    return;
                }

                if(Input.GetMouseButtonDown(0))
                {
                    if(isDiceRolled == false)
                    {
                        return;
                    }

                    if(CurrentPlayer == null)
                    {
                        return;
                    }

                    if(diceTotalValue == 0)
                    {
                        return;
                    }

                    var hitCollider = Physics2D.OverlapPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition), CurrentPlayer.LayerMaskIndex);
                    if(hitCollider == null)
                    {
                        return;
                    }

                    selectedStone = hitCollider.GetComponent<Stone>();
                }

                if(Input.GetMouseButtonUp(0))
                {
                    if(selectedStone == null)
                    {
                        return;
                    }

                    if(selectedStone.HasValidMove(diceTotalValue) == false)
                    {
                        return;
                    }

                    ShowOrHidePossibleMoves(false);

                    selectedStone.Move(diceTotalValue);

                    EndTurn();
                }
            }
            else
            {
                //Debug.Log($"Player: {CurrentPlayer.Index} AI processing turn...");

                if(isDiceRolled == false)
                {
                    RollDice();

                    return;
                }


                if(CurrentPlayer == null)
                {
                    return;
                }

                if(diceTotalValue == 0)
                {
                    return;
                }

                var foo = MovableStones();

                if(foo.Count > 0)
                selectedStone = foo[Random.Range(0, foo.Count - 1)];

                // -------------------------

                if(selectedStone == null)
                {
                    return;
                }

                if(selectedStone.HasValidMove(diceTotalValue) == false)
                {
                    return;
                }

                ShowOrHidePossibleMoves(false);

                selectedStone.Move(diceTotalValue, 1);

                EndTurn();
            }          
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private IEnumerator IOnGameStart()
        {
            yield return new WaitForSeconds(2);

            for(int i = 0; i < Players.Length; i++)
            {
                Players[i].Initialize(i + 1);
            }

            StartPlayerTurn(Players[0]);

            UIManager.Instance.SetRollTheDiceButtonInteractableState(true);
        }

        private List<Stone> MovableStones()
        {
            var results = new List<Stone>();

            for(int i = 0; i < CurrentPlayer.Stones.Length; i++)
            {
                // !!!
                if(CurrentPlayer.Stones[i] == null || CurrentPlayer.Stones[i].gameObject.activeSelf == false)
                {
                    continue;
                }

                if(CurrentPlayer.Stones[i].HasValidMove(diceTotalValue))
                {
                    results.Add(CurrentPlayer.Stones[i]);
                }
            }

            return results;
        }

        private void StartPlayerTurn(Player player)
        {
            CurrentPlayer = player;

            UIManager.Instance.UpdateCurrentPlayerText(CurrentPlayer.Index);
            UIManager.Instance.UpdateDiceTotalResult();
            UIManager.Instance.SetRollTheDiceButtonInteractableState(true);
        }

        public void RollDice()
        {
            diceTotalValue = 0;

            for(int i = 0; i < diceValues.Length; i++)
            {
                diceValues[i] = Random.Range(0, 2);

                diceTotalValue += diceValues[i];
            }

            UIManager.Instance.UpdateDiceTotalResult(diceTotalValue);
            UIManager.Instance.SetRollTheDiceButtonInteractableState(false);

            isDiceRolled = true;

            if(diceTotalValue == 0)
            {
                StartCoroutine(IRolledZero());
                return;
            }

            ShowOrHidePossibleMoves(true);
        }

        private void ShowOrHidePossibleMoves(bool show)
        {
            selectableStones.Clear();

            selectableStones = MovableStones();

            foreach(var stone in selectableStones)
            {
                if(show)
                {
                    //Debug.Log($"Valid stone: {stone.gameObject.name}", stone.gameObject);

                    stone.SetInteractability(true);
                    LeanTween.scale(stone.gameObject, Vector2.one * 0.75f, 0.25f).setLoopPingPong();
                    LeanTween.color(stone.gameObject, Color.white, 0.8f).setLoopPingPong();
                }
                else
                {
                    LeanTween.cancel(stone.gameObject);

                    LeanTween.scale(stone.gameObject, Vector2.one * 0.7f, 0.4f).setFrom(stone.transform.localScale);
                    LeanTween.color(stone.gameObject, CurrentPlayer.StoneColor, 0.2f);
                }          
            }

            if(selectableStones.Count == 0)
            {
                Debug.LogWarning("0 moves and roll was: " + diceTotalValue);
            }
        }

        private void EndTurn()
        {
            if(CurrentPlayer.Score >= 6)
            {
                Debug.Log($"Player {CurrentPlayer.Index} Wins!");
            }

            StartCoroutine(IEndTurn());     
        }

        private IEnumerator IEndTurn()
        {
            if(selectedStone)
            {
                yield return new WaitWhile(() => LeanTween.isTweening(selectedStone.gameObject));
            }

            //Debug.Log($"{selectedStone.gameObject.name} done animating!");

            selectedStone = null;

            isDiceRolled = false;
            //isAnimating = false;

            yield return new WaitWhile(() => IsAnimating);

            if(CurrentPlayer.ShouldRollAgain)
            {
                CurrentPlayer.ShouldRollAgain = false;
                StartPlayerTurn(CurrentPlayer);
            }
            else
            {
                StartPlayerTurn(CurrentPlayer.Index == 1 ? Players[1] : Players[0]);
            }

            yield return null;
        }

        private IEnumerator IRolledZero()
        {
            UIManager.Instance.ShowMessageText("You Rolled 0.");

            yield return new WaitUntil(() => UIManager.Instance.MessageText.gameObject.activeSelf == false);

            EndTurn();
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
