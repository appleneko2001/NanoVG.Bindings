using System.Numerics;
using NanoVG.Extensions;
using NanoVG.Objects;

namespace NanoVG.Demo;

public class DemoGame : IDisposable
{
    const string TextStr =
        "This is longer chunk of text.\n  \n  Would have used lorem\0 ipsum but she    was busy jumping over the lazy dog with the fox and all the men who came to the aid of the party.🎉中文";

    const string HoverTextStr = "Hover your mouse over the text to see calculated caret position.";

    private readonly Utf8RawString _mainText = new(TextStr);
    private readonly Utf8RawString _mainHoverText = new(HoverTextStr);

    private const int IconSearch = 0x1F50D;
    private const int IconCircledCross = 0x2716;
    private const int IconChevronRight = 0xE75E;
    private const int IconCheck = 0x2713;
    private const int IconLogin = 0xE740;
    private const int IconTrash = 0xE729;

    private float Clamp(float v, float min, float max) =>
        v >= max ? max : v <= min ? min : v;

    public static string CodePointToUtf8String(int codePoint) => 
        char.ConvertFromUtf32(codePoint);

    private bool IsBlack(Vector4 col)
    {
        return col is { W: 0.0f, X: 0.0f, Y: 0.0f, Z: 0.0f };
    }

    private void DrawWindow(NvgContext vg, string title, float x, float y, float w, float h)
    {
        var cornerRadius = 3.0f;

        vg.Save();
//	Nvg.ClearState(vg);

        // Window
        vg.BeginPath();
        vg.RoundedRect(x, y, w, h, cornerRadius);
        vg.FillColor(Nvg.RGBA(28, 30, 34, 192));
//	Nvg.FillColor(vg, Nvg.RGBA(0,0,0,128));
        vg.Fill();

        // Drop shadow
        var shadowPaint = vg.BoxGradient(x, y + 2, w, h, cornerRadius * 2, 10, Nvg.RGBA(0, 0, 0, 128),
            Nvg.RGBA(0, 0, 0, 0));
        vg.BeginPath();
        vg.Rect(x - 10, y - 10, w + 20, h + 30);
        vg.RoundedRect(x, y, w, h, cornerRadius);
        vg.PathWinding(NvgSolidity.Hole);
        vg.FillPaint(shadowPaint);
        vg.Fill();

        // Header
        var headerPaint = vg.LinearGradient(x, y, x, y + 15, Nvg.RGBA(255, 255, 255, 8), Nvg.RGBA(0, 0, 0, 16));
        vg.BeginPath();
        vg.RoundedRect(x + 1, y + 1, w - 2, 30, cornerRadius - 1);
        vg.FillPaint(headerPaint);
        vg.Fill();
        vg.BeginPath();
        vg.MoveTo(x + 0.5f, y + 0.5f + 30);
        vg.LineTo(x + 0.5f + w - 1, y + 0.5f + 30);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 32));
        vg.Stroke();

        vg.FontSize(15.0f);
        vg.FontFace("sans-bold");
        vg.TextAlign(NvgAlign.Center | NvgAlign.Middle);

        vg.FontBlur(2);
        vg.FillColor(Nvg.RGBA(0, 0, 0, 128));
        vg.Text(x + w / 2, y + 16 + 1, title);

        vg.FontBlur(0);
        vg.FillColor(Nvg.RGBA(220, 220, 220, 160));
        vg.Text(x + w / 2, y + 16, title);

        vg.Restore();
    }

    private void DrawSearchBox(NvgContext vg, string text, float x, float y, float w, float h)
    {
        var cornerRadius = h / 2 - 1;

        // Edit
        var bg = vg.BoxGradient(x, y + 1.5f, w, h, h / 2, 5, Nvg.RGBA(0, 0, 0, 16), Nvg.RGBA(0, 0, 0, 92));
        vg.BeginPath();
        vg.RoundedRect(x, y, w, h, cornerRadius);
        vg.FillPaint(bg);
        vg.Fill();

/*	Nvg.BeginPath(vg);
    Nvg.RoundedRect(vg, x+0.5f,y+0.5f, w-1,h-1, cornerRadius-0.5f);
    Nvg.StrokeColor(vg, Nvg.RGBA(0,0,0,48));
    Nvg.Stroke(vg);*/

        vg.FontSize(h * 1.3f);
        vg.FontFace("icons");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 64));
        vg.TextAlign(NvgAlign.Center | NvgAlign.Middle);

        vg.Text(x + h * 0.55f, y + h * 0.55f, CodePointToUtf8String(IconSearch));

        vg.FontSize(17.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 32));

        vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);
        vg.Text(x + h * 1.05f, y + h * 0.5f, text);

        vg.FontSize(h * 1.3f);
        vg.FontFace("icons");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 32));
        vg.TextAlign(NvgAlign.Center | NvgAlign.Middle);

        vg.Text(x + w - h * 0.55f, y + h * 0.55f, CodePointToUtf8String(IconCircledCross));
    }

    private void DrawDropDown(NvgContext vg, string text, float x, float y, float w, float h)
    {
        var cornerRadius = 4.0f;

        var bg = vg.LinearGradient(x, y, x, y + h, Nvg.RGBA(255, 255, 255, 16), Nvg.RGBA(0, 0, 0, 16));
        vg.BeginPath();
        vg.RoundedRect(x + 1, y + 1, w - 2, h - 2, cornerRadius - 1);
        vg.FillPaint(bg);
        vg.Fill();

        vg.BeginPath();
        vg.RoundedRect(x + 0.5f, y + 0.5f, w - 1, h - 1, cornerRadius - 0.5f);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 48));
        vg.Stroke();

        vg.FontSize(17.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 160));
        vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);
        vg.Text(x + h * 0.3f, y + h * 0.5f, text);

        vg.FontSize(h * 1.3f);
        vg.FontFace("icons");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 64));
        vg.TextAlign(NvgAlign.Center | NvgAlign.Middle);

        vg.Text(x + w - h * 0.5f, y + h * 0.5f, CodePointToUtf8String(IconChevronRight));
    }

    private void DrawLabel(NvgContext vg, string text, float x, float y, float w, float h)
    {
        vg.FontSize(15.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 128));

        vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);
        vg.Text(x, y + h * 0.5f, text);
    }

    private void DrawEditBoxBase(NvgContext vg, float x, float y, float w, float h)
    {
        var bg =
            // Edit
            vg.BoxGradient(x + 1, y + 1 + 1.5f, w - 2, h - 2, 3, 4, Nvg.RGBA(255, 255, 255, 32),
                Nvg.RGBA(32, 32, 32, 32));
        vg.BeginPath();
        vg.RoundedRect(x + 1, y + 1, w - 2, h - 2, 4 - 1);
        vg.FillPaint(bg);
        vg.Fill();

        vg.BeginPath();
        vg.RoundedRect(x + 0.5f, y + 0.5f, w - 1, h - 1, 4 - 0.5f);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 48));
        vg.Stroke();
    }

    private void DrawEditBox(NvgContext vg, string text, float x, float y, float w, float h)
    {
        DrawEditBoxBase(vg, x, y, w, h);

        vg.FontSize(17.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 64));
        vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);
        vg.Text(x + h * 0.3f, y + h * 0.5f, text);
    }

    private void DrawEditBoxNum(NvgContext vg,
        string text, string units, float x, float y, float w, float h)
    {
        DrawEditBoxBase(vg, x, y, w, h);

        var uw = vg.TextBounds(0, 0, units, null);

        vg.FontSize(15.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 64));
        vg.TextAlign(NvgAlign.Right | NvgAlign.Middle);
        vg.Text(x + w - h * 0.3f, y + h * 0.5f, units);

        vg.FontSize(17.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 128));
        vg.TextAlign(NvgAlign.Right | NvgAlign.Middle);
        vg.Text(x + w - uw - h * 0.5f, y + h * 0.5f, text);
    }

    private void DrawCheckBox(NvgContext vg, string text, float x, float y, float w, float h)
    {
        vg.FontSize(15.0f);
        vg.FontFace("sans");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 160));

        vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);
        vg.Text(x + 28, y + h * 0.5f, text);

        var bg = vg.BoxGradient(x + 1, y + (int)(h * 0.5f) - 9 + 1, 18, 18, 3, 3, Nvg.RGBA(0, 0, 0, 32),
            Nvg.RGBA(0, 0, 0, 92));
        vg.BeginPath();
        vg.RoundedRect(x + 1, y + (int)(h * 0.5f) - 9, 18, 18, 3);
        vg.FillPaint(bg);
        vg.Fill();

        vg.FontSize(33);
        vg.FontFace("icons");
        vg.FillColor(Nvg.RGBA(255, 255, 255, 128));
        vg.TextAlign(NvgAlign.Center | NvgAlign.Middle);

        vg.Text(x + 9 + 2, y + h * 0.5f, CodePointToUtf8String(IconCheck));
    }

    private void DrawButton(NvgContext vg, int preicon, string text, float x, float y, float w, float h,
        Vector4 col)
    {
        var cornerRadius = 4.0f;
        float iw = 0;

        var bg = vg.LinearGradient(x, y, x, y + h, Nvg.RGBA(255, 255, 255, (byte)(IsBlack(col) ? 16 : 32)),
            Nvg.RGBA(0, 0, 0, (byte)(IsBlack(col) ? 16 : 32)));
        vg.BeginPath();
        vg.RoundedRect(x + 1, y + 1, w - 2, h - 2, cornerRadius - 1);
        if (!IsBlack(col))
        {
            vg.FillColor(col);
            vg.Fill();
        }

        vg.FillPaint(bg);
        vg.Fill();

        vg.BeginPath();
        vg.RoundedRect(x + 0.5f, y + 0.5f, w - 1, h - 1, cornerRadius - 0.5f);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 48));
        vg.Stroke();

        vg.FontSize(17.0f);
        vg.FontFace("sans-bold");
        var tw = vg.TextBounds(0, 0, text, null);
        if (preicon != 0)
        {
            vg.FontSize(h * 1.3f);
            vg.FontFace("icons");

            iw = vg.TextBounds(0, 0, CodePointToUtf8String(preicon), IntPtr.Zero, null);

            iw += h * 0.15f;
        }

        if (preicon != 0)
        {
            vg.FontSize(h * 1.3f);
            vg.FontFace("icons");
            vg.FillColor(Nvg.RGBA(255, 255, 255, 96));
            vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);

            vg.Text(x + w * 0.5f - tw * 0.5f - iw * 0.75f, y + h * 0.5f, CodePointToUtf8String(preicon));
        }

        vg.FontSize(17.0f);
        vg.FontFace("sans-bold");
        vg.TextAlign(NvgAlign.Left | NvgAlign.Middle);
        vg.FillColor(Nvg.RGBA(0, 0, 0, 160));
        vg.Text(x + w * 0.5f - tw * 0.5f + iw * 0.25f, y + h * 0.5f - 1, text);
        vg.FillColor(Nvg.RGBA(255, 255, 255, 160));
        vg.Text(x + w * 0.5f - tw * 0.5f + iw * 0.25f, y + h * 0.5f, text);
    }

    private void DrawSlider(NvgContext vg, float pos, float x, float y, float w, float h)
    {
        var cy = y + (int)(h * 0.5f);
        float kr = (int)(h * 0.25f);

        vg.Save();
        //	Nvg.ClearState(vg);
        // Slot
        var bg = vg.BoxGradient(x, cy - 2 + 1, w, 4, 2, 2, Nvg.RGBA(0, 0, 0, 32), Nvg.RGBA(0, 0, 0, 128));
        vg.BeginPath();
        vg.RoundedRect(x, cy - 2, w, 4, 2);
        vg.FillPaint(bg);
        vg.Fill();

        // Knob Shadow
        bg = vg.RadialGradient(x + (int)(pos * w), cy + 1, kr - 3, kr + 3, Nvg.RGBA(0, 0, 0, 64), Nvg.RGBA(0, 0, 0, 0));
        vg.BeginPath();
        vg.Rect(x + (int)(pos * w) - kr - 5, cy - kr - 5, kr * 2 + 5 + 5, kr * 2 + 5 + 5 + 3);
        vg.Circle(x + (int)(pos * w), cy, kr);
        vg.PathWinding(NvgSolidity.Hole);
        vg.FillPaint(bg);
        vg.Fill();

        // Knob
        var knob = vg.LinearGradient(x, cy - kr, x, cy + kr, Nvg.RGBA(255, 255, 255, 16), Nvg.RGBA(0, 0, 0, 16));
        vg.BeginPath();
        vg.Circle(x + (int)(pos * w), cy, kr - 1);
        vg.FillColor(Nvg.RGBA(40, 43, 48, 255));
        vg.Fill();
        vg.FillPaint(knob);
        vg.Fill();

        vg.BeginPath();
        vg.Circle(x + (int)(pos * w), cy, kr - 0.5f);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 92));
        vg.Stroke();

        vg.Restore();
    }

    private void DrawEyes(NvgContext vg, float x, float y, float w, float h, float mx, float my, float t)
    {
        var ex = w * 0.23f;
        var ey = h * 0.5f;
        var lx = x + ex;
        var ly = y + ey;
        var rx = x + w - ex;
        var ry = y + ey;
        var br = (ex < ey ? ex : ey) * 0.5f;
        var blink = 1 - Math.Pow(Math.Sin(t * 0.5f), 200) * 0.8f;

        var bg = vg.LinearGradient(x, y + h * 0.5f, x + w * 0.1f, y + h, Nvg.RGBA(0, 0, 0, 32), Nvg.RGBA(0, 0, 0, 16));
        vg.BeginPath();
        vg.Ellipse(lx + 3.0f, ly + 16.0f, ex, ey);
        vg.Ellipse(rx + 3.0f, ry + 16.0f, ex, ey);
        vg.FillPaint(bg);
        vg.Fill();

        bg = vg.LinearGradient(x, y + h * 0.25f, x + w * 0.1f, y + h, Nvg.RGBA(220, 220, 220, 255),
            Nvg.RGBA(128, 128, 128, 255));
        vg.BeginPath();
        vg.Ellipse(lx, ly, ex, ey);
        vg.Ellipse(rx, ry, ex, ey);
        vg.FillPaint(bg);
        vg.Fill();

        var dx = (mx - rx) / (ex * 10);
        var dy = (my - ry) / (ey * 10);
        var d = (float)Math.Sqrt(dx * dx + dy * dy);
        if (d > 1.0f)
        {
            dx /= d;
            dy /= d;
        }

        dx *= ex * 0.4f;
        dy *= ey * 0.5f;
        vg.BeginPath();
        vg.Ellipse(lx + dx, (float)(ly + dy + ey * 0.25f * (1 - blink)), br, (float)(br * blink));
        vg.FillColor(Nvg.RGBA(32, 32, 32, 255));
        vg.Fill();

        dx = (mx - rx) / (ex * 10);
        dy = (my - ry) / (ey * 10);
        d = (float)Math.Sqrt(dx * dx + dy * dy);
        if (d > 1.0f)
        {
            dx /= d;
            dy /= d;
        }

        dx *= ex * 0.4f;
        dy *= ey * 0.5f;
        vg.BeginPath();
        vg.Ellipse(rx + dx, (float)(ry + dy + ey * 0.25f * (1 - blink)), br, (float)(br * blink));
        vg.FillColor(Nvg.RGBA(32, 32, 32, 255));
        vg.Fill();

        var gloss = vg.RadialGradient(lx - ex * 0.25f, ly - ey * 0.5f, ex * 0.1f, ex * 0.75f,
            Nvg.RGBA(255, 255, 255, 128),
            Nvg.RGBA(255, 255, 255, 0));
        vg.BeginPath();
        vg.Ellipse(lx, ly, ex, ey);
        vg.FillPaint(gloss);
        vg.Fill();

        gloss = vg.RadialGradient(rx - ex * 0.25f, ry - ey * 0.5f, ex * 0.1f, ex * 0.75f, Nvg.RGBA(255, 255, 255, 128),
            Nvg.RGBA(255, 255, 255, 0));
        vg.BeginPath();
        vg.Ellipse(rx, ry, ex, ey);
        vg.FillPaint(gloss);
        vg.Fill();
    }

    private void DrawGraph(NvgContext vg, float x, float y, float w, float h, float t)
    {
        var samples = new float [6];
        var sx = new float[6];
        var sy = new float[6];
        var dx = w / 5.0f;
        int i;

        samples[0] = (float)(1 + Math.Sin(t * 1.2345f + Math.Cos(t * 0.33457f) * 0.44f)) * 0.5f;
        samples[1] = (float)(1 + Math.Sin(t * 0.68363f + Math.Cos(t * 1.3f) * 1.55f)) * 0.5f;
        samples[2] = (float)(1 + Math.Sin(t * 1.1642f + Math.Cos(t * 0.33457f) * 1.24f)) * 0.5f;
        samples[3] = (float)(1 + Math.Sin(t * 0.56345f + Math.Cos(t * 1.63f) * 0.14f)) * 0.5f;
        samples[4] = (float)(1 + Math.Sin(t * 1.6245f + Math.Cos(t * 0.254f) * 0.3f)) * 0.5f;
        samples[5] = (float)(1 + Math.Sin(t * 0.345f + Math.Cos(t * 0.03f) * 0.6f)) * 0.5f;

        for (i = 0; i < 6; i++)
        {
            sx[i] = x + i * dx;
            sy[i] = y + h * samples[i] * 0.8f;
        }

        // Graph background
        var bg = vg.LinearGradient(x, y, x, y + h, Nvg.RGBA(0, 160, 192, 0), Nvg.RGBA(0, 160, 192, 64));
        vg.BeginPath();
        vg.MoveTo(sx[0], sy[0]);
        for (i = 1; i < 6; i++)
            vg.BezierTo(sx[i - 1] + dx * 0.5f, sy[i - 1], sx[i] - dx * 0.5f, sy[i], sx[i], sy[i]);
        vg.LineTo(x + w, y + h);
        vg.LineTo(x, y + h);
        vg.FillPaint(bg);
        vg.Fill();

        // Graph line
        vg.BeginPath();
        vg.MoveTo(sx[0], sy[0] + 2);
        for (i = 1; i < 6; i++)
            vg.BezierTo(sx[i - 1] + dx * 0.5f, sy[i - 1] + 2, sx[i] - dx * 0.5f, sy[i] + 2, sx[i], sy[i] + 2);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 32));
        vg.StrokeWidth(3.0f);
        vg.Stroke();

        vg.BeginPath();
        vg.MoveTo(sx[0], sy[0]);
        for (i = 1; i < 6; i++)
            vg.BezierTo(sx[i - 1] + dx * 0.5f, sy[i - 1], sx[i] - dx * 0.5f, sy[i], sx[i], sy[i]);
        vg.StrokeColor(Nvg.RGBA(0, 160, 192, 255));
        vg.StrokeWidth(3.0f);
        vg.Stroke();

        // Graph sample pos
        for (i = 0; i < 6; i++)
        {
            bg = vg.RadialGradient(sx[i], sy[i] + 2, 3.0f, 8.0f, Nvg.RGBA(0, 0, 0, 32), Nvg.RGBA(0, 0, 0, 0));
            vg.BeginPath();
            vg.Rect(sx[i] - 10, sy[i] - 10 + 2, 20, 20);
            vg.FillPaint(bg);
            vg.Fill();
        }

        vg.BeginPath();
        for (i = 0; i < 6; i++)
            vg.Circle(sx[i], sy[i], 4.0f);
        vg.FillColor(Nvg.RGBA(0, 160, 192, 255));
        vg.Fill();
        vg.BeginPath();
        for (i = 0; i < 6; i++)
            vg.Circle(sx[i], sy[i], 2.0f);
        vg.FillColor(Nvg.RGBA(220, 220, 220, 255));
        vg.Fill();

        vg.StrokeWidth(1.0f);
    }

    private void DrawSpinner(NvgContext vg, float cx, float cy, float r, float t)
    {
        var a0 = 0.0f + t * 6;
        var a1 = (float)(Math.PI + t * 6);
        var r0 = r;
        var r1 = r * 0.75f;

        vg.Save();

        vg.BeginPath();
        vg.Arc(cx, cy, r0, a0, a1, NvgWinding.CW);
        vg.Arc(cx, cy, r1, a1, a0, NvgWinding.CCW);
        vg.ClosePath();
        var ax = (float)(cx + Math.Cos(a0) * (r0 + r1) * 0.5f);
        var ay = (float)(cy + Math.Sin(a0) * (r0 + r1) * 0.5f);
        var bx = (float)(cx + Math.Cos(a1) * (r0 + r1) * 0.5f);
        var by = (float)(cy + Math.Sin(a1) * (r0 + r1) * 0.5f);
        var paint = vg.LinearGradient(ax, ay, bx, by, Nvg.RGBA(0, 0, 0, 0), Nvg.RGBA(0, 0, 0, 128));
        vg.FillPaint(paint);
        vg.Fill();

        vg.Restore();
    }

//const int* images
    private  void DrawThumbnails(NvgContext vg, float x, float y, float w, float h, int[] images, float t)
    {
        var imageCounts = images.Length;
        var cornerRadius = 3.0f;
        var thumb = 60.0f;
        var arry = 30.5f;
        var stackH = imageCounts / 2.0f * (thumb + 10) + 10;
        int i;
        var u = (float)(1 + Math.Cos(t * 0.5f)) * 0.5f;
        var u2 = (float)(1 - Math.Cos(t * 0.2f)) * 0.5f;

        vg.Save();
        //	Nvg.ClearState(vg);
        // Drop shadow
        var shadowPaint = vg.BoxGradient(x, y + 4, w, h, cornerRadius * 2, 20, Nvg.RGBA(0, 0, 0, 128),
            Nvg.RGBA(0, 0, 0, 0));
        vg.BeginPath();
        vg.Rect(x - 10, y - 10, w + 20, h + 30);
        vg.RoundedRect(x, y, w, h, cornerRadius);
        vg.PathWinding(NvgSolidity.Hole);
        vg.FillPaint(shadowPaint);
        vg.Fill();

        // Window
        vg.BeginPath();
        vg.RoundedRect(x, y, w, h, cornerRadius);
        vg.MoveTo(x - 10, y + arry);
        vg.LineTo(x + 1, y + arry - 11);
        vg.LineTo(x + 1, y + arry + 11);
        vg.FillColor(Nvg.RGBA(200, 200, 200, 255));
        vg.Fill();

        vg.Save();
        vg.Scissor(x, y, w, h);
        vg.Translate(0, -(stackH - h) * u);

        var dV = 1.0f / (imageCounts - 1);

        for (i = 0; i < imageCounts; i++)
        {
            var tx = x + 10;
            var ty = y + 10;
            tx += i % 2 * (thumb + 10);
            // ReSharper disable once PossibleLossOfFraction
            ty += i / 2 * (thumb + 10);
            vg.ImageSize(images[i], out var imgW, out var imgH);
            float ix;
            float iy;
            float iw;
            float ih;
            if (imgW < imgH)
            {
                iw = thumb;
                ih = iw * imgH / imgW;
                ix = 0;
                iy = -(ih - thumb) * 0.5f;
            }
            else
            {
                ih = thumb;
                iw = ih * imgW / imgH;
                ix = -(iw - thumb) * 0.5f;
                iy = 0;
            }

            var v = i * dV;
            var a = Clamp((u2 - v) / dV, 0, 1);

            if (a < 1.0f)
                DrawSpinner(vg, tx + thumb / 2, ty + thumb / 2, thumb * 0.25f, t);

            var imgPaint = vg.ImagePattern(tx + ix, ty + iy, iw, ih, (float)(0.0f / 180.0f * Math.PI), images[i], a);
            vg.BeginPath();
            vg.RoundedRect(tx, ty, thumb, thumb, 5);
            vg.FillPaint(imgPaint);
            vg.Fill();

            shadowPaint = vg.BoxGradient(tx - 1, ty, thumb + 2, thumb + 2, 5, 3, Nvg.RGBA(0, 0, 0, 128),
                Nvg.RGBA(0, 0, 0, 0));
            vg.BeginPath();
            vg.Rect(tx - 5, ty - 5, thumb + 10, thumb + 10);
            vg.RoundedRect(tx, ty, thumb, thumb, 6);
            vg.PathWinding(NvgSolidity.Hole);
            vg.FillPaint(shadowPaint);
            vg.Fill();

            vg.BeginPath();
            vg.RoundedRect(tx + 0.5f, ty + 0.5f, thumb - 1, thumb - 1, 4 - 0.5f);
            vg.StrokeWidth(1.0f);
            vg.StrokeColor(Nvg.RGBA(255, 255, 255, 192));
            vg.Stroke();
        }

        vg.Restore();

        // Hide fades
        var fadePaint = vg.LinearGradient(x, y, x, y + 6, Nvg.RGBA(200, 200, 200, 255), Nvg.RGBA(200, 200, 200, 0));
        vg.BeginPath();
        vg.Rect(x + 4, y, w - 8, 6);
        vg.FillPaint(fadePaint);
        vg.Fill();

        fadePaint = vg.LinearGradient(x, y + h, x, y + h - 6, Nvg.RGBA(200, 200, 200, 255), Nvg.RGBA(200, 200, 200, 0));
        vg.BeginPath();
        vg.Rect(x + 4, y + h - 6, w - 8, 6);
        vg.FillPaint(fadePaint);
        vg.Fill();

        // Scroll bar
        shadowPaint = vg.BoxGradient(x + w - 12 + 1, y + 4 + 1, 8, h - 8, 3, 4, Nvg.RGBA(0, 0, 0, 32),
            Nvg.RGBA(0, 0, 0, 92));
        vg.BeginPath();
        vg.RoundedRect(x + w - 12, y + 4, 8, h - 8, 3);
        vg.FillPaint(shadowPaint);
//	Nvg.FillColor(vg, Nvg.RGBA(255,0,0,128));
        vg.Fill();

        var scrollH = (h / stackH) * (h - 8);
        shadowPaint = vg.BoxGradient(x + w - 12 - 1, y + 4 + (h - 8 - scrollH) * u - 1, 8, scrollH, 3, 4,
            Nvg.RGBA(220, 220, 220, 255), Nvg.RGBA(128, 128, 128, 255));
        vg.BeginPath();
        vg.RoundedRect(x + w - 12 + 1, y + 4 + 1 + (h - 8 - scrollH) * u, 8 - 2, scrollH - 2, 2);
        vg.FillPaint(shadowPaint);
//	Nvg.FillColor(vg, Nvg.RGBA(0,0,0,128));
        vg.Fill();

        vg.Restore();
    }

    private void DrawColorwheel(NvgContext vg, float x, float y, float w, float h, float t)
    {
        int i;
        float r0, r1, ax, ay, bx, by, cx, cy, aeps, r;
        var hue = (float)(Math.Sin(t * 0.12f));
        NvgPaint paint;

        vg.Save();

/*	Nvg.BeginPath(vg);
    Nvg.Rect(vg, x,y,w,h);
    Nvg.FillColor(vg, Nvg.RGBA(255,0,0,128));
    Nvg.Fill(vg);*/

        cx = x + w * 0.5f;
        cy = y + h * 0.5f;
        r1 = (w < h ? w : h) * 0.5f - 5.0f;
        r0 = r1 - 20.0f;
        aeps = 0.5f / r1; // half a pixel arc length in radians (2pi cancels out).

        for (i = 0; i < 6; i++)
        {
            var a0 = (float)(i / 6.0f * Math.PI * 2.0f - aeps);
            var a1 = (float)((i + 1.0f) / 6.0f * Math.PI * 2.0f + aeps);
            vg.BeginPath();
            vg.Arc(cx, cy, r0, a0, a1, NvgWinding.CW);
            vg.Arc(cx, cy, r1, a1, a0, NvgWinding.CCW);
            vg.ClosePath();
            ax = (float)(cx + Math.Cos(a0) * (r0 + r1) * 0.5f);
            ay = (float)(cy + Math.Sin(a0) * (r0 + r1) * 0.5f);
            bx = (float)(cx + Math.Cos(a1) * (r0 + r1) * 0.5f);
            by = (float)(cy + Math.Sin(a1) * (r0 + r1) * 0.5f);
            paint = vg.LinearGradient(ax, ay, bx, by, Nvg.HSLA((float)(a0 / (Math.PI * 2)), 1.0f, 0.55f, 255),
                Nvg.HSLA((float)(a1 / (Math.PI * 2)), 1.0f, 0.55f, 255));
            vg.FillPaint(paint);
            vg.Fill();
        }

        vg.BeginPath();
        vg.Circle(cx, cy, r0 - 0.5f);
        vg.Circle(cx, cy, r1 + 0.5f);
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 64));
        vg.StrokeWidth(1.0f);
        vg.Stroke();

        // Selector
        vg.Save();
        vg.Translate(cx, cy);
        vg.Rotate((float)(hue * Math.PI * 2));

        // Marker on
        vg.StrokeWidth(2.0f);
        vg.BeginPath();
        vg.Rect(r0 - 1, -3, r1 - r0 + 2, 6);
        vg.StrokeColor(Nvg.RGBA(255, 255, 255, 192));
        vg.Stroke();

        paint = vg.BoxGradient(r0 - 3, -5, r1 - r0 + 6, 10, 2, 4, Nvg.RGBA(0, 0, 0, 128), Nvg.RGBA(0, 0, 0, 0));
        vg.BeginPath();
        vg.Rect(r0 - 2 - 10, -4 - 10, r1 - r0 + 4 + 20, 8 + 20);
        vg.Rect(r0 - 2, -4, r1 - r0 + 4, 8);
        vg.PathWinding(NvgSolidity.Hole);
        vg.FillPaint(paint);
        vg.Fill();

        // Center triangle
        r = r0 - 6;
        ax = (float)(Math.Cos(120.0f / 180.0f * Math.PI) * r);
        ay = (float)(Math.Sin(120.0f / 180.0f * Math.PI) * r);
        bx = (float)(Math.Cos(-120.0f / 180.0f * Math.PI) * r);
        by = (float)(Math.Sin(-120.0f / 180.0f * Math.PI) * r);
        vg.BeginPath();
        vg.MoveTo(r, 0);
        vg.LineTo(ax, ay);
        vg.LineTo(bx, by);
        vg.ClosePath();
        paint = vg.LinearGradient(r, 0, ax, ay, Nvg.HSLA(hue, 1.0f, 0.5f, 255), Nvg.RGBA(255, 255, 255, 255));
        vg.FillPaint(paint);
        vg.Fill();
        paint = vg.LinearGradient((r + ax) * 0.5f, (0 + ay) * 0.5f, bx, by, Nvg.RGBA(0, 0, 0, 0),
            Nvg.RGBA(0, 0, 0, 255));
        vg.FillPaint(paint);
        vg.Fill();
        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 64));
        vg.Stroke();

        // Select circle on triangle
        ax = (float)(Math.Cos(120.0f / 180.0f * Math.PI) * r * 0.3f);
        ay = (float)(Math.Sin(120.0f / 180.0f * Math.PI) * r * 0.4f);
        vg.StrokeWidth(2.0f);
        vg.BeginPath();
        vg.Circle(ax, ay, 5);
        vg.StrokeColor(Nvg.RGBA(255, 255, 255, 192));
        vg.Stroke();

        paint = vg.RadialGradient(ax, ay, 7, 9, Nvg.RGBA(0, 0, 0, 64), Nvg.RGBA(0, 0, 0, 0));
        vg.BeginPath();
        vg.Rect(ax - 20, ay - 20, 40, 40);
        vg.Circle(ax, ay, 7);
        vg.PathWinding(NvgSolidity.Hole);
        vg.FillPaint(paint);
        vg.Fill();

        vg.Restore();

        vg.Restore();
    }

    private void DrawLines(NvgContext vg, float x, float y, float w, float h, float t)
    {
        const float pad = 5.0f;
        var s = w / 9.0f - pad * 2;
        var pts = new float[4 * 2];
        NvgLineCap[] joins = [
            NvgLineCap.Miter,
            NvgLineCap.Round,
            NvgLineCap.Bevel
        ];

        NvgLineCap[] caps = [
            NvgLineCap.Butt,
            NvgLineCap.Round,
            NvgLineCap.Square
        ];

        vg.Save();
        pts[0] = (float)(-s * 0.25f + Math.Cos(t * 0.3f) * s * 0.5f);
        pts[1] = (float)(Math.Sin(t * 0.3f) * s * 0.5f);
        pts[2] = -s * 0.25f;
        pts[3] = 0;
        pts[4] = s * 0.25f;
        pts[5] = 0;
        pts[6] = (float)(s * 0.25f + Math.Cos(-t * 0.3f) * s * 0.5f);
        pts[7] = (float)(Math.Sin(-t * 0.3f) * s * 0.5f);

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var fx = x + s * 0.5f + (i * 3 + j) / 9.0f * w + pad;
                var fy = y - s * 0.5f + pad;

                vg.LineCap(caps[i]);
                vg.LineJoin(joins[j]);

                vg.StrokeWidth(s * 0.3f);
                vg.StrokeColor(Nvg.RGBA(0, 0, 0, 160));
                vg.BeginPath();
                vg.MoveTo(fx + pts[0], fy + pts[1]);
                vg.LineTo(fx + pts[2], fy + pts[3]);
                vg.LineTo(fx + pts[4], fy + pts[5]);
                vg.LineTo(fx + pts[6], fy + pts[7]);
                vg.Stroke();

                vg.LineCap(NvgLineCap.Butt);
                vg.LineJoin(NvgLineCap.Bevel);

                vg.StrokeWidth(1.0f);
                vg.StrokeColor(Nvg.RGBA(0, 192, 255, 255));
                vg.BeginPath();
                vg.MoveTo(fx + pts[0], fy + pts[1]);
                vg.LineTo(fx + pts[2], fy + pts[3]);
                vg.LineTo(fx + pts[4], fy + pts[5]);
                vg.LineTo(fx + pts[6], fy + pts[7]);
                vg.Stroke();
            }
        }


        vg.Restore();
    }

    public void LoadDemoData(NvgContext? vgInst, DemoData data)
    {
        int i;

        if (vgInst == null)
            throw new InvalidOperationException("NanoVG context is not initialised");

        var vg = vgInst.Value;

        for (i = 0; i < 12; i++)
        {
            var file = $"assets/images/image{i + 1}.jpg";
            data.images[i] = vg.CreateImage(file, 0);

            if (data.images[i] != 0)
                continue;

            throw new InvalidOperationException($"Could not load {file}.");
        }

        data.fontIcons = vg.CreateFont("icons", "assets/entypo.ttf");
        if (data.fontIcons == -1)
        {
            throw new InvalidOperationException("Could not add font icons.");
        }

        // To add CJK text rendering support, use font asset that supported such characters
        // data.fontNormal = vg.CreateFont("sans", "assets/SourceHanSansCN-Regular.ttf");
        data.fontNormal = vg.CreateFont("sans", "assets/Roboto-Regular.ttf");
        if (data.fontNormal == -1)
        {
            throw new InvalidOperationException("Could not add font italic.");
        }

        data.fontBold = vg.CreateFont("sans-bold", "assets/Roboto-Bold.ttf");
        if (data.fontBold == -1)
        {
            throw new InvalidOperationException("Could not add font bold.");
        }

        data.fontEmoji = vg.CreateFont("emoji", "assets/NotoEmoji-Regular.ttf");
        if (data.fontEmoji == -1)
        {
            throw new InvalidOperationException("Could not add font emoji.");
        }

        vg.AddFallbackFontId(data.fontNormal, data.fontEmoji);
        vg.AddFallbackFontId(data.fontBold, data.fontEmoji);
    }

    public void FreeDemoData(NvgContext? vg, DemoData data)
    {
        int i;

        if (vg == null)
            return;

        for (i = 0; i < 12; i++)
            vg?.DeleteImage(data.images[i]);
    }

    private void DrawParagraph(NvgContext vg, float x, float y, float width, float height, Utf8RawString text,
        Utf8RawString hoverText, float mx, float my)
    {
        var rows = new NvgTextRow[3];
        var glyphs = new NVGglyphPosition[100];
        var lNum = 0;
        float lineH;
        float pX;
        var bounds = Vector4.Zero;
        float gx = 0, gy = 0;
        var gutter = 0;

        vg.Save();

        vg.FontSize(15.0f);
        vg.FontFace("sans");
        vg.TextAlign(NvgAlign.Left | NvgAlign.Top);
        vg.TextMetrics(out _, out _, out lineH);

        // The text break API can be used to fill a large buffer of rows,
        // or to iterate over the text just few lines (or just one) at a time.
        // The "next" variable of the last returned item tells where to continue.

        var start = text.GetPointer();
        var end = text.GetEndAddress();

        int nRows;
        while ((nRows = vg.TextBreakLines(start, end, width, rows, 3)) > 0)
        {
            int i;
            for (i = 0; i < nRows; i++)
            {
                var row = rows[i];

                var hit = mx > x && mx < (x + width) && my >= y && my < (y + lineH);

                vg.BeginPath();
                vg.FillColor(Nvg.RGBA(255, 255, 255, hit ? (byte)64 : (byte)16));
                vg.Rect(x + row.minx, y, row.maxx - row.minx, lineH);
                vg.Fill();

                vg.FillColor(Nvg.RGBA(255, 255, 255, 255));
                vg.Text(x, y, row.start, row.end);

                if (hit)
                {
                    var caretX = mx < x + row.width / 2 ? x : x + row.width;
                    pX = x;
                    var nGlyphs = vg.TextGlyphPositions(x, y, row.start, row.end, glyphs, 100);
                    int j;
                    for (j = 0; j < nGlyphs; j++)
                    {
                        var x0 = glyphs[j].x;
                        var x1 = (j + 1 < nGlyphs) ? glyphs[j + 1].x : x + row.width;
                        var gx1 = x0 * 0.3f + x1 * 0.7f;
                        if (mx >= pX && mx < gx1)
                            caretX = glyphs[j].x;
                        pX = gx1;
                    }

                    vg.BeginPath();
                    vg.FillColor(Nvg.RGBA(255, 192, 0, 255));
                    vg.Rect(caretX, y, 1, lineH);
                    vg.Fill();

                    gutter = lNum + 1;
                    gx = x - 10;
                    gy = y + lineH / 2;
                }

                lNum++;
                y += lineH;
            }

            // Keep going...
            start = rows[nRows - 1].next;
        }

        if (gutter > 0)
        {
            using var txtStr = new Utf8RawString(gutter.ToString());

            vg.FontSize(12.0f);
            vg.TextAlign(NvgAlign.Right | NvgAlign.Middle);

            vg.TextBounds(gx, gy, txtStr, ref bounds);

            vg.BeginPath();
            vg.FillColor(Nvg.RGBA(255, 192, 0, 255));
            vg.RoundedRect(
                (int)bounds.GetLeft() - 4,
                (int)bounds.GetTop() - 2,
                bounds.GetWidth() + 8,
                bounds.GetHeight() + 4,
                (bounds.GetHeight() + 4) / 2.0f - 1
            );
            vg.ClosePath();
            vg.Fill();

            vg.FillColor(Nvg.RGBA(32, 32, 32, 255));
            vg.Text(gx, gy, txtStr);
        }

        y += 20.0f;

        bounds = Vector4.Zero;

        vg.FontSize(11.0f);
        vg.TextAlign(NvgAlign.Left | NvgAlign.Top);
        vg.TextLineHeight(1.2f);
        vg.TextBoxBounds(x, y, 150, hoverText, ref bounds);

        // Fade the tooltip out when close to it.
        gx = Clamp(mx, bounds.GetLeft(), bounds.GetRight()) - mx;
        gy = Clamp(my, bounds.GetTop(), bounds.GetBottom()) - my;
        var a = (float)(Math.Sqrt(gx * gx + gy * gy) / 30.0f);
        a = Clamp(a, 0, 1);
        vg.GlobalAlpha(a);

        vg.BeginPath();
        vg.FillColor(Nvg.RGBA(220, 220, 220, 255));
        vg.RoundedRect(
            bounds.GetLeft() - 2,
            bounds.GetTop() - 2,
            bounds.GetWidth() + 4,
            bounds.GetHeight() + 4,
            3);
        pX = (int)((bounds.GetRight() + bounds.GetLeft()) / 2);
        vg.MoveTo(pX, bounds.GetTop() - 10);
        vg.LineTo(pX + 7, bounds.GetTop() + 1);
        vg.LineTo(pX - 7, bounds.GetTop() + 1);
        vg.ClosePath();
        vg.Fill();

        vg.FillColor(Nvg.RGBA(0, 0, 0, 220));
        vg.TextBox(x, y, 150, hoverText);
        
        vg.Restore();
    }

    private void DrawWidths(NvgContext vg, float x, float y, float width)
    {
        int i;

        vg.Save();

        vg.StrokeColor(Nvg.RGBA(0, 0, 0, 255));

        for (i = 0; i < 20; i++)
        {
            var w = (i + 0.5f) * 0.1f;
            vg.StrokeWidth(w);
            vg.BeginPath();
            vg.MoveTo(x, y);
            vg.LineTo(x + width, y + width * 0.3f);
            vg.Stroke();
            y += 10;
        }

        vg.Restore();
    }

    private void DrawCaps(NvgContext vg, float x, float y, float width)
    {
        int i;
        // TODO: caps
        NvgLineCap[] caps = [
            NvgLineCap.Butt, 
            NvgLineCap.Round,
            NvgLineCap.Round
        ];
        var lineWidth = 8.0f;

        vg.Save();

        vg.BeginPath();
        vg.Rect(x - lineWidth / 2, y, width + lineWidth, 40);
        vg.FillColor(Nvg.RGBA(255, 255, 255, 32));
        vg.Fill();

        vg.BeginPath();
        vg.Rect(x, y, width, 40);
        vg.FillColor(Nvg.RGBA(255, 255, 255, 32));
        vg.Fill();

        vg.StrokeWidth(lineWidth);
        for (i = 0; i < 3; i++)
        {
            vg.LineCap(caps[i]);
            vg.StrokeColor(Nvg.RGBA(0, 0, 0, 255));
            vg.BeginPath();
            vg.MoveTo(x, y + i * 10 + 5);
            vg.LineTo(x + width, y + i * 10 + 5);
            vg.Stroke();
        }

        vg.Restore();
    }

    private void DrawScissor(NvgContext vg, float x, float y, float t)
    {
        vg.Save();

        // Draw first rect and set scissor to it's area.
        vg.Translate(x, y);
        vg.Rotate(Nvg.DegToRad(5));
        vg.BeginPath();
        vg.Rect(-20, -20, 60, 40);
        vg.FillColor(Nvg.RGBA(255, 0, 0, 255));
        vg.Fill();
        vg.Scissor(-20, -20, 60, 40);

        // Draw second rectangle with offset and rotation.
        vg.Translate(40, 0);
        vg.Rotate(t);

        // Draw the intended second rectangle without any scissoring.
        vg.Save();
        vg.ResetScissor();
        vg.BeginPath();
        vg.Rect(-20, -10, 60, 30);
        vg.FillColor(Nvg.RGBA(255, 128, 0, 64));
        vg.Fill();
        vg.Restore();

        // Draw second rectangle with combined scissoring.
        vg.IntersectScissor(-20, -10, 60, 30);
        vg.BeginPath();
        vg.Rect(-20, -10, 60, 30);
        vg.FillColor(Nvg.RGBA(255, 128, 0, 255));
        vg.Fill();

        vg.Restore();
    }

    public void RenderDemo(NvgContext vg, float mx, float my, float width, float height,
        float t, bool blowup, DemoData data)
    {
        float x, y, popy;

        DrawEyes(vg, width - 250, 50, 150, 100, mx, my, t);
        DrawParagraph(vg, width - 450, 50, 150, 100, _mainText, _mainHoverText, mx, my);
        DrawGraph(vg, 0, height / 2, width, height / 2, t);
        DrawColorwheel(vg, width - 300, height - 300, 250.0f, 250.0f, t);

        // Line joints
        DrawLines(vg, 120, height - 50, 600, 50, t);

        // Line caps
        DrawWidths(vg, 10, 50, 30);

        // Line caps
        DrawCaps(vg, 10, 300, 30);

        DrawScissor(vg, 50, height - 80, t);

        vg.Save();
        if (blowup)
        {
            vg.Rotate((float)(Math.Sin(t * 0.3f) * 5.0f / 180.0f * Math.PI));
            vg.Scale(2.0f, 2.0f);
        }

        // Widgets
        DrawWindow(vg, "Widgets `n Stuff", 50, 50, 300, 400);
        x = 60;
        y = 95;
        DrawSearchBox(vg, "Search", x, y, 280, 25);
        y += 40;
        DrawDropDown(vg, "Effects", x, y, 280, 28);
        popy = y + 14;
        y += 45;

        // Form
        DrawLabel(vg, "Login", x, y, 280, 20);
        y += 25;
        DrawEditBox(vg, "Email", x, y, 280, 28);
        y += 35;
        DrawEditBox(vg, "Password", x, y, 280, 28);
        y += 38;
        DrawCheckBox(vg, "Remember me", x, y, 140, 28);
        DrawButton(vg, IconLogin, "Sign in", x + 138, y, 140, 28, Nvg.RGBA(0, 96, 128, 255));
        y += 45;

        // Slider
        DrawLabel(vg, "Diameter", x, y, 280, 20);
        y += 25;
        DrawEditBoxNum(vg, "123.00", "px", x + 180, y, 100, 28);
        DrawSlider(vg, 0.4f, x, y, 170, 28);
        y += 55;

        DrawButton(vg, IconTrash, "Delete", x, y, 160, 28, Nvg.RGBA(128, 16, 8, 255));
        DrawButton(vg, 0, "Cancel", x + 170, y, 110, 28, Nvg.RGBA(0, 0, 0, 0));

        // Thumbnails box
        DrawThumbnails(vg, 365, popy - 30, 160, 300, data.images, t);

        vg.Restore();
    }

    public void Dispose()
    {
        _mainText.Dispose();
        _mainHoverText.Dispose();
    }
}