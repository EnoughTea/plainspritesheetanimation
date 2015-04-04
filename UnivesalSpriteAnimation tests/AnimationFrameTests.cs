using NUnit.Framework;
using Unisa;

namespace UnivesalSpriteAnimation_tests {
    [TestFixture]
    public class AnimationFrameTests {
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
            var frame = new AnimationFrame(new TextureRegion(10, 10, 32, 32), 0.15f);
            var clone = frame.Clone();
            Assert.That(!ReferenceEquals(frame, clone));
            Assert.AreEqual(frame.Duration, clone.Duration);
            Assert.AreEqual(frame.Origin, clone.Origin);
            Assert.AreEqual(frame.Source, clone.Source);
        }
    }
}
