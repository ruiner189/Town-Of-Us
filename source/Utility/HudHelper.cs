using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfUs.Utility
{
    class HudHelper
    {
        public static float Width { get; private set; }
        public static float Height { get; private set; }
        public static Vector2 BottomLeft { get; private set; }
        public static Vector2 BottomRight { get; private set; }
        public static Vector2 TopLeft { get; private set; }
        public static Vector2 TopRight { get; private set; }

        public readonly static Vector2 DefaultAspect = new Vector2(1360, 768);

        public static void UpdateHudAlignment()
        {
            BottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)) - Camera.main.transform.localPosition;
            BottomRight = new Vector2(-BottomLeft.x, BottomLeft.y);
            TopLeft = new Vector2(BottomLeft.x, -BottomLeft.y);
            TopRight = new Vector2(-BottomLeft.x, -BottomLeft.y);

            Width = -BottomLeft.x * 2;
            Height = -BottomLeft.y * 2;
        }

        public static Vector2 GetAlignment(HudAlignment alignment)
        {
            UpdateHudAlignment();
            switch (alignment)
            {
                case HudAlignment.BottomLeft:
                    return BottomLeft;
                case HudAlignment.BottomRight:
                    return BottomRight;
                case HudAlignment.TopLeft:
                    return TopLeft;
                case HudAlignment.TopRight:
                    return TopRight;
            }

            return new Vector2();
        }

        public static Vector2 OffsetRatio()
        {
            Vector2 currentAspect = new Vector2(Screen.width, Screen.height);
            return currentAspect / DefaultAspect;
        }

        public static Vector2 Offset(Vector2 position)
        {

            return OffsetRatio() * position;
        }
    }
}
