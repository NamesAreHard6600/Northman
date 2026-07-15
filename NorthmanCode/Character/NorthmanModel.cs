using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using Northman.NorthmanCode.Cards;
using Northman.NorthmanCode.Piles;

namespace Northman.NorthmanCode.Character;

public class NorthmanModel(): CustomSingletonModel(HookType.Combat)
{
    // internal static readonly SpireField<Player, RageQueuePile> RageQueue = new(() => null);
    
    internal static readonly SpireField<Player, int> RageAmount = new(() => 0);
}