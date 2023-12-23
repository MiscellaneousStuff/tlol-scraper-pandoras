using System;
using System.Runtime.InteropServices;

namespace Api.Internal.Game.Objects;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public unsafe struct Obfuscation
{
    public byte IsFilled;
    public byte LengthXor32;
    public byte LengthXor8;
    public uint Key;
    public byte Index;
    public fixed byte Values[4];

    public uint Get()
    {
        try
        {
            if (IsFilled == 1)
                return default;

            var result = Values[Index];

            if (LengthXor32 > 0)
            {
                // for (byte i = 0; i < LengthXor32; i++)
                // {
                //     ((byte*)&result)[i] ^= (byte)(~((byte*)&Key)[i]);
                // }
                
                for (var i = 0; i < LengthXor32; i++)
                {
                    (&result)[i] ^= (byte)~((byte*)Key)[i];
                    //reinterpret_cast<PDWORD>(&tResult)[i] ^= ~(reinterpret_cast<PDWORD>(&this->m_tKey)[i]);
                }
            }
            else
            {
                return default;
            }
            return result;
        }
        catch
        {
            return default;
        }
    }
}