using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards;



public class StrikeNorthman : NorthmanCard  
{
    public StrikeNorthman() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(3, 2);
        WithTags(CardTag.Strike);
        WithRageCard();
        
        this.WithCardTip<StrikeNorthmanRageCard>();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        StrikeNorthman source = this;
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        if (CombatState == null) return;
        
        
        NorthmanRageCard card = CombatState.CreateCard<StrikeNorthmanRageCard>(cardPlay.Card.Owner);
        if (source.IsUpgraded)
            CardCmd.Upgrade(card);
        
        await NorthmanCmd.AddCard(ctx, card);
    }
}