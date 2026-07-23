using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Common;


public class WideSwing : NorthmanCard  
{
    public WideSwing() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithAnger(1);
        WithDamage(4, 2);
        WithRageCard();
        
        this.WithCardTip<WideSwingRageCard>();
        WithVar(new HpLossVar(1));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await AdjustAnger(ctx, cardPlay);
        
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        
        await AddRageCard<WideSwingRageCard>(ctx, cardPlay);
    }
}




public class WideSwingRageCard : NorthmanRageCard
{
    public WideSwingRageCard() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithDamage(4, 2);
        WithVar(new HpLossVar(1));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await CreatureCmd.Damage(ctx, Owner.Creature,
            DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            this, cardPlay);
    }
}