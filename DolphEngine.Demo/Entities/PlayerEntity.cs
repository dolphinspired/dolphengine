using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using System.Collections.Generic;

namespace DolphEngine.Demo.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity() : base("Player")
        {
            this.AddComponent<PositionComponent2d>()
                .AddComponent<SpeedComponent2d>()
                .AddComponent<DrawComponent>()
                .AddComponent<AnimatedSpriteComponent>()
                .AddComponent<DrawStateComponent>();

            this.Animation.DurationPerFrame = 100;
            this.DrawState.SequenceStates = new Dictionary<int, List<int>>
            {
                { (int)PlayerDrawStates.IdleNorth,  new List<int> { 6 } },
                { (int)PlayerDrawStates.IdleEast,   new List<int> { 0 } },
                { (int)PlayerDrawStates.IdleSouth,  new List<int> { 12 } },
                { (int)PlayerDrawStates.IdleWest,   new List<int> { 18 } },
                { (int)PlayerDrawStates.WalkNorth,  new List<int> { 7, 6, 8, 6 } },
                { (int)PlayerDrawStates.WalkEast,   new List<int> { 1, 0, 2, 0 } },
                { (int)PlayerDrawStates.WalkSouth,  new List<int> { 13, 12, 14, 12 } },
                { (int)PlayerDrawStates.WalkWest,   new List<int> { 19, 18, 20, 18 } }
            };
        }

        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        public SpeedComponent2d Speed => this.GetComponent<SpeedComponent2d>();

        public AnimatedSpriteComponent Animation => this.GetComponent<AnimatedSpriteComponent>();

        public DrawStateComponent DrawState => this.GetComponent<DrawStateComponent>();
    }

    public enum PlayerDrawStates
    {
        IdleNorth,
        IdleEast,
        IdleSouth,
        IdleWest,
        WalkNorth,
        WalkEast,
        WalkSouth,
        WalkWest
    }
}
