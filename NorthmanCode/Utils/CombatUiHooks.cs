// Source: https://github.com/lamali292/Downfall/blob/main/DownfallCode/Utils/CombatUiHooks.cs
using MegaCrit.Sts2.Core.Combat;

namespace Northman.NorthmanCode.Utils;

public static class CombatUiHooks
{
    private static readonly List<Action<CombatState>> Handlers = new();

    public static void Register(Action<CombatState> handler) => Handlers.Add(handler);

    internal static void RaiseActivate(CombatState state)
    {
        foreach (var handler in Handlers)
        {
            try { handler(state); }
            catch (Exception e) { MainFile.Logger.Error($"CombatUi activate handler failed: {e}"); }
        }
    }
}