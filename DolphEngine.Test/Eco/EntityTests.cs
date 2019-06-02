using System;
using DolphEngine.Eco;
using Xunit;

namespace DolphEngine.Test.Eco
{
    public class EntityTests
    {
        private const string Tag1 = "tag1";
        private const string Tag2 = "tag2";
        private const string Tag3 = "tag3";
        private const string Tag4 = "tag4";

        #region Component tests

        [Fact]
        public void CanAddComponentsToEntityAndRetrieveThem()
        {
            var component1 = new MockComponent1("comp1", 1);
            var component2 = new MockComponent2("comp2", true);
            var entity = new Entity().AddComponent(component1).AddComponent(component2);

            Assert.True(entity.HasComponent<MockComponent1>());
            Assert.True(entity.HasComponent<MockComponent2>());
            Assert.Equal(component1, entity.GetComponent<MockComponent1>());
            Assert.Equal(component2, entity.GetComponent<MockComponent2>());
            Assert.True(entity.TryGetComponent<MockComponent1>(out var retrievedComponent1));
            Assert.True(entity.TryGetComponent<MockComponent2>(out var retrievedComponent2));
            Assert.Equal(component1, retrievedComponent1);
            Assert.Equal(component2, retrievedComponent2);
        }

        [Fact]
        public void CannotAddDuplicateComponentsToEntity()
        {
            var component = new MockComponent1("comp1", 1);
            var componentDupe = new MockComponent1("dupe", 2);
            var entity = new Entity().AddComponent(component);

            Assert.Throws<InvalidOperationException>(() => entity.AddComponent(componentDupe));
        }

        [Fact]
        public void CanRemoveComponentsFromEntity()
        {
            var component1 = new MockComponent1("comp1", 1);
            var component2 = new MockComponent2("comp2", true);
            var entity = new Entity().AddComponent(component1).AddComponent(component2).RemoveComponent<MockComponent1>();

            Assert.False(entity.HasComponent<MockComponent1>());
            Assert.ThrowsAny<Exception>(() => entity.GetComponent<MockComponent1>());
            Assert.False(entity.TryGetComponent<MockComponent1>(out var retrievedComponent1));
            Assert.Null(retrievedComponent1);

            Assert.True(entity.HasComponent<MockComponent2>());           
            Assert.Equal(component2, entity.GetComponent<MockComponent2>());            
            Assert.True(entity.TryGetComponent<MockComponent2>(out var retrievedComponent2));            
            Assert.Equal(component2, retrievedComponent2);
        }

        #endregion

        #region Tag tests

        [Fact]
        public void CanAddTagsToEntity()
        {
            var entity = new Entity().AddTags(Tag1, Tag2);

            Assert.True(entity.HasTag(Tag1));
            Assert.True(entity.HasTag(Tag2));
            Assert.Contains(Tag1, entity.Tags);
            Assert.Contains(Tag2, entity.Tags);
        }

        [Fact]
        public void CanRemoveTagsFromEntity()
        {
            var entity = new Entity().AddTags(Tag1, Tag2).RemoveTag(Tag1);

            Assert.False(entity.HasTag(Tag1));
            Assert.True(entity.HasTag(Tag2));
            Assert.DoesNotContain(Tag1, entity.Tags);
            Assert.Contains(Tag2, entity.Tags);
        }

        [Fact]
        public void CanIgnoreDuplicateTagsWhenAddedToEntity()
        {
            var entity = new Entity().AddTags(Tag1, Tag1);

            Assert.True(entity.HasTag(Tag1));
            Assert.Equal(1, entity.Tags.Count);
        }

        [Fact]
        public void CanCheckEntityHasAnyTags()
        {
            var entity = new Entity().AddTags(Tag1, Tag2);

            Assert.True(entity.HasAnyTags(Tag1));
            Assert.True(entity.HasAnyTags(Tag1, Tag2));
            Assert.True(entity.HasAnyTags(Tag1, Tag3));
            Assert.False(entity.HasAnyTags(Tag3, Tag4));
        }

        [Fact]
        public void CanCheckEntityHasAllTags()
        {
            var entity = new Entity().AddTags(Tag1, Tag2);

            Assert.True(entity.HasAllTags(Tag1));
            Assert.True(entity.HasAllTags(Tag1, Tag2));
            Assert.False(entity.HasAllTags(Tag1, Tag3));
            Assert.False(entity.HasAllTags(Tag3, Tag4));
        }

        #endregion        
    }
}