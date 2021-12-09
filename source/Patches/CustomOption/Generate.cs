using System;
using TownOfUs.Patches.CustomOption;

namespace TownOfUs.CustomOption
{
    public class Generate
    {
        // Enable Crewmate Roles
        public static CustomHeaderOption CrewmateRoles;
        public static CustomNumberOption MayorOn;
        public static CustomNumberOption SheriffOn;
        public static CustomNumberOption EngineerOn;
        public static CustomNumberOption SwapperOn;
        public static CustomNumberOption InvestigatorOn;
        public static CustomNumberOption TimeLordOn;
        public static CustomNumberOption MedicOn;
        public static CustomNumberOption SeerOn;
        public static CustomNumberOption SpyOn;
        public static CustomNumberOption SnitchOn;
        public static CustomNumberOption AltruistOn;
        public static CustomNumberOption ButtonBarryOn;


        // Max Crewmate Roles
        public static CustomNumberOption MayorMax;
        public static CustomNumberOption SheriffMax;
        public static CustomNumberOption EngineerMax;
        public static CustomNumberOption SwapperMax;
        public static CustomNumberOption InvestigatorMax;
        public static CustomNumberOption TimeLordMax;
        public static CustomNumberOption MedicMax;
        public static CustomNumberOption SeerMax;
        public static CustomNumberOption SpyMax;
        public static CustomNumberOption SnitchMax;
        public static CustomNumberOption AltruistMax;

        // Enable Neutral Roles
        public static CustomHeaderOption NeutralRoles;
        public static CustomNumberOption JesterOn;
        public static CustomNumberOption ShifterOn;
        public static CustomNumberOption GlitchOn;
        public static CustomNumberOption ExecutionerOn;
        public static CustomNumberOption ArsonistOn;
        public static CustomNumberOption PhantomOn;

        // Max Neutral Roles
        public static CustomNumberOption JesterMax;
        public static CustomNumberOption ShifterMax;
        public static CustomNumberOption GlitchMax;
        public static CustomNumberOption ExecutionerMax;
        public static CustomNumberOption ArsonistMax;
        //public static CustomNumberOption PhantomMax;

        // Enable Impostor Roles
        public static CustomHeaderOption ImpostorRoles;
        public static CustomNumberOption JanitorOn;
        public static CustomNumberOption MorphlingOn;
        public static CustomNumberOption CamouflagerOn;
        public static CustomNumberOption MinerOn;
        public static CustomNumberOption SwooperOn;
        public static CustomNumberOption UndertakerOn;
        public static CustomNumberOption AssassinOn;
        public static CustomNumberOption UnderdogOn;

        // Max Impostor Roles
        public static CustomNumberOption JanitorMax;
        public static CustomNumberOption MorphlingMax;
        public static CustomNumberOption CamouflagerMax;
        public static CustomNumberOption MinerMax;
        public static CustomNumberOption SwooperMax;
        public static CustomNumberOption UndertakerMax;
        public static CustomNumberOption AssassinMax;
        public static CustomNumberOption UnderdogMax;

        // Enable Modifiers
        public static CustomHeaderOption Modifiers;
        public static CustomNumberOption TorchOn;
        public static CustomNumberOption DiseasedOn;
        public static CustomNumberOption FlashOn;
        public static CustomNumberOption TiebreakerOn;
        public static CustomNumberOption DrunkOn;
        public static CustomNumberOption BigBoiOn;
        public static CustomNumberOption LoversOn;


        // Custom Game Settings
        public static CustomHeaderOption CustomGameSettings;
        public static CustomToggleOption ColourblindComms;
        public static CustomToggleOption MeetingColourblind;
        public static CustomToggleOption ImpostorSeeRoles;
        public static CustomToggleOption DeadSeeRoles;
        public static CustomNumberOption MaxImpostorRoles;
        public static CustomNumberOption MaxNeutralRoles;
        public static CustomToggleOption RoleUnderName;
        public static CustomNumberOption VanillaGame;

        // Role Progression
        public static CustomTabOption RoleProgressionTab;
        public static CustomToggleOption RoleProgressionOn;
        public static CustomToggleOption RoleProgressionFlash;

        // Role Settings
        public static CustomTabOption RoleChance;

        // Mayor Settings
        public static CustomTabOption MayorTab;
        public static CustomNumberOption MayorVoteBank;
        public static CustomToggleOption MayorAnonymous;

        // Swapper Settings
        public static CustomTabOption SwapperTab;

        // Sheriff Settings
        public static CustomTabOption SheriffTab;
        public static CustomToggleOption ShowSheriff;
        public static CustomToggleOption SheriffFirstRoundOn;
        public static CustomToggleOption SheriffKillOther;
        public static CustomToggleOption SheriffKillsJester;
        public static CustomToggleOption SheriffKillsGlitch;
        public static CustomToggleOption SheriffKillsArsonist;
        public static CustomToggleOption SheriffKillsSheriff;
        public static CustomNumberOption SheriffKillCd;
        public static CustomToggleOption SheriffBodyReport;

        // Jester Settings
        public static CustomTabOption JesterTab;

        // Shifter Settings
        public static CustomTabOption ShifterTab;
        public static CustomNumberOption ShifterCd;
        public static CustomToggleOption ShifterCdResetAfterMeeting;
        public static CustomNumberOption ShifterDuration;
        public static CustomStringOption WhoShifts;

        // Engineer Settings
        public static CustomTabOption EngineerTab;
        public static CustomStringOption EngineerPer;

        // Investigator Settings
        public static CustomTabOption InvestigatorTab;
        public static CustomNumberOption FootprintSize;
        public static CustomNumberOption FootprintInterval;
        public static CustomNumberOption FootprintDuration;
        public static CustomToggleOption AnonymousFootPrint;
        public static CustomToggleOption VentFootprintVisible;

        // Timelord Settings
        public static CustomTabOption TimeLordTab;
        public static CustomToggleOption RewindRevive;
        public static CustomNumberOption RewindDuration;
        public static CustomNumberOption RewindCooldown;
        public static CustomToggleOption TimeLordVitals;

        // Medic Settings
        public static CustomTabOption MedicTab;
        public static CustomStringOption ShowShielded;
        public static CustomToggleOption MedicReportSwitch;
        public static CustomNumberOption MedicReportNameDuration;
        public static CustomNumberOption MedicReportColorDuration;
        public static CustomStringOption WhoGetsNotification;
        public static CustomToggleOption ShieldBreaks;

        // Seer Settings
        public static CustomTabOption SeerTab;
        public static CustomNumberOption SeerCooldown;
        public static CustomStringOption SeerInfo;
        public static CustomStringOption SeeReveal;
        public static CustomToggleOption NeutralRed;

        // Spy Settings
        public static CustomTabOption SpyTab;

        // Glitch Settings
        public static CustomTabOption GlitchTab;
        public static CustomNumberOption MimicCooldownOption;
        public static CustomNumberOption MimicDurationOption;
        public static CustomNumberOption HackCooldownOption;
        public static CustomNumberOption HackDurationOption;
        public static CustomNumberOption GlitchKillCooldownOption;
        public static CustomNumberOption InitialGlitchKillCooldownOption;
        public static CustomStringOption GlitchHackDistanceOption;

        // Morphling Settings
        public static CustomTabOption MorphlingTab;
        public static CustomNumberOption MorphlingCooldown;
        public static CustomNumberOption MorphlingDuration;

        // Camouflager Settings
        public static CustomTabOption CamouflagerTab;
        public static CustomNumberOption CamouflagerCooldown;
        public static CustomNumberOption CamouflagerDuration;

        // Executioner Settings
        public static CustomTabOption ExecutionerTab;
        public static CustomStringOption OnTargetDead;

        // Snitch Settings
        public static CustomTabOption SnitchTab;
        public static CustomToggleOption SnitchOnLaunch;
        public static CustomToggleOption SnitchSeesNeutrals;

        // Altruist Settings
        public static CustomTabOption AltruistTab;
        public static CustomNumberOption ReviveDuration;
        public static CustomToggleOption AltruistTargetBody;

        // Miner Settings
        public static CustomTabOption MinerTab;
        public static CustomNumberOption MineCooldown;

        // Swooper Settings
        public static CustomTabOption SwooperTab;
        public static CustomNumberOption SwoopCooldown;
        public static CustomNumberOption SwoopDuration;

        // Arsonist Settings
        public static CustomTabOption ArsonistTab;
        public static CustomNumberOption DouseCooldown;
        public static CustomToggleOption ArsonistGameEnd;

        // Underdog Settings
        public static CustomTabOption UnderdogTab;

        // Undertaker Settings
        public static CustomTabOption UndertakerTab;
        public static CustomNumberOption DragCooldown;

        // Assassin Settings
        public static CustomTabOption AssassinTab;
        public static CustomNumberOption AssassinKills;
        public static CustomToggleOption AssassinGuessNeutrals;
        public static CustomToggleOption AssassinCrewmateGuess;
        public static CustomToggleOption AssassinMultiKill;

        // Janitor Settings
        public static CustomTabOption JanitorTab;

        // Modifier Settings
        public static CustomTabOption ModifierChance;

        // Lover Settings
        public static CustomTabOption LoverTab;
        public static CustomToggleOption BothLoversDie;



        //Dividers
        public static CustomHeaderOption SubMenus;
        public static CustomHeaderOption ModifierDivider;
        public static CustomHeaderOption CrewmateDivider;
        public static CustomHeaderOption ImpostorDivider;
        public static CustomHeaderOption NeutralDivider;

        public static Func<object, string> PercentFormat { get; } = value => $"{value:0}%";
        private static Func<object, string> CooldownFormat { get; } = value => $"{value:0.0#}s";


        public static void GenerateAll()
        {
            var num = 0;

            Patches.ExportButton = 
                new Export(num++);
            Patches.ImportButton = 
                new Import(num++);

            CrewmateRoles = 
                new CustomHeaderOption(num++, "Crewmate Roles", "Custom");
            MayorOn = 
                new CustomNumberOption(true, num++, "<color=#704FA8FF>Mayor</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            SheriffOn = 
                new CustomNumberOption(true, num++, "<color=#FFFF00FF>Sheriff</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            EngineerOn = 
                new CustomNumberOption(true, num++, "<color=#FFA60AFF>Engineer</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            SwapperOn = 
                new CustomNumberOption(true, num++, "<color=#66E666FF>Swapper</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            InvestigatorOn = 
                new CustomNumberOption(true, num++, "<color=#00B3B3FF>Investigator</color>", 0f, 0f, 100f,
                10f, PercentFormat, "Custom");
            TimeLordOn = 
                new CustomNumberOption(true, num++, "<color=#0000FFFF>Time Lord</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            MedicOn = 
                new CustomNumberOption(true, num++, "<color=#006600FF>Medic</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            SeerOn = 
                new CustomNumberOption(true, num++, "<color=#FFCC80FF>Seer</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            SpyOn = 
                new CustomNumberOption(true, num++, "<color=#CCA3CCFF>Spy</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            SnitchOn = 
                new CustomNumberOption(true, num++, "<color=#D4AF37FF>Snitch</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            AltruistOn = 
                new CustomNumberOption(true, num++, "<color=#660000FF>Altruist</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");

            NeutralRoles = 
                new CustomHeaderOption(num++, "Neutral Roles", "Custom");
            JesterOn = 
                new CustomNumberOption(true, num++, "<color=#FFBFCCFF>Jester</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            ShifterOn = 
                new CustomNumberOption(true, num++, "<color=#999999FF>Shifter</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            GlitchOn = 
                new CustomNumberOption(true, num++, "<color=#00FF00FF>The Glitch</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            ExecutionerOn = 
                new CustomNumberOption(true, num++, "<color=#8C4005FF>Executioner</color>", 0f, 0f, 100f,
                10f, PercentFormat, "Custom");
            ArsonistOn = 
                new CustomNumberOption(true, num++, "<color=#FF4D00FF>Arsonist</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            PhantomOn = 
                new CustomNumberOption(true, num++, "<color=#662962>Phantom</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");

            ImpostorRoles = 
                new CustomHeaderOption(num++, "Impostor Roles", "Custom");
            AssassinOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Assassin</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            JanitorOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Janitor</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            MorphlingOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Morphling</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            CamouflagerOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Camouflager</color>", 0f, 0f, 100f,
                10f, PercentFormat, "Custom");
            MinerOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Miner</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            SwooperOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Swooper</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            UndertakerOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Undertaker</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            UnderdogOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Underdog</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");

            TorchOn = 
                new CustomNumberOption(true, num++, "<color=#FFFF99FF>Torch</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            DiseasedOn =
                new CustomNumberOption(true, num++, "<color=#808080FF>Diseased</color>", 0f, 0f, 100f, 10f,
                    PercentFormat, "Custom");
            FlashOn = 
                new CustomNumberOption(true, num++, "<color=#FF8080FF>Flash</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            TiebreakerOn = 
                new CustomNumberOption(true, num++, "<color=#99E699FF>Tiebreaker</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            DrunkOn = 
                new CustomNumberOption(true, num++, "<color=#758000FF>Drunk</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            BigBoiOn = 
                new CustomNumberOption(true, num++, "<color=#FF8080FF>Giant</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");
            ButtonBarryOn =
                new CustomNumberOption(true, num++, "<color=#E600FFFF>Button Barry</color>", 0f, 0f, 100f, 10f,
                    PercentFormat, "Custom");
            LoversOn = 
                new CustomNumberOption(true, num++, "<color=#FF66CCFF>Lovers</color>", 0f, 0f, 100f, 10f,
                PercentFormat, "Custom");


            CustomGameSettings =
                new CustomHeaderOption(num++, "Redux Settings", MenuLoader.ReduxMenuName);
            ColourblindComms = 
                new CustomToggleOption(num++, "Camouflaged Comms", false, MenuLoader.ReduxMenuName);
            MeetingColourblind = 
                new CustomToggleOption(num++, "Camouflaged Meetings", false, MenuLoader.ReduxMenuName);
            ImpostorSeeRoles = 
                new CustomToggleOption(num++, "Impostors can see the roles of their team", false, MenuLoader.ReduxMenuName);

            DeadSeeRoles =
                new CustomToggleOption(num++, "Dead can see everyone's roles", false, MenuLoader.ReduxMenuName);
            MaxImpostorRoles =
                new CustomNumberOption(num++, "Max Impostor Roles", 1f, 1f, 3f, 1f, null, MenuLoader.ReduxMenuName);
            MaxNeutralRoles =
                new CustomNumberOption(num++, "Max Neutral Roles", 1f, 1f, 5f, 1f, null, MenuLoader.ReduxMenuName);
            RoleUnderName = 
                new CustomToggleOption(num++, "Role Appears Under Name",true, MenuLoader.ReduxMenuName);
            VanillaGame = 
                new CustomNumberOption(num++, "Probability of a completely vanilla game", 0f, 0f, 100f, 5f,
                PercentFormat, MenuLoader.ReduxMenuName);

            SubMenus = new CustomHeaderOption(-1, "Menus", MenuLoader.ReduxMenuName);
            // Role Menu
            RoleChance = new CustomTabOption(num++, "<color=#e08bbd>Role Chances</color>", MenuLoader.ReduxMenuName);
            RoleChance.AddOptions(CrewmateRoles, MayorOn, SheriffOn, EngineerOn, SwapperOn, InvestigatorOn,
                TimeLordOn, MedicOn, SeerOn, SpyOn, SnitchOn, AltruistOn, NeutralRoles, JesterOn,
                ShifterOn, GlitchOn, ExecutionerOn, ArsonistOn, PhantomOn, ImpostorRoles, AssassinOn,
                JanitorOn, MorphlingOn, CamouflagerOn, MinerOn, SwooperOn, UnderdogOn, UndertakerOn);
            // RoleProgression Settings
            RoleProgressionTab = 
                new CustomTabOption(num++, "<color=#9803fc>Role Progression Settings</color>", MenuLoader.ReduxMenuName);
            RoleProgressionOn = 
                new CustomToggleOption(num++, "Role Progression", false, "Custom");
            RoleProgressionFlash =
                new CustomToggleOption(num++, "Flash on Upgrade", false, "Custom");
            RoleProgressionTab.AddOptions(RoleProgressionOn, RoleProgressionFlash);

            CrewmateDivider = new CustomHeaderOption(-1, "Crewmate Roles", MenuLoader.ReduxMenuName);
            // Mayor Settings
            MayorTab =
                 new CustomTabOption(num++, "<color=#704FA8FF>Mayor Settings</color>", MenuLoader.ReduxMenuName);
            MayorMax = 
                new CustomNumberOption(num++, "Max Mayors", 1, 1, 15, 1, null, "Custom");
            MayorVoteBank =
                new CustomNumberOption(num++, "Initial Mayor Vote Bank", 1, 1, 5, 1, null, "Custom");
            MayorAnonymous =
                new CustomToggleOption(num++, "Mayor Votes Show Anonymous", false, "Custom");
            MayorTab.AddOptions(MayorOn, MayorVoteBank, MayorAnonymous);


            // Sheriff Settings
            SheriffTab =
                new CustomTabOption(num++, "<color=#FFFF00FF>Sheriff Settings</color>", MenuLoader.ReduxMenuName);
            SheriffMax =
                new CustomNumberOption(num++, "Max Sheriffs", 1, 1, 15, 1, null, "Custom");
            ShowSheriff = 
                new CustomToggleOption(num++, "Show Sheriff", false, "Custom");
            SheriffFirstRoundOn =
                new CustomToggleOption(num++, "Sheriff Can Kill First Round", true, "Custom");
            SheriffKillOther =
                new CustomToggleOption(num++, "Sheriff Miskill Kills Crewmate", false, "Custom");
            SheriffKillsSheriff =
                new CustomToggleOption(num++, "Sheriff Kills Sheriff", false, "Custom");
            SheriffKillsJester =
                new CustomToggleOption(num++, "Sheriff Kills Jester", false, "Custom");
            SheriffKillsGlitch =
                new CustomToggleOption(num++, "Sheriff Kills The Glitch", false, "Custom");
            SheriffKillsArsonist =
                new CustomToggleOption(num++, "Sheriff Kills Arsonist", false, "Custom");
            SheriffKillCd =
                new CustomNumberOption(num++, "Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat, "Custom");
            SheriffBodyReport = 
                new CustomToggleOption(num++, "Sheriff can report who they've killed", true, "Custom");
            SheriffTab.AddOptions(SheriffOn, SheriffMax, ShowSheriff, SheriffFirstRoundOn,
                SheriffKillOther, SheriffKillsSheriff, SheriffKillsJester, SheriffKillsGlitch, SheriffKillsArsonist, SheriffKillCd);

            // Swapper Options
            SwapperTab = new CustomTabOption(num++, "<color=#66E666FF>Swapper Settings</color>", MenuLoader.ReduxMenuName);
            SwapperMax =
                new CustomNumberOption(num++, "Max Swappers", 1, 1, 1, 1, null, "Custom");
            SwapperTab.AddOptions(SwapperOn, SwapperMax);
                
            // Engineer Options
            EngineerTab =
                new CustomTabOption(num++, "<color=#FFA60AFF>Engineer Settings</color>", MenuLoader.ReduxMenuName);
            EngineerMax =
                new CustomNumberOption(num++, "Max Engineers", 1, 1, 15, 1, null, "Custom");
            EngineerPer =
                new CustomStringOption(num++, "Engineer Fix Per", new[] {"Round", "Game"}, "Custom");
            EngineerTab.AddOptions(EngineerOn, EngineerMax, EngineerPer);

            // Investigator Options
            InvestigatorTab =
                new CustomTabOption(num++, "<color=#00B3B3FF>Investigator Settings</color>", MenuLoader.ReduxMenuName);
            InvestigatorMax =
                new CustomNumberOption(num++, "Max Investigators", 1, 1, 15, 1, null, "Custom");
            FootprintSize = new CustomNumberOption(num++, "Footprint Size", 4f, 1f, 10f, 1f, null, "Custom");
            FootprintInterval =
                new CustomNumberOption(num++, "Footprint Interval", 1f, 0.25f, 5f, 0.25f, CooldownFormat, "Custom");
            FootprintDuration = 
                new CustomNumberOption(num++, "Footprint Duration", 10f, 1f, 10f, 0.5f, CooldownFormat, "Custom");
            AnonymousFootPrint = 
                new CustomToggleOption(num++, "Anonymous Footprint", false, "Custom");
            VentFootprintVisible = 
                new CustomToggleOption(num++, "Footprint Vent Visible", false, "Custom");
            InvestigatorTab.AddOptions(InvestigatorOn, InvestigatorMax, FootprintSize, FootprintInterval,
                FootprintDuration, AnonymousFootPrint, VentFootprintVisible);

            // Timelord Options
            TimeLordTab =
                new CustomTabOption(num++, "<color=#0000FFFF>Time Lord Settings</color>", MenuLoader.ReduxMenuName);
            TimeLordMax =
                new CustomNumberOption(num++, "Max Timelords", 1, 1, 1, 1, null, "Custom");
            RewindRevive = 
                new CustomToggleOption(num++, "Revive During Rewind", false, "Custom");
            RewindDuration = 
                new CustomNumberOption(num++, "Rewind Duration", 3f, 3f, 15f, 0.5f, CooldownFormat, "Custom");
            RewindCooldown = 
                new CustomNumberOption(num++, "Rewind Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat, "Custom");
            TimeLordVitals =
                new CustomToggleOption(num++, "Time Lord can use Vitals", false, "Custom");
            TimeLordTab.AddOptions(TimeLordOn, TimeLordMax, RewindRevive, RewindDuration, RewindCooldown, TimeLordVitals);

            // Medic Options
            MedicTab =
                new CustomTabOption(num++, "<color=#006600FF>Medic Settings</color>", MenuLoader.ReduxMenuName);
            MedicMax =
                new CustomNumberOption(num++, "Max Medics", 1, 1, 15, 1, null, "Custom");
            ShowShielded =
                new CustomStringOption(num++, "Show Shielded Player",
                    new[] {"Self", "Medic", "Self+Medic", "Everyone"}, "Custom");
            MedicReportSwitch = 
                new CustomToggleOption(num++, "Show Medic Reports", true, "Custom");
            MedicReportNameDuration =
                new CustomNumberOption(num++, "Time Where Medic Reports Will Have Name", 0, 0, 60, 2.5f,
                    CooldownFormat, "Custom");
            MedicReportColorDuration =
                new CustomNumberOption(num++, "Time Where Medic Reports Will Have Color Type", 15, 0, 120, 2.5f,
                    CooldownFormat, "Custom");
            WhoGetsNotification =
                new CustomStringOption(num++, "Who gets murder attempt indicator",
                    new[] {"Medic", "Shielded", "Everyone", "Nobody"}, "Custom");
            ShieldBreaks = new CustomToggleOption(num++, "Shield breaks on murder attempt", false, "Custom");
            MedicTab.AddOptions(MedicOn, MedicMax, ShowShielded, MedicReportSwitch, MedicReportNameDuration,
                MedicReportColorDuration, WhoGetsNotification);

            // Seer Settings
            SeerTab =
                new CustomTabOption(num++, "<color=#FFCC80FF>Seer Settings</color>", MenuLoader.ReduxMenuName);
            SeerMax =
                new CustomNumberOption(num++, "Max Seers", 1, 1, 15, 1, null, "Custom");
            SeerCooldown =
                new CustomNumberOption(num++, "Seer Cooldown", 25f, 10f, 100f, 2.5f, CooldownFormat, "Custom");
            SeerInfo =
                new CustomStringOption(num++, "Info that Seer sees", new[] {"Role", "Team"}, "Custom");
            SeeReveal =
                new CustomStringOption(num++, "Who Sees That They Are Revealed",
                    new[] {"Crew", "Imps+Neut", "All", "Nobody"}, "Custom");
            NeutralRed =
                new CustomToggleOption(num++, "Neutrals show up as Impostors", false, "Custom");
            SeerTab.AddOptions(SeerOn, SeerMax, SeerCooldown, SeerInfo, SeeReveal, NeutralRed);

            // Snitch Settings
            SnitchTab = 
                new CustomTabOption(num++, "<color=#D4AF37FF>Snitch Settings</color>", MenuLoader.ReduxMenuName);
            SnitchMax =
                new CustomNumberOption(num++, "Max Snitches", 1, 1, 15, 1, null, "Custom");
            SnitchOnLaunch =
                new CustomToggleOption(num++, "Snitch knows who they are on Game Start", false, "Custom");
            SnitchSeesNeutrals = new CustomToggleOption(num++, "Snitch sees neutral roles", false, "Custom");
            SnitchTab.AddOptions(SnitchOn, SnitchMax, SnitchOnLaunch, SnitchSeesNeutrals);

            // Spy Settings
            SpyTab =
                new CustomTabOption(num++, "<color=#CCA3CCFF>Spy Settings</color>", MenuLoader.ReduxMenuName);
            SpyMax =
                new CustomNumberOption(num++, "Max Spies", 1, 1, 15, 1, null, "Custom");
            SpyTab.AddOptions(SpyOn, SpyMax);

            // Altruist Settings
            AltruistTab = 
                new CustomTabOption(num++, "<color=#660000FF>Altruist Settings</color>", MenuLoader.ReduxMenuName);
            AltruistMax =
                new CustomNumberOption(num++, "Max Altruists", 1, 1, 15, 1, null, "Custom");
            ReviveDuration =
                new CustomNumberOption(num++, "Altruist Revive Duration", 10, 1, 30, 1f, CooldownFormat.Invoke,"Custom");
            AltruistTargetBody =
                new CustomToggleOption(num++, "Target's body disappears on beginning of revive", false, "Custom");
            AltruistTab.AddOptions(AltruistOn, AltruistMax, ReviveDuration, AltruistTargetBody);

            NeutralDivider = new CustomHeaderOption(-1, "Neutral Roles", MenuLoader.ReduxMenuName);
            // Jester Settings
            JesterTab =
                new CustomTabOption(num++, "<color=#FFBFCCFF>Jester Settings</color>", MenuLoader.ReduxMenuName);
            JesterMax = 
                new CustomNumberOption(num++, "Max Jesters", 1, 1, 15, 1, null, "Custom");
            JesterTab.AddOptions(JesterOn, JesterMax);

            // Shifter Settings
            ShifterTab =
                new CustomTabOption(num++, "<color=#999999FF>Shifter Settings</color>", MenuLoader.ReduxMenuName);
            ShifterMax =
                new CustomNumberOption(num++, "Max Shifters", 1, 1, 15, 1, null, "Custom");
            ShifterCd =
                new CustomNumberOption(num++, "Shifter Cooldown", 60f, 10f, 180f, 5f, CooldownFormat, "Custom");
            ShifterCdResetAfterMeeting =
                new CustomToggleOption(num++, "Shifter Cooldown Reset After Meeting", false, "Custom");
            ShifterDuration =
                new CustomNumberOption(num++, "Shift Delay", 10f, 0f, 60f, 2.5f, CooldownFormat, "Custom");
            WhoShifts = new CustomStringOption(num++,
                "Who gets the Shifter role on Shift", new[] {"NoImps", "RegCrew", "Nobody"}, "Custom");
            ShifterTab.AddOptions(ShifterOn, ShifterMax, ShifterCd,
                ShifterDuration, ShifterCdResetAfterMeeting, WhoShifts);

            // Gltich Settings
            GlitchTab =
                new CustomTabOption(num++, "<color=#00FF00FF>The Glitch Settings</color>", MenuLoader.ReduxMenuName);
            GlitchMax =
                new CustomNumberOption(num++, "Max Glitches", 1, 1, 15, 1, null, "Custom");
            MimicCooldownOption = 
                new CustomNumberOption(num++, "Mimic Cooldown", 30, 10, 120, 2.5f, CooldownFormat, "Custom");
            MimicDurationOption = 
                new CustomNumberOption(num++, "Mimic Duration", 10, 1, 30, 1f, CooldownFormat, "Custom");
            HackCooldownOption = 
                new CustomNumberOption(num++, "Hack Cooldown", 30, 10, 120, 2.5f, CooldownFormat, "Custom");
            HackDurationOption = 
                new CustomNumberOption(num++, "Hack Duration", 10, 1, 30, 1f, CooldownFormat, "Custom");
            GlitchKillCooldownOption =
                new CustomNumberOption(num++, "Glitch Kill Cooldown", 30, 10, 120, 2.5f, CooldownFormat, "Custom");
            InitialGlitchKillCooldownOption =
                new CustomNumberOption(num++, "Initial Glitch Kill Cooldown", 10, 10, 120, 2.5f, CooldownFormat, "Custom");
            GlitchHackDistanceOption =
                new CustomStringOption(num++, "Glitch Hack Distance", new[] {"Short", "Normal", "Long"}, "Custom");
            GlitchTab.AddOptions(GlitchOn, GlitchMax, MimicCooldownOption, MimicDurationOption, HackCooldownOption, HackDurationOption,
                GlitchKillCooldownOption, InitialGlitchKillCooldownOption, GlitchHackDistanceOption);

            // Executioner Settings
            ExecutionerTab =
                new CustomTabOption(num++, "<color=#8C4005FF>Executioner Settings</color>", MenuLoader.ReduxMenuName);
            ExecutionerMax =
                new CustomNumberOption(num++, "Max Executioners", 1, 1, 15, 1,null, "Custom");
            OnTargetDead = 
                new CustomStringOption(num++, "Executioner becomes on Target Dead",
                    new[] {"Crew", "Jester"}, "Custom");
            ExecutionerTab.AddOptions(ExecutionerOn, ExecutionerMax, OnTargetDead);

            // Arsonist Settings
            ArsonistTab = 
                new CustomTabOption(num++, "<color=#FF4D00FF>Arsonist Settings</color>", MenuLoader.ReduxMenuName);
            ArsonistMax =
                new CustomNumberOption(num++, "Max Arsonists", 1, 1, 15, 1, null, "Custom");
            DouseCooldown =
                new CustomNumberOption(num++, "Douse Cooldown", 25, 10, 40, 2.5f, CooldownFormat, "Custom");
            ArsonistGameEnd = 
                new CustomToggleOption(num++, "Game keeps going so long as Arsonist is alive", false, "Custom");
            ArsonistTab.AddOptions(ArsonistOn, ArsonistMax, DouseCooldown, ArsonistGameEnd);

            ImpostorDivider = new CustomHeaderOption(-1, "Impostor Roles", MenuLoader.ReduxMenuName);
            // Morphling Settings
            MorphlingTab =
                new CustomTabOption(num++, "<color=#FF0000FF>Morphling Settings</color>", MenuLoader.ReduxMenuName);
            MorphlingMax =
                 new CustomNumberOption(num++, "Max Morphlings", 1, 1, 3, 1, null, "Custom");
            MorphlingCooldown =
                new CustomNumberOption(num++, "Morphling Cooldown", 25, 10, 40, 2.5f, CooldownFormat, "Custom");
            MorphlingDuration =
                new CustomNumberOption(num++, "Morphling Duration", 10, 5, 15, 1f, CooldownFormat, "Custom");
            MorphlingTab.AddOptions(MorphlingOn, MorphlingMax, MorphlingCooldown, MorphlingDuration);

            // Camouflager Settings
            CamouflagerTab =
                new CustomTabOption(num++, "<color=#FF0000FF>Camouflager Settings</color>", MenuLoader.ReduxMenuName);
            CamouflagerMax =
                new CustomNumberOption(num++, "Max Camouflagers", 1, 1, 3, 1, null, "Custom");
            CamouflagerCooldown =
                new CustomNumberOption(num++, "Camouflager Cooldown", 25, 10, 40, 2.5f, CooldownFormat, "Custom");
            CamouflagerDuration =
                new CustomNumberOption(num++, "Camouflager Duration", 10, 5, 15, 1f, CooldownFormat, "Custom");
            CamouflagerTab.AddOptions(CamouflagerOn, CamouflagerMax, CamouflagerCooldown, CamouflagerDuration);

            // Janitor Settings
            JanitorTab =
                new CustomTabOption(num++, "<color=#FF0000FF>Janitor Settings</color>", MenuLoader.ReduxMenuName);
            JanitorMax =
                new CustomNumberOption(num++, "Max Janitors", 1, 1, 3, 1, null, "Custom");
            JanitorTab.AddOptions(JanitorOn, JanitorMax);

            // Miner Settings
            MinerTab = 
                new CustomTabOption(num++, "<color=#FF0000FF>Miner Settings</color>", MenuLoader.ReduxMenuName);
            MinerMax =
                new CustomNumberOption(num++, "Max Miners", 1, 1, 3, 1, null, "Custom");
            MineCooldown =
                new CustomNumberOption(num++, "Mine Cooldown", 25, 10, 40, 2.5f, CooldownFormat, "Custom");
            MinerTab.AddOptions(MinerOn, MinerMax, MineCooldown);

            // Swooper Settings
            SwooperTab = 
                new CustomTabOption(num++, "<color=#FF0000FF>Swooper Settings</color>", MenuLoader.ReduxMenuName);
            SwooperMax =
                new CustomNumberOption(num++, "Max Swoopers", 1, 1, 3, 1, null, "Custom");
            SwoopCooldown =
                new CustomNumberOption(num++, "Swoop Cooldown", 25, 10, 40, 2.5f, CooldownFormat, "Custom");
            SwoopDuration =
                new CustomNumberOption(num++, "Swoop Duration", 10, 5, 15, 1f, CooldownFormat, "Custom");
            SwooperTab.AddOptions(SwooperOn, SwooperMax, SwoopCooldown, SwoopDuration);

            // Underdog Settings
            UnderdogTab =
                new CustomTabOption(num++, "<color=#FF0000FF>Underdog Settings</color>", MenuLoader.ReduxMenuName);
            UnderdogMax =
                new CustomNumberOption(num++, "Max Underdogs", 1, 1, 3, 1, null, "Custom");
            UnderdogTab.AddOptions(UnderdogOn, UnderdogMax);

            // Undertaker Settings
            UndertakerTab = new CustomTabOption(num++, "<color=#FF0000FF>Undertaker Settings</color>", MenuLoader.ReduxMenuName);
            UndertakerMax =
                new CustomNumberOption(num++, "Max Undertakers", 1, 1, 3, 1, null, "Custom");
            DragCooldown = new CustomNumberOption(num++, "Drag Cooldown", 25, 10, 40, 2.5f, CooldownFormat, "Custom");
            UndertakerTab.AddOptions(UndertakerOn, UndertakerMax, DragCooldown);

            // Assassin Settings
            AssassinTab = 
                new CustomTabOption(num++, "<color=#FF0000FF>Assassin Settings</color>", MenuLoader.ReduxMenuName);
            AssassinMax =
                new CustomNumberOption(num++, "Max Assassins", 1, 1, 3, 1, null, "Custom");
            AssassinKills = 
                new CustomNumberOption(num++, "Number of Assassin Kills", 1, 1, 15, 1, null, "Custom");
            AssassinCrewmateGuess = 
                new CustomToggleOption(num++, "Assassin can Guess \"Crewmate\"", false, "Custom");
            AssassinGuessNeutrals = 
                new CustomToggleOption(num++, "Assassin can Guess Neutral roles", false, "Custom");
            AssassinMultiKill = 
                new CustomToggleOption(num++, "Assassin can kill more than once per meeting", true, "Custom");
            AssassinTab.AddOptions(AssassinOn, AssassinMax, AssassinKills, AssassinCrewmateGuess,
                AssassinGuessNeutrals, AssassinMultiKill);

            ModifierDivider = new CustomHeaderOption(-1, "Modifiers", MenuLoader.ReduxMenuName);
            ModifierChance = new CustomTabOption(num++, "Modifier Chances", MenuLoader.ReduxMenuName);
            ModifierChance.AddOptions(LoversOn, ButtonBarryOn, FlashOn, BigBoiOn,
                DiseasedOn, TorchOn, TiebreakerOn, DrunkOn);
            // Lover Settings
            LoverTab =
                new CustomTabOption(num++, "<color=#FF66CCFF>Lovers Settings</color>", MenuLoader.ReduxMenuName);
            BothLoversDie =
                new CustomToggleOption(num++, "Both Lovers Die", true, "Custom");
            LoverTab.AddOptions(LoversOn, BothLoversDie);
        }
    }
}
