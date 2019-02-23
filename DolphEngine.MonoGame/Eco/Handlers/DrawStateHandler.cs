using DolphEngine.Eco;
using DolphEngine.MonoGame.Eco.Components;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class DrawStateHandler : EcosystemHandler<DrawStateComponent>
    {
        public override void Draw(Entity entity)
        {
            var state = entity.GetComponent<DrawStateComponent>();

            if (entity.TryGetComponent<AtlasSpriteComponent>(out var atlas))
            {
                if (state.FrameStates != null && state.FrameStates.TryGetValue(state.State, out var frame))
                {
                    // Set the current atlas frame to the one mapped to the entity's current state
                    atlas.CurrentFrame = frame;
                }
            }

            if (entity.TryGetComponent<AnimatedSpriteComponent>(out var anim))
            {
                if (state.SequenceStates != null && state.SequenceStates.TryGetValue(state.State, out var sequence))
                {
                    // The current animation sequence to the one mapped to the entity's current state
                    anim.Sequence = sequence;
                }
            }
        }
    }
}
