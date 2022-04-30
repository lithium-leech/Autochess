using UnityEngine;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary>The board that fights take place on</summary>
    private Board Board { get; set; }
    
    /// <summary>The board that holds the player's available pieces</summary>
    private Board Tableu { get; set; }

    private void Start() {
        
        // Create the game boards
        Board = new Board(8,8,0,5);
        Tableu = new Board(8,3,0,2.5);

        // Create a sample setup
        Board.Add(new Pawn(), 5, 2, true);
        Board.Add(new Pawn(), 4, 7, false);
    }

    private void Update() {
        
    }
}
