using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Interactive RuleTile")]
public class InteractiveRuleTile : RuleTile
{
    public RuleTile[] compatibleTiles; // Add compatible tiles here in the Inspector

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        if (neighbor == TilingRuleOutput.Neighbor.This)
        {
            // Check if the tile is this tile or a compatible tile
            return tile == this || System.Array.Exists(compatibleTiles, t => t == tile);
        }
        else if (neighbor == TilingRuleOutput.Neighbor.NotThis)
        {
            // Check if the tile is neither this nor a compatible tile
            return tile != this && !System.Array.Exists(compatibleTiles, t => t == tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}