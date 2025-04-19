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
    private readonly int _maxBytesLength;
    
    public Utf8RawString(string s, int maxBytesLength = -1)
    {
        if (s == null)
        {
            _ptr = IntPtr.Zero;
            _end = IntPtr.Zero;
            return;
        }

        _maxBytesLength = maxBytesLength < 0 ? s.Length : maxBytesLength;
        _ptr = StringToUtf8StringHGlobal(s, maxBytesLength, out var cb);
        _end = GetUtf8StringEndAddress(_ptr, cb);
    }

    /// <summary>
    /// Update the buffer string, not recommended to use if you are created this <see cref="Utf8RawString"/> instance by using non-fixed max length buffer.
    /// </summary>
    /// <param name="s">new string (if your string is longer than buffer will be trimmed automatically)</param>
    public void UpdateData(string s)
    {
        UpdateData(Encoding.UTF8.GetBytes(s));
    }

    public void UpdateData(byte[] buf)
    {
        UpdateDataPrivate(buf, _ptr, Math.Min(buf.Length, _maxBytesLength));
    }

    private static void UpdateDataPrivate(string str, IntPtr ptr, int inputLen)
    {
        UpdateDataPrivate(Encoding.UTF8.GetBytes(str), ptr, inputLen);
    }
    
    private static void UpdateDataPrivate(byte[] buf, IntPtr ptr, int inputLen)
    {
        // copy all data to the buffer
        Marshal.Copy(buf, 0, ptr, inputLen);
    }
    
    public static IntPtr StringToUtf8StringHGlobal(string str, int maxLen, out int bytesCount)
    {
        bytesCount = 0;
        if (str == null)
            return IntPtr.Zero;

        var inputLen = Encoding.UTF8.GetByteCount(str);
        var byteCount = maxLen < 0 ? inputLen : maxLen;
        
        var ptr = Marshal.AllocHGlobal(byteCount + 1);
        if (ptr == IntPtr.Zero)
            return ptr;

        UpdateDataPrivate(str, ptr, inputLen);
        
        // put null-terminator at the end of buffer
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

    public override string ToString()
    {
        return ToString(true);
    }

    public string ToString(bool getBufferData)
    {
        return getBufferData ? GetStringFromBuffer() : $"{_maxBytesLength} bytes buffer";
    }

    protected string GetStringFromBuffer()
    {
        var buf = GetBufferDataPrivate();
        return Encoding.UTF8.GetString(buf, 0, buf.Length);
    }
    
    protected byte[] GetBufferDataPrivate()
    {
        var len = _maxBytesLength;
        var buf = new byte[len];
        for (var i = 0; i < len; i++)
        {
            buf[i] = Marshal.ReadByte(_ptr, i);
        }

        return buf;
    }
}