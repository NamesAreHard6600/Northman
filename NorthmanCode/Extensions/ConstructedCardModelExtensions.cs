using System;
using System.Reflection;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Northman.NorthmanCode.Extensions;

public static class ConstructedCardModelExtensions
{
    // Retrieve and cache the protected 'WithTip' method once via reflection
    private static readonly MethodInfo? WithTipMethod = typeof(ConstructedCardModel)
        .GetMethod("WithTip", BindingFlags.Instance | BindingFlags.NonPublic);

    public static ConstructedCardModel WithCardTip<T>(
        this ConstructedCardModel cons, 
        Action<T, CardModel>? modifyTipCard = null) where T : CardModel
    {
        // 1. Create the TooltipSource object exactly as you did before
        var tooltipSource = new TooltipSource(card =>
        {
            CardModel mutable = ModelDb.Card<T>().ToMutable();
            if (card.IsUpgraded)
                mutable.UpgradeInternal();
            if (mutable is T obj2)
            {
                modifyTipCard?.Invoke(obj2, card);
            }
            return HoverTipFactory.FromCard(mutable);
        });

        // 2. Safely invoke the protected 'WithTip' method on 'cons'
        if (WithTipMethod != null)
        {
            WithTipMethod.Invoke(cons, [tooltipSource]);
        }

        return cons;
    }
}