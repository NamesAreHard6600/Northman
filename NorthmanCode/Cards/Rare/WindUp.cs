using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Rare;



public class WindUp : NorthmanCard  
{
    public WindUp() : base(2, CardType.Attack, CardRarity.Rare, TargetType.Self)
    {
        WithRageCard();
        WithCostUpgradeBy(-1);
        
        this.WithCardTip<WindUpRageCard1>();
        WithDamage(50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AddRageCard<WindUpRageCard1>(ctx, cardPlay);
    }
}


public class WindUpRageCard1 : NorthmanRageCard
{
    public WindUpRageCard1() : base(0, CardType.Attack, CardRarity.Token, TargetType.Self)
    {
        WithRageCard();
        
        this.WithCardTip<WindUpRageCard2>();
        WithDamage(50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AddRageCard<WindUpRageCard2>(ctx, cardPlay);
        
    }
}

public class WindUpRageCard2 : NorthmanRageCard
{
    public WindUpRageCard2() : base(0, CardType.Attack, CardRarity.Token, TargetType.Self)
    {
        WithRageCard();
        
        this.WithCardTip<WindUpRageCard3>();
        WithDamage(50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AddRageCard<WindUpRageCard3>(ctx, cardPlay);
    }
}

public class WindUpRageCard3 : NorthmanRageCard
{
    public WindUpRageCard3() : base(0, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
    {
        WithDamage(50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
    }
}
