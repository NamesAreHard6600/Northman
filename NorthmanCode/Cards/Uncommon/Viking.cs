using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Uncommon;


// As if to go on a Viking
public class Viking : NorthmanCard  
{
    public Viking() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithAnger(1);
        WithCards(3, 1);
        WithRageCard();
        
        this.WithCardTip<VikingRageCard>();
        WithSkip(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);

        await CommonActions.Draw(this, ctx);
        
        await AddRageCard<VikingRageCard>(ctx, cardPlay);
    }
}


public class VikingRageCard : NorthmanRageCard
{
    public VikingRageCard() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithSkip(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await Skip(ctx, cardPlay);
    }
}