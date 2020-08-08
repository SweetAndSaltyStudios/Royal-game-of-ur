using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class GameManager : Singelton<GameManager>
    {
        #region VARIABLES

        [Space]
        [Header("DEBUG")]

        [Range(0, 10)] public float TurnEndDelay = 2;
        [Range(0, 10)] public float WaitAfterRolledZero_Delay = 2;
        [Range(0, 10)] public float WaitAfterNoMovableStone_Delay = 2;
        [Range(0, 10)] public float AI_WaitForDiceRoll_Delay = 2;
        [Range(0, 10)] public float AI_GetSelectedStone_Delay = 2;

        [Space]
        [Header("PLAYER DATA")]
        public PlayerData[] PlayerData = new PlayerData[NUMBER_OF_PLAYERS];
        private Player[] players;
        private const int NUMBER_OF_PLAYERS = 2;

        [Space]
        [Header("Tiles")]
        public Color32 DefaultColor;
        public Color32 RollAgainColor;
        public Color32 GoalColor;

        private Player currentPlayer;
        private Coroutine iProcessTurn_Coroutine;

        public GAME_STATE CurrentGameState;

        #endregion VARIABLES

        #region PROPERTIES   

        public bool IsDicedRolled
        {
            get;
            private set;
        }

        public int TotalDiceRoll
        {
            get;
            private set;
        }

        public Player CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;

                UIManager.Instance.UpdateCurrentPlayerText(currentPlayer.Index);
            }
        }

        public int NumberOfGameStates
        {
            get
            {
                return System.Enum.GetValues(typeof(GAME_STATE)).Length - 1;
            }
        }


        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            InitializeTiles();
            InitializePlayers();
        }

        private void Start()
        {
            StartGame();
        }

        private void OnValidate()
        {
            if(PlayerData == null || PlayerData.Length == 0)
            {
                return;
            }

            for(int i = 0; i < PlayerData.Length; i++)
            {
                PlayerData[i].Name = $"Player {i + 1}";
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void InitializeTiles()
        {
            var allTiles = FindObjectsOfType<Tile>();

            for(int i = 0; i < allTiles.Length; i++)
            {
                allTiles[i].Initialize(GetTileColor(allTiles[i].TileType));
            }
        }

        private void InitializePlayers()
        {
            players = new Player[PlayerData.Length];

            Debug.LogWarning("We should create ai and human classes after user is decided player types...");

            for(int i = 0; i < PlayerData.Length; i++)
            {
                UIManager.Instance.CreatePlayerInfoDisplay(PlayerData[i], i + 1);

                switch(PlayerData[i].Type)
                {
                    case PLAYER_TYPE.HUMAN:

                        players[i] = new Player_Human(PlayerData[i], i + 1);

                        break;

                    case PLAYER_TYPE.AI:

                        players[i] = new Player_AI(PlayerData[i], i + 1);

                        break;

                    default:

                        players[i] = new Player_Human(PlayerData[i], i + 1);

                        break;
                }
            }
        }

        private Color32 GetTileColor(TILE_TYPE tileType)
        {
            switch(tileType)
            {
                case TILE_TYPE.DEFAULT:
                    return DefaultColor;
                case TILE_TYPE.ROLL_AGAIN:
                    return RollAgainColor;
                case TILE_TYPE.GOAL:
                    return GoalColor;
                default:
                    return DefaultColor;
            }
        }

        private void StartGame()
        {
            StartCoroutine(IStartGame());
        }

        private IEnumerator IStartGame()
        {
            yield return new WaitUntil(() => CurrentGameState == GAME_STATE.GAME);

            CurrentPlayer = players[0];

            StartTurn(CurrentPlayer);
        }

        private void StartTurn(Player player)
        {
            TotalDiceRoll = -1;
            IsDicedRolled = false;
            UIManager.Instance.UpdateDiceRollText(-1);
            UIManager.Instance.UpdateMessageText("");

            if(player == null)
            {
                Debug.LogWarning($"New Player Turn -- Already running and the player is: Player {player.Index}.");
                return;
            }

            if(iProcessTurn_Coroutine != null)
            {
                Debug.LogWarning($" Can not start processing Player {player.Index}'s turn, brcause there is already turn in process!");
                return;
            }

            iProcessTurn_Coroutine = StartCoroutine(IProcessTurn(player));
        }

        public void RollTheDice()
        {
            TotalDiceRoll = 0;

            for(int i = 0; i < 4; i++)
            {
                TotalDiceRoll += Random.Range(0, 2);
            }

            IsDicedRolled = true;
        }

        private IEnumerator IProcessTurn(Player player)
        {
            yield return player.IHandleDiceRoll();

            yield return new WaitUntil(() => IsDicedRolled);

            UIManager.Instance.UpdateDiceRollText(TotalDiceRoll);

            yield return StartCoroutine(player.IHandleTurn(TotalDiceRoll, TurnEndDelay));

            iProcessTurn_Coroutine = null;

            if(CurrentPlayer.ShouldRollAgain == false)
            {
                CurrentPlayer = players[CurrentPlayer.Index == 1 ? 1 : 0];
            }        

            StartTurn(CurrentPlayer);
        }

        public void ChangeGameState(int stateIndex)
        {
            if(stateIndex < NumberOfGameStates)
            {
                stateIndex = 0;
            }
            else if(stateIndex > NumberOfGameStates)
            {
                stateIndex = NumberOfGameStates;
            }

            ChangeGameState((GAME_STATE)stateIndex);
        }

        public void ChangeGameState(GAME_STATE newGameState)
        {
            CurrentGameState = newGameState;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

#endregion CUSTOM_FUNCTIONS
    }
}
