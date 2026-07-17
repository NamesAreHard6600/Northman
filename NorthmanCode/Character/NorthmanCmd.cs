using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using Northman.NorthmanCode.Cards;
using Northman.NorthmanCode.Displays;
using Northman.NorthmanCode.Piles;

namespace Northman.NorthmanCode.Character;

public class NorthmanCmd
{
    public static int GetMax(Player player)
    {
        return NorthmanModel.RageQueueSlots.Get(player);
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
    
    public static int GetRageQueueSize(Player player)
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

    public static bool IsIndexInRange(Player player, int index)
    {
        return index < GetRageQueueSize(player);
    }

    public static bool GetRaging(Player player)
    {
        return NorthmanModel.raging.Get(player);
    }

    public static List<CardModel>? GetSnapshot(Player player)
    {
        return GetRaging(player) ? NorthmanModel.snapshot.Get(player) : [];
    }
    
    public static int GetCurrentIndex(Player player)
    {
        return GetRaging(player) ? NorthmanModel.currentIndex.Get(player) : -1;
    }

    public static int GetInvoke(Player player)
    {
        return NorthmanModel.invoke.Get(player);
    }

    public static async Task ResetRageQueue(PlayerChoiceContext ctx,
        Player player)
    {
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;
        pile.Clear();
        
    }
    
    public static async Task AddCard(PlayerChoiceContext ctx, NorthmanRageCard card)
    {
        var creature = card.Owner;
        var pile = CustomPiles.GetCustomPile(creature.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;

        if (pile.Cards.Count >= GetMax(creature))
        {
            MainFile.Logger.Info("Rage Queue Full. Skipping for now. Later should push out old cards.");
            return;
        }
        
        await CardPileCmd.Add(card, pile);
        
        MainFile.Logger.Info("Card Added");
        NorthmanDisplay.Refresh(creature);
        MainFile.Logger.Info("Display Updated");
    }

    public static async Task TriggerRageQueue(PlayerChoiceContext ctx, Player player)
    {
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;
        var snapshot = pile.Cards.ToList();
        
        NorthmanModel.raging.Set(player, true);
        NorthmanModel.currentIndex.Set(player, 0);
        NorthmanModel.snapshot.Set(player, snapshot);
        NorthmanModel.invoke.Set(player, 0);
        pile.Clear();
        
        MainFile.Logger.Info(GetCurrentIndex(player).ToString());
        MainFile.Logger.Info(GetRageQueueSize(player).ToString());
        MainFile.Logger.Info(IsIndexInRange(player, GetCurrentIndex(player)).ToString());
        
        
        while (IsIndexInRange(player, GetCurrentIndex(player))) {
            // Pre Card Trigger
            int triggers = GetInvoke(player) + 1;
            NorthmanModel.invoke.Set(player, 0);
            // NorthmanModel.currentIndex.Set(player, rageIndex);
            
            // Card Trigger
            await TriggerRageCard(ctx, player, GetCurrentIndex(player), triggers);
            
            // Post Card Trigger
            NorthmanModel.currentIndex.Set(player,  GetCurrentIndex(player)+1);
        }
        
        // Post Rage
        NorthmanModel.raging.Set(player, false);
        NorthmanDisplay.Refresh(player);
        return;
    }

    public static async Task TriggerRageCard(PlayerChoiceContext ctx, Player player, int index, int triggers)
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