using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Rare;

public class Prioritize : NorthmanCard  
{
    public Prioritize() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithAnger(1);
        WithRageCard();
        
        this.WithCardTip<PrioritizeRageCard>();
        WithInvoke(4);
        WithSkip(3);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);

    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AddRageCard<PrioritizeRageCard>(ctx, cardPlay);
    }
}

public class PrioritizeRageCard : NorthmanRageCard
{
    public PrioritizeRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.Self)
    {
        WithInvoke(4);
        WithSkip(3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await Skip(ctx, cardPlay);
        await Invoke(ctx, cardPlay);
    }
}