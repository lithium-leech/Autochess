using UnityEngine;

/// <summary>
/// A single game object
/// </summary>
public abstract class ChessObject : MonoBehaviour
{
    /// <summary>The board this object is on</summary>
    public Board Board { get; set; }

    /// <summary>The object's space on the board</summary>
    public Space Space { get; set; }

    /// <summary>True if this object can be picked up by the player</summary>
    public bool IsGrabable { get; set; } = false;

    /// <summary>True if the object is moving towards a target location</summary>
    protected bool IsMoving { get; set; } = false;
    
    /// <summary>The target location this object is moving towards</summary>
    protected Vector3 Target { get; set; }

    /// <summary>The incrementor used for lerp movement</summary>
    private float LerpIncrement { get; set; } = 0;

    public void Update()
    {
        // Move towards the target when moving is activated
        if (IsMoving)
        {
            LerpIncrement += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, Target, LerpIncrement);
            if (Vector3.Distance(transform.position, Target) == 0) IsMoving = false;
        }

        // Run inherited implementations of Update
        Update2();
    }

    /// <summary>A continuation of the Update method for classes inheriting ChessObject</summary>
    protected abstract void Update2();

    /// <summary>Removes this object from the entire game</summary>
    public abstract void Destroy();

    /// <summary>Warps the object to the specified location</summary>
    /// <param name="space">The space to warp to</param>
    public void WarpTo(Vector2Int space)
    {
        Vector3 position = Board.ToPosition(space);
        transform.position = position;
        Target = position;
        IsMoving = true;
        LerpIncrement = 0;
    }

    /// <summary>Warps the object to the specified location</summary>
    /// <param name="space">The world coordinates to warp to</param>
    public void WarpTo(Vector3 position)
    {
        transform.position = position;
        Target = position;
        IsMoving = true;
        LerpIncrement = 0;
    }

    /// <summary>The object moves to the specified location</summary>
    /// <param name="space">The space to move to</param>
    public void MoveTo(Vector2Int space)
    {
        Vector3 position = Board.ToPosition(space);
        Target = position;
        IsMoving = true;
        LerpIncrement = 0;
    }
}
