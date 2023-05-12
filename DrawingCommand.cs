using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SkiaSharp.Views.WPF;

namespace CAD_1
{
    public abstract class DrawingCommand
    {
        public SKPoint _startPoint;
        public SKPoint _endPoint;
        public abstract void Start(SKPoint startPoint);

        public abstract void Move(SKPoint startPoint);

        public abstract void End(SKPoint startPoint);

        public abstract void Draw(SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e, SKElement element, List<DrawingCommand> drawings);


    }
}
