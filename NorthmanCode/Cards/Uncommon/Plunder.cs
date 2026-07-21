using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Uncommon;


public class Plunder : NorthmanCard  
{
    public Plunder() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.Self)
    {
        WithAnger(1);
        
        WithRageCard();
        
        this.WithCardTip<PlunderRageCard>();
        WithDamage(6, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await AddRageCard<PlunderRageCard>(ctx, cardPlay);
    }
}



public class PlunderRageCard : NorthmanRageCard
{
    public PlunderRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        ArgumentNullException.ThrowIfNull(cardPlay.Target.Monster);
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);

        if (cardPlay.Target.Monster.IntendsToAttack)
            return;
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);

    }
}