using System.Numerics;

namespace NanoVG.Extensions;

public static class Vector4Extensions
{
    public static float GetWidth(this Vector4 v) => v.Z - v.X;

    public static float GetHeight(this Vector4 v) => v.W - v.Y;

    public static float GetLeft(this Vector4 v) => v.X;

    public static float GetTop(this Vector4 v) => v.Y;
    
    public static float GetRight(this Vector4 v) => v.Z;

    public static float GetBottom(this Vector4 v) => v.W;
}