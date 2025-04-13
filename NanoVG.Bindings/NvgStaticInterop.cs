#if NVG_STATIC_INTEROP
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NanoVG;

internal static unsafe partial class Nvg
{
    private const string FuncPrefix = "nvg";
    private const string LibName = "libNanoVG_GLES";
    internal const CallingConvention CConv = CallingConvention.Cdecl;

    // May or may not exist
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateGLES3))]
    public static extern NvgContext CreateGLES3(int flags);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateGLES2))]
    public static extern NvgContext CreateGLES2(int flags);
    
    public static NvgContext? CreateGLES2(NvgCreateFlags flags) => CreateGLES2((int)flags);

    public static NvgContext? CreateGLES3(NvgCreateFlags flags) => CreateGLES3((int)flags);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DeleteGLES2))]
    public static extern void DeleteGLES2(NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DeleteGLES3))]
    public static extern void DeleteGLES3(NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateInternal))]
    public static extern NvgContext CreateInternal(IntPtr @params);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DeleteInternal))]
    public static extern void DeleteInternal(NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Text))]
    public static extern float Text(this NvgContext ctx, float x, float y, [MarshalAs(UnmanagedType.LPStr)] string str,
        IntPtr end);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFont))]
    public static extern int CreateFont(this NvgContext ctx, [MarshalAs(UnmanagedType.LPStr)] string name,
        [MarshalAs(UnmanagedType.LPStr)] string filename);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontFace))]
    public static extern void FontFace(this NvgContext ctx, [MarshalAs(UnmanagedType.LPStr)] string font);

    #region AUTO_GENERATED

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BeginFrame))]
    public static extern void BeginFrame(this NvgContext ctx, float windowWidth, float windowHeight,
        float devicePixelRatio);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CancelFrame))]
    public static extern void CancelFrame(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(EndFrame))]
    public static extern void EndFrame(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalCompositeOperation))]
    public static extern void GlobalCompositeOperation(this NvgContext ctx, int op);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalCompositeBlendFunc))]
    public static extern void GlobalCompositeBlendFunc(this NvgContext ctx, int sfactor, int dfactor);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalCompositeBlendFuncSeparate))]
    public static extern void GlobalCompositeBlendFuncSeparate(this NvgContext ctx, int srcRGB, int dstRGB,
        int srcAlpha, int dstAlpha);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGB))]
    public static extern Vector4 RGB(byte r, byte g, byte b);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGBf))]
    public static extern Vector4 RGBf(float r, float g, float b);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGBA))]
    public static extern Vector4 RGBA(byte r, byte g, byte b, byte a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGBAf))]
    public static extern Vector4 RGBAf(float r, float g, float b, float a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LerpRGBA))]
    public static extern Vector4 LerpRGBA(Vector4 c0, Vector4 c1, float u);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransRGBA))]
    public static extern Vector4 TransRGBA(Vector4 c0, byte a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransRGBAf))]
    public static extern Vector4 TransRGBAf(Vector4 c0, float a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(HSL))]
    public static extern Vector4 HSL(float h, float s, float l);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(HSLA))]
    public static extern Vector4 HSLA(float h, float s, float l, byte a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Save))]
    public static extern void Save(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Restore))]
    public static extern void Restore(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Reset))]
    public static extern void Reset(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ShapeAntiAlias))]
    public static extern void ShapeAntiAlias(this NvgContext ctx, int enabled);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(StrokeColor))]
    public static extern void StrokeColor(this NvgContext ctx, Vector4 color);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(StrokePaint))]
    public static extern void StrokePaint(this NvgContext ctx, NvgPaint paint);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FillColor))]
    public static extern void FillColor(this NvgContext ctx, Vector4 color);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FillPaint))]
    public static extern void FillPaint(this NvgContext ctx, NvgPaint paint);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(MiterLimit))]
    public static extern void MiterLimit(this NvgContext ctx, float limit);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(StrokeWidth))]
    public static extern void StrokeWidth(this NvgContext ctx, float size);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineCap))]
    public static extern void LineCap(this NvgContext ctx, int cap);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineCap))]
    public static extern void LineCap(this NvgContext ctx, NvgLineCap cap);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineJoin))]
    public static extern void LineJoin(this NvgContext ctx, int join);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineJoin))]
    public static extern void LineJoin(this NvgContext ctx, NvgLineCap join);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalAlpha))]
    public static extern void GlobalAlpha(this NvgContext ctx, float alpha);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ResetTransform))]
    public static extern void ResetTransform(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Transform))]
    public static extern void Transform(this NvgContext ctx, float a, float b, float c, float d, float e, float f);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Translate))]
    public static extern void Translate(this NvgContext ctx, float x, float y);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Rotate))]
    public static extern void Rotate(this NvgContext ctx, float angle);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(SkewX))]
    public static extern void SkewX(this NvgContext ctx, float angle);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(SkewY))]
    public static extern void SkewY(this NvgContext ctx, float angle);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Scale))]
    public static extern void Scale(this NvgContext ctx, float x, float y);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CurrentTransform))]
    public static extern void CurrentTransform(this NvgContext ctx, float* xform);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformIdentity))]
    public static extern void TransformIdentity(float* dst);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformTranslate))]
    public static extern void TransformTranslate(float* dst, float tx, float ty);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformScale))]
    public static extern void TransformScale(float* dst, float sx, float sy);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformRotate))]
    public static extern void TransformRotate(float* dst, float a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformSkewX))]
    public static extern void TransformSkewX(float* dst, float a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformSkewY))]
    public static extern void TransformSkewY(float* dst, float a);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformMultiply))]
    public static extern void TransformMultiply(float* dst, float* src);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformPremultiply))]
    public static extern void TransformPremultiply(float* dst, float* src);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformInverse))]
    public static extern int TransformInverse(float* dst, float* src);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformPoint))]
    public static extern void TransformPoint(float* dstx, float* dsty, float* xform, float srcx, float srcy);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DegToRad))]
    public static extern float DegToRad(float deg);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RadToDeg))]
    public static extern float RadToDeg(float rad);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImage))]
    public static extern int CreateImage(this NvgContext ctx, byte[] filename, int imageFlags);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImageMem))]
    public static extern int CreateImageMem(this NvgContext ctx, int imageFlags, byte* data, int ndata);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImageRGBA))]
    public static extern int CreateImageRGBA(this NvgContext ctx, int w, int h, int imageFlags, byte* data);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(UpdateImage))]
    public static extern void UpdateImage(this NvgContext ctx, int image, byte* data);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ImageSize))]
    public static extern void ImageSize(this NvgContext ctx, int image, int* w, int* h);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ImageSize))]
    public static extern void ImageSize(this NvgContext ctx, int image, out int w, out int h);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DeleteImage))]
    public static extern void DeleteImage(this NvgContext ctx, int image);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LinearGradient))]
    public static extern NvgPaint LinearGradient(this NvgContext ctx, float sx, float sy, float ex, float ey,
        Vector4 icol, Vector4 ocol);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BoxGradient))]
    public static extern NvgPaint BoxGradient(this NvgContext ctx, float x, float y, float w, float h, float r,
        float f, Vector4 icol, Vector4 ocol);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RadialGradient))]
    public static extern NvgPaint RadialGradient(this NvgContext ctx, float cx, float cy, float inr, float outr,
        Vector4 icol, Vector4 ocol);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ImagePattern))]
    public static extern NvgPaint ImagePattern(this NvgContext ctx, float ox, float oy, float ex, float ey,
        float angle, int image, float alpha);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Scissor))]
    public static extern void Scissor(this NvgContext ctx, float x, float y, float w, float h);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(IntersectScissor))]
    public static extern void IntersectScissor(this NvgContext ctx, float x, float y, float w, float h);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ResetScissor))]
    public static extern void ResetScissor(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BeginPath))]
    public static extern void BeginPath(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(MoveTo))]
    public static extern void MoveTo(this NvgContext ctx, float x, float y);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineTo))]
    public static extern void LineTo(this NvgContext ctx, float x, float y);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BezierTo))]
    public static extern void BezierTo(this NvgContext ctx, float c1x, float c1y, float c2x, float c2y, float x,
        float y);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(QuadTo))]
    public static extern void QuadTo(this NvgContext ctx, float cx, float cy, float x, float y);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ArcTo))]
    public static extern void ArcTo(this NvgContext ctx, float x1, float y1, float x2, float y2, float radius);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ClosePath))]
    public static extern void ClosePath(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(PathWinding))]
    public static extern void PathWinding(this NvgContext ctx, NvgSolidity dir);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Arc))]
    public static extern void Arc(this NvgContext ctx, float cx, float cy, float r, float a0, float a1,
        NvgWinding dir);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Rect))]
    public static extern void Rect(this NvgContext ctx, float x, float y, float w, float h);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RoundedRect))]
    public static extern void RoundedRect(this NvgContext ctx, float x, float y, float w, float h, float r);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RoundedRectVarying))]
    public static extern void RoundedRectVarying(this NvgContext ctx, float x, float y, float w, float h,
        float radTopLeft, float radTopRight, float radBottomRight, float radBottomLeft);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Ellipse))]
    public static extern void Ellipse(this NvgContext ctx, float cx, float cy, float rx, float ry);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Circle))]
    public static extern void Circle(this NvgContext ctx, float cx, float cy, float r);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Fill))]
    public static extern void Fill(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Stroke))]
    public static extern void Stroke(this NvgContext ctx);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFont))]
    public static extern int CreateFont(this NvgContext ctx, byte[] name, byte[] filename);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFontMem))]
    public static extern int CreateFontMem(this NvgContext ctx, byte[] name, byte* data, int ndata, int freeData);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FindFont))]
    public static extern int FindFont(this NvgContext ctx, byte[] name);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(AddFallbackFontId))]
    public static extern int AddFallbackFontId(this NvgContext ctx, int baseFont, int fallbackFont);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(AddFallbackFont))]
    public static extern int AddFallbackFont(this NvgContext ctx, byte[] baseFont, byte[] fallbackFont);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontSize))]
    public static extern void FontSize(this NvgContext ctx, float size);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontBlur))]
    public static extern void FontBlur(this NvgContext ctx, float blur);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextLetterSpacing))]
    public static extern void TextLetterSpacing(this NvgContext ctx, float spacing);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextLineHeight))]
    public static extern void TextLineHeight(this NvgContext ctx, float lineHeight);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextAlign))]
    public static extern void TextAlign(this NvgContext ctx, NvgAlign align);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontFaceId))]
    public static extern void FontFaceId(this NvgContext ctx, int font);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontFace))]
    public static extern void FontFace(this NvgContext ctx, byte[] font);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Text))]
    public static extern float Text(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Text))]
    public static extern float Text(this NvgContext ctx, float x, float y, void* str, void* end);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBox))]
    public static extern void TextBox(this NvgContext ctx, float x, float y, float breakRowWidth, IntPtr str,
        IntPtr end);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBounds))]
    public static extern float TextBounds(this NvgContext ctx, float x, float y, [MarshalAs(UnmanagedType.LPStr)]
        string str, IntPtr end,
        [Out] float[] bounds);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBounds))]
    public static extern float TextBounds(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end,
        [Out] float[] bounds);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBounds))]
    public static extern float TextBounds(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end,
        ref Vector4 bounds);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextMetrics))]
    public static extern void TextMetrics(this NvgContext ctx, float* ascender, float* descender, float* lineH);
    
    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextMetrics))]
    public static extern void TextMetrics(this NvgContext ctx, out float ascender, out float descender, out float lineH);


    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBreakLines))]
    public static extern int TextBreakLines(this NvgContext ctx, IntPtr str, IntPtr endOfStr, float breakRowWidth,
        [Out] NvgTextRow[] rows, int maxRows);
    
    // TODO: experimental APIs

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextGlyphPositions))]
    public static extern int TextGlyphPositions(this NvgContext ctx, float x, float y, void* str, void* end,
        NVGglyphPosition[] positions, int maxPositions);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextGlyphPositions))]
    public static extern int TextGlyphPositions(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end,
        [Out] NVGglyphPosition[] positions, int maxPositions);


    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImage))]
    public static extern int CreateImage(this NvgContext ctx, string filename, int imageFlags);


    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBoxBounds))]
    public static extern void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, IntPtr str,
        IntPtr endOfStr, ref float[] bounds);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBoxBounds))]
    public static extern void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, IntPtr str,
        IntPtr endOfStr, ref Vector4 bounds);

    [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBoxBounds))]
    public static extern void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth,
        [MarshalAs(UnmanagedType.LPStr)] string str,
        IntPtr endOfStr, ref Vector4 bounds);



    #endregion
}

// TODO: consider to remove
public static partial class Nvg
{
    public static NvgContext CreateContext(NvgParameters paramsImpl, IntPtr? userPtr = null, bool edgeAntiAlias = true)
    {
        object NvgParamsObj = new NvgParams();

        var ImplType = paramsImpl.GetType();
        var Fields = typeof(NvgParams).GetFields();

        // Convert all interface methods to delegates and populate the struct
        foreach (var Field in Fields)
        {
            FuncPtrAttribute FuncPtr;

            // If it's not a function pointer (delegate) field, continue
            if ((FuncPtr = Field.GetCustomAttribute<FuncPtrAttribute>()) == null)
                continue;

            var ImplMethodName = char.ToUpper(Field.Name[0]) + Field.Name.Substring(1);
            var ImplMethod = ImplType.GetMethod(ImplMethodName);

            var D = Delegate.CreateDelegate(FuncPtr.DelegateType, paramsImpl, ImplMethod);
            GCHandle.Alloc(D);

            Field.SetValue(NvgParamsObj, Marshal.GetFunctionPointerForDelegate(D));
        }

        var NvgParams = (NvgParams)NvgParamsObj;
        NvgParams.EdgeAntiAlias = edgeAntiAlias ? 1 : 0;
        NvgParams.UserPtr = userPtr ?? IntPtr.Zero;

        // Fix if changes
        // nvgCreateInternal copies the struct, so i don't need to keep the pointer to the object
        var NvgParamsHandle = GCHandle.Alloc(NvgParams, GCHandleType.Pinned);
        var ctx = CreateInternal(NvgParamsHandle.AddrOfPinnedObject());
        NvgParamsHandle.Free();
        return ctx;
    }


}

#endif