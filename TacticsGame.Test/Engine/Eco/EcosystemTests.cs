using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Engine.Eco;
using Xunit;

namespace TacticsGame.Test.Engine.Eco
{
    public class EcosystemTests
    {
        #region Handler add/remove tests

        [Fact]
        public void CanAddHandlerToEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();

            ecosystem
                .AddHandler(handler)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
        }

        [Fact]
        public void CanAddMultipleHandlersToEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .RunAllHandlers();

            Assert.Equal(1, handler1.Called);
            Assert.Equal(1, handler2.Called);
        }

        [Fact]
        public void CanAddMultipleHandlersToEcosystemShorthand()
        {
            var ecosystem = new Ecosystem().AddHandler<MockHandler1>().AddHandler<MockHandler2>();

            ecosystem.RunAllHandlers();
        }

        [Fact]
        public void CannotAddNullHandlerToEcosystem()
        {
            var ecosystem = new Ecosystem();

            Assert.Throws<ArgumentNullException>(() => ecosystem.AddHandler(null));
        }

        [Fact]
        public void CannotAddDuplicateHandlerToEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler1 = new MockHandler1();

            ecosystem.AddHandler(handler1);

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler1));
        }

        [Fact]
        public void CannotAddHandlerWithoutSubscriptionsToEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockUnsubscribedHandler();

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler));
            handler.SubscribesTo = new List<Type>();
            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler));
        }

        [Fact]
        public void CannotSubscribeHandlerToNonComponentType()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockUnsubscribedHandler();
            handler.SubscribesTo = new[] { typeof(MockComponent1), typeof(EcosystemTests) };

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler));
        }

        [Fact]
        public void CannotAddEmptyHandlerListToEcosystem()
        {
            var ecosystem = new Ecosystem();

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandlers());
            Assert.Throws<ArgumentException>(() => ecosystem.AddHandlers(new List<IEcosystemHandler>()));
        }

        [Fact]
        public void CanRemoveHandlerFromEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();

            ecosystem
                .AddHandler(handler)
                .RemoveHandler(handler)
                .RunAllHandlers();

            Assert.Equal(0, handler.Called);
        }

        [Fact]
        public void CanRemoveMultipleHandlersFromEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .RemoveHandlers(handler1, handler2)
                .RunAllHandlers();

            Assert.Equal(0, handler1.Called);
            Assert.Equal(0, handler2.Called);
        }

        [Fact]
        public void CanClearHandlersFromEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .ClearHandlers()
                .RunAllHandlers();

            Assert.Equal(0, handler1.Called);
            Assert.Equal(0, handler2.Called);
        }

        [Fact]
        public void CanAddHandlerToEcosystemWithEntities()
        {
            var ecosystem = CreateEcosystemWithEntities();
            var handler = new MockHandler1();

            ecosystem
                .AddHandler(handler)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
        }

        [Fact]
        public void CanRemoveHandlerFromEcosystemWithEntities()
        {
            var ecosystem = CreateEcosystemWithEntities();
            var handler = new MockHandler1();

            ecosystem
                .AddHandler(handler)
                .RemoveHandler(handler)
                .RunAllHandlers();

            Assert.Equal(0, handler.Called);
        }

        [Fact]
        public void CanClearHandlersFromEcosystemWithEntities()
        {
            var ecosystem = CreateEcosystemWithEntities();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .ClearHandlers()
                .RunAllHandlers();

            Assert.Equal(0, handler1.Called);
            Assert.Equal(0, handler2.Called);
        }

        #endregion

        #region Entity add/remove tests

        [Fact]
        public void CanAddEntityToEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddEntity(entity)
                .AddHandler(handler)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanAddEntityWithComponentsToEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddEntity(entity)
                .AddHandler(handler)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Equal(entity, handler.EntitiesHandled.Single());
        }

        [Fact]
        public void CanRemoveEntityFromEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddEntity(entity)
                .RemoveEntity(entity)
                .AddHandler(handler)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanRemoveEntityWithComponentsFromEcosystem()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddEntity(entity)
                .RemoveEntity(entity)
                .AddHandler(handler)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanAddEntityToEcosystemWithHandlers()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanAddEntityWithComponentsToEcosystemWithHandlers()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Equal(entity, handler.EntitiesHandled.Single());
        }

        [Fact]
        public void CanRemoveEntityFromEcosystemWithHandlers()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .RemoveEntity(entity)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanRemoveEntityWithComponentsFromEcosystemWithHandlers()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .RemoveEntity(entity)
                .RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        #endregion

        #region Components added-to/removed-from entities

        [Fact]
        public void CanAddComponentToEntityInEcosystem()
        {
            var ecosystem = new Ecosystem();
            var entity = new Entity();

            ecosystem.AddEntity(entity);

            var entitiesWithComponent = ecosystem.GetEntitiesWithComponent<MockComponent1>();            
            Assert.Empty(entitiesWithComponent);

            entity.AddComponent<MockComponent1>();

            entitiesWithComponent = ecosystem.GetEntitiesWithComponent<MockComponent1>();
            Assert.Equal(entity, entitiesWithComponent.Single());
        }

        [Fact]
        public void CanRemoveComponentFromEntityInEcosystem()
        {
            var ecosystem = new Ecosystem();
            var entity = new Entity();

            ecosystem.AddEntity(entity);
            entity.AddComponent<MockComponent1>().RemoveComponent<MockComponent1>();

            var entitiesWithComponent = ecosystem.GetEntitiesWithComponent<MockComponent1>();
            Assert.Empty(entitiesWithComponent);
        }

        [Fact]
        public void CanAddComponentToEntityInEcosystemWithHandlers()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity);
            entity.AddComponent<MockComponent1>();
            ecosystem.RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Equal(entity, handler.EntitiesHandled.Single());
        }

        [Fact]
        public void CanRemoveComponentFromEntityInEcosystemWithHandlers()
        {
            var ecosystem = new Ecosystem();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity);
            entity.AddComponent<MockComponent1>().RemoveComponent<MockComponent1>();
            ecosystem.RunAllHandlers();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        #endregion

        #region Private methods

        private static Ecosystem CreateEcosystemWithEntities()
        {
            var ecosystem = new Ecosystem();
            ecosystem.AddEntities(
                new Entity("empty-1"),
                new Entity("empty-2"),
                new Entity("comp1-1").AddComponent(new MockComponent1("c1-e1", 1)),
                new Entity("comp1-2").AddComponent(new MockComponent1("c1-e2", 2)),
                new Entity("comp2-1").AddComponent(new MockComponent2("c2-e3", true)),
                new Entity("comp2-2").AddComponent(new MockComponent2("c2-e4", false)),
                new Entity("cmp12-1").AddComponent(new MockComponent1("c1-e5", 5)).AddComponent(new MockComponent2("c2-e5", true)),
                new Entity("cmp12-2").AddComponent(new MockComponent1("c1-e6", 6)).AddComponent(new MockComponent2("c2-e6", false))
            );
            return ecosystem;
        }

        #endregion
    }
}