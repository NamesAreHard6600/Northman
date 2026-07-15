using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Northman.NorthmanCode.Cards.Token;

public class InvokeTestRageCard : NorthmanRageCard
{
    public InvokeTestRageCard() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
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