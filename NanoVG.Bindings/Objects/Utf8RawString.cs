using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NanoVG.Objects;

public class Utf8RawString : IDisposable
{
    public IntPtr GetPointer() => _ptr;
    public IntPtr GetEndAddress() => _end;
    
    private readonly IntPtr _ptr;
    private readonly IntPtr _end;
    
    public Utf8RawString(string s)
    {
        if (s == null)
        {
            _ptr = IntPtr.Zero;
            _end = IntPtr.Zero;
            return;
        }
        
        _ptr = StringToUtf8StringHGlobal(s, out var cb);
        _end = GetUtf8StringEndAddress(_ptr, cb);
    }
    
    public static IntPtr StringToUtf8StringHGlobal(string str, out int bytesCount)
    {
        bytesCount = 0;
        if (str == null)
            return IntPtr.Zero;

        var byteCount = Encoding.UTF8.GetByteCount(str);
        var ptr = Marshal.AllocHGlobal(byteCount + 1);
        if (ptr == IntPtr.Zero)
            return ptr;
        
        var buffer = Encoding.UTF8.GetBytes(str);
        Marshal.Copy(buffer, 0, ptr, byteCount);
        Marshal.WriteByte(ptr, byteCount, 0);

        bytesCount = byteCount;
        return ptr;
    }
    
    public static IntPtr GetUtf8StringEndAddress(IntPtr strPtr, int bytesCount)
    {
        if (strPtr == IntPtr.Zero)
            return IntPtr.Zero;
        
        return strPtr + bytesCount;
    }

    public void Dispose()
    {
        if(_ptr != IntPtr.Zero)
            Marshal.FreeHGlobal(_ptr);
    }
}