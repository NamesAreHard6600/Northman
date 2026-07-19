using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using Northman.NorthmanCode.Cards;
using Northman.NorthmanCode.Displays;
using Northman.NorthmanCode.Piles;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Northman.NorthmanCode.Character;

public class NorthmanModel(): CustomSingletonModel(HookType.Combat), ISecondaryResourceHookListener 
{
    // internal static readonly SpireField<Player, RageQueuePile> RageQueue = new(() => null);
    // Anger amount now handled by Secondary Resource
    //internal static readonly SpireField<Player, int> AngerAmount = new(() => 0);
    internal static readonly SpireField<Player, int> RageQueueSlots = new(() => -1);
    
    // Rage queue info for public access
    internal static readonly SpireField<Player, bool> raging = new(() => false);
    internal static readonly SpireField<Player, int> currentIndex = new(() => 0);
    internal static readonly SpireField<Player, List<CardModel>> snapshot = new(() =>
        []);
    internal static readonly SpireField<Player, int> invoke = new(() => 0);

    internal static readonly String ResourceId = "NORTHMAN_SECONDARY_RESOURCE_ANGER";
    
    internal static void SetupNorthmanUi(CombatState state)
    {
        var combatRoomNode = NCombatRoom.Instance;
        if (combatRoomNode == null) return;
        
        MainFile.Logger.Info("We set up the Northman UI now");
        foreach (var player in state.Players)
        {
            if (player.Character is not Northman) {
                RageQueueSlots.Set(player, -1);
                continue;
            }
            MainFile.Logger.Info("There's a Northman, he has 5 slots now. Prepping his UI");
            InitRageQueueUi(player);
        }
    }

    internal static void InitRageQueueUi(Player player)
    {
        // Here we essentially assume that all rage is a Northman and that other characters are incapable of receiving rage
        // This differs from my main source, The Guardian from Downfall which can give other players Stasis cards
        RageQueueSlots.Set(player, 5);
        
        var combatRoom = NCombatRoom.Instance;
        if (combatRoom != null && !NorthmanDisplay.HasDisplay(player))
                NorthmanDisplay.SetupRageQueueUi(combatRoom, player);
        
        NorthmanDisplay.Refresh(player);
    }
    
    // Hooks
    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var player = cardPlay.Player;
        if (NorthmanCmd.GetAnger(player) >= NorthmanCmd.GetAngerMax(player))
        {
            await NorthmanCmd.TriggerRageQueue(ctx, player);
        }
    }
    
    public bool ShouldSpendSecondaryResource(SecondaryResourceSpendContext context)
    {
        // Block Spending Resource for all Anger Cards
        // Any cards that remove anger will need to go through Lose and not Spend
        return context.Definition.Id != ResourceId;
    }
    
    /*
    public SecondaryResourceInsufficientPayment ModifySecondaryResourceInsufficientPayment(
        SecondaryResourceInsufficientPaymentContext context,
        SecondaryResourceInsufficientPayment payment)

    {
        if (context.Definition.Id == ResourceId && !payment.AllowsPlay)
        {
            return SecondaryResourceInsufficientPayment.AllowPlay();
        }

        return payment;
    }
    /*
    // Handled by the card itself
    public async Task AfterSecondaryResourceSpent(SecondaryResourceSpendContext context)
    {
        if (context.Definition.Id == ResourceId)
        {
            MainFile.Logger.Info("Resource was spent, giving it back: " + context.Amount);
            await SecondaryResourceCmd.Gain(context.Player, ResourceId, context.Amount, source: context.Card);
        }
    }

    public SecondaryResourceInsufficientPayment ModifySecondaryResourceInsufficientPayment(
        SecondaryResourceInsufficientPaymentContext context, SecondaryResourceInsufficientPayment payment)
    {
        payment.AllowsPlay = true;
    }*/
    
}