using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using Northman.NorthmanCode.Cards;
using Northman.NorthmanCode.Piles;

namespace Northman.NorthmanCode.Character;

public class NorthmanModel(): CustomSingletonModel(HookType.Combat)
{
    // internal static readonly SpireField<Player, RageQueuePile> RageQueue = new(() => null);
    
    internal static readonly SpireField<Player, int> RageAmount = new(() => 0);
    
    // Rage queue info for public access
    internal static readonly SpireField<Player, bool> raging = new(() => false);
    internal static readonly SpireField<Player, int> currentIndex = new(() => 0);
    internal static readonly SpireField<Player, List<CardModel>> snapshot = new(() =>
        []);
    internal static readonly SpireField<Player, int> invoke = new(() => 0);

}