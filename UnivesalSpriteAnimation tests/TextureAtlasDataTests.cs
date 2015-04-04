using System.IO;
using System.Linq;
using NUnit.Framework;
using Unisa;
using Unisa.TexturePacker;

namespace UnivesalSpriteAnimation_tests {
    [TestFixture]
    public class TextureAtlasDataTests {
        private const string TexturePackerExportedXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!-- Created with TexturePacker http://texturepacker.com-->
<!-- $TexturePacker:SmartUpdate:78d3aff951abb5c82993a205c536379f$ -->
<!--Format:
n  => name of the sprite
x  => sprite x pos in texture
y  => sprite y pos in texture
w  => sprite width (may be trimmed)
h  => sprite height (may be trimmed)
oX => sprite's x-corner offset (only available if trimmed)
oY => sprite's y-corner offset (only available if trimmed)
oW => sprite's original width (only available if trimmed)
oH => sprite's original height (only available if trimmed)
r => 'y' only set if sprite is rotated
-->
<TextureAtlas imagePath=""character texture.png"" width=""256"" height=""256"">
    <sprite n=""ducking west.png"" x=""0"" y=""0"" w=""37"" h=""30""/>
    <sprite n=""ducking east.png"" x=""0"" y=""0"" w=""37"" h=""30"" r=""y""/>
    <sprite n=""standing east.png"" x=""37"" y=""0"" w=""31"" h=""49""/>
    <sprite n=""standing north.png"" x=""68"" y=""0"" w=""48"" h=""50""/>
    <sprite n=""standing south.png"" x=""116"" y=""0"" w=""43"" h=""49""/>
    <sprite n=""walking east 1.png"" x=""159"" y=""0"" w=""26"" h=""49""/>
    <sprite n=""walking east 2.png"" x=""185"" y=""0"" w=""29"" h=""50""/>
    <sprite n=""walking east 3.png"" x=""214"" y=""0"" w=""41"" h=""48""/>
    <sprite n=""walking east 4.png"" x=""0"" y=""50"" w=""28"" h=""50""/>
    <sprite n=""walking east 5.png"" x=""28"" y=""50"" w=""34"" h=""49""/>
    <sprite n=""walking east 6.png"" x=""62"" y=""50"" w=""42"" h=""48""/>
    <sprite n=""walking north 1.png"" x=""104"" y=""50"" w=""40"" h=""51""/>
    <sprite n=""walking north 2.png"" x=""144"" y=""50"" w=""44"" h=""48""/>
    <sprite n=""walking north 3.png"" x=""188"" y=""50"" w=""44"" h=""48""/>
    <sprite n=""walking north 4.png"" x=""0"" y=""101"" w=""43"" h=""47""/>
    <sprite n=""walking north 5.png"" x=""43"" y=""101"" w=""40"" h=""52""/>
    <sprite n=""walking north 6.png"" x=""83"" y=""101"" w=""33"" h=""55""/>
    <sprite n=""walking south 1.png"" x=""116"" y=""101"" w=""38"" h=""45""/>
    <sprite n=""walking south 2.png"" x=""154"" y=""101"" w=""41"" h=""50""/>
    <sprite n=""walking south 3.png"" x=""195"" y=""101"" w=""39"" h=""48""/>
    <sprite n=""walking south 4.png"" x=""0"" y=""156"" w=""43"" h=""45""/>
    <sprite n=""walking south 5.png"" x=""43"" y=""156"" w=""38"" h=""50""/>
    <sprite n=""walking south 6.png"" x=""81"" y=""156"" w=""37"" h=""48""/>
</TextureAtlas>
";
        private TexturePackerAtlas _correspondingAtlas;

        [Test]
        public void WhenAtlasIsLoadedItsOwnParamsAreValid() {
            using (var xml = MakeStream(TexturePackerExportedXml)) {
                var atlasData = TexturePackerAtlas.Load(xml);

                Assert.That(atlasData.ImagePath == _correspondingAtlas.ImagePath);
                Assert.That(atlasData.Width == _correspondingAtlas.Width);
                Assert.That(atlasData.Height == _correspondingAtlas.Height);
            }
        }

        [Test]
        public void WhenTAtlasIsLoadedItsSpritesAreLoadedAsWell() {
            using (var xml = MakeStream(TexturePackerExportedXml)) {
                var atlasData = TexturePackerAtlas.Load(xml);

                Assert.That(atlasData.Sprites != null);
                Assert.That(atlasData.Sprites.Count == _correspondingAtlas.Sprites.Count);

                for(int i = 0; i < atlasData.Sprites.Count; i++) {
                    var loadedSprite = atlasData.Sprites[i];
                    var correspondingSprite = _correspondingAtlas.Sprites[i];

                    Assert.That(loadedSprite != null);
                    Assert.That(loadedSprite.Name == correspondingSprite.Name);

                    Assert.That(loadedSprite.X == correspondingSprite.X);
                    Assert.That(loadedSprite.Y == correspondingSprite.Y);
                    Assert.That(loadedSprite.Width == correspondingSprite.Width);
                    Assert.That(loadedSprite.Height == correspondingSprite.Height);

                    Assert.That(loadedSprite.OffsetX == correspondingSprite.OffsetX);
                    Assert.That(loadedSprite.OffsetY == correspondingSprite.OffsetY);

                    Assert.That(loadedSprite.OriginalWidth == correspondingSprite.OriginalWidth);
                    Assert.That(loadedSprite.OriginalHeight == correspondingSprite.OriginalHeight);

                    Assert.That(loadedSprite.RotationFlag == correspondingSprite.RotationFlag);
                    Assert.That(loadedSprite.Rotated == correspondingSprite.Rotated);
                }
            }
        }

        [Test]
        public void WhenAnimationIsCreatedFromTextureAtlasItIsValid() {
            var createdAnimation = _correspondingAtlas.CreateAnimation();
            Assert.That(createdAnimation.TextureId == _correspondingAtlas.ImagePath);
            Assert.That(createdAnimation.Sequences.Count == 8);
            Assert.That(!createdAnimation.GetVisibleSequences().Any());
            Assert.That(!createdAnimation.GetAnimatingSequences().Any());
            Assert.That(createdAnimation.GetMaxVisibleFrameSize() == TextureSize.Zero);
        }

        [SetUp]
        public void Init() {
            _correspondingAtlas = new TexturePackerAtlas();
            _correspondingAtlas.Width = _correspondingAtlas.Height = 256;
            _correspondingAtlas.ImagePath = "character texture.png";

            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "ducking west.png", X = 0, Y = 0, Width = 37, Height = 30, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "ducking east.png", X = 0, Y = 0, Width = 37, Height = 30, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = "y" });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "standing east.png", X = 37, Y = 0, Width = 31, Height = 49, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "standing north.png", X = 68, Y = 0, Width = 48, Height = 50, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "standing south.png", X = 116, Y = 0, Width = 43, Height = 49, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking east 1.png", X = 159, Y = 0, Width = 26, Height = 49, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking east 2.png", X = 185, Y = 0, Width = 29, Height = 50, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking east 3.png", X = 214, Y = 0, Width = 41, Height = 48, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking east 4.png", X = 0, Y = 50, Width = 28, Height = 50, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking east 5.png", X = 28, Y = 50, Width = 34, Height = 49, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking east 6.png", X = 62, Y = 50, Width = 42, Height = 48, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking north 1.png", X = 104, Y = 50, Width = 40, Height = 51, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking north 2.png", X = 144, Y = 50, Width = 44, Height = 48, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking north 3.png", X = 188, Y = 50, Width = 44, Height = 48, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking north 4.png", X = 0, Y = 101, Width = 43, Height = 47, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking north 5.png", X = 43, Y = 101, Width = 40, Height = 52, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking north 6.png", X = 83, Y = 101, Width = 33, Height = 55, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking south 1.png", X = 116, Y = 101, Width = 38, Height = 45, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking south 2.png", X = 154, Y = 101, Width = 41, Height = 50, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking south 3.png", X = 195, Y = 101, Width = 39, Height = 48, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking south 4.png", X = 0, Y = 156, Width = 43, Height = 45, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking south 5.png", X = 43, Y = 156, Width = 38, Height = 50, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
            _correspondingAtlas.Sprites.Add(new TexturePackerSprite { Name = "walking south 6.png", X = 81, Y = 156, Width = 37, Height = 48, OffsetX = 0, OffsetY = 0, OriginalWidth = 0, OriginalHeight = 0, RotationFlag = null });
        }

        private static Stream MakeStream(string s) {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
