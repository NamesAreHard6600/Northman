using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.Patches;

namespace Northman.NorthmanCode.Vfx;

[GlobalClass]
public partial class NRageQueueDisplay : Control
{
    private const float CardScale = 1;

    private const string DisplayScenePath = "res://Northman/scenes/northman_display.tscn";
    private const string RageQueueSlotScenePath = "res://Northman/scenes/rage_queue_slot.tscn";
    private readonly List<NCustomCardHolder> _cardHolders = [];

    private readonly List<NRageQueueSlot> _slots = [];
    private float _bobTime;
    private int _currentMax = 5;
    private bool _initialized;

    private HBoxContainer? _slotContainer;
    private PackedScene? _stasisSlotScene;

    private Player? _trackedPlayer;

    // Changed to internal static
    internal static NRageQueueDisplay Create(Player player)
    {
        var scene = ResourceLoader.Load<PackedScene>(DisplayScenePath);
        var node = scene.Instantiate<NRageQueueDisplay>();
        node._trackedPlayer = player;
        node.Scale = Vector2.One * CardScale;
        return node;
    }

    public override void _Ready()
    {
        _slotContainer = GetNode<HBoxContainer>("%SlotContainer");
        _stasisSlotScene = ResourceLoader.Load<PackedScene>(RageQueueSlotScenePath);
    }

    public override void _ExitTree()
    {
        ReleaseAllCards();
    }

    private void ReleaseHolder(NCustomCardHolder holder)
    {
        if (holder.CardModel != null)
            FindOnTablePatch.Unregister(holder.CardModel);

        var cardNode = holder.CardNode;
        if (cardNode == null || !IsInstanceValid(cardNode)) return;

        var stillOwned = cardNode.IsInsideTree() && IsAncestorOf(cardNode);
        if (!stillOwned) return;

        cardNode.GetParent()?.RemoveChild(cardNode);
        cardNode.QueueFree();
    }

    private void ReleaseAllCards()
    {
        foreach (var h in _cardHolders) ReleaseHolder(h);
        _cardHolders.Clear();
    }

    private void EnsureSlotCount(int count)
    {
        if (_slotContainer == null || _stasisSlotScene == null) return;
        while (_slots.Count > count)
        {
            var lastSlot = _slots[^1];
            _slots.RemoveAt(_slots.Count - 1);
            lastSlot.QueueFree();
        }

        while (_slots.Count < count)
        {
            var slot = _stasisSlotScene.Instantiate<NRageQueueSlot>();
            _slotContainer.AddChild(slot);
            _slots.Add(slot);
        }
    }

    // Changed to internal
    internal Vector2 GetSlotGlobalPosition(int index)
    {
        var clamped = Math.Clamp(index, 0, _currentMax - 1);
        return clamped < _slots.Count ? _slots[clamped].CardAnchorGlobal : GlobalPosition;
    }

    // Changed to internal
    internal void Refresh()
    {
        if (_trackedPlayer == null) return;

        var sequence = NorthmanCmd.GetRageQueue(_trackedPlayer);
        _currentMax = NorthmanCmd.GetMax(_trackedPlayer);
        _initialized = true;

        ReleaseAllCards();
        foreach (var slot in _slots) slot.ClearCard();
        EnsureSlotCount(_currentMax);

        for (var i = 0; i < _slots.Count; i++)
        {
            var slot = _slots[i];
            slot.Visible = i < _currentMax;

            if (i >= _currentMax || i >= sequence.Count) continue;

            var cardNode = NCard.Create(sequence[i]);
            if (cardNode == null) continue;

            var holder = slot.SetCard(cardNode);
            if (holder == null)
            {
                cardNode.QueueFree();
                continue;
            }

            holder.SetClickable(true);
            var captured = i;
            holder.Pressed += _ => NGame.Instance?.GetInspectCardScreen()
                .Open(AllCardsForInspect(), captured);

            cardNode.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
            FindOnTablePatch.Register(sequence[i], cardNode);
            _cardHolders.Add(holder);
        }
    }

    private List<CardModel> AllCardsForInspect()
    {
        return _cardHolders.Where(h => h.CardModel != null).Select(h => h.CardModel!).ToList();
    }

    // Changed to internal
    internal NCard? GetNCard(CardModel card)
    {
        var holder = _cardHolders.Find(h => h.CardModel == card);
        if (holder == null) return null;

        var cardNode = holder.CardNode;

        if (IsInstanceValid(cardNode) && cardNode.IsInsideTree() && cardNode.Model == card)
        {
            return cardNode;
        }

        return null;
    }

    // Changed to internal
    internal Vector2? GetTargetPosition(CardModel card)
    {
        if (_trackedPlayer == null) return GlobalPosition;

        var sequence = NorthmanCmd.GetRageQueue(_trackedPlayer);
        
        var existingIndex = sequence.IndexOf(card);
        if (existingIndex >= 0)
            return existingIndex < _slots.Count ? _slots[existingIndex].CardAnchorGlobal : GlobalPosition;
        var nextIndex = sequence.Count;
        if (nextIndex >= _currentMax)
            nextIndex = _currentMax - 1;

        return nextIndex < _slots.Count ? _slots[nextIndex].CardAnchorGlobal : GlobalPosition;
    }
}