using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.Powers;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Common;


public class FuryStrike : NorthmanCard  
{
    public FuryStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithAnger(1);
        WithDamage(6, 3);
        WithTags(CardTag.Strike);
        WithRageCard();
        
        this.WithCardTip<FuryStrikeRageCard>();
        WithPower<VulnerablePower>(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        
        await AddRageCard<FuryStrikeRageCard>(ctx, cardPlay);
    }
}


public class FuryStrikeRageCard : NorthmanRageCard
{
    public FuryStrikeRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithPower<VulnerablePower>(1, 1);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CommonActions.Apply<VulnerablePower>(ctx, this, cardPlay);
    }
}