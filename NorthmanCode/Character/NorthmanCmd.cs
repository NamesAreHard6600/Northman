using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using Northman.NorthmanCode.Cards;
using Northman.NorthmanCode.Piles;

namespace Northman.NorthmanCode.Character;

public class NorthmanCmd
{
    public static int GetMax()
    {
        return 5;
    }
    
    public static async Task AddCard(PlayerChoiceContext ctx, NorthmanRageCard card)
    {
        var creature = card.Owner;
        var pile = CustomPiles.GetCustomPile(creature.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;

        if (pile.Cards.Count >= GetMax())
        {
            MainFile.Logger.Info("Rage Queue Full. Skipping for now. Later should push out old cards.");
            return;
        }
        
        await CardPileCmd.Add(card, pile);
        
        MainFile.Logger.Info("Card Added");
        return;
    }

    public static async Task TriggerRageQueue(PlayerChoiceContext ctx, Player player)
    {
        var pile = CustomPiles.GetCustomPile(player.PlayerCombatState, RageQueuePile.RageQueue);
        if (pile == null) return;
        var snapshot = pile.Cards.ToList();
        
        foreach (var card in snapshot) {
            // This will hopefully be removed later to make it go into the ether, but the exhaust pile is fine for now. 
            card.AddKeyword(CardKeyword.Exhaust);
            await CardCmd.AutoPlay(ctx, card, null);
        }

        return;
    }
}