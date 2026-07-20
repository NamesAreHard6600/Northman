using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.Powers;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Common;

public class DualHorned : NorthmanCard  
{
    public DualHorned() : base(1, CardType.Attack, CardRarity.Common, TargetType.Self)
    {
        WithAnger(1);
        WithRageCard();
        
        this.WithCardTip<DualHornedRageCard>();
        WithDamage(5, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await AddRageCard<DualHornedRageCard>(ctx, cardPlay);
        await AddRageCard<DualHornedRageCard>(ctx, cardPlay);
    }
}



public class DualHornedRageCard : NorthmanRageCard
{
    public DualHornedRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithDamage(5, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
    }
}