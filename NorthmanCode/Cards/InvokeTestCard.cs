using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Northman.NorthmanCode.Cards;
using BaseLib.Utils;
using Northman.NorthmanCode.Cards.Token;
using Northman.NorthmanCode.Extensions;


namespace Northman.NorthmanCode.Cards;


public class InvokeTestCard : NorthmanCard  
{
    public InvokeTestCard() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
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