
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Northman.NorthmanCode.Vfx;

[GlobalClass]
public partial class NRageQueueSlot : Control
{
    private float _baseY;
    private MegaLabel? _count;
    private NCustomCardHolder? _holder;

    private Control? _visualParent;
    public Vector2 CardAnchorGlobal => GetGlobalTransform().Origin + Size * GetGlobalTransform().Scale / 2f;

    private static float CardScale => 0.15f;
    private static float BigCardScale => 0.75f;

    public override void _Ready()
    {
        var anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Play("idle");
        anim.Seek((float)GD.RandRange(0, anim.CurrentAnimationLength), true);
        _visualParent = GetNode<Control>("%Visuals");
        
        _baseY = _visualParent.Position.Y;
    }

    public NCustomCardHolder? SetCard(NCard cardNode)
    {
        ClearCard();

        _holder = NCustomCardHolder.Create(cardNode, CardScale, BigCardScale);
        if (_holder == null) return null;

        _visualParent!.AddChild(_holder);

        Callable.From(() =>
        {
            if (_holder == null || _visualParent == null) return;
            _holder.Position = _visualParent.Size / 2f - _holder.Size / 2f;
        }).CallDeferred();

        return _holder;
    }

    public void ClearCard()
    {
        _holder?.QueueFree();
        _holder = null;
        if (!IsInstanceValid(_count)) return;
        _count!.Visible = false;
    }
}