using NUnit.Framework;

namespace PlainSpritesheetAnimation.Tests {
    [TestFixture]
    public class AnimationFrameTests {
        public const string TestFrameXmlRepresentation = @"<?xml version=""1.0"" encoding=""utf-16""?>
<animFrame xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
  <src>
    <x>10</x>
    <y>10</y>
    <w>32</w>
    <h>32</h>
  </src>
  <time>0.15</time>
  <ogn>
    <x>2</x>
    <y>4</y>
  </ogn>
</animFrame>";

        private AnimationFrame _testFrame;

        [Test]
        public void WhenFrameIsCreatedItsDefaultPropertiesAreSensible() {
            var frame = new AnimationFrame();
            Assert.AreEqual(frame.Duration, 0);
            Assert.AreEqual(frame.Origin, TexturePoint.Zero);
            Assert.AreEqual(frame.Source, TextureRegion.Empty);
        }

        [Test]
        public void WhenFrameIsCreatedItsPropertiesAreSensible() {
            var source = new TextureRegion(10, 10, 32, 32);
            var duration = 0.15f;
            var frame = new AnimationFrame(source, duration);
            Assert.AreEqual(frame.Duration, duration);
            Assert.AreEqual(frame.Origin, TexturePoint.Zero);
            Assert.AreEqual(frame.Source, source);
        }

        [Test]
        public void WhenFrameIsClonedItsCloneIsValid() {
            var clone = _testFrame.Clone();
            Assert.That(!ReferenceEquals(_testFrame, clone));
            Assert.AreEqual(_testFrame.Duration, clone.Duration);
            Assert.AreEqual(_testFrame.Origin, clone.Origin);
            Assert.AreEqual(_testFrame.Source, clone.Source);
        }

        [Test]
        public void WhenFrameIsSerializedResultingXmlIsValid() {
            string xml = Tools.SerializeToXml(_testFrame);
            Assert.AreEqual(xml, TestFrameXmlRepresentation);
        }

        [Test]
        public void WhenFrameIsDeserializedResultingPropertiesAreValid() {
            var deserialized = Tools.DeserializeFromXml<AnimationFrame>(TestFrameXmlRepresentation);
            Assert.That(deserialized != null);
            Assert.That(_testFrame != deserialized);
            Assert.AreEqual(_testFrame.Source, deserialized.Source);
            Assert.AreEqual(_testFrame.Duration, deserialized.Duration);
            Assert.AreEqual(_testFrame.Origin, deserialized.Origin);
        }

        [SetUp]
        public void Init() {
            _testFrame = new AnimationFrame(new TextureRegion(10, 10, 32, 32), 0.15f) {
                Origin = new TexturePoint(2, 4)
            };
        }
    }
}
