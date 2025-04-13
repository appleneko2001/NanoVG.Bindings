using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace NanoVG;

public unsafe partial class Nvg
{
    public static NvgContext? CreateGLES3(NvgCreateFlags flags) => _createGLES3!((int)flags);

    public static NvgContext? CreateGLES2(NvgCreateFlags flags) => _createGLES2!((int)flags);

    public static void DeleteGLES2(NvgContext ctx) => _deleteGLES2!(ctx);

    public static void DeleteGLES3(NvgContext ctx) => _deleteGLES3!(ctx);

    public static NvgContext CreateInternal(IntPtr @params) => _createInternal!(@params);

    public static void DeleteInternal(this NvgContext ctx) => _deleteInternal!(ctx);

    public static float Text(this NvgContext ctx, float x, float y, string str, IntPtr end) =>
        _text!(ctx, x, y, str, end);

    public static int CreateFont(this NvgContext ctx, string name, string filename) =>
        _createFont!(ctx, name, filename);

    public static void FontFace(this NvgContext ctx, string font) => _fontFace!(ctx, font);

    public static void BeginFrame(this NvgContext ctx, float windowWidth, float windowHeight, float devicePixelRatio) =>
        _beginFrame!(ctx, windowWidth, windowHeight, devicePixelRatio);

    public static void CancelFrame(this NvgContext ctx) => _cancelFrame!(ctx);

    public static void EndFrame(this NvgContext ctx) => _endFrame!(ctx);

    public static void GlobalCompositeOperation(this NvgContext ctx, int op) => _globalCompositeOperation!(ctx, op);

    public static void GlobalCompositeBlendFunc(this NvgContext ctx, int sfactor, int dfactor) =>
        _globalCompositeBlendFunc!(ctx, sfactor, dfactor);

    public static void GlobalCompositeBlendFuncSeparate(this NvgContext ctx, int srcRGB, int dstRGB, int srcAlpha,
        int dstAlpha) => _globalCompositeBlendFuncSeparate!(ctx, srcRGB, dstRGB, srcAlpha, dstAlpha);

    public static Vector4 RGB(byte r, byte g, byte b) => _rgb!(r, g, b);

    public static Vector4 RGBf(float r, float g, float b) => _rgBf!(r, g, b);

    public static Vector4 RGBA(byte r, byte g, byte b, byte a) => _rgba!(r, g, b, a);

    public static Vector4 RGBAf(float r, float g, float b, float a) => _rgbAf!(r, g, b, a);

    public static Vector4 LerpRGBA(Vector4 c0, Vector4 c1, float u) => _lerpRgba!(c0, c1, u);

    public static Vector4 TransRGBA(Vector4 c0, byte a) => _transRgba!(c0, a);

    public static Vector4 TransRGBAf(Vector4 c0, float a) => _transRgbAf!(c0, a);

    public static Vector4 HSL(float h, float s, float l) => _hsl!(h, s, l);

    public static Vector4 HSLA(float h, float s, float l, byte a) => _hsla!(h, s, l, a);

    public static void Save(this NvgContext ctx) => _save!(ctx);

    public static void Restore(this NvgContext ctx) => _restore!(ctx);

    public static void Reset(this NvgContext ctx) => _reset!(ctx);

    public static void ShapeAntiAlias(this NvgContext ctx, int enabled) => _shapeAntiAlias!(ctx, enabled);

    public static void StrokeColor(this NvgContext ctx, Vector4 color) => _strokeColor!(ctx, color);

    public static void StrokePaint(this NvgContext ctx, NvgPaint paint) => _strokePaint!(ctx, paint);

    public static void FillColor(this NvgContext ctx, Vector4 color) => _fillColor!(ctx, color);

    public static void FillPaint(this NvgContext ctx, NvgPaint paint) => _fillPaint!(ctx, paint);

    public static void MiterLimit(this NvgContext ctx, float limit) => _miterLimit!(ctx, limit);

    public static void StrokeWidth(this NvgContext ctx, float size) => _strokeWidth!(ctx, size);

    public static void LineCap(this NvgContext ctx, int cap) => _lineCap!(ctx, cap);

    public static void LineCap(this NvgContext ctx, NvgLineCap cap) => _lineCapEnum!(ctx, cap);

    public static void LineJoin(this NvgContext ctx, int join) => _lineJoin!(ctx, join);

    public static void LineJoin(this NvgContext ctx, NvgLineCap join) => _lineJoinEnum!(ctx, join);

    public static void GlobalAlpha(this NvgContext ctx, float alpha) => _globalAlpha!(ctx, alpha);

    public static void ResetTransform(this NvgContext ctx) => _resetTransform!(ctx);

    public static void Transform(this NvgContext ctx, float a, float b, float c, float d, float e, float f) =>
        _transform!(ctx, a, b, c, d, e, f);

    public static void Translate(this NvgContext ctx, float x, float y) => _translate!(ctx, x, y);

    public static void Rotate(this NvgContext ctx, float angle) => _rotate!(ctx, angle);

    public static void SkewX(this NvgContext ctx, float angle) => _skewX!(ctx, angle);

    public static void SkewY(this NvgContext ctx, float angle) => _skewY!(ctx, angle);

    public static void Scale(this NvgContext ctx, float x, float y) => _scale!(ctx, x, y);

    public static void CurrentTransform(this NvgContext ctx, float* xform) => _currentTransform!(ctx, xform);

    public static void TransformIdentity(float* dst) => _transformIdentity!(dst);

    public static void TransformTranslate(float* dst, float tx, float ty) => _transformTranslate!(dst, tx, ty);

    public static void TransformScale(float* dst, float sx, float sy) => _transformScale!(dst, sx, sy);

    public static void TransformRotate(float* dst, float a) => _transformRotate!(dst, a);

    public static void TransformSkewX(float* dst, float a) => _transformSkewX!(dst, a);

    public static void TransformSkewY(float* dst, float a) => _transformSkewY!(dst, a);

    public static void TransformMultiply(float* dst, float* src) => _transformMultiply!(dst, src);

    public static void TransformPremultiply(float* dst, float* src) => _transformPremultiply!(dst, src);

    public static int TransformInverse(float* dst, float* src) => _transformInverse!(dst, src);

    public static void TransformPoint(float* dstx, float* dsty, float* xform, float srcx, float srcy) =>
        _transformPoint!(dstx, dsty, xform, srcx, srcy);

    public static float DegToRad(float deg) => _degToRad!(deg);

    public static float RadToDeg(float rad) => _radToDeg!(rad);

    public static int CreateImage(this NvgContext ctx, byte[] filename, int imageFlags) =>
        _createImage!(ctx, filename, imageFlags);

    public static int CreateImageMem(this NvgContext ctx, int imageFlags, byte* data, int ndata) =>
        _createImageMem!(ctx, imageFlags, data, ndata);

    public static int CreateImageRGBA(this NvgContext ctx, int w, int h, int imageFlags, byte* data) =>
        _createImageRgba!(ctx, w, h, imageFlags, data);

    public static void UpdateImage(this NvgContext ctx, int image, byte* data) => _updateImage!(ctx, image, data);

    public static void ImageSize(this NvgContext ctx, int image, int* w, int* h) => _imageSize!(ctx, image, w, h);

    public static void ImageSize(this NvgContext ctx, int image, out int w, out int h) =>
        _imageSizeOut!(ctx, image, out w, out h);

    public static void DeleteImage(this NvgContext ctx, int image) => _deleteImage!(ctx, image);

    public static NvgPaint LinearGradient(this NvgContext ctx, float sx, float sy, float ex, float ey, Vector4 icol,
        Vector4 ocol) => _linearGradient!(ctx, sx, sy, ex, ey, icol, ocol);

    public static NvgPaint BoxGradient(this NvgContext ctx, float x, float y, float w, float h, float r, float f,
        Vector4 icol, Vector4 ocol) => _boxGradient!(ctx, x, y, w, h, r, f, icol, ocol);

    public static NvgPaint RadialGradient(this NvgContext ctx, float cx, float cy, float inr, float outr, Vector4 icol,
        Vector4 ocol) => _radialGradient!(ctx, cx, cy, inr, outr, icol, ocol);

    public static NvgPaint ImagePattern(this NvgContext ctx, float ox, float oy, float ex, float ey, float angle,
        int image, float alpha) => _imagePattern!(ctx, ox, oy, ex, ey, angle, image, alpha);

    public static void Scissor(this NvgContext ctx, float x, float y, float w, float h) => _scissor!(ctx, x, y, w, h);

    public static void IntersectScissor(this NvgContext ctx, float x, float y, float w, float h) =>
        _intersectScissor!(ctx, x, y, w, h);

    public static void ResetScissor(this NvgContext ctx) => _resetScissor!(ctx);

    public static void BeginPath(this NvgContext ctx) => _beginPath!(ctx);

    public static void MoveTo(this NvgContext ctx, float x, float y) => _moveTo!(ctx, x, y);

    public static void LineTo(this NvgContext ctx, float x, float y) => _lineTo!(ctx, x, y);

    public static void BezierTo(this NvgContext ctx, float c1x, float c1y, float c2x, float c2y, float x, float y) =>
        _bezierTo!(ctx, c1x, c1y, c2x, c2y, x, y);

    public static void QuadTo(this NvgContext ctx, float cx, float cy, float x, float y) => _quadTo!(ctx, cx, cy, x, y);

    public static void ArcTo(this NvgContext ctx, float x1, float y1, float x2, float y2, float radius) =>
        _arcTo!(ctx, x1, y1, x2, y2, radius);

    public static void ClosePath(this NvgContext ctx) => _closePath!(ctx);

    public static void PathWinding(this NvgContext ctx,NvgSolidity dir) => _pathWinding!(ctx, dir);

    public static void Arc(this NvgContext ctx, float cx, float cy, float r, float a0, float a1, NvgWinding dir) =>
        _arc!(ctx, cx, cy, r, a0, a1, dir);

    public static void Rect(this NvgContext ctx, float x, float y, float w, float h) => _rect!(ctx, x, y, w, h);

    public static void RoundedRect(this NvgContext ctx, float x, float y, float w, float h, float r) =>
        _roundedRect!(ctx, x, y, w, h, r);

    public static void RoundedRectVarying(this NvgContext ctx, float x, float y, float w, float h, float radTopLeft,
        float radTopRight, float radBottomRight, float radBottomLeft) => _roundedRectVarying!(ctx, x, y, w, h,
        radTopLeft, radTopRight, radBottomRight, radBottomLeft);

    public static void Ellipse(this NvgContext ctx, float cx, float cy, float rx, float ry) =>
        _ellipse!(ctx, cx, cy, rx, ry);

    public static void Circle(this NvgContext ctx, float cx, float cy, float r) => _circle!(ctx, cx, cy, r);

    public static void Fill(this NvgContext ctx) => _fill!(ctx);

    public static void Stroke(this NvgContext ctx) => _stroke!(ctx);

//public static int CreateFont(this NvgContext ctx, string name, string filename) => _createFontByteArray!(ctx, name, filename);

    public static int CreateFontMem(this NvgContext ctx, string name, IntPtr data, int nData, int freeData) =>
        _createFontMem!(ctx, name, data, nData, freeData);

    public static int FindFont(this NvgContext ctx, string name) => _findFont!(ctx, name);

    public static int AddFallbackFontId(this NvgContext ctx, int baseFont, int fallbackFont) =>
        _addFallbackFontId!(ctx, baseFont, fallbackFont);

    public static int AddFallbackFont(this NvgContext ctx, byte[] baseFont, byte[] fallbackFont) =>
        _addFallbackFont!(ctx, baseFont, fallbackFont);

    public static void FontSize(this NvgContext ctx, float size) => _fontSize!(ctx, size);

    public static void FontBlur(this NvgContext ctx, float blur) => _fontBlur!(ctx, blur);

    public static void TextLetterSpacing(this NvgContext ctx, float spacing) => _textLetterSpacing!(ctx, spacing);

    public static void TextLineHeight(this NvgContext ctx, float lineHeight) => _textLineHeight!(ctx, lineHeight);

    public static void TextAlign(this NvgContext ctx, NvgAlign align) => _textAlign!(ctx, align);

    public static void FontFaceId(this NvgContext ctx, int font) => _fontFaceId!(ctx, font);

    public static void FontFace(this NvgContext ctx, byte[] font) => _fontFaceByteArray!(ctx, font);

    public static float Text(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end) =>
        _textIntPtr!(ctx, x, y, str, end);

    public static float Text(this NvgContext ctx, float x, float y, void* str, void* end) =>
        _textVoidPtr!(ctx, x, y, str, end);

    public static void TextBox(this NvgContext ctx, float x, float y, float breakRowWidth, IntPtr str, IntPtr end) =>
        _textBox!(ctx, x, y, breakRowWidth, str, end);

    public static float
        TextBounds(this NvgContext ctx, float x, float y, string str, IntPtr end, [Out] float[] bounds) =>
        _textBoundsString!(ctx, x, y, str, end, bounds);

    public static float
        TextBounds(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end, [Out] float[] bounds) =>
        _textBoundsIntPtrArray!(ctx, x, y, str, end, bounds);

    public static float TextBounds(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end, ref Vector4 bounds) =>
        _textBoundsIntPtrVector4!(ctx, x, y, str, end, ref bounds);

    public static void TextMetrics(this NvgContext ctx, float* ascender, float* descender, float* lineH) =>
        _textMetrics!(ctx, ascender, descender, lineH);

    public static void TextMetrics(this NvgContext ctx, out float ascender, out float descender, out float lineH) =>
        _textMetricsOut!(ctx, out ascender, out descender, out lineH);

    public static int TextBreakLines(this NvgContext ctx, IntPtr str, IntPtr endOfStr, float breakRowWidth,
        [Out] NvgTextRow[] rows, int maxRows) => _textBreakLines!(ctx, str, endOfStr, breakRowWidth, rows, maxRows);

    public static int TextGlyphPositions(this NvgContext ctx, float x, float y, void* str, void* end,
        NVGglyphPosition[] positions, int maxPositions) =>
        _textGlyphPositionsVoidPtr!(ctx, x, y, str, end, positions, maxPositions);

    public static int TextGlyphPositions(this NvgContext ctx, float x, float y, IntPtr str, IntPtr end,
        [Out] NVGglyphPosition[] positions, int maxPositions) =>
        _textGlyphPositionsIntPtr!(ctx, x, y, str, end, positions, maxPositions);

    public static int CreateImage(this NvgContext ctx, string filename, int imageFlags) =>
        _createImageString!(ctx, filename, imageFlags);

    public static void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, IntPtr str,
        IntPtr endOfStr, ref float[] bounds) =>
        _textBoxBoundsFloatArray!(ctx, x, y, breakRowWidth, str, endOfStr, ref bounds);

    public static void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, IntPtr str,
        IntPtr endOfStr, ref Vector4 bounds) =>
        _textBoxBoundsVector4!(ctx, x, y, breakRowWidth, str, endOfStr, ref bounds);

    public static void TextBoxBounds(this NvgContext ctx, float x, float y, float breakRowWidth, string str,
        IntPtr endOfStr, ref Vector4 bounds) =>
        _textBoxBoundsString!(ctx, x, y, breakRowWidth, str, endOfStr, ref bounds);
}