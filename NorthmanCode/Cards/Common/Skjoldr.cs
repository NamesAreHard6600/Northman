using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using Northman.NorthmanCode.Extensions;


namespace Northman.NorthmanCode.Cards.Common;



public class Skjoldr : NorthmanCard  
{
    public Skjoldr() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithAnger(1);
        WithRageCard();
        
        this.WithCardTip<SkjoldrRageCard>();
        WithBlock(8, 3);
        
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await AddRageCard<SkjoldrRageCard>(ctx, cardPlay);
    }
}




public class SkjoldrRageCard : NorthmanRageCard
{
    public SkjoldrRageCard() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithBlock(8, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
}