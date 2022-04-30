using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary></summary>
    private Board Board { get; set; }
    
    /// <summary></summary>
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
