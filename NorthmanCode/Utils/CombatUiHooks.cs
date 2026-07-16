// Source: https://github.com/lamali292/Downfall/blob/main/DownfallCode/Utils/CombatUiHooks.cs
using MegaCrit.Sts2.Core.Combat;

namespace Northman.NorthmanCode.Utils;

public static class CombatUiHooks
{
    private static readonly List<Action<CombatState>> handlers = new();

    public static void Register(Action<CombatState> handler) => handlers.Add(handler);

    internal static void RaiseActivate(CombatState state)
    {
        foreach (var handler in handlers)
        {
            try { handler(state); }
            catch (Exception e) { MainFile.Logger.Error($"CombatUi activate handler failed: {e}"); }
        }
    }
}