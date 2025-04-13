using System;

namespace NanoVG;

[Flags]
public enum NvgCreateFlags : int
{
    Antialias = 1 << 0,
    StencilStrokes = 1 << 1,
    Debug = 1 << 2
}

[Flags]
public enum NvgImageFlags : int
{
    /// <summary>
    /// Generate mipmaps during creation of the image.
    /// </summary>
    GenerateMipmaps = 1 << 0,
    
    /// <summary>
    /// Repeat image in X direction.
    /// </summary>
    RepeatX = 1 << 1,
    
    /// <summary>
    /// Repeat image in Y direction.
    /// </summary>
    RepeatY = 1 << 2,
    
    /// <summary>
    /// Flips (inverses) image in Y direction when rendered.
    /// </summary>
    FlipY = 1 << 3,
    
    /// <summary>
    /// Image data has premultiplied alpha.
    /// </summary>
    Premultiplied = 1 << 4,
    
    /// <summary>
    /// Image interpolation is Nearest instead Linear
    /// </summary>
    Nearest = 1 << 5,
}

public enum NvgSolidity : int
{
    Solid = 1, // CCW
    Hole = 2, // CW
}

[Flags]
public enum NvgAlign : int
{
    // Horizontal align
    
    /// <summary>
    /// Default, align text horizontally to left.
    /// </summary>
    Left = 1 << 0,
    
    /// <summary>
    /// Align text horizontally to center.
    /// </summary>
    Center = 1 << 1,
    
    /// <summary>
    /// Align text horizontally to right.
    /// </summary>
    Right = 1 << 2,

    // Vertical align
    
    /// <summary>
    /// Align text vertically to top.
    /// </summary>
    Top = 1 << 3,
    
    /// <summary>
    /// Align text vertically to middle.
    /// </summary>
    Middle = 1 << 4,
    
    /// <summary>
    /// Align text vertically to bottom.
    /// </summary>
    Bottom = 1 << 5,
    
    /// <summary>
    /// Default, align text vertically to baseline.
    /// </summary>
    Baseline = 1 << 6,
}

public enum NvgLineCap : int
{
    Butt,
    Round,
    Square,
    Bevel,
    Miter
}

public enum NvgWinding : int
{
    CCW = 1, // Winding for solid shapes
    CW = 2, // Winding for holes
}
