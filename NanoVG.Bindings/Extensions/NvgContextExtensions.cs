using System;
using System.Numerics;
using NanoVG.Objects;

namespace NanoVG.Extensions;

public static class NvgContextExtensions
{
    public static NvgDrawState NewState(this NvgContext ctx)
    {
        ctx.Save();
        return new NvgDrawState(ctx);
    }
    
    /// <summary>
    /// Render a text from a UTF8 string with auto-appended null-terminated byte
    /// </summary>
    /// <param name="ctx">NanoVG context</param>
    /// <param name="str">A UTF8 string without '\0'</param>
    /// <returns></returns>
    public static float Text(this NvgContext ctx, float x, float y, string str) =>
        ctx.Text(x, y, str + '\0', IntPtr.Zero);
    
    public static float TextBounds(this NvgContext ctx, float x, float y, string str, float[] bounds) =>
        ctx.TextBounds(x, y, str + '\0', IntPtr.Zero, bounds);

    public static float TextBounds(this NvgContext ctx, float x, float y, Utf8RawString str, float[] bounds) =>
        ctx.TextBounds(x, y, str.GetPointer(), str.GetEndAddress(), bounds);
    
    public static float TextBounds(this NvgContext ctx, float x, float y, Utf8RawString str, ref Vector4 bounds) =>
        ctx.TextBounds(x, y, str.GetPointer(), str.GetEndAddress(), ref bounds);

    public static void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, Utf8RawString str,
        ref float[] bounds)
        => ctx.TextBoxBounds(x, y, breakRowWidth, str.GetPointer(), str.GetEndAddress(), ref bounds);
    
    public static void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, Utf8RawString str,
        ref Vector4 bounds)
        => ctx.TextBoxBounds(x, y, breakRowWidth, str.GetPointer(), str.GetEndAddress(), ref bounds);
    
    public static float Text(this NvgContext ctx, float x, float y, Utf8RawString str) =>
        ctx.Text(x, y, str.GetPointer(), str.GetEndAddress());
    
    public static void TextBox(this NvgContext ctx, float x, float y, float breakRowWidth, Utf8RawString str)
        => ctx.TextBox(x, y, breakRowWidth, str.GetPointer(), str.GetEndAddress());
}