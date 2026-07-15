using BaseLib.Abstracts;
using Northman.NorthmanCode.Extensions;
using Godot;

namespace Northman.NorthmanCode.Character;

public class NorthmanPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Northman.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}