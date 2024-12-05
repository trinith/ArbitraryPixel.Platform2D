using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.BuildInfo;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitraryPixel.Platform2D_Tests.BuildInfo
{
    [TestClass]
    public class BuildInfoOverlayLayer_ColourConstructor_Tests
    {
        private BuildInfoOverlayLayerModel _sut;
        private IBuildInfoStore _mockBuildInfoStore;
        private ITextObjectBuilder _mockTextBuilder;
        private List<ITextObject> _mockTextObjects;

        [TestInitialize]
        public void Initialize()
        {
            _mockBuildInfoStore = Substitute.For<IBuildInfoStore>();
            _mockTextBuilder = Substitute.For<ITextObjectBuilder>();

            _mockBuildInfoStore.Platform.Returns("SomePlatform");
            _mockBuildInfoStore.Title.Returns("SomeTitle");
            _mockBuildInfoStore.Version.Returns("SomeVersion");
            _mockBuildInfoStore.Date.Returns("SomeDate");

            _mockTextObjects = new List<ITextObject>();
            _mockTextBuilder.Build(Arg.Any<string>(), Arg.Any<RectangleF>()).Returns(_mockTextObjects);

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.MeasureString("ObjA").Returns(new SizeF(100, 20));
            mockFont.MeasureString("ObjB").Returns(new SizeF(50, 10));

            ITextObject textObjA = Substitute.For<ITextObject>();
            textObjA.TextDefinition.Font.Returns(mockFont);
            textObjA.TextDefinition.Text.Returns("ObjA");
            textObjA.Location.Returns(Vector2.Zero);
            ITextObject textObjB = Substitute.For<ITextObject>();
            textObjB.TextDefinition.Font.Returns(mockFont);
            textObjB.TextDefinition.Text.Returns("ObjB");
            textObjB.Location.Returns(new Vector2(0, 20));

            _mockTextObjects.Add(textObjA);
            _mockTextObjects.Add(textObjB);

            _sut = new BuildInfoOverlayLayerModel(_mockBuildInfoStore, _mockTextBuilder, new Color(111, 222, 123, 213));
        }

        [TestMethod]
        public void ConstructShouldCallTextBuilderBuild()
        {
            StringBuilder expectedText = new StringBuilder();
            expectedText.Append("{Alignment:Centre}{C:111, 222, 123, 213}");
            expectedText.AppendLine("SomeTitle (SomePlatform)");
            expectedText.AppendLine("SomeVersion - SomeDate");
            expectedText.AppendLine("DO NOT DISTRIBUTE");

            _mockTextBuilder.Received(1).Build(expectedText.ToString(), new RectangleF(Vector2.Zero, new SizeF(1)));
        }
    }
}
