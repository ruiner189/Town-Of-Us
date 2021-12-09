﻿using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    public class PlayerTabPatch
    {
        public static bool Prefix(PlayerTab __instance)
        {
            var outfit = PlayerControl.LocalPlayer.CurrentOutfit;
            var preview = __instance.PlayerPreview;
            PlayerControl.SetPlayerMaterialColors(outfit.ColorId, preview.Body);
            preview.HatSlot.SetHat(SaveManager.LastHat, outfit.ColorId);
            preview.SetSkin(SaveManager.LastSkin);
            PlayerControl.SetPetImage(SaveManager.LastPet, outfit.ColorId, preview.PetSlot);
            __instance.currentColor = outfit.ColorId;
            var colors = Palette.PlayerColors;
            var num = colors.Length / 4f;
            for (int i = 0;i < colors.Length;i++)
            {
                var x = __instance.XRange.Lerp((i % 4) / 4f) + 0.25f;
                var y = __instance.YStart - (i / 4) * 0.55f;
                var colorChip = Object.Instantiate(__instance.ColorTabPrefab, __instance.ColorTabArea, true);
                colorChip.transform.localScale *= 0.8f;
                colorChip.transform.localPosition = new Vector3(x, y, -1f);
                var colorId = (byte)i;
                colorChip.Button.OnClick.AddListener((Action) (() =>
                {
                    __instance.SelectColor(colorId);
                    __instance.ClickEquip();
                    SaveManager.BodyColor = colorId;
                }));
                colorChip.Inner.color = colors[i];
                __instance.ColorChips.Add(colorChip);
            }

            return false;
        }
    }
}
