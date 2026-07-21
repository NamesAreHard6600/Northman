using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using Northman.NorthmanCode.Extensions;


namespace Northman.NorthmanCode.Cards.Uncommon;


public class Intimidate : NorthmanCard  
{
    public Intimidate() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithAnger(1);
        WithRageCard();
        WithPower<WeakPower>(2, 1);
        
        this.WithCardTip<IntimidateRageCard>();
        WithVar("RageCardWeak", 1);

    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await CommonActions.Apply<WeakPower>(ctx, this, cardPlay);
        
        await AddRageCard<IntimidateRageCard>(ctx, cardPlay);
    }
}


public class IntimidateRageCard : NorthmanRageCard
{
    public IntimidateRageCard() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithPower<WeakPower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CommonActions.Apply<WeakPower>(ctx, this, cardPlay);

    }
}