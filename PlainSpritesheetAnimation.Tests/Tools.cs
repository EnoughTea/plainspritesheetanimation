using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace PlainSpritesheetAnimation.Tests {
    internal static class Tools {
        public static void CheckTextureRegionXml(XElement actual, TextureRegion expected) {
            Assert.That(actual != null);
            var x = actual.Element("x");
            Assert.That(x != null);
            var y = actual.Element("y");
            Assert.That(y != null);
            var w = actual.Element("w");
            Assert.That(w != null);
            var h = actual.Element("h");
            Assert.That(h != null);
            Assert.AreEqual(new TextureRegion(int.Parse(x.Value), int.Parse(y.Value), int.Parse(w.Value),
                int.Parse(h.Value)), expected);
        }

        public static void CheckTexturePointXml(XElement actual, TexturePoint expected) {
            Assert.That(actual != null);
            var x = actual.Element("x");
            Assert.That(x != null);
            var y = actual.Element("y");
            Assert.That(y != null);
            Assert.AreEqual(new TexturePoint(int.Parse(x.Value), int.Parse(y.Value)), expected);
        }
        public static void CheckTextureSizeXml(XElement actual, TextureSize expected) {
            Assert.That(actual != null);
            var w = actual.Element("w");
            Assert.That(w != null);
            var h = actual.Element("h");
            Assert.That(h != null);
            Assert.AreEqual(new TextureSize(int.Parse(w.Value), int.Parse(h.Value)), expected);
        }

        public static string SerializeToXml<T>(T target) {
            var serializer = new DataContractSerializer(typeof(T));
            var contentBuilder = new StringBuilder(1024);
            var settings = new XmlWriterSettings {
                Indent = true,
                Encoding = Encoding.Unicode,
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };

            using (var stream = XmlWriter.Create(contentBuilder, settings)) {
                serializer.WriteObject(stream, target);
            }

            return contentBuilder.ToString();
        }

        public static T DeserializeFromXml<T>(string xml) {
            var serializer = new DataContractSerializer(typeof(T));
            return (T) serializer.ReadObject(MakeStream(xml, Encoding.Unicode));
        }

        public static Stream MakeStream(string s, Encoding encoding = null) {
            if (encoding == null) { encoding = Encoding.UTF8; }

            return new MemoryStream(encoding.GetBytes(s ?? String.Empty));
        }
    }
}
