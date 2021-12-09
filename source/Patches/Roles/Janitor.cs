using Hazel;
using Reactor;
using TownOfUs.ImpostorRoles.JanitorMod;
using TownOfUs.Patches.Buttons;

namespace TownOfUs.Roles
{
    public class Janitor : Role
    {
        public ModdedButton CleanButton;

        public Janitor(PlayerControl player) : base(player)
        {
            Name = "Janitor";
            ImpostorText = () => "Clean up bodies";
            TaskText = () => "Clean bodies to prevent Crewmates from discovering them.";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Janitor;
            Faction = Faction.Impostors;



            CleanButton = new ModdedButton(player);
            CleanButton.ButtonType = ButtonType.AbilityButton;
            CleanButton.ButtonTarget = ButtonTarget.DeadBody;
            CleanButton.UseDefault = true;
            CleanButton.SetAction(CleanAction);
            CleanButton.Sprite = TownOfUs.JanitorClean;
            CleanButton.RegisterButton();

            GenerateKillButton();

            ModdedButton.LinkUseTime(KillButton, CleanButton);
            
        }

        public bool CleanAction(ModdedButton button)
        {
            var playerId = button.ClosestBody.ParentId;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.JanitorClean, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Coroutines.Start(Coroutine.CleanCoroutine(button.ClosestBody, this));
            return false;
        }

    }
}