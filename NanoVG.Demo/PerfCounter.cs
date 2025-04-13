using NanoVG.Extensions;

namespace NanoVG.Demo
{
	public class PerfCounter : IDisposable
	{
		private const int GraphHistoryCount = 100;
		
		internal GraphRenderStyle style;
		internal string name;
		internal float[] values = new float[GraphHistoryCount];
		internal int head;

		public int? fpsLimiter;
		
		public enum GraphRenderStyle {
			RenderFPS,
			RenderMs,
			RenderPercent,
		};
		
		public void initGraph(GraphRenderStyle style, string name)
		{
			this.style = style;
			this.name = name;
		}

		public void updateGraph(float frameTime)
		{
			head = (head+1) % GraphHistoryCount;
			values[head] = frameTime;
		}

		float getGraphAverage()
		{
			int i;
			float avg = 0;
			for (i = 0; i < GraphHistoryCount; i++) {
				avg += values[i];
			}
			return avg / (float)GraphHistoryCount;
		}

		public void renderGraph(NvgContext vg, float x, float y)
		{
			int i;
			float avg, w, h;

			avg = getGraphAverage();

			w = 200;
			h = 35;

			vg.BeginPath();
			vg.Rect(x,y, w,h);
			vg.FillColor(Nvg.RGBA(0,0,0,128));
			vg.Fill();

			vg.BeginPath();
			vg.MoveTo(x, y+h);
			switch (style)
			{
				case GraphRenderStyle.RenderFPS:
				{
					var max = fpsLimiter ?? 80.0f;
					
					for (i = 0; i < GraphHistoryCount; i++) {
						var v = 1.0f / (0.00001f + values[(head+i) % GraphHistoryCount]);
						if (v > max)
							v = max;
						var vx = x + ((float)i/(GraphHistoryCount-1)) * w;
						var vy = y + h - ((v / max) * h);
						vg.LineTo(vx, vy);
					}

					break;
				}
				case GraphRenderStyle.RenderPercent:
				{
					for (i = 0; i < GraphHistoryCount; i++) {
						float v = values[(head+i) % GraphHistoryCount] * 1.0f;
						if (v > 100.0f)
							v = 100.0f;
						var vx = x + ((float)i/(GraphHistoryCount-1)) * w;
						var vy = y + h - ((v / 100.0f) * h);
						vg.LineTo(vx, vy);
					}

					break;
				}
				default:
				{
					for (i = 0; i < GraphHistoryCount; i++) {
						float v = values[(head+i) % GraphHistoryCount] * 1000.0f;
						float vx, vy;
						if (v > 20.0f) v = 20.0f;
						vx = x + ((float)i/(GraphHistoryCount-1)) * w;
						vy = y + h - ((v / 20.0f) * h);
						vg.LineTo(vx, vy);
					}

					break;
				}
			}
			vg.LineTo(x+w, y+h);
			vg.FillColor(Nvg.RGBA(255,192,0,128));
			vg.Fill();

			vg.FontFace("sans");

			if (string.IsNullOrEmpty(name)) {
				vg.FontSize(12.0f);
				vg.TextAlign(NvgAlign.Left | NvgAlign.Top);
				vg.FillColor(Nvg.RGBA(240,240,240,192));
				vg.Text(x+3,y+3, name);
			}

			switch (style)
			{
				case GraphRenderStyle.RenderFPS:
					vg.FontSize(15.0f);
					vg.TextAlign(NvgAlign.Right | NvgAlign.Top);
					vg.FillColor(Nvg.RGBA(240,240,240,255));
					
					vg.Text(x+w-3,y+3, $"{(int)(1.0f / avg)}{(fpsLimiter.HasValue ? $" / {fpsLimiter}" : "")} FPS");

					vg.FontSize(13.0f);
					vg.TextAlign(NvgAlign.Right | NvgAlign.Baseline);
					vg.FillColor(Nvg.RGBA(240,240,240,160));
					
					vg.Text(x+w-3,y+h-3, $"{(avg * 1000.0f):F2} ms");
					break;
				case GraphRenderStyle.RenderPercent:
					vg.FontSize(15.0f);
					vg.TextAlign(NvgAlign.Right | NvgAlign.Top);
					vg.FillColor(Nvg.RGBA(240,240,240,255));
					
					vg.Text(x+w-3,y+3, $"{(avg * 1.0f):P1} %");
					break;
				default:
					vg.FontSize(15.0f);
					vg.TextAlign(NvgAlign.Right | NvgAlign.Top);
					vg.FillColor(Nvg.RGBA(240,240,240,255));
					vg.Text(x+w-3,y+3, $"{(avg * 1000.0f):F2} ms");
					break;
			}
		}

		public void Dispose()
		{
			// TODO release managed resources here
		}
	}
}