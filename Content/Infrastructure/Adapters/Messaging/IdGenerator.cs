using System.Security.Cryptography;

namespace Infrastructure.Adapters.Messaging;

internal static class IdGenerator
{
    private static readonly byte[] OID_NAMESPACE_BYTES = Guid.Parse("6ba7b812-9dad-11d1-80b4-00c04fd430c8").ToByteArray();

    static IdGenerator() => IdGenerator.SwapGuidByteOrder(IdGenerator.OID_NAMESPACE_BYTES);

    internal static Guid FromBytes(byte[] body)
    {
        return IdGenerator.GetGuidType5FromSHA1(IdGenerator.OID_NAMESPACE_BYTES, body);
    }

    private static Guid GetGuidType5FromSHA1(byte[] namespacePrefix, byte[] body)
    {
        byte[] numArray1 = new byte[namespacePrefix.Length + body.Length];
        Buffer.BlockCopy((Array) namespacePrefix, 0, (Array) numArray1, 0, namespacePrefix.Length);
        Buffer.BlockCopy((Array) body, 0, (Array) numArray1, namespacePrefix.Length, body.Length);
        using (HashAlgorithm hashAlgorithm = (HashAlgorithm) SHA1.Create())
        {
            byte[] hash = hashAlgorithm.ComputeHash(numArray1);
            byte[] numArray2 = new byte[16];
            Array.Copy((Array) hash, (Array) numArray2, 16);
            numArray2[6] &= (byte) 15;
            numArray2[6] |= (byte) 80;
            numArray2[8] &= (byte) 63;
            numArray2[8] |= (byte) 128;
            IdGenerator.SwapGuidByteOrder(numArray2);
            return new Guid(numArray2);
        }
    }

    private static void SwapGuidByteOrder(IList<byte> guid)
    {
        IdGenerator.SwapBytes(guid, 0, 3);
        IdGenerator.SwapBytes(guid, 1, 2);
        IdGenerator.SwapBytes(guid, 4, 5);
        IdGenerator.SwapBytes(guid, 6, 7);
    }

    private static void SwapBytes(IList<byte> array, int left, int right)
    {
        (array[left], array[right]) = (array[right], array[left]);
    }
}
