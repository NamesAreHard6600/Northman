using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Northman.NorthmanCode.Piles;

public class RageQueuePile() : CustomPile(RageQueue)
{
    [CustomEnum] public static PileType RageQueue;
    
    public override bool CardShouldBeVisible(CardModel card)
    {
        return true;
    }

    public override Vector2 GetTargetPosition(CardModel card, Vector2 size)
    {
        return Vector2.Zero;
    }
}