// Source: https://github.com/lamali292/Downfall/blob/main/DownfallCode/Utils/RichTextEffectRegistry.cs and Adapted (with AI shhhh)

using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.RichTextTags;

namespace Northman.NorthmanCode.Utils;

/// <summary>
/// Central registry for custom [bbcode] rich-text effects.
/// </summary>
public static class RichTextEffectRegistry
{
    private static readonly List<AbstractMegaRichTextEffect> Effects = [];
    private static readonly HashSet<Type> RegisteredTypes = [];

    /// <summary>Register one effect instance. Deduplicated by concrete type.</summary>
    private static void Register(AbstractMegaRichTextEffect effect)
    {
        if (RegisteredTypes.Add(effect.GetType()))
            Effects.Add(effect);
    }

    /// <summary>Register an effect type with a parameterless constructor.</summary>
    public static void Register<T>() where T : AbstractMegaRichTextEffect, new()
        => Register(new T());

    /// <summary>
    /// Called by the patch for every label. InstallEffectsIfNeeded rebuilds CustomEffects
    /// with only the built-ins when it runs, so we re-add anything missing afterward.
    /// </summary>
    internal static void InstallInto(MegaRichTextLabel label)
    {
        foreach (var effect in Effects)
        {
            // Check if CustomEffects already has an effect of the exact same type
            bool alreadyHasEffect = label.CustomEffects.Any(e => e.GetType() == effect.GetType());

            if (!alreadyHasEffect)
            {
                label.CustomEffects.Add(effect);
            }
        }
    }
}