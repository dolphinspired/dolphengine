using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Input.Controls;
using DolphEngine.MonoGame.Eco.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolphEngine.Demo.Handlers
{
    public class PlayerHandler : EcosystemHandler<PlayerComponent>
    {
        private static readonly List<int> IdleNorth =  new List<int> { 2 };
        private static readonly List<int> IdleEast =   new List<int> { 11 };
        private static readonly List<int> IdleSouth =  new List<int> { 17 };
        private static readonly List<int> IdleWest =   new List<int> { 27 };
        private static readonly List<int> WalkNorth =  new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        private static readonly List<int> WalkEast =   new List<int> { 8, 9, 10, 11, 12, 13 };
        private static readonly List<int> WalkSouth =  new List<int> { 16, 17, 18, 19, 20, 21, 22 };
        private static readonly List<int> WalkWest =   new List<int> { 24, 25, 26, 27, 28, 29 };

        public override void Update(Entity entity)
        {
            var player = entity.GetComponent<PlayerComponent>();

            if (entity.TryGetComponent<AnimatedSpriteComponent>(out var anim))
            {
                List<int> sequence;
                if ((player.Direction & Direction.Up) > 0)
                {
                    sequence = player.Speed == 0 ? IdleNorth : WalkNorth;
                }
                else if ((player.Direction & Direction.Right) > 0)
                {
                    sequence = player.Speed == 0 ? IdleEast : WalkEast;
                }
                else if ((player.Direction & Direction.Left) > 0)
                {
                    sequence = player.Speed == 0 ? IdleWest : WalkWest;
                }
                else
                {
                    sequence = player.Speed == 0 ? IdleSouth : WalkSouth;
                }
                anim.Sequence = sequence;
            }

            if (entity.TryGetComponent<PositionComponent2d>(out var position) && player.Speed > 0)
            {
                if ((player.Direction & Direction.Up) > 0)
                {
                    position.Y -= player.Speed;
                }
                if ((player.Direction & Direction.Right) > 0)
                {
                    position.X += player.Speed;
                }
                if ((player.Direction & Direction.Left) > 0)
                {
                    position.X -= player.Speed;
                }
                if ((player.Direction & Direction.Down) > 0)
                {
                    position.Y += player.Speed;
                }
            }
        }
    }
}
