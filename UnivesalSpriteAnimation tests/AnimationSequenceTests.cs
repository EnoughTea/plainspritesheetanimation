using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unisa;

namespace UnivesalSpriteAnimation_tests {
    [TestFixture]
    public class AnimationSequenceTests {
        private AnimationSequence testSequence;

        [Test]
        public void WhenSequenceIsCreatedItsDefaultPropertiesAreSensible() {
            var sequence = new AnimationSequence();
            Assert.IsFalse(sequence.Animating);
            Assert.IsFalse(sequence.Visible);
            Assert.IsFalse(sequence.Reverse);
            Assert.That(sequence.Name == null);
            Assert.That(sequence.Mirror == MirrorDirections.None);
            Assert.That(sequence.AnimationType == AnimationType.Once);
            Assert.That(sequence.CurrentFrame == null);
            Assert.That(sequence.CurrentFrameIndex == 0);
            Assert.That(sequence.Frames != null);
            Assert.That(sequence.Frames.Count == 0);
        }

        [Test]
        public void WhenSequenceIsCreatedItsPropertiesAreSensible() {
            string name = "Test sequence";
            var frames = new List<AnimationFrame> {
                new AnimationFrame(new TextureRegion(10, 72, 23, 59), 1f),
                new AnimationFrame(),
            };

            var sequence = new AnimationSequence(name, frames);
            Assert.IsFalse(sequence.Animating);
            Assert.IsFalse(sequence.Visible);
            Assert.IsFalse(sequence.Reverse);
            Assert.That(sequence.Name == name);
            Assert.That(sequence.Mirror == MirrorDirections.None);
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
        public void WhenSequenceIsClonedItsCloneIsValid() {
            const string CloneName = "Cloned sequence";
            var clone = testSequence.Clone(CloneName);

            Assert.That(!ReferenceEquals(testSequence, clone));
            Assert.That(clone.Animating == testSequence.Animating);
            Assert.That(clone.Visible == testSequence.Visible);
            Assert.That(clone.Reverse == testSequence.Reverse);
            Assert.That(clone.Name == CloneName);
            Assert.That(clone.Mirror == testSequence.Mirror);
            Assert.That(clone.AnimationType == testSequence.AnimationType);
            Assert.That(clone.CurrentFrameIndex == testSequence.CurrentFrameIndex);
            Assert.That(!ReferenceEquals(testSequence.Frames, clone.Frames));
            Assert.That(clone.Frames.Count == testSequence.Frames.Count);
            for (int i = 0; i < testSequence.Frames.Count; i++) {
                var originalFrame = testSequence.Frames[i];
                var clonedFrame = clone.Frames[i];
                Assert.That(!ReferenceEquals(clonedFrame, originalFrame));
                Assert.AreEqual(clonedFrame.Duration, originalFrame.Duration);
                Assert.AreEqual(clonedFrame.Source, originalFrame.Source);
                Assert.AreEqual(clonedFrame.Origin, originalFrame.Origin);
            }
        }

        [Test]
        public void WhenAnimationTypeOnceReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.Once;
            testSequence.Reverse = true;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, testSequence.Frames.Count - 1);
            Assert.IsFalse(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceHoldLastReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.OnceHoldLast;
            testSequence.Reverse = true;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            Assert.IsFalse(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceDisappearReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.OnceDisappear;
            testSequence.Reverse = true;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            Assert.IsFalse(testSequence.Animating);
            Assert.IsFalse(testSequence.Visible);
        }


        [Test]
        public void WhenAnimationTypeLoopingReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.Looping;
            testSequence.Reverse = true;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            Assert.IsTrue(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypePingPongReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.PingPong;
            testSequence.Reverse = true;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            Assert.IsTrue(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.Once;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            Assert.IsFalse(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceHoldLastFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.OnceHoldLast;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            Assert.IsFalse(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceDisappearFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.OnceDisappear;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            Assert.IsFalse(testSequence.Animating);
            Assert.IsFalse(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeLoopingFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.Looping;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            Assert.IsTrue(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypePingPongFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            testSequence.AnimationType = AnimationType.PingPong;
            testSequence.Start();
            Assert.That(testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 2);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            AdvanceFrameAndVerifyIndex(testSequence, 0);
            AdvanceFrameAndVerifyIndex(testSequence, 1);
            Assert.IsTrue(testSequence.Animating);
            Assert.IsTrue(testSequence.Visible);
        }

        [Test]
        public void WhenFrameBoundsAreCalculatedTheyAreEqualToBiggestEncompassingFrameSize() {
            var maxFrameSize = testSequence.GetFrameBounds();
            Assert.AreEqual(maxFrameSize, new TextureSize(32, 16));
        }

        [Test]
        public void WhenSequenceDurationIsCalculatedItIsEqualToAllFrameDurationsCombined() {
            float seqDuration = testSequence.GetDuration();
            Assert.AreEqual(seqDuration, 0.6f);
        }

        [Test]
        public void WhenSequenceDurationIsSetAllFrameDurationsAreProperlyScaled() {
            testSequence.SetDuration(1.2f);
            Assert.AreEqual(testSequence.Frames[0].Duration, 0.2f);
            Assert.AreEqual(testSequence.Frames[1].Duration, 0.6f);
            Assert.AreEqual(testSequence.Frames[2].Duration, 0.4f);
        }

        [SetUp]
        public void Init() {
            string name = "Test sequence";
            var frames = new List<AnimationFrame> {
                new AnimationFrame(new TextureRegion(0, 0, 16, 16), 0.1f),
                new AnimationFrame(new TextureRegion(16, 0, 16, 16), 0.3f),
                new AnimationFrame(new TextureRegion(0, 16, 32, 8), 0.2f)
            };
            testSequence = new AnimationSequence(name, frames);
        }

        private static void AdvanceFrameAndVerifyIndex(IAnimationSequence sequence, int expectedIndex) {
            sequence.AdvanceFrame();
            Assert.That(sequence.CurrentFrameIndex == expectedIndex);
            Assert.That(sequence.CurrentFrame == sequence.Frames[expectedIndex]);
        }

        private static void AdvanceToEnd(IAnimationSequence sequence) {
            if (sequence.AnimationType == AnimationType.Once || sequence.AnimationType == AnimationType.OnceHoldLast ||
                sequence.AnimationType == AnimationType.OnceDisappear) {
                for (int i = 0; i < sequence.Frames.Count; i++) {
                    sequence.AdvanceFrame();
                }
            }
            else throw new ArgumentException("Can't run to end an infinite animation type", "sequence");
        }
    }
}
