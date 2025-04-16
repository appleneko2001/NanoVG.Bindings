#if NVG_DYNAMIC_INTEROP
#nullable enable

using System;
using System.Runtime.InteropServices;

namespace NanoVG;

public static partial class Nvg
{
    private const string FuncPrefix = "nvg";
    public const string LibNamePrefix = "libNanoVG";
    private const string LibName = $"{LibNamePrefix}_GLES";
    internal const CallingConvention CConv = CallingConvention.Cdecl;

    private static ILibraryLoader? _loader;
    private static IntPtr? _libHandle;

    // Delegate fields
    private static CreateGLES3Delegate? _createGLES3;
    private static CreateGLES2Delegate? _createGLES2;
    private static DeleteGLES2Delegate? _deleteGLES2;
    private static DeleteGLES3Delegate? _deleteGLES3;
    private static CreateInternalDelegate? _createInternal;
    private static DeleteInternalDelegate? _deleteInternal;
    private static TextDelegate? _text;
    private static CreateFontDelegate? _createFont;
    private static FontFaceDelegate? _fontFace;
    private static BeginFrameDelegate? _beginFrame;
    private static CancelFrameDelegate? _cancelFrame;
    private static EndFrameDelegate? _endFrame;
    private static GlobalCompositeOperationDelegate? _globalCompositeOperation;
    private static GlobalCompositeBlendFuncDelegate? _globalCompositeBlendFunc;
    private static GlobalCompositeBlendFuncSeparateDelegate? _globalCompositeBlendFuncSeparate;
    private static RGBDelegate? _rgb;
    private static RGBfDelegate? _rgBf;
    private static RGBADelegate? _rgba;
    private static RGBAfDelegate? _rgbAf;
    private static LerpRGBADelegate? _lerpRgba;
    private static TransRGBADelegate? _transRgba;
    private static TransRGBAfDelegate? _transRgbAf;
    private static HSLDelegate? _hsl;
    private static HSLADelegate? _hsla;
    private static SaveDelegate? _save;
    private static RestoreDelegate? _restore;
    private static ResetDelegate? _reset;
    private static ShapeAntiAliasDelegate? _shapeAntiAlias;
    private static StrokeColorDelegate? _strokeColor;
    private static StrokePaintDelegate? _strokePaint;
    private static FillColorDelegate? _fillColor;
    private static FillPaintDelegate? _fillPaint;
    private static MiterLimitDelegate? _miterLimit;
    private static StrokeWidthDelegate? _strokeWidth;
    private static LineCapDelegate? _lineCap;
    private static LineCapEnumDelegate? _lineCapEnum;
    private static LineJoinDelegate? _lineJoin;
    private static LineJoinEnumDelegate? _lineJoinEnum;
    private static GlobalAlphaDelegate? _globalAlpha;
    private static ResetTransformDelegate? _resetTransform;
    private static TransformDelegate? _transform;
    private static TranslateDelegate? _translate;
    private static RotateDelegate? _rotate;
    private static SkewXDelegate? _skewX;
    private static SkewYDelegate? _skewY;
    private static ScaleDelegate? _scale;
    private static CurrentTransformDelegate? _currentTransform;
    private static CurrentTransformMatDelegate? _currentTransformMat;
    private static TransformIdentityDelegate? _transformIdentity;
    private static TransformTranslateDelegate? _transformTranslate;
    private static TransformScaleDelegate? _transformScale;
    private static TransformRotateDelegate? _transformRotate;
    private static TransformSkewXDelegate? _transformSkewX;
    private static TransformSkewYDelegate? _transformSkewY;
    private static TransformMultiplyDelegate? _transformMultiply;
    private static TransformPremultiplyDelegate? _transformPremultiply;
    private static TransformInverseDelegate? _transformInverse;
    private static TransformPointDelegate? _transformPoint;
    private static DegToRadDelegate? _degToRad;
    private static RadToDegDelegate? _radToDeg;
    private static CreateImageDelegate? _createImage;
    private static CreateImageMemDelegate? _createImageMem;
    private static CreateImageRGBADelegate? _createImageRgba;
    private static UpdateImageDelegate? _updateImage;
    private static ImageSizeDelegate? _imageSize;
    private static ImageSizeOutDelegate? _imageSizeOut;
    private static DeleteImageDelegate? _deleteImage;
    private static LinearGradientDelegate? _linearGradient;
    private static BoxGradientDelegate? _boxGradient;
    private static RadialGradientDelegate? _radialGradient;
    private static ImagePatternDelegate? _imagePattern;
    private static ScissorDelegate? _scissor;
    private static IntersectScissorDelegate? _intersectScissor;
    private static ResetScissorDelegate? _resetScissor;
    private static BeginPathDelegate? _beginPath;
    private static MoveToDelegate? _moveTo;
    private static LineToDelegate? _lineTo;
    private static BezierToDelegate? _bezierTo;
    private static QuadToDelegate? _quadTo;
    private static ArcToDelegate? _arcTo;
    private static ClosePathDelegate? _closePath;
    private static PathWindingDelegate? _pathWinding;
    private static ArcDelegate? _arc;
    private static RectDelegate? _rect;
    private static RoundedRectDelegate? _roundedRect;
    private static RoundedRectVaryingDelegate? _roundedRectVarying;
    private static EllipseDelegate? _ellipse;
    private static CircleDelegate? _circle;
    private static FillDelegate? _fill;
    private static StrokeDelegate? _stroke;
    private static CreateFontRawDelegate? _createFontByteArray;
    private static CreateFontMemDelegate? _createFontMem;
    private static FindFontDelegate? _findFont;
    private static AddFallbackFontIdDelegate? _addFallbackFontId;
    private static AddFallbackFontDelegate? _addFallbackFont;
    private static FontSizeDelegate? _fontSize;
    private static FontBlurDelegate? _fontBlur;
    private static TextLetterSpacingDelegate? _textLetterSpacing;
    private static TextLineHeightDelegate? _textLineHeight;
    private static TextAlignDelegate? _textAlign;
    private static FontFaceIdDelegate? _fontFaceId;
    private static FontFaceByteArrayDelegate? _fontFaceByteArray;
    private static TextIntPtrDelegate? _textIntPtr;
    private static TextVoidPtrDelegate? _textVoidPtr;
    private static TextBoxDelegate? _textBox;
    private static TextBoundsStringDelegate? _textBoundsString;
    private static TextBoundsIntPtrArrayDelegate? _textBoundsIntPtrArray;
    private static TextBoundsIntPtrVector4Delegate? _textBoundsIntPtrVector4;
    private static TextMetricsDelegate? _textMetrics;
    private static TextMetricsOutDelegate? _textMetricsOut;
    private static TextBreakLinesDelegate? _textBreakLines;
    private static TextGlyphPositionsVoidPtrDelegate? _textGlyphPositionsVoidPtr;
    private static TextGlyphPositionsIntPtrDelegate? _textGlyphPositionsIntPtr;
    private static CreateImageStringDelegate? _createImageString;
    private static TextBoxBoundsFloatArrayDelegate? _textBoxBoundsFloatArray;
    private static TextBoxBoundsVector4Delegate? _textBoxBoundsVector4;
    private static TextBoxBoundsStringDelegate? _textBoxBoundsString;

    public static bool LoadLibrary(ILibraryLoader loader, string? libName = null)
    {
        _loader = loader;
        
        if (IsHandleEmpty(_libHandle))
        {
            var ptr = loader.Load(libName ?? LibName);
            if (!ptr.HasValue)
                return false;

            _libHandle = ptr.Value;
        }

        _createGLES3 = GetFunction<CreateGLES3Delegate>(FuncPrefix + nameof(CreateGLES3));
        _createGLES2 = GetFunction<CreateGLES2Delegate>(FuncPrefix + nameof(CreateGLES2));
        _deleteGLES2 = GetFunction<DeleteGLES2Delegate>(FuncPrefix + nameof(DeleteGLES2));
        _deleteGLES3 = GetFunction<DeleteGLES3Delegate>(FuncPrefix + nameof(DeleteGLES3));
        _createInternal = GetFunction<CreateInternalDelegate>(FuncPrefix + nameof(CreateInternal));
        _deleteInternal = GetFunction<DeleteInternalDelegate>(FuncPrefix + nameof(DeleteInternal));
        _text = GetFunction<TextDelegate>(FuncPrefix + nameof(Text));
        _createFont = GetFunction<CreateFontDelegate>(FuncPrefix + nameof(CreateFont));
        _fontFace = GetFunction<FontFaceDelegate>(FuncPrefix + nameof(FontFace));
        _beginFrame = GetFunction<BeginFrameDelegate>(FuncPrefix + nameof(BeginFrame));
        _cancelFrame = GetFunction<CancelFrameDelegate>(FuncPrefix + nameof(CancelFrame));
        _endFrame = GetFunction<EndFrameDelegate>(FuncPrefix + nameof(EndFrame));
        _globalCompositeOperation = GetFunction<GlobalCompositeOperationDelegate>(FuncPrefix + nameof(GlobalCompositeOperation));
        _globalCompositeBlendFunc = GetFunction<GlobalCompositeBlendFuncDelegate>(FuncPrefix + nameof(GlobalCompositeBlendFunc));
        _globalCompositeBlendFuncSeparate = GetFunction<GlobalCompositeBlendFuncSeparateDelegate>(FuncPrefix + nameof(GlobalCompositeBlendFuncSeparate));
        _rgb = GetFunction<RGBDelegate>(FuncPrefix + nameof(RGB));
        _rgBf = GetFunction<RGBfDelegate>(FuncPrefix + nameof(RGBf));
        _rgba = GetFunction<RGBADelegate>(FuncPrefix + nameof(RGBA));
        _rgbAf = GetFunction<RGBAfDelegate>(FuncPrefix + nameof(RGBAf));
        _lerpRgba = GetFunction<LerpRGBADelegate>(FuncPrefix + nameof(LerpRGBA));
        _transRgba = GetFunction<TransRGBADelegate>(FuncPrefix + nameof(TransRGBA));
        _transRgbAf = GetFunction<TransRGBAfDelegate>(FuncPrefix + nameof(TransRGBAf));
        _hsl = GetFunction<HSLDelegate>(FuncPrefix + nameof(HSL));
        _hsla = GetFunction<HSLADelegate>(FuncPrefix + nameof(HSLA));
        _save = GetFunction<SaveDelegate>(FuncPrefix + nameof(Save));
        _restore = GetFunction<RestoreDelegate>(FuncPrefix + nameof(Restore));
        _reset = GetFunction<ResetDelegate>(FuncPrefix + nameof(Reset));
        _shapeAntiAlias = GetFunction<ShapeAntiAliasDelegate>(FuncPrefix + nameof(ShapeAntiAlias));
        _strokeColor = GetFunction<StrokeColorDelegate>(FuncPrefix + nameof(StrokeColor));
        _strokePaint = GetFunction<StrokePaintDelegate>(FuncPrefix + nameof(StrokePaint));
        _fillColor = GetFunction<FillColorDelegate>(FuncPrefix + nameof(FillColor));
        _fillPaint = GetFunction<FillPaintDelegate>(FuncPrefix + nameof(FillPaint));
        _miterLimit = GetFunction<MiterLimitDelegate>(FuncPrefix + nameof(MiterLimit));
        _strokeWidth = GetFunction<StrokeWidthDelegate>(FuncPrefix + nameof(StrokeWidth));
        _lineCap = GetFunction<LineCapDelegate>(FuncPrefix + nameof(LineCap));
        _lineCapEnum = GetFunction<LineCapEnumDelegate>(FuncPrefix + nameof(LineCap));
        _lineJoin = GetFunction<LineJoinDelegate>(FuncPrefix + nameof(LineJoin));
        _lineJoinEnum = GetFunction<LineJoinEnumDelegate>(FuncPrefix + nameof(LineJoin));
        _globalAlpha = GetFunction<GlobalAlphaDelegate>(FuncPrefix + nameof(GlobalAlpha));
        _resetTransform = GetFunction<ResetTransformDelegate>(FuncPrefix + nameof(ResetTransform));
        _transform = GetFunction<TransformDelegate>(FuncPrefix + nameof(Transform));
        _translate = GetFunction<TranslateDelegate>(FuncPrefix + nameof(Translate));
        _rotate = GetFunction<RotateDelegate>(FuncPrefix + nameof(Rotate));
        _skewX = GetFunction<SkewXDelegate>(FuncPrefix + nameof(SkewX));
        _skewY = GetFunction<SkewYDelegate>(FuncPrefix + nameof(SkewY));
        _scale = GetFunction<ScaleDelegate>(FuncPrefix + nameof(Scale));
        _currentTransform = GetFunction<CurrentTransformDelegate>(FuncPrefix + nameof(CurrentTransform));
        _currentTransformMat = GetFunction<CurrentTransformMatDelegate>(FuncPrefix + nameof(CurrentTransform));
        _transformIdentity = GetFunction<TransformIdentityDelegate>(FuncPrefix + nameof(TransformIdentity));
        _transformTranslate = GetFunction<TransformTranslateDelegate>(FuncPrefix + nameof(TransformTranslate));
        _transformScale = GetFunction<TransformScaleDelegate>(FuncPrefix + nameof(TransformScale));
        _transformRotate = GetFunction<TransformRotateDelegate>(FuncPrefix + nameof(TransformRotate));
        _transformSkewX = GetFunction<TransformSkewXDelegate>(FuncPrefix + nameof(TransformSkewX));
        _transformSkewY = GetFunction<TransformSkewYDelegate>(FuncPrefix + nameof(TransformSkewY));
        _transformMultiply = GetFunction<TransformMultiplyDelegate>(FuncPrefix + nameof(TransformMultiply));
        _transformPremultiply = GetFunction<TransformPremultiplyDelegate>(FuncPrefix + nameof(TransformPremultiply));
        _transformInverse = GetFunction<TransformInverseDelegate>(FuncPrefix + nameof(TransformInverse));
        _transformPoint = GetFunction<TransformPointDelegate>(FuncPrefix + nameof(TransformPoint));
        _degToRad = GetFunction<DegToRadDelegate>(FuncPrefix + nameof(DegToRad));
        _radToDeg = GetFunction<RadToDegDelegate>(FuncPrefix + nameof(RadToDeg));
        _createImage = GetFunction<CreateImageDelegate>(FuncPrefix + nameof(CreateImage));
        _createImageMem = GetFunction<CreateImageMemDelegate>(FuncPrefix + nameof(CreateImageMem));
        _createImageRgba = GetFunction<CreateImageRGBADelegate>(FuncPrefix + nameof(CreateImageRGBA));
        _updateImage = GetFunction<UpdateImageDelegate>(FuncPrefix + nameof(UpdateImage));
        _imageSize = GetFunction<ImageSizeDelegate>(FuncPrefix + nameof(ImageSize));
        _imageSizeOut = GetFunction<ImageSizeOutDelegate>(FuncPrefix + nameof(ImageSize));
        _deleteImage = GetFunction<DeleteImageDelegate>(FuncPrefix + nameof(DeleteImage));
        _linearGradient = GetFunction<LinearGradientDelegate>(FuncPrefix + nameof(LinearGradient));
        _boxGradient = GetFunction<BoxGradientDelegate>(FuncPrefix + nameof(BoxGradient));
        _radialGradient = GetFunction<RadialGradientDelegate>(FuncPrefix + nameof(RadialGradient));
        _imagePattern = GetFunction<ImagePatternDelegate>(FuncPrefix + nameof(ImagePattern));
        _scissor = GetFunction<ScissorDelegate>(FuncPrefix + nameof(Scissor));
        _intersectScissor = GetFunction<IntersectScissorDelegate>(FuncPrefix + nameof(IntersectScissor));
        _resetScissor = GetFunction<ResetScissorDelegate>(FuncPrefix + nameof(ResetScissor));
        _beginPath = GetFunction<BeginPathDelegate>(FuncPrefix + nameof(BeginPath));
        _moveTo = GetFunction<MoveToDelegate>(FuncPrefix + nameof(MoveTo));
        _lineTo = GetFunction<LineToDelegate>(FuncPrefix + nameof(LineTo));
        _bezierTo = GetFunction<BezierToDelegate>(FuncPrefix + nameof(BezierTo));
        _quadTo = GetFunction<QuadToDelegate>(FuncPrefix + nameof(QuadTo));
        _arcTo = GetFunction<ArcToDelegate>(FuncPrefix + nameof(ArcTo));
        _closePath = GetFunction<ClosePathDelegate>(FuncPrefix + nameof(ClosePath));
        _pathWinding = GetFunction<PathWindingDelegate>(FuncPrefix + nameof(PathWinding));
        _arc = GetFunction<ArcDelegate>(FuncPrefix + nameof(Arc));
        _rect = GetFunction<RectDelegate>(FuncPrefix + nameof(Rect));
        _roundedRect = GetFunction<RoundedRectDelegate>(FuncPrefix + nameof(RoundedRect));
        _roundedRectVarying = GetFunction<RoundedRectVaryingDelegate>(FuncPrefix + nameof(RoundedRectVarying));
        _ellipse = GetFunction<EllipseDelegate>(FuncPrefix + nameof(Ellipse));
        _circle = GetFunction<CircleDelegate>(FuncPrefix + nameof(Circle));
        _fill = GetFunction<FillDelegate>(FuncPrefix + nameof(Fill));
        _stroke = GetFunction<StrokeDelegate>(FuncPrefix + nameof(Stroke));
            
        // TODO: fix
        _createFontByteArray = GetFunction<CreateFontRawDelegate>(FuncPrefix + nameof(CreateFont));
            
        _createFontMem = GetFunction<CreateFontMemDelegate>(FuncPrefix + nameof(CreateFontMem));
        _findFont = GetFunction<FindFontDelegate>(FuncPrefix + nameof(FindFont));
        _addFallbackFontId = GetFunction<AddFallbackFontIdDelegate>(FuncPrefix + nameof(AddFallbackFontId));
        _addFallbackFont = GetFunction<AddFallbackFontDelegate>(FuncPrefix + nameof(AddFallbackFont));
        _fontSize = GetFunction<FontSizeDelegate>(FuncPrefix + nameof(FontSize));
        _fontBlur = GetFunction<FontBlurDelegate>(FuncPrefix + nameof(FontBlur));
        _textLetterSpacing = GetFunction<TextLetterSpacingDelegate>(FuncPrefix + nameof(TextLetterSpacing));
        _textLineHeight = GetFunction<TextLineHeightDelegate>(FuncPrefix + nameof(TextLineHeight));
        _textAlign = GetFunction<TextAlignDelegate>(FuncPrefix + nameof(TextAlign));
        _fontFaceId = GetFunction<FontFaceIdDelegate>(FuncPrefix + nameof(FontFaceId));
        _fontFaceByteArray = GetFunction<FontFaceByteArrayDelegate>(FuncPrefix + nameof(FontFace));
        _textIntPtr = GetFunction<TextIntPtrDelegate>(FuncPrefix + nameof(Text));
        _textVoidPtr = GetFunction<TextVoidPtrDelegate>(FuncPrefix + nameof(Text));
        _textBox = GetFunction<TextBoxDelegate>(FuncPrefix + nameof(TextBox));
        _textBoundsString = GetFunction<TextBoundsStringDelegate>(FuncPrefix + nameof(TextBounds));
        _textBoundsIntPtrArray = GetFunction<TextBoundsIntPtrArrayDelegate>(FuncPrefix + nameof(TextBounds));
        _textBoundsIntPtrVector4 = GetFunction<TextBoundsIntPtrVector4Delegate>(FuncPrefix + nameof(TextBounds));
        _textMetrics = GetFunction<TextMetricsDelegate>(FuncPrefix + nameof(TextMetrics));
        _textMetricsOut = GetFunction<TextMetricsOutDelegate>(FuncPrefix + nameof(TextMetrics));
        _textBreakLines = GetFunction<TextBreakLinesDelegate>(FuncPrefix + nameof(TextBreakLines));
        _textGlyphPositionsVoidPtr = GetFunction<TextGlyphPositionsVoidPtrDelegate>(FuncPrefix + nameof(TextGlyphPositions));
        _textGlyphPositionsIntPtr = GetFunction<TextGlyphPositionsIntPtrDelegate>(FuncPrefix + nameof(TextGlyphPositions));
        _createImageString = GetFunction<CreateImageStringDelegate>(FuncPrefix + nameof(CreateImage));
        _textBoxBoundsFloatArray = GetFunction<TextBoxBoundsFloatArrayDelegate>(FuncPrefix + nameof(TextBoxBounds));
        _textBoxBoundsVector4 = GetFunction<TextBoxBoundsVector4Delegate>(FuncPrefix + nameof(TextBoxBounds));
        _textBoxBoundsString = GetFunction<TextBoxBoundsStringDelegate>(FuncPrefix + nameof(TextBoxBounds));

        return true;
    }

    private static bool IsHandleEmpty(IntPtr? inst) => inst.HasValue == false || inst.Value == IntPtr.Zero;

    public static void UnloadLibrary()
    {
        if (IsHandleEmpty(_libHandle))
            return;
        
        FreeLibraryPrivate();

        _createGLES3 = null;
        _createGLES2 = null;
        _deleteGLES2 = null;
        _deleteGLES3 = null;
        _createInternal = null;
        _deleteInternal = null;
        _text = null;
        _createFont = null;
        _fontFace = null;
        _beginFrame = null;
        _cancelFrame = null;
        _endFrame = null;
        _globalCompositeOperation = null;
        _globalCompositeBlendFunc = null;
        _globalCompositeBlendFuncSeparate = null;
        _rgb = null;
        _rgBf = null;
        _rgba = null;
        _rgbAf = null;
        _lerpRgba = null;
        _transRgba = null;
        _transRgbAf = null;
        _hsl = null;
        _hsla = null;
        _save = null;
        _restore = null;
        _reset = null;
        _shapeAntiAlias = null;
        _strokeColor = null;
        _strokePaint = null;
        _fillColor = null;
        _fillPaint = null;
        _miterLimit = null;
        _strokeWidth = null;
        _lineCap = null;
        _lineCapEnum = null;
        _lineJoin = null;
        _lineJoinEnum = null;
        _globalAlpha = null;
        _resetTransform = null;
        _transform = null;
        _translate = null;
        _rotate = null;
        _skewX = null;
        _skewY = null;
        _scale = null;
        _currentTransform = null;
        _currentTransformMat = null;
        _transformIdentity = null;
        _transformTranslate = null;
        _transformScale = null;
        _transformRotate = null;
        _transformSkewX = null;
        _transformSkewY = null;
        _transformMultiply = null;
        _transformPremultiply = null;
        _transformInverse = null;
        _transformPoint = null;
        _degToRad = null;
        _radToDeg = null;
        _createImage = null;
        _createImageMem = null;
        _createImageRgba = null;
        _updateImage = null;
        _imageSize = null;
        _imageSizeOut = null;
        _deleteImage = null;
        _linearGradient = null;
        _boxGradient = null;
        _radialGradient = null;
        _imagePattern = null;
        _scissor = null;
        _intersectScissor = null;
        _resetScissor = null;
        _beginPath = null;
        _moveTo = null;
        _lineTo = null;
        _bezierTo = null;
        _quadTo = null;
        _arcTo = null;
        _closePath = null;
        _pathWinding = null;
        _arc = null;
        _rect = null;
        _roundedRect = null;
        _roundedRectVarying = null;
        _ellipse = null;
        _circle = null;
        _fill = null;
        _stroke = null;
        _createFontByteArray = null;
        _createFontMem = null;
        _findFont = null;
        _addFallbackFontId = null;
        _addFallbackFont = null;
        _fontSize = null;
        _fontBlur = null;
        _textLetterSpacing = null;
        _textLineHeight = null;
        _textAlign = null;
        _fontFaceId = null;
        _fontFaceByteArray = null;
        _textIntPtr = null;
        _textVoidPtr = null;
        _textBox = null;
        _textBoundsString = null;
        _textBoundsIntPtrArray = null;
        _textBoundsIntPtrVector4 = null;
        _textMetrics = null;
        _textMetricsOut = null;
        _textBreakLines = null;
        _textGlyphPositionsVoidPtr = null;
        _textGlyphPositionsIntPtr = null;
        _createImageString = null;
        _textBoxBoundsFloatArray = null;
        _textBoxBoundsVector4 = null;
        _textBoxBoundsString = null;
    }

    private static T? GetFunction<T>(string procName) where T : Delegate
    {
        GetLoaderInstanceOrThrowError(out var loader, out var ptr);
        
        var addr = loader.GetFunction(ptr, procName);

        if (!IsHandleEmpty(addr))
        {
            var @delegate = Marshal.GetDelegateForFunctionPointer(addr!.Value, typeof(T));
            if (@delegate is not T target)
            {
                FailFunctionLoadCallback?.Invoke(procName);
                return null;
            }
            
            return target;
        }

        FailFunctionLoadCallback?.Invoke(procName);
        //var delegateInfo = typeof(T);
        //Console.WriteLine($"Unable to retrieve function: {procName} ({delegateInfo})");
        return null;
    }

    private static void GetLoaderInstanceOrThrowError(out ILibraryLoader loader, out IntPtr ptr)
    {
        var handle = _libHandle;
        var loaderInst = _loader;
        
        if (IsHandleEmpty(handle))
            throw new InvalidOperationException("Library not loaded.");

        if(loaderInst == null)
            throw new ArgumentNullException(nameof(loader), "Library loader not initialised");

        loader = loaderInst;
        ptr = handle!.Value;
    }

    public static Action<string>? FailFunctionLoadCallback;

    private static void FreeLibraryPrivate()
    {
        GetLoaderInstanceOrThrowError(out var loader, out var ptr);
        loader.Free(ptr);
        
        _libHandle = IntPtr.Zero;
    }
}
#endif