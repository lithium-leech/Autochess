using UnityEngine;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary>Prefab for a white king</summary>
    public GameObject WhiteKing;

    /// <summary>Prefab for a white queen</summary>
    public GameObject WhiteQueen;

    /// <summary>Prefab for a white bishop</summary>
    public GameObject WhiteBishop;

    /// <summary>Prefab for a white knight</summary>
    public GameObject WhiteKnight;

    /// <summary>Prefab for a white rook</summary>
    public GameObject WhiteRook;

    /// <summary>Prefab for a white pawn</summary>
    public GameObject WhitePawn;

    /// <summary>Prefab for a black king</summary>
    public GameObject BlackKing;

    /// <summary>Prefab for a black queen</summary>
    public GameObject BlackQueen;

    /// <summary>Prefab for a black bishop</summary>
    public GameObject BlackBishop;

    /// <summary>Prefab for a black knight</summary>
    public GameObject BlackKnight;

    /// <summary>Prefab for a black rook</summary>
    public GameObject BlackRook;

    /// <summary>Prefab for a black pawn</summary>
    public GameObject BlackPawn;

    private void Start()
    {
        // Set game states
        GameState.FightStarted = false;
        GameState.InFight = false;
        GameState.FightOver = false;
        GameState.Victory = false;

        // Create the game boards
        GameState.GameBoard = new Board(8, 8, -4.0f, 1.0f);
        GameState.SideBoard = new Board(8, 3, -4.0f, -4.0f);

        // Create a sample setup
        GameObject object1 = Instantiate(WhitePawn);
        Pawn piece1 = object1.GetComponent<Pawn>();
        piece1.IsPlayerPiece = false;
        piece1.Board = GameState.GameBoard;
        GameState.GameBoard.Add(piece1, new Vector2Int(3, 6));
        
        GameObject object2 = Instantiate(BlackPawn);
        Pawn piece2 = object2.GetComponent<Pawn>();
        piece2.IsPlayerPiece = true;
        piece2.Board = GameState.GameBoard;
        GameState.GameBoard.Add(piece2, new Vector2Int(4, 1));

        GameObject object3 = Instantiate(WhiteQueen);
        Queen piece3 = object3.GetComponent<Queen>();
        piece3.IsPlayerPiece = false;
        piece3.Board = GameState.GameBoard;
        GameState.GameBoard.Add(piece3, new Vector2Int(7, 6));

        GameObject object4 = Instantiate(BlackQueen);
        Queen piece4 = object4.GetComponent<Queen>();
        piece4.IsPlayerPiece = true;
        piece4.Board = GameState.GameBoard;
        GameState.GameBoard.Add(piece4, new Vector2Int(0, 1));
    }

    private float timeWaited = 0;
    private bool whiteTurn = true;

    private void Update()
    {
        // Start a fight once it's been switched on
        if (GameState.FightStarted && !GameState.InFight)
        {
            GameState.InFight = true;
            GameState.FightStarted = false;
            timeWaited = 0;
            whiteTurn = true;
        }

        // Run fight operations
        if (GameState.InFight)
        {
            timeWaited += Time.deltaTime;
            if (timeWaited > GameState.TurnPause)
            {
                // Move the current player's pieces
                if (whiteTurn)
                {
                    foreach (Piece piece in GameState.GameBoard.EnemyPieces) piece.TakeTurn();
                }
                else
                {
                    foreach (Piece piece in GameState.GameBoard.PlayerPieces) piece.TakeTurn();
                }

                // Check if the battle is over
                if (GameState.GameBoard.PlayerPieces.Count < 1)
                {
                    GameState.FightOver = true;
                    GameState.InFight = false;
                }
                else if (GameState.GameBoard.EnemyPieces.Count < 1)
                {
                    GameState.Victory = true;
                    GameState.FightOver = true;
                    GameState.InFight = false;
                }

                // Go to the next turn
                whiteTurn = !whiteTurn;
                timeWaited = 0;
            }
        }

        // Perform end of fight actions
        if (GameState.FightOver)
        {
            GameState.FightOver = false;
            if (GameState.Victory)
            {
                
            }
            else
            {
                
            }
        }
    }
}
