using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;

namespace Northman.NorthmanCode.Cards.Common;


public class GetAngry : NorthmanCard  
{
    public GetAngry() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithAnger(2);
        WithCards(1);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        await CommonActions.Draw(this, ctx);
    }
}
