using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;

namespace Northman.NorthmanCode.Cards.Basic;


public class LetItOut : NorthmanCard  
{
    public LetItOut() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithAnger(-1);
        WithDamage(10, 14);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
    }
}


