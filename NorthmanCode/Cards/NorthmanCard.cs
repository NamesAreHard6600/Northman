using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Northman.NorthmanCode.Cards.Token;
using Northman.NorthmanCode.CustomEnums;

namespace Northman.NorthmanCode.Cards;

/// <summary>
/// This is the base class for your mod's cards, which is set up to load the card's images from your mod's resources.
/// When creating a card, right click the Cards folder and create a new file with the Custom Card template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
/// </summary>
[Pool(typeof(NorthmanCardPool))]
public abstract class NorthmanCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    
    protected NorthmanCard WithRageCard()
    {
        this.WithKeyword(NorthmanKeyword.Rage);
        return this;
    }

    protected NorthmanCard WithInvoke(int invoke, int upgrade = 0)
    {
        this.WithKeyword(NorthmanKeyword.Invoke);
        WithVar("Invoke", invoke, upgrade);
        return this;
    }

    protected async Task AddRageCard<T>(
        PlayerChoiceContext ctx,
        CardPlay cardPlay) where T : NorthmanRageCard
    {
        NorthmanCard source = this;
        if (CombatState == null) return;
        
        NorthmanRageCard card = CombatState.CreateCard<T>(cardPlay.Card.Owner);
        if (source.IsUpgraded)
            CardCmd.Upgrade(card);
        
        await NorthmanCmd.AddCard(ctx, card);
    }
}