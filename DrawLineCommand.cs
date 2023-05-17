using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CAD_1
{
    public class DrawLineCommand : DrawingCommand
    {
     
     
        public List<SKPath> Paths=new List<SKPath>();
        SKElement _element = new SKElement();

        public override void Start(SKPoint startPoint)
        {
            _startPoint = startPoint;
            _endPoint = startPoint;
        }

        public override void Move(SKPoint currentPoint)
        {
            _endPoint = currentPoint;
        }

        public override void End(SKPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public override void Draw(SKPaintSurfaceEventArgs e, SKElement element, List<DrawingCommand> drawings)
        {
            var canvas = e.Surface.Canvas;
            _element = element;
            // 获取视图控件的尺寸
            var viewWidth = (float)element.ActualWidth;
            var viewHeight = (float)element.ActualHeight;

            // 计算视图控件和画布之间的比例
            var scale = Math.Min(viewWidth / e.Info.Width, viewHeight / e.Info.Height);

            // 在视图控件坐标系中转换起始点和终止点
            var transformedStartPoint = new SKPoint(_startPoint.X / scale, _startPoint.Y / scale);
            var transformedEndPoint = new SKPoint(_endPoint.X / scale, _endPoint.Y / scale);

            canvas.DrawLine(transformedStartPoint, transformedEndPoint, new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                IsStroke = true,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            });
        }
    }
}
