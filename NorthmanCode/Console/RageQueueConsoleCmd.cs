using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Runs;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.Piles;

namespace Northman.NorthmanCode.Console;

public class RageQueueTriggerCmd : AbstractConsoleCmd
{
    public override string CmdName => "northman-trigger";
    public override string Args => "";
    public override string Description => "Triggers the rage queue (which will also clear it).";
    public override bool IsNetworked => true;

    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (!RunManager.Instance.IsInProgress)
            return new CmdResult(false, "No run in progress.");
        if (issuingPlayer == null)
            return new CmdResult(false, "No player context.");
        
        var ctx = new BlockingPlayerChoiceContext();
        
        TaskHelper.RunSafely(NorthmanCmd.TriggerRageQueue(ctx, issuingPlayer));

        return new CmdResult(true, "Rage Queue Triggered");
    }
}

public class RageQueueLogCmd : AbstractConsoleCmd
{
    public override string CmdName => "northman-log";
    public override string Args => "";
    public override string Description => "Logs the state of the rage queue.";
    public override bool IsNetworked => true;

    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (!RunManager.Instance.IsInProgress)
            return new CmdResult(false, "No run in progress.");
        if (issuingPlayer == null)
            return new CmdResult(false, "No player context.");
        
        var ctx = new BlockingPlayerChoiceContext();
        
        var pile = CustomPiles.GetCustomPile(issuingPlayer.PlayerCombatState, RageQueuePile.RageQueue);
        var size = NorthmanCmd.GetSlotSize(issuingPlayer);
        
        MainFile.Logger.Info("Anger: " + NorthmanCmd.GetAnger(issuingPlayer) + "/" + NorthmanCmd.GetAngerMax(issuingPlayer));
        MainFile.Logger.Info("Size of Rage Queue: " + size);
        if (pile == null || size == 0)
            return new CmdResult(true, "No Cards in Rage Queue.");

        var cardNumber = 1;
        foreach (var card in pile.Cards)
        {
            MainFile.Logger.Info("Card " + cardNumber + ": " + card.Title);
        }

        return new CmdResult(true, "Queue logged to debug.");
    }
    
}