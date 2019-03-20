using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Handlers
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
                switch (facing.Direction)
                {
                    case Direction2d.Up:
                        sprite.AnimatedSprite = isMoving ? "WalkNorth" : "IdleNorth";
                        break;
                    case Direction2d.Right:
                        sprite.AnimatedSprite = isMoving ? "WalkEast" : "IdleEast";
                        break;
                    case Direction2d.Down:
                        sprite.AnimatedSprite = isMoving ? "WalkSouth" : "IdleSouth";
                        break;
                    case Direction2d.Left:
                        sprite.AnimatedSprite = isMoving ? "WalkWest" : "IdleWest";
                        break;
                }
            }
        }
    }
}
