using System;
using System.Collections.Generic;
using System.Linq;
using DolphEngine.Eco;
using Xunit;

namespace DolphEngine.Test.Eco
{
    public class EcosystemTests
    {
        #region Handler add/remove tests

        [Fact]
        public void CanAddHandlerToEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();

            ecosystem
                .AddHandler(handler)
                .Update();

            Assert.Equal(1, handler.Called);
        }

        [Fact]
        public void CanAddMultipleHandlersToEcosystem()
        {
            var ecosystem = TestEco();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .Update();

            Assert.Equal(1, handler1.Called);
            Assert.Equal(1, handler2.Called);
        }

        [Fact]
        public void CanAddMultipleHandlersToEcosystemShorthand()
        {
            var ecosystem = TestEco().AddHandler<MockHandler1>().AddHandler<MockHandler2>();

            ecosystem.Update();
        }

        [Fact]
        public void CannotAddNullHandlerToEcosystem()
        {
            var ecosystem = TestEco();

            Assert.Throws<ArgumentNullException>(() => ecosystem.AddHandler(null));
        }

        [Fact]
        public void CannotAddDuplicateHandlerToEcosystem()
        {
            var ecosystem = TestEco();
            var handler1 = new MockHandler1();

            ecosystem.AddHandler(handler1);

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler1));
        }

        [Fact]
        public void CannotAddHandlerWithoutSubscriptionsToEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockUnsubscribedHandler();

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler));
            handler.SetSubscribedTypes(new List<Type>());
            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler));
        }

        [Fact]
        public void CannotSubscribeHandlerToNonComponentType()
        {
            var ecosystem = TestEco();
            var handler = new MockUnsubscribedHandler();
            handler.SetSubscribedTypes(new[] { typeof(MockComponent1), typeof(EcosystemTests) });

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandler(handler));
        }

        [Fact]
        public void CannotAddEmptyHandlerListToEcosystem()
        {
            var ecosystem = TestEco();

            Assert.Throws<ArgumentException>(() => ecosystem.AddHandlers());
            Assert.Throws<ArgumentException>(() => ecosystem.AddHandlers(new List<EcosystemHandler>()));
        }

        [Fact]
        public void CanRemoveHandlerFromEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();

            ecosystem
                .AddHandler(handler)
                .RemoveHandler(handler)
                .Update();

            Assert.Equal(0, handler.Called);
        }

        [Fact]
        public void CanRemoveMultipleHandlersFromEcosystem()
        {
            var ecosystem = TestEco();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .RemoveHandlers(handler1, handler2)
                .Update();

            Assert.Equal(0, handler1.Called);
            Assert.Equal(0, handler2.Called);
        }

        [Fact]
        public void CanClearHandlersFromEcosystem()
        {
            var ecosystem = TestEco();
            var handler1 = new MockHandler1();
            var handler2 = new MockHandler2();

            ecosystem
                .AddHandlers(handler1, handler2)
                .ClearHandlers()
                .Update();

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
                .Update();

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
                .Update();

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
                .Update();

            Assert.Equal(0, handler1.Called);
            Assert.Equal(0, handler2.Called);
        }

        #endregion

        #region Entity add/remove tests

        [Fact]
        public void CanAddEntityToEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddEntity(entity)
                .AddHandler(handler)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanAddEntityWithComponentsToEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddEntity(entity)
                .AddHandler(handler)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Equal(entity, handler.EntitiesHandled.Single());
        }

        [Fact]
        public void CanRemoveEntityFromEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddEntity(entity)
                .RemoveEntity(entity)
                .AddHandler(handler)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanRemoveEntityWithComponentsFromEcosystem()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddEntity(entity)
                .RemoveEntity(entity)
                .AddHandler(handler)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanAddEntityToEcosystemWithHandlers()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanAddEntityWithComponentsToEcosystemWithHandlers()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Equal(entity, handler.EntitiesHandled.Single());
        }

        [Fact]
        public void CanRemoveEntityFromEcosystemWithHandlers()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .RemoveEntity(entity)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        [Fact]
        public void CanRemoveEntityWithComponentsFromEcosystemWithHandlers()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity().AddComponent<MockComponent1>();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity)
                .RemoveEntity(entity)
                .Update();

            Assert.Equal(1, handler.Called);
            Assert.Empty(handler.EntitiesHandled);
        }

        #endregion

        #region Components added-to/removed-from entities

        [Fact]
        public void CanAddComponentToEntityInEcosystem()
        {
            var ecosystem = TestEco();
            var entity = new Entity();

            ecosystem.AddEntity(entity);

            var entitiesWithComponent = ecosystem.GetEntities().Where(x => x.HasComponent<MockComponent1>());            
            Assert.Empty(entitiesWithComponent);

            entity.AddComponent<MockComponent1>();

            entitiesWithComponent = ecosystem.GetEntities().Where(x => x.HasComponent<MockComponent1>());
            Assert.Equal(entity, entitiesWithComponent.Single());
        }

        [Fact]
        public void CanRemoveComponentFromEntityInEcosystem()
        {
            var ecosystem = TestEco();
            var entity = new Entity();

            ecosystem.AddEntity(entity);
            entity.AddComponent<MockComponent1>().RemoveComponent<MockComponent1>();

            var entitiesWithComponent = ecosystem.GetEntities().Where(x => x.HasComponent<MockComponent1>());
            Assert.Empty(entitiesWithComponent);
        }

        [Fact]
        public void CanAddComponentToEntityInEcosystemWithHandlers()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity);
            entity.AddComponent<MockComponent1>();
            ecosystem.Update();

            Assert.Equal(1, handler.Called);
            Assert.Equal(entity, handler.EntitiesHandled.Single());
        }

        [Fact]
        public void CanRemoveComponentFromEntityInEcosystemWithHandlers()
        {
            var ecosystem = TestEco();
            var handler = new MockHandler1();
            var entity = new Entity();

            ecosystem
                .AddHandler(handler)
                .AddEntity(entity);
            entity.AddComponent<MockComponent1>();
            ecosystem.Update();

            entity.RemoveComponent<MockComponent1>();
            ecosystem.Update();

            Assert.Equal(2, handler.Called);
            Assert.Empty(handler.EntitiesHandled); // No entities handled on most recent update
        }

        #endregion

        #region Private methods

        private static Ecosystem CreateEcosystemWithEntities()
        {
            var ecosystem = TestEco();
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

        private static Ecosystem TestEco()
        {
            return new Ecosystem(new GameTimer());
        }

        #endregion
    }
}