using System;
using System.Runtime.InteropServices;
using NVGColor = System.Numerics.Vector4;
using NVGVertex = System.Numerics.Vector4;

namespace NanoVG;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
sealed class FuncPtrAttribute : Attribute
{
    public Type DelegateType;
}

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate int renderCreateFunc(IntPtr uptr);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate int renderCreateTextureFunc(IntPtr uptr, int type, int w, int h, NvgImageFlags imageFlags, IntPtr data);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate int renderDeleteTextureFunc(IntPtr uptr, int image);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate int renderUpdateTextureFunc(IntPtr uptr, int image, int x, int y, int w, int h, IntPtr data);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate int renderGetTextureSizeFunc(IntPtr uptr, int image, out int w, out int h);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate void renderViewportFunc(IntPtr uptr, float width, float height, float devicePixelRatio);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate void renderCancelFunc(IntPtr uptr);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate void renderFlushFunc(IntPtr uptr);

[UnmanagedFunctionPointer(Nvg.CConv)]
delegate void renderDeleteFunc(IntPtr uptr);

// TODO
internal unsafe delegate void RenderFillFunc(IntPtr uPtr, NvgPaint* paint, NvgCompositeOpState op, NvgScissor* scsr,
    float fringe, float* bounds, NvgPath* paths, int num);

internal unsafe delegate void RenderStrokeFunc(IntPtr uPtr, NvgPaint* paint, NvgCompositeOpState op, NvgScissor* scsr,
    float fringe, float strokeWdth, NvgPath* paths, int num);

internal unsafe delegate void RenderTrianglesFunc(IntPtr uPtr, NvgPaint* paint, NvgCompositeOpState op,
    NvgScissor* scsr, NVGVertex* vrts, int num);

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NvgContext {
    public IntPtr Handle;
}

// TODO: Experimental
[StructLayout(LayoutKind.Sequential)]//, Pack = 1
public struct NvgTextRow {
    //[MarshalAs(UnmanagedType.LPStr)]
    /// <summary>
    /// Pointer to the input text where the row starts.
    /// </summary>
    public IntPtr start;
    
    //[MarshalAs(UnmanagedType.LPStr)]
    /// <summary>
    /// Pointer to the input text where the row ends (one past the last character).
    /// </summary>
    public IntPtr end;
    
    //[MarshalAs(UnmanagedType.LPStr)]
    /// <summary>
    /// Pointer to the beginning of the next row.
    /// </summary>
    public IntPtr next;	
    
    /// <summary>
    /// Logical width of the row.
    /// </summary>
    public float width;
    
    /// <summary>
    /// Actual bounds of the row. Logical with and bounds can differ because of kerning and some parts over extending.
    /// </summary>
    public float minx, maxx;
};

// TODO: Experimental
public struct NVGglyphPosition {
    //[MarshalAs(UnmanagedType.LPStr)]
    
    /// <summary>
    /// Position of the glyph in the input string.
    /// </summary>
    public IntPtr str;
    
    /// <summary>
    /// The x-coordinate of the logical glyph position.
    /// </summary>
    public float x;
    
    /// <summary>
    /// The bounds of the glyph shape.
    /// </summary>
    public float minx, maxx;
};

// TODO: no idea why this is not working
[StructLayout(LayoutKind.Sequential, Pack = 1)]
unsafe struct NvgParams
{
    public IntPtr UserPtr;
    public int EdgeAntiAlias;

    [FuncPtr(DelegateType = typeof(renderCreateFunc))]
    public IntPtr renderCreate;

    [FuncPtr(DelegateType = typeof(renderCreateTextureFunc))]
    public IntPtr renderCreateTexture;

    [FuncPtr(DelegateType = typeof(renderDeleteTextureFunc))]
    public IntPtr renderDeleteTexture;

    [FuncPtr(DelegateType = typeof(renderUpdateTextureFunc))]
    public IntPtr renderUpdateTexture;

    [FuncPtr(DelegateType = typeof(renderGetTextureSizeFunc))]
    public IntPtr renderGetTextureSize;

    [FuncPtr(DelegateType = typeof(renderViewportFunc))]
    public IntPtr renderViewport;

    [FuncPtr(DelegateType = typeof(renderCancelFunc))]
    public IntPtr renderCancel;

    [FuncPtr(DelegateType = typeof(renderFlushFunc))]
    public IntPtr renderFlush;

    [FuncPtr(DelegateType = typeof(RenderFillFunc))]
    public IntPtr renderFill;

    [FuncPtr(DelegateType = typeof(RenderStrokeFunc))]
    public IntPtr renderStroke;

    [FuncPtr(DelegateType = typeof(RenderTrianglesFunc))]
    public IntPtr renderTriangles;

    [FuncPtr(DelegateType = typeof(renderDeleteFunc))]
    public IntPtr renderDelete;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NvgCompositeOpState
{
    public int SrcRGB;
    public int DstRGB;
    public int SrcAlpha;
    public int DstAlpha;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NvgScissor
{
    //float xform[6];
    //float extent[2];

    public float XForm1;
    public float XForm2;
    public float XForm3;
    public float XForm4;
    public float XForm5;
    public float XForm6;

    public float Extent1;
    public float Extent2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NvgPaint
{
    public float XForm1;
    public float XForm2;
    public float XForm3;
    public float XForm4;
    public float XForm5;
    public float XForm6;

    public float Extent1;
    public float Extent2;

    public float Radius;
    public float Feather;

    public NVGColor InnerColor;
    public NVGColor OuterColor;

    public int Image;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct NvgPath
{
    public int First;
    public int Count;
    public byte Closed;
    public int NBevel;
    public NVGVertex* Fill;
    public int NFill;
    public NVGVertex* Stroke;
    public int NStroke;
    public int Winding;
    public int Convex;
}

// used to beat the strange issue with bindings
[StructLayout(LayoutKind.Sequential)]
internal struct NvgColor
{
    public float r;
    public float g;
    public float b;
    public float a;
}

public abstract unsafe class NvgParameters
{
    public abstract int RenderCreate(IntPtr UPtr);
    public abstract int RenderCreateTexture(IntPtr UPtr, int Type, int W, int H, NvgImageFlags ImageFlags, IntPtr Data);
    public abstract int RenderDeleteTexture(IntPtr UPtr, int Image);
    public abstract int RenderUpdateTexture(IntPtr UPtr, int Image, int X, int Y, int W, int H, IntPtr Data);
    public abstract int RenderGetTextureSize(IntPtr UPtr, int Image, out int W, out int H);
    public abstract void RenderViewport(IntPtr UPtr, float W, float H, float DevPixelRatio);
    public abstract void RenderCancel(IntPtr UPtr);
    public abstract void RenderFlush(IntPtr UPtr);
    public abstract void RenderDelete(IntPtr UPtr);

    // TODO
    public virtual void RenderFill(IntPtr UPtr, NvgPaint* Paint, NvgCompositeOpState Op, NvgScissor* Scsr, float Fringe,
        float* Bounds, NvgPath* Paths, int Num)
    {
        NvgPath[] PathsArr = new NvgPath[Num];
        for (int i = 0; i < Num; i++)
            PathsArr[i] = Paths[i];

        RenderFillSafe(UPtr, *Paint, Op, *Scsr, Fringe, new float[] { Bounds[0], Bounds[1], Bounds[2], Bounds[3] },
            PathsArr);
    }

    public virtual void RenderStroke(IntPtr UPtr, NvgPaint* Paint, NvgCompositeOpState Op, NvgScissor* Scsr,
        float Fringe, float StrokeWdth, NvgPath* Paths, int Num)
    {
        NvgPath[] PathsArr = new NvgPath[Num];
        for (int i = 0; i < Num; i++)
            PathsArr[i] = Paths[i];

        RenderStrokeSafe(UPtr, *Paint, Op, *Scsr, Fringe, StrokeWdth, PathsArr);
    }

    public virtual void RenderTriangles(IntPtr UPtr, NvgPaint* Paint, NvgCompositeOpState Op, NvgScissor* Scsr,
        NVGVertex* Vrts, int Num)
    {
        NVGVertex[] VrtsArr = new NVGVertex[Num];
        for (int i = 0; i < Num; i++)
            VrtsArr[i] = Vrts[i];

        RenderTrianglesSafe(UPtr, *Paint, Op, *Scsr, VrtsArr);
    }

    public abstract void RenderFillSafe(IntPtr UPtr, NvgPaint Paint, NvgCompositeOpState Op, NvgScissor Scsr,
        float Fringe, float[] Bounds, NvgPath[] Paths);

    public abstract void RenderStrokeSafe(IntPtr UPtr, NvgPaint Paint, NvgCompositeOpState Op, NvgScissor Scsr,
        float Fringe, float StrokeWdth, NvgPath[] Paths);

    public abstract void RenderTrianglesSafe(IntPtr UPtr, NvgPaint Paint, NvgCompositeOpState Op, NvgScissor Scsr,
        NVGVertex[] Vrts);
}