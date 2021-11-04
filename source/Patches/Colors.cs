﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfUs.Patches
{
    class Colors {

        // Crew Colors
        public readonly static Color Crewmate = Color.white;
        public readonly static Color Mayor = new Color(0.44f, 0.31f, 0.66f, 1f);
        public readonly static Color Sheriff = Color.yellow;
        public readonly static Color Engineer = new Color(1f, 0.65f, 0.04f, 1f);
        public readonly static Color Swapper = new Color(0.4f, 0.9f, 0.4f, 1f);
        public readonly static Color Investigator = new Color(0f, 0.7f, 0.7f, 1f);
        public readonly static Color TimeLord = new Color(0f, 0f, 1f, 1f);
        public readonly static Color Lovers = new Color(1f, 0.4f, 0.8f, 1f);
        public readonly static Color Medic = new Color(0f, 0.4f, 0f, 1f);
        public readonly static Color Seer = new Color(1f, 0.8f, 0.5f, 1f);
        public readonly static Color Spy = new Color(0.8f, 0.64f, 0.8f, 1f);
        public readonly static Color Snitch = new Color(0.83f, 0.69f, 0.22f, 1f);
        public readonly static Color Altruist = new Color(0.4f, 0f, 0f, 1f);

        // Neutral Colors
        public readonly static Color Jester = new Color(1f, 0.75f, 0.8f, 1f);
        public readonly static Color Shifter = new Color(0.6f, 0.6f, 0.6f, 1f);
        public readonly static Color Executioner = new Color(0.55f, 0.25f, 0.02f, 1f);
        public readonly static Color Glitch = Color.green;
        public readonly static Color Arsonist = new Color(1f, 0.3f, 0f);
        public readonly static Color Phantom = new Color(0.4f, 0.16f, 0.38f, 1f);

        //Imposter Colors
        public readonly static Color Impostor = Palette.ImpostorRed;

    }
}
