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
    public class DrawArcCommand : DrawingCommand
    {
        protected float _radius;
        public override void Draw(SKPaintSurfaceEventArgs e, SKElement element, List<DrawingCommand> drawings)
        {
            // 在画布上绘制圆弧
            // ...

            // 以下是绘制圆弧的代码示例，你可以根据需要进行修改
            var canvas = e.Surface.Canvas;

            // 获取视图控件的尺寸
            var viewWidth = (float)element.ActualWidth;
            var viewHeight = (float)element.ActualHeight;

            // 计算视图控件和画布之间的比例
            var scale = Math.Min(viewWidth / e.Info.Width, viewHeight / e.Info.Height);

            // 在视图控件坐标系中转换起始点和终止点
            var transformedStartPoint = new SKPoint(_startPoint.X / scale, _startPoint.Y / scale);
            var transformedEndPoint = new SKPoint(_endPoint.X / scale, _endPoint.Y / scale);


         

            // 使用SkiaSharp绘制圆弧
            var path = new SKPath();
         
            canvas.DrawPath(path, new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                Style = SKPaintStyle.Stroke
            });

            if (_radius > 0)
            {
                //double dx = _startPoint.X - _currentPoint.X;
                //double dy = _startPoint.Y - _currentPoint.Y;
                //double startAngle = Math.Atan2(dy, dx) * 180 / Math.PI;

                //double endAngle = Math.Atan2(_endPoint.Y - _currentPoint.Y, _endPoint.X - _currentPoint.X) * 180 / Math.PI;
                //double sweepAngle = endAngle - startAngle;
                //SKRect arcRect = new SKRect(_startPoint.X - _radius, _startPoint.Y - _radius, _startPoint.X + _radius, _startPoint.Y + _radius);

                double dx = transformedStartPoint.X - _currentPoint.X;
                double dy = transformedEndPoint.Y - _currentPoint.Y;
                double startAngle = Math.Atan2(dy, dx) * 180 / Math.PI;

                double endAngle = Math.Atan2(transformedEndPoint.Y - _currentPoint.Y, transformedEndPoint.X - _currentPoint.X) * 180 / Math.PI;
                double sweepAngle = endAngle - startAngle;
                SKRect arcRect = new SKRect(transformedStartPoint.X - _radius, transformedStartPoint.Y - _radius, transformedStartPoint.X + _radius, transformedStartPoint.Y + _radius);
                // 在canvas上根据起始点、结束点和半径绘制圆弧
                canvas.DrawArc(arcRect, (float)startAngle, (float)sweepAngle, false, new SKPaint { Color=SKColors.Black,StrokeWidth=2,Style=SKPaintStyle.Stroke});
            }
        }

        public override void End(SKPoint endPoint)
        {
            _endPoint = endPoint;
        }
        SKPoint _currentPoint;
        public override void Move(SKPoint currentPoint)
        {
            // 计算当前点与起始点之间的距离作为半径
            _radius = SKPoint.Distance(currentPoint, _startPoint);
            _endPoint = currentPoint;
            _currentPoint = currentPoint;
        }

        public override void Start(SKPoint startPoint)
        {
            _startPoint = startPoint;
            _endPoint = startPoint;
        }
    }
}
