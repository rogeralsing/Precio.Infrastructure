using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Precio.Serialization;

namespace Precio.Domain
{
    internal static class InterfaceExtensions
    {
        internal static T Clone<T>(this T self) where T : ValueObject, new()
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.SurrogateSelector = new SurrogateSelector();
            formatter.SurrogateSelector.ChainSelector(
                new NonSerialiazableTypeSurrogateSelector());
            var ms = new MemoryStream();
            formatter.Serialize(ms, self);
            ms.Position = 0;
            return (T) formatter.Deserialize(ms);
        }

        internal static bool Compare<T>(this T self, T other) where T : ValueObject
        {
            byte[] bytes1 = GetBytes(self);
            byte[] bytes2 = GetBytes(other);

            if (bytes1.Length != bytes1.Length)
                return false;

            for (int i = 0; i < bytes1.Length; i++)
                if (bytes1[i] != bytes2[i])
                    return false;

            return true;
        }

        private static byte[] GetBytes<T>(T obj) where T : ValueObject
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.SurrogateSelector = new SurrogateSelector();
            formatter.SurrogateSelector.ChainSelector(
                new NonSerialiazableTypeSurrogateSelector());

            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return ms.ToArray();
            }
        }

        internal static int ComputeHash<T>(this T obj) where T : ValueObject
        {
            byte[] data = GetBytes(obj);
            unchecked
            {
                const int p = 16777619;
                var hash = (int) 2166136261;
                for (int i = 0; i < data.Length; i++) hash = (hash ^ data[i])*p;
                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }
    }
}