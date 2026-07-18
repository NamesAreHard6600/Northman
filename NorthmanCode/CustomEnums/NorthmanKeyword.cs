﻿using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Northman.NorthmanCode.CustomEnums;

public class NorthmanKeyword
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Rage;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Invoke;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Anger;
}