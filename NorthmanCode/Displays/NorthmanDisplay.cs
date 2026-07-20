using Godot;
using Northman.NorthmanCode.Vfx;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Northman.NorthmanCode.Displays;

public class NorthmanDisplay
{
    private static readonly Dictionary<Player, NRageQueueDisplay> Displays = new();

    public static bool HasDisplay(Player player)
    {
        return Displays.TryGetValue(player, out var display) && GodotObject.IsInstanceValid(display);
    }

    public static void Refresh(Player creature)
    {
        var display = Displays.GetValueOrDefault(creature);
        if (GodotObject.IsInstanceValid(display))
            display.Refresh();
        else
            Displays.Remove(creature);
    }
    
    public static void ClearCard(Player creature, int index)
    {
        var display = Displays.GetValueOrDefault(creature);
        if (GodotObject.IsInstanceValid(display))
            display.ClearCard(index);
        else
            Displays.Remove(creature);
    }

    private static void Register(Player creature, NRageQueueDisplay display)
    {
        if (Displays.TryGetValue(creature, out var old) && GodotObject.IsInstanceValid(old))
            old.QueueFree();

        Displays[creature] = display;
    }

    public static NCard? GetNCard(CardModel card)
    {
        var display = Displays.GetValueOrDefault(card.Owner);
        return GodotObject.IsInstanceValid(display) ? display.GetNCard(card) : null;
    }

    public static Vector2? GetPosition(CardModel model)
    {
        var display = Displays.GetValueOrDefault(model.Owner);
        return GodotObject.IsInstanceValid(display) ? display.GetTargetPosition(model) : null;
    }

    public static void SetupRageQueueUi(NCombatRoom combatRoom, Player player)
    {
        var display = NRageQueueDisplay.Create(player);
        var vfxContainer = combatRoom.CombatVfxContainer;
        vfxContainer.AddChildSafely(display);

        var creatureNode = combatRoom.GetCreatureNode(player.Creature);
        if (creatureNode != null)
        {
            var globalTopPos = creatureNode.GetTopOfHitbox();
            display.Position = vfxContainer.GetGlobalTransform().AffineInverse() * globalTopPos;
            display.Position += new Vector2(0f, -120f);
        }

        Register(player, display);
    }
    
    
}