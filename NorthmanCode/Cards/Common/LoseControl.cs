using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.Powers;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Common;



public class LoseControl : NorthmanCard  
{
    public LoseControl() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithAnger(1);
        WithDamage(10, 4);
        WithRageCard();
        
        this.WithCardTip<LoseControlRageCard>();
        WithSkip(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        
        await AddRageCard<LoseControlRageCard>(ctx, cardPlay);
    }
}



public class LoseControlRageCard : NorthmanRageCard
{
    public LoseControlRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithSkip(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await Skip(ctx, cardPlay);
    }
}