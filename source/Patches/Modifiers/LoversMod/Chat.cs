using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.Modifiers.LoversMod
{
    public static class Chat
    {
        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public static class AddChat
        {
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer)
            {
                if (__instance != HudManager.Instance.Chat) return true;
                var localPlayer = PlayerControl.LocalPlayer;
                if (localPlayer == null) return true;
                return MeetingHud.Instance != null || LobbyBehaviour.Instance != null || localPlayer.Data.IsDead ||
                       localPlayer.IsLover() || sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class EnableChat
        {
            public static void Postfix(HudManager __instance)
            {
                if (Role.GetRole(PlayerControl.LocalPlayer) == null) return;
                if (PlayerControl.LocalPlayer.IsLover() & !__instance.Chat.isActiveAndEnabled)
                    __instance.Chat?.SetVisible(true);
                else if (!PlayerControl.LocalPlayer.IsLover() & !PlayerControl.LocalPlayer.Data.IsDead & __instance.Chat.isActiveAndEnabled)
                    __instance.Chat?.SetVisible(false); ;
            }
        }
    }
}