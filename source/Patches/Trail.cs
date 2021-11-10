using Reactor;
using Reactor.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;

namespace TownOfUs.Patches
{
    public class Trail
    {
        private LineRenderer Line;
        private readonly PlayerControl Player;
        public int MaxSize = 300;
        private int Index = 0;
        private bool IsFull = false;
        private List<Vector3> Positions;
        private float lastUpdate;

        public Trail(PlayerControl player)
        {
            Player = player;
            CreateTrail();
            Positions = new List<Vector3>();
        }
        public void CreateTrail()
        {
            lastUpdate = Time.time;
            if (Line != null)
                Line.gameObject.Destroy();
            var playerColor = Palette.PlayerColors[Player.Data.DefaultOutfit.ColorId];
            var gameObj = new GameObject();
            gameObj.layer = 4;
            Line = gameObj.AddComponent<LineRenderer>();
            gameObj.transform.parent = Player.gameObject.transform;
            Line.SetMaterial(new Material(Player.MyRend.material.shader));
            Line.startColor = new Color(playerColor.r / 255f, playerColor.g / 255f, playerColor.b / 255f, 0.1f);
            Line.endColor = new Color(playerColor.r / 255f, playerColor.g / 255f, playerColor.b / 255f, 0.5f);
            Line.endWidth = 0.5f;
            Line.startWidth = 0.25f;
            Line.enabled = true;
            Line.positionCount = MaxSize;
            Line.numCornerVertices = 20;
            Line.numCapVertices = 20;
        }

        public void AddPoint()
        {
            lastUpdate = Time.time;
            var playerColor = Palette.PlayerColors[Player.Data.DefaultOutfit.ColorId];
            Line.startColor = new Color(playerColor.r / 255f, playerColor.g /255f, playerColor.b /255f, 0.1f);
            Line.endColor = new Color(playerColor.r / 255f, playerColor.g / 255f, playerColor.b / 255f, 0.5f);

            var position = Player.gameObject.transform.position;
            var pos = new Vector3(position.x, position.y, position.z);

           Positions.Add(pos);

            if (Positions.Count > MaxSize)
            {
                Positions.RemoveAt(0);
            }

            Line.SetPositions(Positions.ToArray());
        }
    }


}
