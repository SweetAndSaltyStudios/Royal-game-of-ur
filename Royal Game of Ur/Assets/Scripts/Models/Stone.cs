using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Stone : MonoBehaviour
    {
        #region VARIABLES

        public Tile[] Path_1;
        public Tile[] Path_2;

        private int currentPathIndex = -1;

        private SpriteRenderer spriteRenderer;

        private Collider2D hitCollider2D;

        private Tile currentTile;

        private Vector2 startingPosition;

        #endregion VARIABLES

        #region PROPERTIES

        public Player Owner
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            hitCollider2D = GetComponent<Collider2D>();

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();          
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Initialize(Player owner)
        {
            Owner = owner;
            startingPosition = transform.position;
            LeanTween.color(gameObject, owner.StoneColor, 0.25f);
        }

        public bool HasValidMove(int totalDiceRoll)
        {
            if(currentPathIndex + totalDiceRoll < Owner.Path.Length)
            {
                var destinationTile = Owner.Path[currentPathIndex + totalDiceRoll];

                if(destinationTile.OccupiedStone && destinationTile.TileType != TILE_TYPE.DEFAULT)
                {
                    //Debug.Log("FOO !", gameObject);
                    return false;
                }

                if(destinationTile.OccupiedStone && destinationTile.OccupiedStone.Owner.Index == Owner.Index)
                {
                    return false;
                }              
            }

            return true;
        }

        public void Move(int diceTotalValue, float delay = 0f)
        {
            StartCoroutine(IMove(diceTotalValue, delay));
        }

        public IEnumerator IMove(int diceTotalValue, float delay)
        {
            GameManager.Instance.IsAnimating = true;

            yield return new WaitForSeconds(delay);

            //hitCollider2D.enabled = false;
            SetInteractability(false);

            var destinationPathIndex = currentPathIndex + diceTotalValue;

            if(currentTile)
            {
                currentTile.ClearOccupiedStone();
            }

            for(; currentPathIndex < destinationPathIndex;)
            {
                currentPathIndex++;

                if(currentPathIndex >= Owner.Path.Length - 1)
                {
                    LeanTween.move(gameObject, Owner.Path[Owner.Path.Length - 1].transform.position, 0.25f)
                        .setOnComplete(() => 
                        {
                            LeanTween.color(gameObject, Color.gray, 0.25f);
                            LeanTween.scale(gameObject, Vector2.zero, 0.25f)
                            .setOnComplete(() =>
                            {
                                Owner.AddScore();
                                gameObject.SetActive(false);
                            });
                        });

                    yield break;
                }
            
                LeanTween.move(gameObject, Owner.Path[currentPathIndex].transform.position, 0.25f).setEaseInOutExpo();
                yield return new WaitWhile(() => LeanTween.isTweening(gameObject));

            }

            currentTile = Owner.Path[destinationPathIndex];

            if(currentTile.TileType == TILE_TYPE.ROLL_AGAIN)
            {
                // Refactor??
                //Debug.LogError("We end up 'Roll Again Tile'.");
                GameManager.Instance.CurrentPlayer.ShouldRollAgain = true;
            }

            if(currentTile.OccupiedStone)
            {
                if(currentTile.OccupiedStone.Owner.Index != Owner.Index)
                {
                    //currentTile.OccupiedStone.ReturnBackToStart();
                    LeanTween.move(gameObject, startingPosition, 0.25f).setEaseInOutExpo();
                    yield return new WaitWhile(() => LeanTween.isTweening(gameObject));
                    currentPathIndex = -1;

                    yield break;
                }
            }

            currentTile.PlaceStone(this);
            GameManager.Instance.IsAnimating = false;

            //hitCollider2D.enabled = true;
        }

        public void SetInteractability(bool interactable)
        {
            hitCollider2D.enabled = interactable;
        }

        //private void ReturnBackToStart()
        //{
        //    currentPathIndex = -1;

        //    transform.position = startingPosition;
        //}

        #endregion CUSTOM_FUNCTIONS
    }
}
