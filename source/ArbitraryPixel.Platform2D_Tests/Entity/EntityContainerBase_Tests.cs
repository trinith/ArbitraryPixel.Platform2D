using System;
using System.Collections.Generic;
using System.Linq;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Entity
{
    [TestClass]
    public class EntityContainerBase_Tests
    {
        private EntityContainerBase _sut;
        private IEngine _mockEngine;
        private ILogger _mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockLogger = Substitute.For<ILogger>();
            _mockEngine.GetComponent<ILogger>().Returns(_mockLogger);

            _sut = new EntityContainerBase(_mockEngine);
        }

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallUpdateOnEnabledEntities()
        {
            GameTime expectedGT = new GameTime();
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Enabled.Returns(true);
            _sut.AddEntity(mockEntity);

            _sut.Update(expectedGT);

            mockEntity.Received(1).Update(expectedGT);
        }

        [TestMethod]
        public void UpdateShouldNotCallUpdateOnDisabledEntities()
        {
            GameTime expectedGT = new GameTime();
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Enabled.Returns(false);
            _sut.AddEntity(mockEntity);

            _sut.Update(expectedGT);

            mockEntity.Received(0).Update(expectedGT);
        }

        [TestMethod]
        public void UpdateShouldPersistAliveEntities()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Enabled.Returns(true);
            mockEntity.Alive.Returns(true);
            _sut.AddEntity(mockEntity);

            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.Entities.Contains(mockEntity));
        }

        [TestMethod]
        public void UpdateShouldDisposeDeadEntities()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Enabled.Returns(true);
            _sut.AddEntity(mockEntity);

            mockEntity.Alive.Returns(false);
            _sut.Update(new GameTime());

            mockEntity.Received(1).Dispose();
        }

        [TestMethod]
        public void UpdateShouldRemoveDeadEntities()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Enabled.Returns(true);
            mockEntity.Alive.Returns(false);
            _sut.AddEntity(mockEntity);

            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.Entities.Contains(mockEntity));
        }

        [TestMethod]
        public void UpdateShouldDetachDeadEntitiesFromOwningContainer()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Enabled.Returns(true);
            mockEntity.Alive.Returns(false);
            _sut.AddEntity(mockEntity);

            _sut.Update(new GameTime());

            mockEntity.Received(1).OwningContainer = null;
        }

        [TestMethod]
        public void UpdateShouldProcessEntitiesInReverseOrder()
        {
            List<IEntity> updatedEntities = new List<IEntity>();

            IEntity entityA = Substitute.For<IEntity>();
            entityA.Alive.Returns(true);
            entityA.Enabled.Returns(true);
            entityA.When(x => x.Update(Arg.Any<GameTime>())).Do(x => updatedEntities.Add(entityA));
            _sut.AddEntity(entityA);

            IEntity entityB = Substitute.For<IEntity>();
            entityB.Alive.Returns(true);
            entityB.Enabled.Returns(true);
            entityB.When(x => x.Update(Arg.Any<GameTime>())).Do(x => updatedEntities.Add(entityB));
            _sut.AddEntity(entityB);

            _sut.Update(new GameTime());

            Assert.IsTrue(updatedEntities.ToArray().SequenceEqual(new IEntity[] { entityB, entityA }));
        }

        [TestMethod]
        public void UpdateWhenNotEnabledShouldNotCallUpdateOnAnyChildEntities()
        {
            _sut.Enabled = false;
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Alive.Returns(true);
            mockEntity.Enabled.Returns(true);
            _sut.AddEntity(mockEntity);

            _sut.Update(new GameTime());

            mockEntity.Received(0).Update(Arg.Any<GameTime>());
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallDrawOnVisibleEntities()
        {
            GameTime expectedGT = new GameTime();
            IEntity entityA = Substitute.For<IEntity>();
            entityA.Visible.Returns(true);
            _sut.AddEntity(entityA);

            _sut.Draw(expectedGT);

            entityA.Received(1).Draw(expectedGT);
        }

        [TestMethod]
        public void DrawShouldNotCallDrawOnInvisibleEntities()
        {
            GameTime expectedGT = new GameTime();
            IEntity entityA = Substitute.For<IEntity>();
            entityA.Visible.Returns(false);
            _sut.AddEntity(entityA);

            _sut.Draw(expectedGT);

            entityA.Received(0).Draw(expectedGT);
        }

        [TestMethod]
        public void DrawShouldProcessEntitiesInNormalOrder()
        {
            List<IEntity> updatedEntities = new List<IEntity>();

            IEntity entityA = Substitute.For<IEntity>();
            entityA.Alive.Returns(true);
            entityA.Visible.Returns(true);
            entityA.When(x => x.Draw(Arg.Any<GameTime>())).Do(x => updatedEntities.Add(entityA));
            _sut.AddEntity(entityA);

            IEntity entityB = Substitute.For<IEntity>();
            entityB.Alive.Returns(true);
            entityB.Visible.Returns(true);
            entityB.When(x => x.Draw(Arg.Any<GameTime>())).Do(x => updatedEntities.Add(entityB));
            _sut.AddEntity(entityB);

            _sut.Draw(new GameTime());

            Assert.IsTrue(updatedEntities.ToArray().SequenceEqual(new IEntity[] { entityA, entityB }));
        }

        [TestMethod]
        public void DrawWhenNotVisibleShouldNotCallDrawOnChildEntities()
        {
            _sut.Visible = false;
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Visible.Returns(true);
            _sut.AddEntity(mockEntity);

            _sut.Draw(new GameTime());

            mockEntity.Received(0).Draw(Arg.Any<GameTime>());
        }
        #endregion

        #region PreDraw Tests
        [TestMethod]
        public void PreDrawShouldNotCallPreDrawOnInvisibleEntity()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Visible.Returns(false);
            _sut.AddEntity(mockEntity);

            _sut.PreDraw(new GameTime());

            mockEntity.DidNotReceive().PreDraw(Arg.Any<GameTime>());
        }

        [TestMethod]
        public void PreDrawShouldCallPreDrawOnVisibleEntities()
        {
            IEntity mockEntityA = Substitute.For<IEntity>();
            mockEntityA.Visible.Returns(true);
            _sut.AddEntity(mockEntityA);

            IEntity mockEntityB = Substitute.For<IEntity>();
            mockEntityB.Visible.Returns(true);
            _sut.AddEntity(mockEntityB);

            _sut.PreDraw(new GameTime());

            Received.InOrder(
                () =>
                {
                    mockEntityA.PreDraw(Arg.Any<GameTime>());
                    mockEntityB.PreDraw(Arg.Any<GameTime>());
                }
            );
        }

        [TestMethod]
        public void PreDrawWhenNotVisibleShouldNotCallPreDrawOnChildEntities()
        {
            _sut.Visible = false;
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.Visible.Returns(true);
            _sut.AddEntity(mockEntity);

            _sut.PreDraw(new GameTime());

            mockEntity.Received(0).PreDraw(Arg.Any<GameTime>());
        }
        #endregion

        #region AddEntity Tests
        [TestMethod]
        public void AddEntityShouldAddObjectToEntityList()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);

            Assert.IsTrue(_sut.Entities.Contains(mockEntity));
        }

        [TestMethod]
        public void AddEntityShouldSetOwningContainer()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);

            mockEntity.Received(1).OwningContainer = _sut;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEntityWithNullShouldThrowException()
        {
            _sut.AddEntity((IEntity)null);
        }

        [TestMethod]
        public void AddEntityWithExistingEntityShouldLogMessage()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            IEntity entityB = Substitute.For<IEntity>();
            entityB.UniqueId.Returns("A");
            _sut.AddEntity(entityA);
            _mockLogger.ClearReceivedCalls();

            _sut.AddEntity(entityB);

            _mockLogger.Received(1).WriteLine(Arg.Any<string>());
        }

        [TestMethod]
        public void AddEntityWithExistingEntityButNoLoggerShouldNotCrash()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            IEntity entityB = Substitute.For<IEntity>();
            entityB.UniqueId.Returns("A");
            _sut.AddEntity(entityA);
            _mockEngine.GetComponent<ILogger>().Returns((ILogger)null);

            _sut.AddEntity(entityB);
        }

        [TestMethod]
        public void AddEntityWithNewEntityShouldNotLogMessage()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            IEntity entityB = Substitute.For<IEntity>();
            entityB.UniqueId.Returns("B");
            _sut.AddEntity(entityA);
            _mockLogger.ClearReceivedCalls();

            _sut.AddEntity(entityB);

            _mockLogger.Received(0).WriteLine(Arg.Any<string>());
        }

        [TestMethod]
        public void AddEntityWithRemovedEntityShouldNotLogMessage()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            IEntity entityB = Substitute.For<IEntity>();
            entityB.UniqueId.Returns("A");
            _sut.AddEntity(entityA);
            _sut.RemoveEntity(entityA);
            _mockLogger.ClearReceivedCalls();

            _sut.AddEntity(entityB);

            _mockLogger.Received(0).WriteLine(Arg.Any<string>());
        }
        #endregion

        #region RemoveEntity Tests
        [TestMethod]
        public void RemoveEntityShouldRemoveObjectFromEntityList()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);
            mockEntity.ClearReceivedCalls();

            _sut.RemoveEntity(mockEntity);

            Assert.IsFalse(_sut.Entities.Contains(mockEntity));
        }

        [TestMethod]
        public void RemoveEntityShouldClearOwningContainer()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);
            mockEntity.ClearReceivedCalls();

            _sut.RemoveEntity(mockEntity);

            mockEntity.Received(1).OwningContainer = null;
        }

        [TestMethod]
        public void RemoveEntityWithOwnedEntityShouldReturnTrue()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);
            mockEntity.ClearReceivedCalls();

            Assert.IsTrue(_sut.RemoveEntity(mockEntity));
        }

        [TestMethod]
        public void RemoveEntityWithUnownedEntityShouldReturnFalse()
        {
            IEntity mockEntity = Substitute.For<IEntity>();

            Assert.IsFalse(_sut.RemoveEntity(mockEntity));
        }

        [TestMethod]
        public void RemoveEntityShouldCallEntityDispose()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);
            mockEntity.ClearReceivedCalls();

            _sut.RemoveEntity(mockEntity);

            mockEntity.Received(1).Dispose();
        }

        [TestMethod]
        public void RemoveEntityDuringUpdateLoopShouldRemoveObjectFromEntityList()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            mockEntity.When(x => x.Update(Arg.Any<GameTime>())).Do(x => _sut.RemoveEntity(mockEntity));     // When update is called, 
            _sut.AddEntity(mockEntity);
            mockEntity.ClearReceivedCalls();

            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.Entities.Contains(mockEntity));
        }

        [TestMethod]
        public void RemoveEntityDuringUpdateLoopShouldDisposeEntityAfterUpdate()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            entityA.When(x => x.Update(Arg.Any<GameTime>())).Do(x => _sut.RemoveEntity(entityA));     // When update is called, 
            _sut.AddEntity(entityA);
            entityA.ClearReceivedCalls();

            IEntity entityB = Substitute.For<IEntity>();
            entityB.Enabled.Returns(true);
            entityB.UniqueId.Returns("B");
            _sut.AddEntity(entityB);

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    entityB.Update(Arg.Any<GameTime>());
                    entityA.Dispose();
                }
            );
        }

        [TestMethod]
        public void RemoveEntityDuringUpdateLoopShouldClearOwningContainerAfterUpdate()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            entityA.When(x => x.Update(Arg.Any<GameTime>())).Do(x => _sut.RemoveEntity(entityA));     // When update is called, 
            _sut.AddEntity(entityA);
            entityA.ClearReceivedCalls();

            IEntity entityB = Substitute.For<IEntity>();
            entityB.UniqueId.Returns("B");
            entityB.Enabled.Returns(true);
            _sut.AddEntity(entityB);

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    entityB.Update(Arg.Any<GameTime>());
                    entityA.OwningContainer = null;
                }
            );
        }

        [TestMethod]
        public void RemoveEntityWithDuplicateEntityShouldNotThrowException()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");

            IEntity entityB = Substitute.For<IEntity>();
            entityB.UniqueId.Returns("A");

            _sut.AddEntity(entityA);
            _sut.AddEntity(entityB);

            _sut.RemoveEntity(entityA);
            _sut.RemoveEntity(entityB);

            Assert.AreEqual<int>(0, _sut.Entities.Count);
        }
        #endregion

        #region ClearEntities Tests
        [TestMethod]
        public void ClearEntitiesShouldCallDisposeOnEntities()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);

            _sut.ClearEntities();

            mockEntity.Received(1).Dispose();
        }

        [TestMethod]
        public void ClearEntitiesShouldRemoveClearEntitiesList()
        {
            IEntity mockEntity = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntity);

            _sut.ClearEntities();

            Assert.AreEqual<int>(0, _sut.Entities.Count);
        }

        [TestMethod]
        public void ClearEntitiesShouldUnparentEntityFromContainer_EntityA()
        {
            IEntity mockEntityA = Substitute.For<IEntity>();
            IEntity mockEntityB = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntityA);
            _sut.AddEntity(mockEntityB);

            _sut.ClearEntities();

            mockEntityA.Received(1).OwningContainer = null;
        }

        [TestMethod]
        public void ClearEntitiesShouldUnparentEntityFromContainer_EntityB()
        {
            IEntity mockEntityA = Substitute.For<IEntity>();
            IEntity mockEntityB = Substitute.For<IEntity>();
            _sut.AddEntity(mockEntityA);
            _sut.AddEntity(mockEntityB);

            _sut.ClearEntities();

            mockEntityB.Received(1).OwningContainer = null;
        }

        [TestMethod]
        public void ClearEntitiesDuringUpdateShouldAbortUpdateThenClear()
        {
            IEntity mockEntityA = Substitute.For<IEntity>(); mockEntityA.Alive.Returns(true); mockEntityA.Enabled.Returns(true);
            IEntity mockEntityB = Substitute.For<IEntity>(); mockEntityB.Alive.Returns(true); mockEntityB.Enabled.Returns(true);
            _sut.AddEntity(mockEntityA);
            _sut.AddEntity(mockEntityB);

            mockEntityA.When(x => x.Update(Arg.Any<GameTime>())).Do(x => throw new Exception()); // This should never get the chance to update, therefore exception should not be thrown.
            mockEntityB.When(x => x.Update(Arg.Any<GameTime>())).Do(x => _sut.ClearEntities()); // B will update first, it should clear entities.

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    mockEntityA.OwningContainer = null;
                    mockEntityA.Dispose();

                    mockEntityB.OwningContainer = null;
                    mockEntityB.Dispose();
                }
            );
        }

        [TestMethod]
        public void ClearEntitiesDuringUpdateAfterPreviousClearShouldProceedAsExpected()
        {
            bool entityCUpdated = false;

            IEntity mockEntityA = Substitute.For<IEntity>(); mockEntityA.Alive.Returns(true); mockEntityA.Enabled.Returns(true);
            IEntity mockEntityB = Substitute.For<IEntity>(); mockEntityB.Alive.Returns(true); mockEntityB.Enabled.Returns(true);
            IEntity mockEntityC = Substitute.For<IEntity>(); mockEntityC.Alive.Returns(true); mockEntityC.Enabled.Returns(true);
            _sut.AddEntity(mockEntityA);
            _sut.AddEntity(mockEntityB);

            mockEntityA.When(x => x.Update(Arg.Any<GameTime>())).Do(x => throw new Exception()); // This should never get the chance to update, therefore exception should not be thrown.
            mockEntityB.When(x => x.Update(Arg.Any<GameTime>())).Do(x => _sut.ClearEntities()); // B will update first, it should clear entities.
            mockEntityC.When(x => x.Update(Arg.Any<GameTime>())).Do(x => entityCUpdated = true);

            _sut.Update(new GameTime());

            _sut.AddEntity(mockEntityC);

            _sut.Update(new GameTime());

            Assert.IsTrue(entityCUpdated);
        }

        [TestMethod]
        public void ClearEntitiesWithDuplicateEntitiesShouldNotThrowException()
        {
            IEntity entityA = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");
            IEntity entityB = Substitute.For<IEntity>();
            entityA.UniqueId.Returns("A");

            _sut.AddEntity(entityA);
            _sut.AddEntity(entityB);

            _sut.ClearEntities();

            Assert.AreEqual<int>(0, _sut.Entities.Count);
        }
        #endregion
    }
}
