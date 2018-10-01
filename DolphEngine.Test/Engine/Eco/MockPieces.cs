using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Engine.Eco;
using Xunit;

namespace TacticsGame.Test.Engine.Eco
{
    public class MockComponent1 : Component
    {
        public MockComponent1() { }

        public MockComponent1(string name, int amount)
        {
            this.Name = name;
            this.Amount = amount;
        }

        public string Name { get; set; }

        public int Amount { get; set; }

        public override string ToString()
        {
            return $"{{ {nameof(Name)}: {Name}, {nameof(Amount)}: {Amount} }}";
        }
    }

    public class MockComponent2 : Component
    {
        public MockComponent2() { }

        public MockComponent2(string category, bool active)
        {
            this.Category = category;
            this.Active = active;
        }

        public string Category { get; set; }

        public bool Active { get; set; }

        public override string ToString()
        {
            return $"{{ {nameof(Category)}: {Category}, {nameof(Active)}: {Active} }}";
        }
    }
    
    public class MockHandler1 : IEcosystemHandler
    {
        public IEnumerable<Type> SubscribesTo => new[] 
        {
            typeof(MockComponent1)
        };

        public void Handle(IEnumerable<Entity> entities)
        {
            this.Called++;
            this.EntitiesHandled = entities;

            foreach (var entity in entities)
            {
                Assert.True(entity.HasComponent<MockComponent1>());
            }
        }

        public int Called;

        public IEnumerable<Entity> EntitiesHandled;
    }
    
    public class MockHandler2 : IEcosystemHandler
    {
        public IEnumerable<Type> SubscribesTo => new[]
        {
            typeof(MockComponent2)
        };

        public void Handle(IEnumerable<Entity> entities)
        {
            this.Called++;
            this.EntitiesHandled = entities;

            foreach (var entity in entities)
            {
                Assert.True(entity.HasComponent<MockComponent2>());
            }
        }

        public int Called;

        public IEnumerable<Entity> EntitiesHandled;
    }
    
    public class MockHandler3 : IEcosystemHandler
    {
        public IEnumerable<Type> SubscribesTo => new[]
        {
            typeof(MockComponent1),
            typeof(MockComponent2)
        };

        public void Handle(IEnumerable<Entity> entities)
        {
            this.Called++;
            this.EntitiesHandled = entities;

            foreach (var entity in entities)
            {
                Assert.True(entity.HasAllComponents(typeof(MockComponent1), typeof(MockComponent2)));
            }
        }

        public int Called;

        public IEnumerable<Entity> EntitiesHandled;
    }

    public class MockUnsubscribedHandler : IEcosystemHandler
    {
        public IEnumerable<Type> SubscribesTo { get; set; }

        public void Handle(IEnumerable<Entity> entities)
        {
            throw new Exception("You should not get here, this handler should not be subscribed to anything!");
        }
    }
}