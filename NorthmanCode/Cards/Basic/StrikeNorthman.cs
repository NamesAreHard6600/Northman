using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Basic;



public class StrikeNorthman : NorthmanCard  
{
    public StrikeNorthman() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithAnger(1);
        WithDamage(3, 2);
        WithTags(CardTag.Strike);
        WithRageCard();
        
        this.WithCardTip<StrikeNorthmanRageCard>();
        
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        
        await AddRageCard<StrikeNorthmanRageCard>(ctx, cardPlay);
    }
}


public class StrikeNorthmanRageCard : NorthmanRageCard
{
    public StrikeNorthmanRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithDamage(3, 2);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
    }
}