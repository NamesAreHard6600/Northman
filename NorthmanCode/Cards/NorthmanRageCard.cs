using BaseLib.Utils;
using Northman.NorthmanCode.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Northman.NorthmanCode.Cards;

/// <summary>
/// This is the base class for your mod's cards, which is set up to load the card's images from your mod's resources.
/// When creating a card, right-click the Cards folder and create a new file with the Custom Card template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
/// </summary>
[Pool(typeof(TokenCardPool))]
public abstract class NorthmanRageCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    NorthmanCard(cost, type, rarity, target)
{
    protected Task Invoke(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        Player player = cardPlay.Card.Owner;
        if (!NorthmanCmd.GetRaging(player)) return Task.CompletedTask;

        NorthmanModel.Invoke.Set(player, NorthmanCmd.GetInvoke(player) + DynamicVars["Invoke"].IntValue);
        return Task.CompletedTask;
    }
}