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

    public override void Update() { }

    public override void Destroy()
    {
        if (Space != null) 
            Space.RemoveObject(this);
        GameObject.Destroy(gameObject);
    }

    public override bool IsPlaceable(Space space) => space.HasAlly(IsPlayer);

    /// <summary>Equips this to a given piece</summary>
    /// <param name="piece">The piece to equip this to</param>
    public void Equip(Piece piece)
    {
        // Attach this to it's piece
        piece.Equipment = this;
        Piece = piece;

        // Get rid of the gameObject without destroying it
        WarpTo(GameState.ShadowZone);

        // Draw the equipment's sprite over the piece's sprite
        SpriteRenderer pieceRender = Piece.GetComponent<SpriteRenderer>();
        IEnumerable<Sprite> sprites = new List<Sprite>() { pieceRender.sprite, EquippedSprite};
        pieceRender.sprite = CreateMergedSprite(sprites);;

        // Apply the equipment's powers
        EquipPowers();
    }

    /// <summary>Activates the powers of this equipment for the attached piece</summary>
    protected abstract void EquipPowers();

    /// <summary>Creates a new sprite by layering the images of a set of sprites over one another</summary>
    /// <param name="sprites">The set of sprites to merge</param>
    /// <returns>A new Sprite</returns>
    private static Sprite CreateMergedSprite(IEnumerable<Sprite> sprites)
    {
        // Get the base sprite
        Sprite baseSprite = sprites.First();

        // Create a new texture
        Texture2D mergedTexture = new Texture2D(baseSprite.texture.width, baseSprite.texture.height);
        for (int x = 0; x < mergedTexture.width; x++)
        for (int y = 0; y < mergedTexture.height; y++)
            mergedTexture.SetPixel(x, y, new Color(1, 1, 1, 0));

        // Get readable textures from the sprites
        IList<Texture2D> textures = new List<Texture2D>();
        foreach (Sprite sprite in sprites) textures.Add(CreateReadableTexture(sprite));

        // Draw each texture onto the merged texture
        foreach (Texture2D texture in textures)
        for (int x = 0; x < texture.width; x++)
        for (int y = 0; y < texture.height; y++)
        {
            Color color = texture.GetPixel(x, y);
            if (color.a == 0) color = mergedTexture.GetPixel(x, y);
            mergedTexture.SetPixel(x, y, color);
        }
        mergedTexture.Apply();

        // Create a sprite from the merged texture
        Rect rect = new Rect(0, 0, mergedTexture.width, mergedTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite mergedSprite = Sprite.Create(mergedTexture, rect, pivot, baseSprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);
        mergedSprite.texture.filterMode = baseSprite.texture.filterMode;
        return mergedSprite;
    }

    /// <summary>Duplicates a sprites texture, but with read access</summary>
    /// <param name="sprite">The sprite to get a texture from</param>
    /// <returns>A new Texture2D</returns>
    private static Texture2D CreateReadableTexture(Sprite sprite)
    {
        RenderTexture rt = new RenderTexture(sprite.texture.width, sprite.texture.height, 0);
        Graphics.Blit(sprite.texture, rt);
        Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height, sprite.texture.format, false);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        rt.Release();
        Destroy(rt);
        return texture;
    }
}
