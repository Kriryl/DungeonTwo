using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.CharacterScripts;
using Assets.HeroEditor4D.Common.CommonScripts;
using Assets.HeroEditor4D.FantasyInventory.Scripts.Data;
using Assets.HeroEditor4D.FantasyInventory.Scripts.Enums;
using HeroEditor4D.Common;
using HeroEditor4D.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.FantasyInventory.Scripts
{
    public class CharacterInventorySetup
    {
        public static void Setup(Character character, List<Item> equipped, CharacterAppearance appearance)
        {
            if (!appearance.Underwear.IsEmpty())
            {
                character.Underwear = character.SpriteCollection.Armor.Single(i => i.Name == appearance.Underwear).Sprites;
            }

            character.UnderwearColor = appearance.UnderwearColor;
            character.ShowHelmet = appearance.ShowHelmet;
            appearance.Setup(character, initialize: false);
            Setup(character, equipped);
        }

        public static void Setup(Character character, List<Item> equipped)
        {
            character.ResetEquipment();
            character.HideEars = false;
            character.CropHair = false;

            foreach (var item in equipped)
            {
                try
                {
                    switch (item.Params.Type)
                    {
                        case ItemType.Weapon:

                            switch (item.Params.Class)
                            {
                                case ItemClass.Bow:
                                    character.WeaponType = WeaponType.Bow;
                                    character.CompositeWeapon = character.SpriteCollection.Bow.FindSprites(item.Params.SpriteId);
                                    break;
                                case ItemClass.Firearm:
                                    character.WeaponType = WeaponType.Paired;
                                    character.SecondaryWeapon = character.SpriteCollection.Firearm1H.FindSprites(item.Params.SpriteId)[1];
                                    break;
                                default:
                                    if (equipped.Any(i => i.IsFirearm))
                                    {
                                        character.WeaponType = WeaponType.Paired;
                                        character.PrimaryWeapon = character.SpriteCollection.MeleeWeapon1H.FindSprite(item.Params.SpriteId);
                                    }
                                    else
                                    {
                                        character.WeaponType = item.Params.Tags.Contains(ItemTag.TwoHanded) ? WeaponType.Melee2H : WeaponType.Melee1H;
                                        character.PrimaryWeapon = (character.WeaponType == WeaponType.Melee1H ? character.SpriteCollection.MeleeWeapon1H : character.SpriteCollection.MeleeWeapon2H).FindSprite(item.Params.SpriteId);
                                    }
                                    break;
                                case ItemClass.Bomb:
                                    character.WeaponType = WeaponType.Throwable;
                                    character.PrimaryWeapon = character.SpriteCollection.Throwable.FindSprite(item.Params.SpriteId);
                                    break;
                            }
                            break;
                        case ItemType.Shield:
                            character.Shield = character.SpriteCollection.Shield.FindSprites(item.Params.SpriteId);
                            character.WeaponType = WeaponType.Melee1H;
                            break;
                        case ItemType.Armor:
                            character.Armor = character.SpriteCollection.Armor.FindSprites(item.Params.SpriteId);
                            break;
                        case ItemType.Helmet:
                            var entry = character.SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId.Replace(".Helmet.", ".Armor."));
                            character.Equip(entry, EquipmentPart.Helmet);
                            character.HideEars = !entry.Tags.Contains("ShowEars");
                            character.CropHair = !entry.Tags.Contains("FullHair");
                            break;
                        case ItemType.Vest:
                            character.Equip(character.SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId.Replace(".Vest.", ".Armor.")), EquipmentPart.Vest, Color.white);
                            break;
                        case ItemType.Bracers:
                            character.Equip(character.SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId.Replace(".Bracers.", ".Armor.")), EquipmentPart.Bracers, Color.white);
                            break;
                        case ItemType.Leggings:
                            character.Equip(character.SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId.Replace(".Leggings.", ".Armor.")), EquipmentPart.Leggings, Color.white);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Unable to equip {0} ({1})", item.Params.SpriteId, e.Message);
                }
            }

            foreach (var part in new[] { ItemType.Vest, ItemType.Bracers, ItemType.Leggings })
            {
                if (equipped.Any(i => i.Params.Type == part))
                {
                }
                else if (character.Underwear.Any())
                {
                    var entry = character.SpriteCollection.Armor.Single(i => i.Sprites.Contains(character.Underwear[0]));

                    character.Equip(entry, part.ToString().ToEnum<EquipmentPart>(), character.UnderwearColor);
                }
            }

            character.Initialize();
        }
    }
}