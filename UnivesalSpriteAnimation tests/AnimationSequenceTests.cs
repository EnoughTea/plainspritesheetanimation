using System.Collections.Generic;
using NUnit.Framework;
using Unisa;

namespace UnivesalSpriteAnimation_tests {
    [TestFixture]
    public class AnimationSequenceTests {
        [Test]
        public void WhenAnimationSequenceIsCreatedItsDefaultPropertiesAreSensible() {
            var sequence = new AnimationSequence();
            Assert.IsFalse(sequence.Animating);
            Assert.IsFalse(sequence.Visible);
            Assert.IsFalse(sequence.Reverse);
            Assert.That(sequence.Name == null);
            Assert.That(sequence.Mirror == MirrorType.None);
            Assert.That(sequence.AnimationType == AnimationType.Once);
            Assert.That(sequence.CurrentFrame == null);
            Assert.That(sequence.CurrentFrameIndex == 0);
            Assert.That(sequence.Frames != null);
            Assert.That(sequence.Frames.Count == 0);
        }

        [Test]
        public void WhenAnimationFrameIsCreatedItsPropertiesAreSensible() {
            string name = "Test sequence";
            var frames = new List<AnimationFrame> {
                new AnimationFrame(TextureRegion.Empty, 0.1f),
                new AnimationFrame(TextureRegion.Empty, 0.2f),
                new AnimationFrame(TextureRegion.Empty, 0.1f)
            };

            var sequence = new AnimationSequence(name, frames);
            Assert.IsFalse(sequence.Animating);
            Assert.IsFalse(sequence.Visible);
            Assert.IsFalse(sequence.Reverse);
            Assert.That(sequence.Name == name);
            Assert.That(sequence.Mirror == MirrorType.None);
            Assert.That(sequence.AnimationType == AnimationType.Once);
            Assert.That(sequence.CurrentFrame == frames[0]);
            Assert.That(sequence.CurrentFrameIndex == 0);
            Assert.That(sequence.Frames != null);
            Assert.That(sequence.Frames.Count == frames.Count);
            for (int i = 0; i < sequence.Frames.Count; i++) {
                Assert.That(sequence.Frames[i] == frames[i]);
            }
        }

        [Test]
        public void WhenAnimationSequenceIsClonedItsCloneIsValid() {
            string name = "Test sequence";
            string cloneName = "Cloned sequence";
            var frames = new List<AnimationFrame> {
                new AnimationFrame(TextureRegion.Empty, 0.1f),
                new AnimationFrame(TextureRegion.Empty, 0.2f),
                new AnimationFrame(TextureRegion.Empty, 0.1f)
            };

            var sequence = new AnimationSequence(name, frames);
            var clone = sequence.Clone(cloneName);
            Assert.That(!ReferenceEquals(sequence, clone));
            Assert.That(clone.Animating == sequence.Animating);
            Assert.That(clone.Visible == sequence.Visible);
            Assert.That(clone.Reverse == sequence.Reverse);
            Assert.That(clone.Name == cloneName);
            Assert.That(clone.Mirror == sequence.Mirror);
            Assert.That(clone.AnimationType == sequence.AnimationType);
            Assert.That(clone.CurrentFrameIndex == sequence.CurrentFrameIndex);
            Assert.That(!ReferenceEquals(sequence.Frames, clone.Frames));
            Assert.That(clone.Frames.Count == sequence.Frames.Count);
            for (int i = 0; i < sequence.Frames.Count; i++) {
                var originalFrame = sequence.Frames[i];
                var clonedFrame = clone.Frames[i];
                Assert.That(!ReferenceEquals(clonedFrame, originalFrame));
                Assert.AreEqual(clonedFrame.Duration, originalFrame.Duration);
                Assert.AreEqual(clonedFrame.Source, originalFrame.Source);
                Assert.AreEqual(clonedFrame.Origin, originalFrame.Origin);
            }
        }
    }
}
