using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Northman.NorthmanCode.Cards;
using Northman.NorthmanCode.Displays;
using Northman.NorthmanCode.Piles;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Northman.NorthmanCode.Character;

public class NorthmanCmd
{
    private const string ResourceId = "NORTHMAN_SECONDARY_RESOURCE_ANGER";
    
    public static int GetSlotSize(Player player)
    {
        if (GetRaging(player))
        {
            var snapshot = GetSnapshot(player);
            return snapshot?.Count ?? 0;
        }
        
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return -1;
        return pile.Cards.Count;
    }
    
    public static int GetSlotSizeMax(Player player)
    {
        return NorthmanModel.RageQueueSlots.Get(player);
    }

    public static int GetAnger(Player player)
    {
        return SecondaryResourceCmd.Get(player, ResourceId);
    }
    
    public static int GetAngerMax(Player player)
    {
        return 5;
    }

    public static List<CardModel>? GetRageQueue(Player player)
    {
        if (GetRaging(player))
        {
            return GetSnapshot(player);
        }
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        return pile?.Cards.ToList();
    }

    public static bool GetRaging(Player player)
    {
        return NorthmanModel.Raging.Get(player);
    }

    public static List<CardModel>? GetSnapshot(Player player)
    {
        return GetRaging(player) ? NorthmanModel.Snapshot.Get(player) : [];
    }
    
    public static int GetCurrentIndex(Player player)
    {
        return GetRaging(player) ? NorthmanModel.CurrentIndex.Get(player) : -1;
    }

    public static int GetInvoke(Player player)
    {
        return NorthmanModel.Invoke.Get(player);
    }
    
    public static int GetSkip(Player player)
    {
        return NorthmanModel.Skip.Get(player);
    }
    
    public static bool IsIndexInRange(Player player, int index)
    {
        return index < GetSlotSize(player);
    }

    public static void SetAngerAmount(Player player, int amount)
    {
        SecondaryResourceCmd.Set(player, ResourceId, amount);
    }

    public static Task ResetRageQueue(PlayerChoiceContext ctx,
        Player player)
    {
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return Task.CompletedTask;
        pile.Clear();
        // To lazy to make a setter
        SetAngerAmount(player, 0);
        NorthmanDisplay.Refresh(player);
        return Task.CompletedTask;
    }
    
    public static async Task AddCard(PlayerChoiceContext ctx, NorthmanRageCard card)
    {
        var creature = card.Owner;
        var pile = CustomPiles.GetCustomPile(creature.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;

        if (pile.Cards.Count >= GetSlotSizeMax(creature))
        {
            MainFile.Logger.Error("Rage Queue Full. Skipping for now. Later it will push out old cards.");
            return;
        }
        
        await CardPileCmd.Add(card, pile, skipVisuals: true);
        
        MainFile.Logger.Info("Card Added");
        NorthmanDisplay.Refresh(creature);
        MainFile.Logger.Info("Display Updated");
    }
    
    public static async Task TriggerRageQueue(PlayerChoiceContext ctx, Player player)
    {
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;
        var snapshot = pile.Cards.ToList();
        
        NorthmanModel.Raging.Set(player, true);
        NorthmanModel.CurrentIndex.Set(player, 0);
        NorthmanModel.Snapshot.Set(player, snapshot);
        NorthmanModel.Invoke.Set(player, 0);
        NorthmanModel.Skip.Set(player, 0);
        await ResetRageQueue(ctx, player);
        
        MainFile.Logger.Info(GetCurrentIndex(player).ToString());
        MainFile.Logger.Info(GetSlotSize(player).ToString());
        MainFile.Logger.Info(IsIndexInRange(player, GetCurrentIndex(player)).ToString());
        
        
        while (IsIndexInRange(player, GetCurrentIndex(player))) {
            // Pre Card Trigger
            var triggers = GetInvoke(player) + 1;
            var skip = GetSkip(player);
            if (skip > 0)
            {
                triggers--;
                NorthmanModel.Skip.Set(player, skip-1);
            }
            NorthmanModel.Invoke.Set(player, 0);
            // NorthmanModel.currentIndex.Set(player, rageIndex);
            
            // Card Trigger
            await TriggerRageCard(ctx, player, GetCurrentIndex(player), triggers);
            
            // Post Card Trigger
            NorthmanDisplay.ClearCard(player, GetCurrentIndex(player));
            NorthmanModel.CurrentIndex.Set(player,  GetCurrentIndex(player)+1);
        }
        
        // Post Rage
        NorthmanModel.Raging.Set(player, false);
        NorthmanDisplay.Refresh(player);
    }

    private static async Task TriggerRageCard(PlayerChoiceContext ctx, Player player, int index, int triggers)
    {
        if (!GetRaging(player)) return;
        List<CardModel>? snapshot = GetSnapshot(player);
        if (snapshot == null) return;
        CardModel card = snapshot[index];
        
        MainFile.Logger.Info("Title: " + card.Title);
        MainFile.Logger.Info("Triggers: " + triggers);

        for (int i = 0; i < triggers; i++)
        {
            var clone = card.CreateClone();
            clone.AddKeyword(CardKeyword.Exhaust);
            await CardCmd.AutoPlay(ctx, clone, null);
            // skipCardPileVisuals:true removes all visuals (including the card showing up) which I want to keep
            // If I add other animations, I could see it being useful though
            clone.RemoveFromCurrentPile(true);
            // This removes the cards from the pile, but has a small bug where it shows a card in the exhaust pile but 
            // there isn't actually anything in there (i.e. it doesn't really update properly)
        }
        
    }
}