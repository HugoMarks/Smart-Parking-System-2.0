using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SPS.Raspberry.Helpers
{
    public static class JsonHelper
    {
        public static T Deserialize<T>(string json)
        {
            var bytes = Encoding.Unicode.GetBytes(json);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());

                serializer.WriteObject(stream, instance);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
