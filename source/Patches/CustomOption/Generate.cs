using System;

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
        public static CustomHeaderOption RoleProgression;
        public static CustomToggleOption RoleProgressionOn;
        public static CustomToggleOption RoleProgressionFlash;

        // Mayor Settings
        public static CustomHeaderOption Mayor;
        public static CustomNumberOption MayorVoteBank;
        public static CustomToggleOption MayorAnonymous;

        // Swapper Settings
        public static CustomHeaderOption Swapper;

        // Lover Settings
        public static CustomHeaderOption Lovers;
        public static CustomToggleOption BothLoversDie;

        // Sheriff Settings
        public static CustomHeaderOption Sheriff;
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
        public static CustomHeaderOption Jester;

        // Shifter Settings
        public static CustomHeaderOption Shifter;
        public static CustomNumberOption ShifterCd;
        public static CustomStringOption WhoShifts;

        // Engineer Settings
        public static CustomHeaderOption Engineer;
        public static CustomStringOption EngineerPer;

        // Investigator Settings
        public static CustomHeaderOption Investigator;
        public static CustomNumberOption FootprintSize;
        public static CustomNumberOption FootprintInterval;
        public static CustomNumberOption FootprintDuration;
        public static CustomToggleOption AnonymousFootPrint;
        public static CustomToggleOption VentFootprintVisible;

        // Timelord Settings
        public static CustomHeaderOption TimeLord;
        public static CustomToggleOption RewindRevive;
        public static CustomNumberOption RewindDuration;
        public static CustomNumberOption RewindCooldown;
        public static CustomToggleOption TimeLordVitals;

        // Medic Settings
        public static CustomHeaderOption Medic;
        public static CustomStringOption ShowShielded;
        public static CustomToggleOption MedicReportSwitch;
        public static CustomNumberOption MedicReportNameDuration;
        public static CustomNumberOption MedicReportColorDuration;
        public static CustomStringOption WhoGetsNotification;
        public static CustomToggleOption ShieldBreaks;

        // Seer Settings
        public static CustomHeaderOption Seer;
        public static CustomNumberOption SeerCooldown;
        public static CustomStringOption SeerInfo;
        public static CustomStringOption SeeReveal;
        public static CustomToggleOption NeutralRed;

        // Spy Settings
        public static CustomHeaderOption Spy;

        // Glitch Settings
        public static CustomHeaderOption TheGlitch;
        public static CustomNumberOption MimicCooldownOption;
        public static CustomNumberOption MimicDurationOption;
        public static CustomNumberOption HackCooldownOption;
        public static CustomNumberOption HackDurationOption;
        public static CustomNumberOption GlitchKillCooldownOption;
        public static CustomNumberOption InitialGlitchKillCooldownOption;
        public static CustomStringOption GlitchHackDistanceOption;

        // Morphling Settings
        public static CustomHeaderOption Morphling;
        public static CustomNumberOption MorphlingCooldown;
        public static CustomNumberOption MorphlingDuration;

        // Camouflager Settings
        public static CustomHeaderOption Camouflager;
        public static CustomNumberOption CamouflagerCooldown;
        public static CustomNumberOption CamouflagerDuration;

        // Executioner Settings
        public static CustomHeaderOption Executioner;
        public static CustomStringOption OnTargetDead;

        // Snitch Settings
        public static CustomHeaderOption Snitch;
        public static CustomToggleOption SnitchOnLaunch;
        public static CustomToggleOption SnitchSeesNeutrals;

        // Altruist Settings
        public static CustomHeaderOption Altruist;
        public static CustomNumberOption ReviveDuration;
        public static CustomToggleOption AltruistTargetBody;

        // Miner Settings
        public static CustomHeaderOption Miner;
        public static CustomNumberOption MineCooldown;

        // Swooper Settings
        public static CustomHeaderOption Swooper;
        public static CustomNumberOption SwoopCooldown;
        public static CustomNumberOption SwoopDuration;

        // Arsonist Settings
        public static CustomHeaderOption Arsonist;
        public static CustomNumberOption DouseCooldown;
        public static CustomToggleOption ArsonistGameEnd;

        // Underdog Settings
        public static CustomHeaderOption Underdog;

        // Undertaker Settings
        public static CustomHeaderOption Undertaker;
        public static CustomNumberOption DragCooldown;

        // Assassin Settings
        public static CustomHeaderOption Assassin;
        public static CustomNumberOption AssassinKills;
        public static CustomToggleOption AssassinGuessNeutrals;
        public static CustomToggleOption AssassinCrewmateGuess;
        public static CustomToggleOption AssassinMultiKill;

        // Janitor Settings
        public static CustomHeaderOption Janitor;

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
                new CustomHeaderOption(num++, "Crewmate Roles");
            MayorOn = 
                new CustomNumberOption(true, num++, "<color=#704FA8FF>Mayor</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            SheriffOn = 
                new CustomNumberOption(true, num++, "<color=#FFFF00FF>Sheriff</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            EngineerOn = 
                new CustomNumberOption(true, num++, "<color=#FFA60AFF>Engineer</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            SwapperOn = 
                new CustomNumberOption(true, num++, "<color=#66E666FF>Swapper</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            InvestigatorOn = 
                new CustomNumberOption(true, num++, "<color=#00B3B3FF>Investigator</color>", 0f, 0f, 100f,
                10f, PercentFormat);
            TimeLordOn = 
                new CustomNumberOption(true, num++, "<color=#0000FFFF>Time Lord</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            MedicOn = 
                new CustomNumberOption(true, num++, "<color=#006600FF>Medic</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            SeerOn = 
                new CustomNumberOption(true, num++, "<color=#FFCC80FF>Seer</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            SpyOn = 
                new CustomNumberOption(true, num++, "<color=#CCA3CCFF>Spy</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            SnitchOn = 
                new CustomNumberOption(true, num++, "<color=#D4AF37FF>Snitch</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            AltruistOn = 
                new CustomNumberOption(true, num++, "<color=#660000FF>Altruist</color>", 0f, 0f, 100f, 10f,
                PercentFormat);

            NeutralRoles = 
                new CustomHeaderOption(num++, "Neutral Roles");
            JesterOn = 
                new CustomNumberOption(true, num++, "<color=#FFBFCCFF>Jester</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            ShifterOn = 
                new CustomNumberOption(true, num++, "<color=#999999FF>Shifter</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            GlitchOn = 
                new CustomNumberOption(true, num++, "<color=#00FF00FF>The Glitch</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            ExecutionerOn = 
                new CustomNumberOption(true, num++, "<color=#8C4005FF>Executioner</color>", 0f, 0f, 100f,
                10f, PercentFormat);
            ArsonistOn = 
                new CustomNumberOption(true, num++, "<color=#FF4D00FF>Arsonist</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            PhantomOn = 
                new CustomNumberOption(true, num++, "<color=#662962>Phantom</color>", 0f, 0f, 100f, 10f,
                PercentFormat);

            ImpostorRoles = 
                new CustomHeaderOption(num++, "Impostor Roles");
            AssassinOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Assassin</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            JanitorOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Janitor</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            MorphlingOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Morphling</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            CamouflagerOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Camouflager</color>", 0f, 0f, 100f,
                10f, PercentFormat);
            MinerOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Miner</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            SwooperOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Swooper</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            UndertakerOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Undertaker</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            UnderdogOn = 
                new CustomNumberOption(true, num++, "<color=#FF0000FF>Underdog</color>", 0f, 0f, 100f, 10f,
                PercentFormat);

            // Modifiers
            Modifiers = 
                new CustomHeaderOption(num++, "Modifiers");
            TorchOn = 
                new CustomNumberOption(true, num++, "<color=#FFFF99FF>Torch</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            DiseasedOn =
                new CustomNumberOption(true, num++, "<color=#808080FF>Diseased</color>", 0f, 0f, 100f, 10f,
                    PercentFormat);
            FlashOn = 
                new CustomNumberOption(true, num++, "<color=#FF8080FF>Flash</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            TiebreakerOn = 
                new CustomNumberOption(true, num++, "<color=#99E699FF>Tiebreaker</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            DrunkOn = 
                new CustomNumberOption(true, num++, "<color=#758000FF>Drunk</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            BigBoiOn = 
                new CustomNumberOption(true, num++, "<color=#FF8080FF>Giant</color>", 0f, 0f, 100f, 10f,
                PercentFormat);
            ButtonBarryOn =
                new CustomNumberOption(true, num++, "<color=#E600FFFF>Button Barry</color>", 0f, 0f, 100f, 10f,
                    PercentFormat);
            LoversOn = 
                new CustomNumberOption(true, num++, "<color=#FF66CCFF>Lovers</color>", 0f, 0f, 100f, 10f,
                PercentFormat);


            CustomGameSettings =
                new CustomHeaderOption(num++, "Custom Game Settings");
            ColourblindComms = 
                new CustomToggleOption(num++, "Camouflaged Comms", false);
            MeetingColourblind = 
                new CustomToggleOption(num++, "Camouflaged Meetings", false);
            ImpostorSeeRoles = 
                new CustomToggleOption(num++, "Impostors can see the roles of their team", false);

            DeadSeeRoles =
                new CustomToggleOption(num++, "Dead can see everyone's roles", false);
            MaxImpostorRoles =
                new CustomNumberOption(num++, "Max Impostor Roles", 1f, 1f, 3f, 1f);
            MaxNeutralRoles =
                new CustomNumberOption(num++, "Max Neutral Roles", 1f, 1f, 5f, 1f);
            RoleUnderName = 
                new CustomToggleOption(num++, "Role Appears Under Name");
            VanillaGame = 
                new CustomNumberOption(num++, "Probability of a completely vanilla game", 0f, 0f, 100f, 5f,
                PercentFormat);

            // RoleProgression Settings
            RoleProgression = 
                new CustomHeaderOption(num++, "Role Progression Settings");
            RoleProgressionOn = 
                new CustomToggleOption(num++, "Role Progression", false);
            RoleProgressionFlash =
                new CustomToggleOption(num++, "Flash on Upgrade", false);


            // Lover Settings
            Lovers =
                new CustomHeaderOption(num++, "<color=#FF66CCFF>Lovers</color>");
            BothLoversDie = new CustomToggleOption(num++, "Both Lovers Die");

            // Mayor Settings
            Mayor = 
                new CustomHeaderOption(num++, "<color=#704FA8FF>Mayor</color>");
            MayorMax = 
                new CustomNumberOption(num++, "Max Mayors", 1, 1, 15, 1);
            MayorVoteBank =
                new CustomNumberOption(num++, "Initial Mayor Vote Bank", 1, 1, 5, 1);
            MayorAnonymous =
                new CustomToggleOption(num++, "Mayor Votes Show Anonymous", false);

            // Sheriff Settings
            Sheriff =
                new CustomHeaderOption(num++, "<color=#FFFF00FF>Sheriff</color>");
            SheriffMax =
                new CustomNumberOption(num++, "Max Sheriffs", 1, 1, 15, 1);
            ShowSheriff = 
                new CustomToggleOption(num++, "Show Sheriff", false);
            SheriffFirstRoundOn =
                new CustomToggleOption(num++, "Sheriff Can Kill First Round", true);
            SheriffKillOther =
                new CustomToggleOption(num++, "Sheriff Miskill Kills Crewmate", false);
            SheriffKillsSheriff =
                new CustomToggleOption(num++, "Sheriff Kills Sheriff", false);
            SheriffKillsJester =
                new CustomToggleOption(num++, "Sheriff Kills Jester", false);
            SheriffKillsGlitch =
                new CustomToggleOption(num++, "Sheriff Kills The Glitch", false);
            SheriffKillsArsonist =
                new CustomToggleOption(num++, "Sheriff Kills Arsonist", false);
            SheriffKillCd =
                new CustomNumberOption(num++, "Sheriff Kill Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat);
            SheriffBodyReport = new CustomToggleOption(num++, "Sheriff can report who they've killed");

            // Swapper Options
            Swapper =
                new CustomHeaderOption(num++, "<color=#66E666FF>Swapper</color>");
            SwapperMax =
                new CustomNumberOption(num++, "Max Swappers", 1, 1, 15, 1);

            // Engineer Options
            Engineer =
                new CustomHeaderOption(num++, "<color=#FFA60AFF>Engineer</color>");
            EngineerMax =
                new CustomNumberOption(num++, "Max Engineers", 1, 1, 15, 1);
            EngineerPer =
                new CustomStringOption(num++, "Engineer Fix Per", new[] {"Round", "Game"});

            // Investigator Options
            Investigator =
                new CustomHeaderOption(num++, "<color=#00B3B3FF>Investigator</color>");
            InvestigatorMax =
                new CustomNumberOption(num++, "Max Investigators", 1, 1, 15, 1);
            FootprintSize = new CustomNumberOption(num++, "Footprint Size", 4f, 1f, 10f, 1f);
            FootprintInterval =
                new CustomNumberOption(num++, "Footprint Interval", 1f, 0.25f, 5f, 0.25f, CooldownFormat);
            FootprintDuration = 
                new CustomNumberOption(num++, "Footprint Duration", 10f, 1f, 10f, 0.5f, CooldownFormat);
            AnonymousFootPrint = 
                new CustomToggleOption(num++, "Anonymous Footprint", false);
            VentFootprintVisible = 
                new CustomToggleOption(num++, "Footprint Vent Visible", false);

            // Timelord Options
            TimeLord =
                new CustomHeaderOption(num++, "<color=#0000FFFF>Time Lord</color>");
            TimeLordMax =
                new CustomNumberOption(num++, "Max Timelords", 1, 1, 15, 1);
            RewindRevive = 
                new CustomToggleOption(num++, "Revive During Rewind", false);
            RewindDuration = 
                new CustomNumberOption(num++, "Rewind Duration", 3f, 3f, 15f, 0.5f, CooldownFormat);
            RewindCooldown = 
                new CustomNumberOption(num++, "Rewind Cooldown", 25f, 10f, 40f, 2.5f, CooldownFormat);
            TimeLordVitals =
                new CustomToggleOption(num++, "Time Lord can use Vitals", false);

            // Medic Options
            Medic =
                new CustomHeaderOption(num++, "<color=#006600FF>Medic</color>");
            MedicMax =
                new CustomNumberOption(num++, "Max Medics", 1, 1, 15, 1);
            ShowShielded =
                new CustomStringOption(num++, "Show Shielded Player",
                    new[] {"Self", "Medic", "Self+Medic", "Everyone"});
            MedicReportSwitch = 
                new CustomToggleOption(num++, "Show Medic Reports");
            MedicReportNameDuration =
                new CustomNumberOption(num++, "Time Where Medic Reports Will Have Name", 0, 0, 60, 2.5f,
                    CooldownFormat);
            MedicReportColorDuration =
                new CustomNumberOption(num++, "Time Where Medic Reports Will Have Color Type", 15, 0, 120, 2.5f,
                    CooldownFormat);
            WhoGetsNotification =
                new CustomStringOption(num++, "Who gets murder attempt indicator",
                    new[] {"Medic", "Shielded", "Everyone", "Nobody"});
            ShieldBreaks = new CustomToggleOption(num++, "Shield breaks on murder attempt", false);

            // Seer Settings
            Seer =
                new CustomHeaderOption(num++, "<color=#FFCC80FF>Seer</color>");
            SeerMax =
                new CustomNumberOption(num++, "Max Seers", 1, 1, 15, 1);
            SeerCooldown =
                new CustomNumberOption(num++, "Seer Cooldown", 25f, 10f, 100f, 2.5f, CooldownFormat);
            SeerInfo =
                new CustomStringOption(num++, "Info that Seer sees", new[] {"Role", "Team"});
            SeeReveal =
                new CustomStringOption(num++, "Who Sees That They Are Revealed",
                    new[] {"Crew", "Imps+Neut", "All", "Nobody"});
            NeutralRed =
                new CustomToggleOption(num++, "Neutrals show up as Impostors", false);

            // Snitch Settings
            Snitch = 
                new CustomHeaderOption(num++, "<color=#D4AF37FF>Snitch</color>");
            SnitchMax =
                new CustomNumberOption(num++, "Max Snitches", 1, 1, 15, 1);
            SnitchOnLaunch =
                new CustomToggleOption(num++, "Snitch knows who they are on Game Start", false);
            SnitchSeesNeutrals = new CustomToggleOption(num++, "Snitch sees neutral roles", false);

            // Spy Settings
            Spy =
                new CustomHeaderOption(num++, "<color=#CCA3CCFF>Spy</color>");
            SpyMax =
                new CustomNumberOption(num++, "Max Spies", 1, 1, 15, 1);

            // Altruist Settings
            Altruist = 
                new CustomHeaderOption(num++, "<color=#660000FF>Altruist</color>");
            AltruistMax =
                new CustomNumberOption(num++, "Max Altruists", 1, 1, 15, 1);
            ReviveDuration =
                new CustomNumberOption(num++, "Altruist Revive Duration", 10, 1, 30, 1f, CooldownFormat);
            AltruistTargetBody =
                new CustomToggleOption(num++, "Target's body disappears on beginning of revive", false);

            // Jester Settings
            Jester =
                new CustomHeaderOption(num++, "<color=#FFBFCCFF>Jester</color>");
            JesterMax = 
                new CustomNumberOption(num++, "Max Jesters", 1, 1, 15, 1);

            // Shifter Settings
            Shifter =
                new CustomHeaderOption(num++, "<color=#999999FF>Shifter</color>");
            ShifterMax =
                new CustomNumberOption(num++, "Max Shifters", 1, 1, 15, 1);
            ShifterCd =
                new CustomNumberOption(num++, "Shifter Cooldown", 30f, 10f, 60f, 2.5f, CooldownFormat);
            WhoShifts = new CustomStringOption(num++,
                "Who gets the Shifter role on Shift", new[] {"NoImps", "RegCrew", "Nobody"});

            // Gltich Settings
            TheGlitch =
                new CustomHeaderOption(num++, "<color=#00FF00FF>The Glitch</color>");
            GlitchMax =
                new CustomNumberOption(num++, "Max Glitches", 1, 1, 15, 1);
            MimicCooldownOption = 
                new CustomNumberOption(num++, "Mimic Cooldown", 30, 10, 120, 2.5f, CooldownFormat);
            MimicDurationOption = 
                new CustomNumberOption(num++, "Mimic Duration", 10, 1, 30, 1f, CooldownFormat);
            HackCooldownOption = 
                new CustomNumberOption(num++, "Hack Cooldown", 30, 10, 120, 2.5f, CooldownFormat);
            HackDurationOption = 
                new CustomNumberOption(num++, "Hack Duration", 10, 1, 30, 1f, CooldownFormat);
            GlitchKillCooldownOption =
                new CustomNumberOption(num++, "Glitch Kill Cooldown", 30, 10, 120, 2.5f, CooldownFormat);
            InitialGlitchKillCooldownOption =
                new CustomNumberOption(num++, "Initial Glitch Kill Cooldown", 10, 10, 120, 2.5f, CooldownFormat);
            GlitchHackDistanceOption =
                new CustomStringOption(num++, "Glitch Hack Distance", new[] {"Short", "Normal", "Long"});

            // Executioner Settings
            Executioner =
                new CustomHeaderOption(num++, "<color=#8C4005FF>Executioner</color>");
            ExecutionerMax =
                new CustomNumberOption(num++, "Max Executioners", 1, 1, 15, 1);
            OnTargetDead = 
                new CustomStringOption(num++, "Executioner becomes on Target Dead",
                    new[] {"Crew", "Jester"});

            // Arsonist Settings
            Arsonist = 
                new CustomHeaderOption(num++, "<color=#FF4D00FF>Arsonist</color>");
            ArsonistMax =
                new CustomNumberOption(num++, "Max Arsonists", 1, 1, 15, 1);
            DouseCooldown =
                new CustomNumberOption(num++, "Douse Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
            ArsonistGameEnd = 
                new CustomToggleOption(num++, "Game keeps going so long as Arsonist is alive", false);

            // Morphling Settings
            Morphling =
                new CustomHeaderOption(num++, "<color=#FF0000FF>Morphling</color>");
            MorphlingMax =
                 new CustomNumberOption(num++, "Max Morphlings", 1, 1, 3, 1);
            MorphlingCooldown =
                new CustomNumberOption(num++, "Morphling Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
            MorphlingDuration =
                new CustomNumberOption(num++, "Morphling Duration", 10, 5, 15, 1f, CooldownFormat);

            // Camouflager Settings
            Camouflager =
                new CustomHeaderOption(num++, "<color=#FF0000FF>Camouflager</color>");
            CamouflagerMax =
                new CustomNumberOption(num++, "Max Camouflagers", 1, 1, 3, 1);
            CamouflagerCooldown =
                new CustomNumberOption(num++, "Camouflager Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
            CamouflagerDuration =
                new CustomNumberOption(num++, "Camouflager Duration", 10, 5, 15, 1f, CooldownFormat);

            // Janitor Settings
            Janitor =
                new CustomHeaderOption(num++, "<color=#FF0000FF>Janitor</color>");
            JanitorMax =
                new CustomNumberOption(num++, "Max Janitors", 1, 1, 3, 1);

            // Miner Settings
            Miner = 
                new CustomHeaderOption(num++, "<color=#FF0000FF>Miner</color>");
            MinerMax =
                new CustomNumberOption(num++, "Max Miners", 1, 1, 3, 1);
            MineCooldown =
                new CustomNumberOption(num++, "Mine Cooldown", 25, 10, 40, 2.5f, CooldownFormat);

            // Swooper Settings
            Swooper = 
                new CustomHeaderOption(num++, "<color=#FF0000FF>Swooper</color>");
            SwooperMax =
                new CustomNumberOption(num++, "Max Swoopers", 1, 1, 3, 1);
            SwoopCooldown =
                new CustomNumberOption(num++, "Swoop Cooldown", 25, 10, 40, 2.5f, CooldownFormat);
            SwoopDuration =
                new CustomNumberOption(num++, "Swoop Duration", 10, 5, 15, 1f, CooldownFormat);

            // Underdog Settings
            Underdog =
                new CustomHeaderOption(num++, "<color=#FF0000FF>Swooper</color>");
            UnderdogMax =
                new CustomNumberOption(num++, "Max Underdogs", 1, 1, 3, 1);

            // Undertaker Settings
            Undertaker = new CustomHeaderOption(num++, "<color=#FF0000FF>Undertaker</color>");
            UndertakerMax =
                new CustomNumberOption(num++, "Max Undertakers", 1, 1, 3, 1);
            DragCooldown = new CustomNumberOption(num++, "Drag Cooldown", 25, 10, 40, 2.5f, CooldownFormat);

            // Assassin Settings
            Assassin = 
                new CustomHeaderOption(num++, "<color=#FF0000FF>Assassin</color>");
            AssassinMax =
                new CustomNumberOption(num++, "Max Assassins", 1, 1, 3, 1);
            AssassinKills = 
                new CustomNumberOption(num++, "Number of Assassin Kills", 1, 1, 5, 1);
            AssassinCrewmateGuess = 
                new CustomToggleOption(num++, "Assassin can Guess \"Crewmate\"", false);
            AssassinGuessNeutrals = 
                new CustomToggleOption(num++, "Assassin can Guess Neutral roles", false);
            AssassinMultiKill = 
                new CustomToggleOption(num++, "Assassin can kill more than once per meeting");
        }
    }
}