using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Cards;
//using Northman.NorthmanCode.Localization;
using Northman.NorthmanCode.Utils;
using Northman.NorthmanCode.Character;
using Northman.NorthmanCode.SecondaryResource;
using Northman.NorthmanCode.Vfx;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Northman.NorthmanCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Northman"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        
        Logger.Info("Should be registering for stuff I Guess");
        NNorthmanCustomCardHolder.InitPool();
        CombatUiHooks.Register(NorthmanModel.SetupNorthmanUi);

        AngerResource.Register();
    }
}