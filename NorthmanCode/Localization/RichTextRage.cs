using Godot;
using MegaCrit.Sts2.Core.RichTextTags;

namespace Northman.NorthmanCode.Localization;

public partial class RichTextRage : AbstractMegaRichTextEffect
{
    protected override string Bbcode => "rage";

    public override bool _ProcessCustomFX(CharFXTransform charFx)
    {
        charFx.Color = new Color("#bd0606");
        return true;
    }
}