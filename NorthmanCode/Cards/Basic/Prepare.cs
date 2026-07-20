using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Cards;
using Northman.NorthmanCode.Extensions;
using Northman.NorthmanCode.SecondaryResource;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Northman.NorthmanCode.Cards.Basic;


public class Prepare : NorthmanCard  
{
    public Prepare() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithAnger(1);
        WithRageCard();
        
        // For Rage Card
        WithBlock(5, 3);
        WithInvoke(1);
        
        this.WithCardTip<PrepareRageCard>();
        
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await AddRageCard<PrepareRageCard>(ctx, cardPlay);
    }
}



public class PrepareRageCard : NorthmanRageCard
{
    public PrepareRageCard() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithBlock(5, 3);
        WithInvoke(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        await Invoke(ctx, cardPlay);
    }
}