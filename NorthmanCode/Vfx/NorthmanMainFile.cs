using Godot;
using MegaCrit.Sts2.Core.Modding;
using Northman.NorthmanCode.Localization;
using Northman.NorthmanCode.Utils;

namespace Northman.NorthmanCode.Vfx;


[ModInitializer(nameof(Initialize))]
public partial class NorthmanMainFile : Node
{
    public const string ModId = "Northman";
    public static void Initialize()
    {
        MainFile.Logger.Info("NorthmanMainFile.Initialize");
        RichTextEffectRegistry.Register<RichTextRage>();
    }
}
