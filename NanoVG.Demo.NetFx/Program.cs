using System;
using System.Numerics;
using glfw3;

namespace NanoVG.Demo.NetFx;

internal static class Program
{
    private static bool _scaleUpForm;
    private static bool _enableFramerateLock;
    private static int _targetFps = 144;
    private static float? _targetFrameDuration;

    private static GLFWwindow? _window;

    private static float GetFrameDuration() => _targetFrameDuration ??= 1.0f / (_targetFps + 1);

    private enum GlfwAnglePlatformType : int
    {
        None = 0x00037001,
        OpenGl = 0x00037002,
        OpenGlEs = 0x00037003,
        Direct3D9 = 0x00037004,
        Direct3D11 = 0x00037005,
        Vulkan = 0x00037007,
        Metal = 0x00037008
    }

    private enum NvgBackend
    {
        Software,
        GLES2,
        GLES3
    }


    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // GLFW_ANGLE_PLATFORM_TYPE
        Glfw.WindowHint(0x00050002, (int)GlfwAnglePlatformType.OpenGl);
        if (Glfw.Init() != 1)
            throw new InvalidOperationException();

        Glfw.SetErrorCallback(GlfwErrorCallbackPrivate);

        var backend = NvgBackend.GLES3;

        foreach (var arg in args)
        {
            switch (arg.ToLowerInvariant())
            {
                case "gles2":
                    Console.WriteLine("Using NanoVG OpenGLES2 backend instead");
                    backend = NvgBackend.GLES2;
                    break;
                
                case "gles3":
                    Console.WriteLine("Using NanoVG OpenGLES3 backend instead");
                    backend = NvgBackend.GLES2;
                    break;
                
                case "sw":
                    Console.WriteLine("Using NanoVG Software rasteriser backend instead");
                    backend = NvgBackend.Software;
                    break;
            }
        }

        switch (backend)
        {
            case NvgBackend.GLES2:
                Glfw.WindowHint((int)State.ContextCreationApi, (int)State.EglContextApi);
                Glfw.WindowHint((int)State.ClientApi, (int)State.OpenglEsApi);

                Glfw.WindowHint((int)State.ContextVersionMajor, 2);
                Glfw.WindowHint((int)State.ContextVersionMinor, 0);
                break;

            case NvgBackend.GLES3:
                Glfw.WindowHint((int)State.ContextCreationApi, (int)State.EglContextApi);
                Glfw.WindowHint((int)State.ClientApi, (int)State.OpenglEsApi);

                Glfw.WindowHint((int)State.ContextVersionMajor, 3);
                Glfw.WindowHint((int)State.ContextVersionMinor, 1);
                break;

            case NvgBackend.Software:
                //TODO:
                break;
        }

        var window = Glfw.CreateWindow(1000, 600, "NanoVG.Demo", null, null);

        _window = window ?? throw new InvalidOperationException("Cannot create GLFW Window");

        Glfw.SetKeyCallback(window, GlfwKeyCallbackPrivate);
        Glfw.MakeContextCurrent(window);

        var nvgLibName = backend switch
        {
            NvgBackend.GLES3 => $"{Nvg.LibNamePrefix}_GLES3",
            NvgBackend.GLES2 => $"{Nvg.LibNamePrefix}_GLES2",
            NvgBackend.Software => $"{Nvg.LibNamePrefix}_SW",
            _ => throw new ArgumentOutOfRangeException()
        };

        Nvg.FailFunctionLoadCallback = procName =>
            Console.WriteLine($"Cannot load function: {procName}");

        if (!Nvg.LoadLibrary(new Win32NvgLibraryLoader(), nvgLibName))
            throw new Exception("Unable to load NanoVG library");

        var nvgContextFlags = NvgCreateFlags.Antialias | NvgCreateFlags.StencilStrokes | NvgCreateFlags.Debug;

        var ctxInst = backend switch
        {
            NvgBackend.GLES3 => Nvg.CreateGLES3(nvgContextFlags),
            NvgBackend.GLES2 => Nvg.CreateGLES2(nvgContextFlags),
            NvgBackend.Software => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (!ctxInst.HasValue)
            throw new InvalidOperationException("Cannot create NanoVG context");

        var ctx = ctxInst.Value;

        Glfw.SwapInterval(0);
        Glfw.SetTime(0);

        var prevTime = Glfw.GetTime();

        int wW = 0, wH = 0;
        int fbW = 0, fbH = 0;
        double mx = 0, my = 0, ft = 0, dt = 0;

        using (var demo = new DemoGame())
        {
            var demoData = new DemoData();
            demo.LoadDemoData(ctx, demoData);

            var perf = new PerfCounter();
            perf.initGraph(PerfCounter.GraphRenderStyle.RenderFPS, "Frame Time");

            Console.WriteLine("Start loop");
            while (Glfw.WindowShouldClose(window) == 0)
            {
                ft = Glfw.GetTime();
                dt = ft - prevTime;

                Glfw.PollEvents();

                if (_enableFramerateLock && GetFrameDuration() >= dt)
                {
                    // Update with delta time here

                    continue;
                }

                prevTime = ft;

                perf.fpsLimiter = _enableFramerateLock ? _targetFps : null;
                perf.updateGraph((float)dt);

                Glfw.GetCursorPos(window, ref mx, ref my);
                Glfw.GetWindowSize(window, ref wW, ref wH);
                Glfw.GetFramebufferSize(window, ref fbW, ref fbH);

                var pxRatio = (float)fbW / wW;

                GLES.Viewport(0, 0, fbW, fbH);
                GLES.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
                GLES.Clear(0x00000100 | 0x00000400 | 0x00004000);

                ctx.BeginFrame(fbW, fbH, pxRatio);

                demo.RenderDemo(ctx, (float)mx, (float)my, wW, wH, (float)ft, _scaleUpForm, demoData);
                perf.renderGraph(ctx, 5, 5);
                
                DrawCursor(ctx, mx, my);
                //MyDrawPrivate(ctx);

                ctx.EndFrame();

                Glfw.SwapBuffers(window);
            }

            Console.WriteLine("Finalise");
            demo.FreeDemoData(ctx, demoData);
        }

        Glfw.DestroyWindow(window);

        switch (backend)
        {
            case NvgBackend.GLES3:
                Nvg.DeleteGLES3(ctx);
                break;
            case NvgBackend.GLES2:
                Nvg.DeleteGLES2(ctx);
                break;

            case NvgBackend.Software:
                break;
        }

        Glfw.Terminate();
        Nvg.UnloadLibrary();
    }

    private static void DrawCursor(NvgContext ctx, double mx, double my, float size = 16)
    {
        var half = size * 0.5f;
        ctx.Save();
        ctx.Translate((float)mx, (float)my);
        ctx.Rotate((float)(Glfw.GetTime() % 360.0f));
        ctx.BeginPath();
        ctx.FillColor(new Vector4(0.4f, 0.2f, 0.4f, 1.0f));
        ctx.Rect(-half, -half, size, size);
        ctx.Fill();
        ctx.ClosePath();
        ctx.Restore();
    }

    private static void GlfwKeyCallbackPrivate(IntPtr hWnd, int key, int scanCode, int action, int mods)
    {
        if (action != (int)State.Press)
            return;

        switch ((Key)key)
        {
            case Key.Space:
                _scaleUpForm = !_scaleUpForm;
                break;

            case Key.L:
                _enableFramerateLock = !_enableFramerateLock;
                break;

            case Key.I:
                _targetFps = Math.Min(288, ++_targetFps);
                _targetFrameDuration = null;
                break;

            case Key.K:
                _targetFps = Math.Max(1, --_targetFps);
                _targetFrameDuration = null;
                break;

            case Key.Escape:
                Glfw.SetWindowShouldClose(_window, 1);
                break;
        }
    }

    private static void GlfwErrorCallbackPrivate(int code, string desc)
    {
        throw new ApplicationException($"GLFW Error#{code}: {desc}");
    }
}