using Reactor;
using Reactor.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfUs.Patches
{
    public class Arrow
    {
        protected ArrowBehaviour arrowBehaviour { set;  get; }
        private System.Object target;
        public static Sprite Sprite => TownOfUs.Arrow;

        public Arrow()
        {

        }

        private void CreateArrow()
        {
            if (arrowBehaviour != null)
                arrowBehaviour.gameObject.Destroy();
            var gameObj = new GameObject();
            arrowBehaviour = gameObj.AddComponent<ArrowBehaviour>();
            gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
            var renderer = gameObj.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprite;
            arrowBehaviour.image = renderer;
            gameObj.layer = 5;
        }

        public void SetTargetPosition(Vector3 target)
        {
            arrowBehaviour.target = target;
        }

        public void SetTarget(PlayerControl player) {
            target = player;
            CreateArrow();
            update();
        }

        public void SetTarget(DeadBody body) {
            target = body;
            CreateArrow();
            update();
        }

        public void Destroy()
        {
            arrowBehaviour.gameObject.Destroy();
            arrowBehaviour = null;
        }

        public PlayerControl GetPlayerTarget()
        {
            if (target == null) return null;
            if (target.GetType() == typeof(PlayerControl))
                return (PlayerControl)target;
            return null;
        }

        public DeadBody GetDeadBodyTarget()
        {
            if (target == null) return null;
            if (target.GetType() == typeof(DeadBody))
                return (DeadBody)target;
            return null;
        }

        public void update()
        {
            if (target == null) return;
            try {
                if (target.GetType() == typeof(PlayerControl)) {
                    var player = (PlayerControl)target;
                    if (player.Data.IsDead) {
                        Destroy();
                    }
                    SetTargetPosition(player.transform.position);
                } else if (target.GetType() == typeof(DeadBody)) {
                    var body = (DeadBody)target;
                    SetTargetPosition(body.transform.position);
                }
            } catch {
                target = null;
            }
        }

       
    }
}
