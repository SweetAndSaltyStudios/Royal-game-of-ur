using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public abstract class Player
    {     
        #region VARIABLES

        protected Tile[] path;
        protected Stone[] stones;
        protected Stone[] selectableStones;
        protected Stone selectedStone;

        protected LayerMask contactLayer;

        protected readonly WaitForSeconds waitAfterRolledZero;
        protected readonly WaitForSeconds waitAfterNoMovableStone;

        private int score;

        public int Score
        {
            get
            {
                return score;
            }
            private set
            {
                score += value;
                UIManager.Instance.UpdatePlayerScore(Index, score);
            }
        }

        #endregion VARIABLES

        #region PROPERTIES

        public int Index
        {
            get;
            protected set;
        }

        public bool ShouldRollAgain
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        public Player(PlayerData playerData, int index)
        {
            Index = index;
            path = playerData.Path;
            stones = playerData.Stones;

            contactLayer = playerData.ContactLayer;

            for(int i = 0; i < stones.Length; i++)
            {
                stones[i].Initialize(
                    this,
                    Index == 1 ? 8 : 9,
                    playerData.StoneColor
                    );
            }

            waitAfterRolledZero = new WaitForSeconds(GameManager.Instance.WaitAfterRolledZero_Delay);
            waitAfterRolledZero = new WaitForSeconds(GameManager.Instance.WaitAfterNoMovableStone_Delay);
        }

        #endregion CONSTRUCTORS

        #region CUSTOM_FUNCTIONS

        protected abstract IEnumerator IGetSelectedStone();

        public abstract IEnumerator IHandleDiceRoll();

        public virtual IEnumerator IHandleTurn(int totalDiceRoll, float endTurnDelay)
        {
            selectedStone = null;
            ShouldRollAgain = false;

            var WaitTurnEndDelay = new WaitForSeconds(endTurnDelay);
            var targetFlashColor = new Color32(225, 225, 225, 225);

            if(totalDiceRoll == 0)
            {
                UIManager.Instance.UpdateMessageText("Rolled 0... No moves for you!");
                yield return waitAfterRolledZero;
                yield break;
            }

            selectableStones = GetValidMovableStones(totalDiceRoll);

            if(selectableStones.Length == 0)
            {
                UIManager.Instance.UpdateMessageText("We did not have a single stone that we could move!");
                yield return waitAfterNoMovableStone;
                yield break;
            }

            HighlightMovableStones(selectableStones, targetFlashColor, true);

            yield return IGetSelectedStone();

            HighlightMovableStones(selectableStones, targetFlashColor, false);

            yield return selectedStone.IMove(totalDiceRoll, path);

            CheckMoveResult();

            yield return WaitTurnEndDelay;
        }

        private void CheckMoveResult()
        {
            switch(selectedStone.CurrentTile.TileType)
            {
                case TILE_TYPE.ROLL_AGAIN:

                    ShouldRollAgain = true;

                    break;
                case TILE_TYPE.GOAL:

                    Score++;

                    if(Score >= stones.Length)
                    {
                        UIManager.Instance.UpdateMessageText("$Player {Index} Wins!");
                    }

                    break;

                default:

                    break;
            }
        }

        private void HighlightMovableStones(Stone[] stones,Color32 targetFlashColor, bool isHighlighting)
        {
            for(int i = 0; i < stones.Length; i++)
            {
                if(isHighlighting == false && stones[i].IsAnimating)
                {
                    stones[i].CancelAnimations();

                    stones[i].AnimateScale(Vector2.one, 0.8f, false);

                    stones[i].AnimateColor(stones[i].DefaultColor, 0.8f, false);            

                    continue;
                }

                stones[i].AnimateScale(Vector2.one * 1.1f, 0.8f, true);
                stones[i].AnimateColor(targetFlashColor, 0.25f, true);         
            }    
        }

        private bool IsValidMovableStone(int steps, Stone stone)
        {
            // Are we already scored with this stone?...
            if(stone.gameObject.activeSelf == false)
            {

                return false;
            }

            var targetPosition = Vector2.zero;

            var finalDestinationIndex = stone.CurrentPathIndex + steps;

            if(finalDestinationIndex > path.Length - 1)
            {
                return true;
            }

            if(path[finalDestinationIndex].OccupiedStone != null
            && 
            path[finalDestinationIndex].TileType == TILE_TYPE.ROLL_AGAIN)
            {
                return false;
            }
            
            if(path[finalDestinationIndex].OccupiedStone != null
            && 
            path[finalDestinationIndex].OccupiedStone.Owner.Index == Index)
            {
                return false;
            }
            
            return true;
        }

        private Stone[] GetValidMovableStones(int steps)
        {
            var result = new List<Stone>();

            for(int i = 0; i < stones.Length; i++)
            {
                if(IsValidMovableStone(steps, stones[i]))
                {
                    result.Add(stones[i]);

                    stones[i].ChangeInteractability(true);
                }
                else
                {
                    stones[i].ChangeInteractability(false);
                }
            }

            return result.ToArray();
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
