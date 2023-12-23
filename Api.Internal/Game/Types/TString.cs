using System.Runtime.InteropServices;

namespace Api.Internal.Game.Types;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TString
{
    public fixed byte _content[0x10];
    public ulong _contentLength;
    public ulong _maxContentLength;

    public Span<byte> GetSpan()
    {
        if (_contentLength is < 1 or > 128)
        {
            return new Span<byte>();
        }
        var length = Math.Min(0x10, (int)_contentLength);
        fixed (byte* ptr = _content)
        {
            return new Span<byte>(ptr, length);
        }
    }

    public IntPtr GetPtr()
    {
         fixed (byte* ptr = _content)
         {
             var ptrValue = *(long*)ptr;
             if (ptrValue < 0x1000)
             {
                 return IntPtr.Zero;
             }
             return new IntPtr(ptrValue);
         }
    }
}