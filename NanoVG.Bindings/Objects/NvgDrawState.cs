using System;

namespace NanoVG.Objects;

public class NvgDrawState(NvgContext ctx) : IDisposable
{
    private readonly NvgContext _ctx = ctx;

    public void Dispose()
    {
        _ctx.Restore();
    }
}