using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlainSpritesheetAnimation;

namespace UnivesalSpriteAnimation_tests {
    [TestFixture]
    public class AnimationSequenceTests {
        private AnimationSequence _testSequence;

        public const string TestSequenceXmlRepresentation = @"<?xml version=""1.0"" encoding=""utf-16""?>
<animSeq xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" z:Id=""i1"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
  <name>Test sequence</name>
  <texId>Test sequence.png</texId>
  <t>Looping</t>
  <m>Horizontal</m>
  <a>true</a>
  <v>true</v>
  <rev>true</rev>
  <cfi>1</cfi>
  <frames xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
    <d2p1:anyType i:type=""animFrame"">
      <src>
        <x>0</x>
        <y>0</y>
        <w>16</w>
        <h>16</h>
      </src>
      <time>0.1</time>
    </d2p1:anyType>
    <d2p1:anyType i:type=""animFrame"">
      <src>
        <x>16</x>
        <y>0</y>
        <w>16</w>
        <h>16</h>
      </src>
      <time>0.3</time>
    </d2p1:anyType>
    <d2p1:anyType i:type=""animFrame"">
      <src>
        <x>0</x>
        <y>16</y>
        <w>32</w>
        <h>8</h>
      </src>
      <time>0.2</time>
    </d2p1:anyType>
  </frames>
</animSeq>";

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
            const string Name = "Created sequence";
            var frames = new List<AnimationFrame> {
                new AnimationFrame(new TextureRegion(10, 72, 23, 59), 1f),
                new AnimationFrame(),
            };

            var sequence = new AnimationSequence(Name, frames);
            Assert.IsFalse(sequence.Animating);
            Assert.IsFalse(sequence.Visible);
            Assert.IsFalse(sequence.Reverse);
            Assert.That(sequence.Name == Name);
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
            var clone = _testSequence.Clone(CloneName);

            Assert.That(!ReferenceEquals(_testSequence, clone));
            Assert.That(clone.Animating == _testSequence.Animating);
            Assert.That(clone.Visible == _testSequence.Visible);
            Assert.That(clone.Reverse == _testSequence.Reverse);
            Assert.That(clone.Name == CloneName);
            Assert.That(clone.Mirror == _testSequence.Mirror);
            Assert.That(clone.AnimationType == _testSequence.AnimationType);
            Assert.That(clone.CurrentFrameIndex == _testSequence.CurrentFrameIndex);
            Assert.That(!ReferenceEquals(_testSequence.Frames, clone.Frames));
            Assert.That(clone.Frames.Count == _testSequence.Frames.Count);
            for (int i = 0; i < _testSequence.Frames.Count; i++) {
                var originalFrame = _testSequence.Frames[i];
                var clonedFrame = clone.Frames[i];
                Assert.That(!ReferenceEquals(clonedFrame, originalFrame));
                Assert.AreEqual(clonedFrame.Duration, originalFrame.Duration);
                Assert.AreEqual(clonedFrame.Source, originalFrame.Source);
                Assert.AreEqual(clonedFrame.Origin, originalFrame.Origin);
            }
        }

        [Test]
        public void WhenAnimationTypeOnceReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.Once;
            _testSequence.Reverse = true;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == _testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, _testSequence.Frames.Count - 1);
            Assert.IsFalse(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceHoldLastReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.OnceHoldLast;
            _testSequence.Reverse = true;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == _testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            Assert.IsFalse(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceDisappearReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.OnceDisappear;
            _testSequence.Reverse = true;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == _testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            Assert.IsFalse(_testSequence.Animating);
            Assert.IsFalse(_testSequence.Visible);
        }


        [Test]
        public void WhenAnimationTypeLoopingReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.Looping;
            _testSequence.Reverse = true;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == _testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            Assert.IsTrue(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypePingPongReverseFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.PingPong;
            _testSequence.Reverse = true;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == _testSequence.Frames.Count - 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            Assert.IsTrue(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.Once;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            Assert.IsFalse(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceHoldLastFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.OnceHoldLast;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            Assert.IsFalse(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeOnceDisappearFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.OnceDisappear;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            Assert.IsFalse(_testSequence.Animating);
            Assert.IsFalse(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypeLoopingFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.Looping;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            Assert.IsTrue(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenAnimationTypePingPongFrameIsAdvancedItsPropertiesAreProperlyChanged() {
            _testSequence.AnimationType = AnimationType.PingPong;
            _testSequence.Start();
            Assert.That(_testSequence.CurrentFrameIndex == 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 2);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            AdvanceFrameAndVerifyIndex(_testSequence, 0);
            AdvanceFrameAndVerifyIndex(_testSequence, 1);
            Assert.IsTrue(_testSequence.Animating);
            Assert.IsTrue(_testSequence.Visible);
        }

        [Test]
        public void WhenFrameBoundsAreCalculatedTheyAreEqualToBiggestEncompassingFrameSize() {
            var maxFrameSize = _testSequence.GetFrameBounds();
            Assert.AreEqual(maxFrameSize, new TextureSize(32, 16));
        }

        [Test]
        public void WhenSequenceDurationIsCalculatedItIsEqualToAllFrameDurationsCombined() {
            float seqDuration = _testSequence.GetDuration();
            Assert.AreEqual(seqDuration, 0.6f);
        }

        [Test]
        public void WhenSequenceDurationIsSetAllFrameDurationsAreProperlyScaled() {
            _testSequence.SetDuration(1.2f);
            Assert.AreEqual(_testSequence.Frames[0].Duration, 0.2f);
            Assert.AreEqual(_testSequence.Frames[1].Duration, 0.6f);
            Assert.AreEqual(_testSequence.Frames[2].Duration, 0.4f);
        }

        [Test]
        public void WhenSequenceIsSerializedResultingXmlIsValid() {
            SetTestSequencePropertiesForSerializationTest();

            string xml = Tools.SerializeToXml(_testSequence);
            Assert.AreEqual(xml, TestSequenceXmlRepresentation);
        }

        [Test]
        public void WhenSequenceIsDeserializedResultingPropertiesAreValid() {
            SetTestSequencePropertiesForSerializationTest();

            var deserialized = Tools.DeserializeFromXml<AnimationSequence>(TestSequenceXmlRepresentation);
            Assert.That(deserialized != null);
            Assert.That(_testSequence != deserialized);

            Assert.That(_testSequence.Animating == deserialized.Animating);
            Assert.That(_testSequence.AnimationType == deserialized.AnimationType);
            Assert.That(_testSequence.CurrentFrameIndex == deserialized.CurrentFrameIndex);
            Assert.That(_testSequence.CurrentFrame != deserialized.CurrentFrame);
            Assert.That(_testSequence.Mirror == deserialized.Mirror);
            Assert.That(_testSequence.Name == deserialized.Name);
            Assert.That(_testSequence.Reverse == deserialized.Reverse);
            Assert.That(_testSequence.TextureId == deserialized.TextureId);
            Assert.That(_testSequence.Visible == deserialized.Visible);

            Assert.That(_testSequence.Frames != deserialized.Frames);
            Assert.That(_testSequence.Frames.Count == deserialized.Frames.Count);
            for (int i = 0; i < _testSequence.Frames.Count; i++) {
                var testFrame = _testSequence.Frames[i];
                var deserializedFrame = deserialized.Frames[i];
                Assert.AreEqual(testFrame.Source, deserializedFrame.Source);
                Assert.AreEqual(testFrame.Duration, deserializedFrame.Duration);
                Assert.AreEqual(testFrame.Origin, deserializedFrame.Origin);
            }
        }

        [SetUp]
        public void Init() {
            const string Name = "Test sequence";
            var frames = new List<AnimationFrame> {
                new AnimationFrame(new TextureRegion(0, 0, 16, 16), 0.1f),
                new AnimationFrame(new TextureRegion(16, 0, 16, 16), 0.3f),
                new AnimationFrame(new TextureRegion(0, 16, 32, 8), 0.2f)
            };
            _testSequence = new AnimationSequence(Name, frames);
            _testSequence.TextureId = "Test sequence.png";
        }

        private void SetTestSequencePropertiesForSerializationTest() {
            _testSequence.Animating = true;
            _testSequence.Visible = true;
            _testSequence.AnimationType = AnimationType.Looping;
            _testSequence.AdvanceFrame();
            _testSequence.Mirror = MirrorDirections.Horizontal;
            _testSequence.Reverse = true;
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
