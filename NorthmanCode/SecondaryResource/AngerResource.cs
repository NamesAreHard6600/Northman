// Source: https://github.com/Flimsyyy/LexNinja2/blob/master/LexNinja2Code/Api/LexKela.cs
// Then Adapted

using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Northman.NorthmanCode.SecondaryResource;

public class AngerResource
{
    public static SecondaryResourceDefinition Definition { get; private set; } = null!;
    public static string Id { get; private set; } = string.Empty;

    public static void Register()
    {
        var resources = RitsuLibFramework.GetSecondaryResourceRegistry(MainFile.ModId);

        Definition = resources.Register("anger", new SecondaryResourceDefinition(
            defaultAmount: 0,
            baseMaxAmount: null,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.Combat,
            smallIconPath: "res://Northman/images/ui/anger_small.png",
            largeIconPath: "res://Northman/images/ui/anger_large.png"));

        Id = Definition.Id;

        resources.RegisterCombatUi(
            "anger_combat_counter",
            parent =>
            {
                var row = NSecondaryResourceCounter.Create(
                    Definition,
                    new SecondaryResourceCounterStyle
                    {
                        FontSize = 36,
                        OutlineSize = 18,
                        AmountLabelOffset = new Vector2(3, 0),
                        FormatAmount = (amount, max) => amount.ToString(),
                        CounterSize = new Vector2(96, 96),
                        IconSize = new Vector2(90, 90),
                        IconStyle = SecondaryResourceIconStyle.Default with
                        {
                            Size = new Vector2(96, 96),
                            HoverTip = SecondaryResourceHoverTipStyle.Default, // This will need ot change
                        },
                        GainFeedback = new SecondaryResourceCounterGainFeedback
                        {
                            Effects =
                            [
                                SecondaryResourceCounterGainEffects.StarCounterLikeBurst(
                                    StsColors.red
                                ),
                            ],
                        },
                    }
                );
                var energyCounter = parent.GetNode<Control>("%EnergyCounterContainer");
                row.Position = energyCounter.Position + new Vector2(80, 30);
                return row;
            },
            ctx => ctx.Node.Bind(ctx.Player)
        );
        
        resources.RegisterCardUi(
            "anger_card_ui",
            parent =>
            {
                var ui = NSecondaryResourceCardCostUi.Create(
                    Definition,
                    new SecondaryResourceCardCostUiStyle
                    {
                        IconSize = new Vector2(64, 64),
                        FontSize = 32,
                        OutlineSize = 12,
                        AffordableOutlineColor = StsColors.ninetyPercentBlack,
                        OptionalUnavailableOutlineColor = StsColors.ninetyPercentBlack,
                        ReserveVanillaStarCostSlot = true,
                    }
                );

                var energyIcon = parent.GetNode<TextureRect>("%EnergyIcon");
                ui.Position = energyIcon.Position + new Vector2(0, 55);
                return ui;
            },
            ctx => ctx.Node.Refresh(ctx)
        );

        resources.AlwaysShowInCombatUiForCharacter<Character.Northman>(Definition.LocalId);
    }
    
    public static int Get(Player player)
    {
        return SecondaryResourceCmd.Get(player, Id);
    }

    public static async Task Gain(Player player, int amount, AbstractModel? source = null)
    {
        await SecondaryResourceCmd.Gain(player, Id, amount, source);
    }

    public static async Task Gain(CardModel card, int amount)
    {
        await Gain(card.Owner, amount, card);
    }

    public static async Task Gain(CardModel card)
    {
        await Gain(card, card.DynamicVars["anger"].IntValue);
    }

    public static async Task Lose(Player player, int amount, AbstractModel? source = null)
    {
        await SecondaryResourceCmd.Lose(player, Id, amount, source);
    }

    public static async Task Set(Player player, int amount, AbstractModel? source = null)
    {
        await SecondaryResourceCmd.Set(player, Id, amount, source);
    }

    public static async Task<bool> Spend(
        Player player,
        int amount,
        CardModel? card = null,
        AbstractModel? source = null
    )
    {
        return await SecondaryResourceCmd.Spend(player, Id, amount, card, source);
    }

    public static async Task Reset(Player player, AbstractModel? source = null)
    {
        await SecondaryResourceCmd.Reset(player, Id, source: source);
    }

    public static async Task Reset(CardModel card)
    {
        await Reset(card.Owner, card);
    }

    public static IHoverTip HoverTip()
    {
        return SecondaryResourceHoverTipFactory.Create(Definition, 0);
    }
    
    
}