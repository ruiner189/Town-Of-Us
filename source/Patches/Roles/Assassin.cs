﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUs.Patches;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Assassin : Role
    {
        public Dictionary<byte, (GameObject, GameObject, TMP_Text)> Buttons = new Dictionary<byte, (GameObject, GameObject, TMP_Text)>();


        private Dictionary<string, Color> ColorMapping = new Dictionary<string, Color>();

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new Dictionary<byte, string>();


        public Assassin(PlayerControl player) : base(player)
        {
            Name = "Assassin";
            ImpostorText = () => "Kill during meetings if you can guess their roles";
            TaskText = () => "Guess the roles of the people and kill them mid-meeting";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Assassin;
            Faction = Faction.Impostors;

            RemainingKills = CustomGameOptions.AssassinKills;

            // Adds all the roles that have a non-zero chance of being in the game.
            if (CustomGameOptions.MayorOn > 0) ColorMapping.Add("Mayor", Colors.Mayor);
            if (CustomGameOptions.SheriffOn > 0) ColorMapping.Add("Sheriff", Colors.Sheriff);
            if (CustomGameOptions.EngineerOn > 0) ColorMapping.Add("Engineer", Colors.Engineer);
            if (CustomGameOptions.SwapperOn > 0) ColorMapping.Add("Swapper", Colors.Swapper);
            if (CustomGameOptions.InvestigatorOn > 0) ColorMapping.Add("Investigator", Colors.Investigator);
            if (CustomGameOptions.TimeLordOn > 0) ColorMapping.Add("Time Lord", Colors.TimeLord);
           // if (CustomGameOptions.LoversOn > 0) ColorMapping.Add("Lover", Colors.Lovers);
            if (CustomGameOptions.MedicOn > 0) ColorMapping.Add("Medic", Colors.Medic);
            if (CustomGameOptions.SeerOn > 0) ColorMapping.Add("Seer", Colors.Seer);
            if (CustomGameOptions.SpyOn > 0) ColorMapping.Add("Spy", Colors.Spy);
            if (CustomGameOptions.SnitchOn > 0) ColorMapping.Add("Snitch", Colors.Snitch);
            if (CustomGameOptions.AltruistOn > 0) ColorMapping.Add("Altruist", Colors.Altruist);

            // Add Neutral roles if enabled
            if (CustomGameOptions.AssassinGuessNeutrals)
            {
                if(CustomGameOptions.JesterOn > 0) ColorMapping.Add("Jester", Colors.Jester);
                if(CustomGameOptions.ShifterOn > 0) ColorMapping.Add("Shifter", Colors.Shifter);
                if(CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("Executioner", Colors.Executioner);
                if(CustomGameOptions.GlitchOn > 0) ColorMapping.Add("The Glitch", Colors.Glitch);
                if(CustomGameOptions.ArsonistOn > 0) ColorMapping.Add("Arsonist", Colors.Arsonist);
            }

            // Add vanilla crewmate if enabled
            if (CustomGameOptions.AssassinCrewmateGuess) ColorMapping.Add("Crewmate", Colors.Crewmate);

            // Sorts the list alphabetically. 
            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            GenerateKillButton();
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();
    }
}
