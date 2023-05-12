using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CAD_1
{
    public class DrawRectangleCommand : DrawingCommand
    {

        //private SKPoint _startPoint;
        //private SKPoint _endPoint;
        public override void Draw(SKPaintSurfaceEventArgs e, SKElement element, List<DrawingCommand> drawings)
        {
            var canvas = e.Surface.Canvas;

            // 获取视图控件的尺寸
            var viewWidth = (float)element.ActualWidth;
            var viewHeight = (float)element.ActualHeight;
            // 计算视图控件和画布之间的比例
            var scale = Math.Min(viewWidth / e.Info.Width, viewHeight / e.Info.Height);
            // 在视图控件坐标系中转换起始点和终止点
            var transformedStartPoint = new SKPoint(_startPoint.X / scale, _startPoint.Y / scale);
            var transformedEndPoint = new SKPoint(_endPoint.X / scale, _endPoint.Y / scale);

            canvas.DrawRect(transformedStartPoint.X, transformedStartPoint.Y, Math.Abs(transformedEndPoint.X- transformedStartPoint.X), Math.Abs(transformedEndPoint.Y - transformedStartPoint.Y), new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                Style = SKPaintStyle.Stroke
            }); ;
        }

        public override void End(SKPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public override void Move(SKPoint currentPoint)
        {
            _endPoint = currentPoint;
        }

        public override void Start(SKPoint startPoint)
        {
            _startPoint = startPoint;
            _endPoint = startPoint;
        }
    }
}
