using System.Xml;
using System.Xml.Serialization;

namespace DarcEuphoria.Euphoric.Configs
{
    public static class Handler
    {
        public static void Save<t>(object instance, string output)
        {
            var serializer = new XmlSerializer(typeof(t));
            using (var writer = XmlWriter.Create(output, new XmlWriterSettings {Indent = true}))
            {
                serializer.Serialize(writer, instance);
            }
        }

        public static t Load<t>(string input)
        {
            var serializer = new XmlSerializer(typeof(t));

            t buffer;

            using (var reader = XmlReader.Create(input))
            {
                buffer = (t) serializer.Deserialize(reader);
            }

            return buffer;
        }
    }
}