// Source: https://github.com/lamali292/Downfall/blob/main/DownfallCode/Patches/CombatUiActivatePatch.cs

using Northman.NorthmanCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Northman.NorthmanCode.Patches;

[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
internal static class CombatUiActivatePatch
{
    [HarmonyPostfix]
    private static void Postfix(CombatState state)
        => CombatUiHooks.RaiseActivate(state);
}