using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An object which is attached to a piece and grants that piece powers
/// </summary>
public abstract class Equipment : ChessObject
{
    /// Properties to set using Unity interface
    public Sprite EquippedSprite;

    /// <summary>The piece this is equipped to</summary>
    public Piece Piece { get; set; }

    public override void Update()
    {
        // Follow the equipped piece
        if (Piece != null) WarpTo(Piece.transform.position + new Vector3(0.0f, 0.0f, -0.1f));
    }

    public override void Destroy()
    {
        if (Piece != null)
            Piece.Equipment = null;
        base.Destroy();
    }

    /// <summary>Checks if this equipment is protecting it's wearer from an attacking piece</summary>
    /// <param name="piece">The attacking piece</param>
    /// <returns>True if this equipment is protecting it's wearer</returns>
    public virtual bool IsProtected(Piece piece) => false;

    public override bool IsPlaceable(Space space) => space.HasAlly(IsPlayer);

    /// <summary>Equips this to a given piece</summary>
    /// <param name="piece">The piece to equip this to</param>
    public void Equip(Piece piece)
    {
        // Attach this to it's piece
        piece.Equipment = this;
        Piece = piece;

        // Replace current sprite with equipped sprite
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = EquippedSprite;
    }
}
