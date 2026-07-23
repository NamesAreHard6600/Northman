using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.CustomEnums;
using Northman.NorthmanCode.Extensions;

namespace Northman.NorthmanCode.Cards.Rare;



public class Battlecry : NorthmanCard  
{
    public Battlecry() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithKeywords(NorthmanKeyword.Rage, CardKeyword.Exhaust);

    }

    protected override async Task OnPlay(
        PlayerChoiceContext ctx,
        CardPlay cardPlay)
    {
        await NorthmanCmd.TriggerRageQueue(ctx, Owner, true);
    }
}