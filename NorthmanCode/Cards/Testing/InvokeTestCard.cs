using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Northman.NorthmanCode.Cards;
using BaseLib.Utils;
using Northman.NorthmanCode.Extensions;


namespace Northman.NorthmanCode.Cards.Testing;


public class InvokeTestCard : NorthmanCard  
{
    public InvokeTestCard() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithRageCard();
        this.WithCardTip<InvokeTestRageCard>();
        WithInvoke(3, 1); // This is a hopefully temporary solution to get the description working
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AddRageCard<InvokeTestRageCard>(ctx, cardPlay);
    }
}


public class InvokeTestRageCard : NorthmanRageCard
{
    public InvokeTestRageCard() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithInvoke(3, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await Invoke(ctx, cardPlay);
    }
}