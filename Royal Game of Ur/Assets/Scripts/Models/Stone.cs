using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Stone : Model
    {
        #region VARIABLES

        private Collider2D hitCollider2D;

        #endregion VARIABLES

        #region PROPERTIES

        public Player Owner
        {
            get;
            private set;
        }

        public Tile CurrentTile
        {
            get;
            private set;
        }

        public Vector2 StartPosition
        {
            get;
            private set;
        }

        public int CurrentPathIndex
        {
            get;
            private set;
        } = -1;

        public Color32 DefaultColor
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Initialize(Player owner, int contactLayerIndex, Color32 color)
        {
            Base_Initialize();

            hitCollider2D = GetComponent<Collider2D>();

            Owner = owner;

            gameObject.layer = contactLayerIndex;

            StartPosition = transform.position;

            DefaultColor = color;

            AnimateColor(color, 0.25f, false);
        }

        public void ChangeInteractability(bool isInteractable)
        {
            hitCollider2D.enabled = isInteractable;
          
            // We could animate objects that are disabled...         
        }
        
        public IEnumerator IMove(int steps, Tile[] path, float movementSpeed = 0.25f)
        {
            var targetPosition = Vector2.zero;
            var finalDestination = CurrentPathIndex + steps;
            var aniamtionID = 0;

            // Check if stone has already moved atleast once!

            if(CurrentPathIndex > -1 && CurrentPathIndex < path.Length - 1)
            {
                CurrentTile = path[CurrentPathIndex];
            }

            if(CurrentTile != null && CurrentTile.OccupiedStone)
            {
                CurrentTile.ClearOccupiedStone();
            }

            var destinationTile = path
                [
                    finalDestination > path.Length - 1
                    ? path.Length - 1
                    : finalDestination
                ];

            while(CurrentPathIndex < finalDestination)
            {
                CurrentPathIndex++;

                if(CurrentPathIndex > path.Length - 1)
                {
                    targetPosition = path[path.Length - 1].transform.position;

                    aniamtionID = LeanTween.move(gameObject, targetPosition, movementSpeed)
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(gameObject, Vector2.zero, movementSpeed)
                        .setEaseInOutElastic()
                        .setOnComplete(() =>
                        {
                            gameObject.SetActive(false);
                        });
                    })
                    .id;
                   
                    yield return new WaitWhile(() => LeanTween.isTweening(aniamtionID));            
                }
                else
                {
                    targetPosition = path[CurrentPathIndex].transform.position;
                }

                aniamtionID = LeanTween.move(gameObject, targetPosition, movementSpeed).id;

                yield return new WaitWhile(() => LeanTween.isTweening(aniamtionID));
            }

            CurrentTile = destinationTile;


            if(CurrentTile.OccupiedStone && CurrentTile.OccupiedStone.Owner.Index != Owner.Index)
            {
                var stoneToReturn = CurrentTile.OccupiedStone;

                targetPosition = stoneToReturn.StartPosition;

                aniamtionID = LeanTween.move(stoneToReturn.gameObject, targetPosition, movementSpeed).id;

                yield return new WaitWhile(() => LeanTween.isTweening(aniamtionID));

                stoneToReturn.CurrentPathIndex = -1;
                stoneToReturn.CurrentTile = null;
            }

            CurrentTile.PlaceStone(this);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
