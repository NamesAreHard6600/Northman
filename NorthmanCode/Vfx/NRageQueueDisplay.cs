// Source: https://github.com/lamali292/Downfall/blob/main/AutomatonCode/Vfx/NSequenceDisplay.cs
// Then adapted


using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using Northman.NorthmanCode.Cards.Token;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.Piles;

namespace Northman.NorthmanCode.Vfx;

[GlobalClass]
public partial class NRageQueueDisplay : NSlotRevealDisplay
{
    private const float RageQueueCardScale = 0.28f;
    // private const string DisplayScenePath = "res://Automaton/scenes/ui/automaton_display.tscn";

    private CombatManager? _combatManager;
    private Player? _trackedPlayer;
    
    protected override bool IsActive =>
        _trackedPlayer != null && _combatManager is { IsInProgress: true };

    public override void _Ready()
    {
        base._Ready();
        _combatManager = CombatManager.Instance;
    }

    protected override IReadOnlyList<CardModel> GetRageQueueCards()
    {
        var pile = CustomPiles.GetCustomPile(_trackedPlayer?.PlayerCombatState, RageQueuePile.RageQueue)?.Cards;
        return pile ?? [];
    }

    protected override int GetMaxSlots()
    {
        return _trackedPlayer == null ? 5 : NorthmanCmd.GetMax(_trackedPlayer);
    }

    protected override void OnSlotCardSet(int index, CardModel model, NCard node, NCustomCardHolder holder)
    {
        FindOnTablePatch.Register(model, node);
    }

    protected override void OnSlotCardCleared(CardModel model)
    {
        FindOnTablePatch.Unregister(model);
    }

    protected override List<CardModel> BuildInspectList()
    {
        var list = (CustomPiles.GetCustomPile(_trackedPlayer?.PlayerCombatState, EncodePile.FunctionSequence)?.Cards ?? [])
            .Concat(CardHolders.Where(h => h.CardModel != null).Select(h => h.CardModel!)).ToList();
        if (PreviewModel != null) list.Add(PreviewModel);
        return list;
    }
    
    // --- Static lifecycle ---

    private static readonly Dictionary<Player, NSequenceDisplay> Displays = new();

    static NSequenceDisplay()
    {
        CombatManager.Instance.CombatEnded += _ =>
        {
            foreach (var d in Displays.Values.Where(IsInstanceValid))
            {
                // Release BEFORE QueueFree: base implementation unregisters via
                // OnSlotCardCleared and destroys only nodes still under the display.
                d.ReleaseAllSlotCards();
                d.QueueFree();
            }
            Displays.Clear();
        };
    }

    public static NSequenceDisplay? GetDisplay(Player player)
    {
        var display = Displays.GetValueOrDefault(player);
        if (display != null && IsInstanceValid(display)) return display;
        Displays.Remove(player);
        return null;
    }

    public static bool HasDisplay(Player player)
    {
        var display = Displays.GetValueOrDefault(player);
        return display != null && IsInstanceValid(display);
    }

    public override void _ExitTree()
    {
        base._ExitTree(); // releases slot + preview cards (unregisters via OnSlotCardCleared)
        if (_trackedPlayer != null && Displays.GetValueOrDefault(_trackedPlayer) == this)
            Displays.Remove(_trackedPlayer);
    }

    public static void SetupFor(NCombatRoom combatRoom, Player player)
    {
        if (HasDisplay(player)) return;
        var scene = ResourceLoader.Load<PackedScene>(DisplayScenePath);
        var display = scene.Instantiate<NSequenceDisplay>();
        display._trackedPlayer = player;
        display.Scale = Vector2.One * (LocalContext.IsMe(player) ? SequencedCardScale : SequencedCardScale * 0.5f);
        display.Direction = RevealDirection.Right;
        display.ZIndex = LocalContext.IsMe(player) ? 1 : 0;
        var vfxContainer = combatRoom.CombatVfxContainer;
        vfxContainer.AddChildSafely(display);

        var creatureNode = combatRoom.GetCreatureNode(player.Creature);
        if (creatureNode != null)
        {
            var globalTopPos = creatureNode.GetTopOfHitbox();
            var localPos = vfxContainer.GetGlobalTransform().AffineInverse() * globalTopPos;
            var x = LocalContext.IsMe(player) ? -90 : -50;
            var y = LocalContext.IsMe(player) ? -100 : -40;
            display.Position = localPos + new Vector2(x, y);
        }

        Displays[player] = display;
        display.Refresh(true);
    }

    /// <summary>Static refresh used by game logic (AutomatonCmd etc.).</summary>
    public static void Refresh(Player player, bool force = false)
    {
        if (!HasDisplay(player))
        {
            var combatRoom = NCombatRoom.Instance;
            if (combatRoom == null) return;
            SetupFor(combatRoom, player);
            return;
        }
        Displays[player].Refresh(force);
    }
}