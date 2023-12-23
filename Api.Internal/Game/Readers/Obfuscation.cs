using System.Runtime.InteropServices;

namespace Api.Internal.Game.Readers;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ObfuscatedLong
{
    private const int Size = sizeof(long);
    private bool isInit;
    private byte xorCount64;
    private byte xorCount8;
    private long xorKey;
    private byte valueIndex;
    private fixed long valueTable[4];

    public long Deobfuscate()
    {
        if (valueIndex is >= 4 or <= 0)
        {
            return 0;
        }
        
        var value = valueTable[valueIndex];

        var xor64 = xorCount64;
        var xor8 = xorCount8;

        if (xor64 > Size)
        {
            return 0x0;
        }
        
        if (xor8 > Size)
        {
            return 0x0;
        }
        
        fixed (long* pXorKey = &xorKey)
        {
            var xorValuePtr64 = (ulong*)pXorKey;
            for (var i = 0; i < xor64; i++)
                ((ulong*)&value)[i] ^= ~xorValuePtr64[i];

            var xorValuePtr8 = (byte*)pXorKey;
            for (var i = Size - xor8; i < Size; i++)
                ((byte*)&value)[i] ^= (byte)~xorValuePtr8[i];
        }

        return value;
    }
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ObfuscatedBool
{
    private const int Size = sizeof(byte);
    private bool isInit;
    private byte xorCount64;
    private byte xorCount8;
    private byte xorKey;
    private byte valueIndex;
    private fixed byte valueTable[4];

    public bool Deobfuscate()
    {
        if (valueIndex is >= 4 or <= 0)
        {
            return false;
        }
        
        var value = (int)valueTable[valueIndex];

        var xor64 = xorCount64;
        var xor8 = xorCount8;

        if (xor64 > Size)
        {
            return false;
        }
        
        if (xor8 > Size)
        {
            return false;
        }
        
        fixed (byte* pXorKey = &xorKey)
        {
            var xorValuePtr64 = (ulong*)pXorKey;
            for (var i = 0; i < xor64; i++)
                ((ulong*)&value)[i] ^= ~xorValuePtr64[i];

            var xorValuePtr8 = (byte*)pXorKey;
            for (var i = Size - xor8; i < Size; i++)
                ((byte*)&value)[i] ^= (byte)~xorValuePtr8[i];
        }

        /*
         long v5 = heroOffset + 0x274;
        int v8 = mem.getByte(v5 + mem.getByte(v5 + 4) + 5);
        long decryptionKey = mem.getLong(v5 + 3);
        isDead = (byte) (v8 ^ ~decryptionKey);
         */
        return value > 0;
    }
}