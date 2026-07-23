using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;

namespace Northman.NorthmanCode.Cards.Common;


public class FallBack : NorthmanCard  
{
    public FallBack() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithAnger(-1);
        WithBlock(10, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        await CommonActions.CardBlock(this, cardPlay);
    }
}