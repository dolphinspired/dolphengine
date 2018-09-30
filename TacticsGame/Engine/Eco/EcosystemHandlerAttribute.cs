using System;

namespace TacticsGame.Engine.Eco
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EcosystemHandlerAttribute : Attribute
    {
        public readonly Type[] Types;

        public EcosystemHandlerAttribute(params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                throw new ArgumentException($"No types were specified for {nameof(EcosystemHandlerAttribute)}!");
            }

            foreach (var type in types)
            {
                if (!type.IsAssignableFrom(typeof(Component)))
                {
                    throw new ArgumentException($"Cannot subscribe to type {type.Name}: Does not inherit from type {typeof(Component)}");
                }
            }            

            this.Types = types;
        }
    }
}
