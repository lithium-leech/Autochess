using UnityEngine;

public class PieceBehavior : MonoBehaviour
{
    /// <summary>True if this piece has been captured</summary>
    private bool IsCaptured { get; set; } = false;

    /// <summary>True if the piece is moving towards a target location</summary>
    private bool IsMoving { get; set; } = false;
    
    /// <summary>The target location this piece is moving towards</summary>
    private Vector3 Target { get; set; }

    /// <summary>The z-coordinate that pieces exist at</summary>
    private const float Z = -1.0f;

    void Update()
    {
        // Move towards the target when moving is activated
        if (IsMoving)
        {
            float step = 2.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Target, step);
            if (Vector3.Distance(transform.position, Target) == 0) IsMoving = false;
        }

        // Destroy the object when captured
        if (IsCaptured) GameObject.Destroy(gameObject);
    }

    /// <summary>Warps the piece to the specified location</summary>
    /// <param name="position">The world coordinates to warp to</param>
    public void WarpTo(Vector2 position)
    {

        transform.position = AddZ(position);
        Target = AddZ(position);
        IsMoving = true;
    }

    /// <summary>Moves the piece to the specified location</summary>
    /// <param name="position">The world coordinates to move to</param>
    public void MoveTo(Vector2 position)
    {
        Target = AddZ(position);
        IsMoving = true;
    }

    /// <summary>Marks this piece as captured</summary>
    public void Captured() => IsCaptured = true;

    /// <summary>Adds a z-coordinate to the given position</summary>
    /// <param name="position">The position to add a z-coordinate to</param>
    /// <returns>A Vector3</returns>
    private Vector3 AddZ(Vector2 position) => new(position.x, position.y, Z);
}
