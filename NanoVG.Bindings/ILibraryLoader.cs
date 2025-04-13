using System;

namespace NanoVG;

public interface ILibraryLoader
{
    IntPtr? Load(string libName);
    IntPtr? GetFunction(IntPtr pLib, string procName);
    void Free(IntPtr pLib);
}