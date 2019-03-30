using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Animations;

namespace DolphEngine.Demo.Games.TestMap.Handlers
{
    public class SpriteStateHandler : EcosystemHandler<SpriteComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();

            var isMoving = false;
            if (entity.TryGetComponent<SpeedComponent>(out var speed) && (speed.X != 0 || speed.Y != 0))
            {
                isMoving = true;
            }

            if (entity.TryGetComponent<FacingComponent>(out var facing))
            {
                string animName;

                switch (facing.Direction)
                {
                    case Direction2d.Up:
                        animName = isMoving ? "WalkNorth" : "IdleNorth";
                        break;
                    case Direction2d.Right:
                        animName = isMoving ? "WalkEast" : "IdleEast";
                        break;
                    case Direction2d.Down:
                        animName = isMoving ? "WalkSouth" : "IdleSouth";
                        break;
                    default: // Left
                        animName = isMoving ? "WalkWest" : "IdleWest";
                        break;
                }

                sprite.SpriteAnimation = Animations.Player.GetAnimation<SpritesheetAnimation>(animName);
            }
        }
    }
}
