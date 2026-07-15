using BaseLib.Abstracts;
using BaseLib.Utils;
using Northman.NorthmanCode.Character;

namespace Northman.NorthmanCode.Potions;

[Pool(typeof(NorthmanPotionPool))]
public abstract class NorthmanPotion : CustomPotionModel;