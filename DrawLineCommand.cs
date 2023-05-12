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
                StrokeCap = SKStrokeCap.Round
            });
        }

        //// 定义一个捕捉最近点的方法
        //public SKPoint GetClosestPoint(SKPoint point)
        //{
        //    // 定义初始值为极大值
        //    var closestDistance = float.MaxValue;
        //    var closestPoint = SKPoint.Empty;

        //    // 循环遍历每一个图形路径
        //    foreach (var path in _element)
        //    {
        //        // 获取路径的所有点
        //        var points = path.GetPoints();

        //        // 遍历路径上的每个点
        //        for (var i = 0; i < points.Length; i++)
        //        {
        //            // 获取路径上的当前点和下一个点
        //            var startPoint = points[i];
        //            var endPoint = i < points.Length - 1 ? points[i + 1] : points[0];

        //            // 如果路径是一个圆弧
        //            if (path.IsArc(out var arc))
        //            {
        //                // 计算圆弧上最近的点
        //                var arcClosestPoint = GetClosestPointOnArc(point, arc);
        //                var distance = SKPoint.Distance(point, arcClosestPoint);

        //                // 如果距离比之前更接近，则更新最近点的位置
        //                if (distance < closestDistance)
        //                {
        //                    closestDistance = distance;
        //                    closestPoint = arcClosestPoint;
        //                }
        //            }
        //            // 如果路径是一个线段
        //            else if (path.IsLine(out var line))
        //            {
        //                // 计算线段上最近的点
        //                var lineClosestPoint = GetClosestPointOnLine(point, line.P0, line.P1);
        //                var distance = SKPoint.Distance(point, lineClosestPoint);

        //                // 如果距离比之前更接近，则更新最近点的位置
        //                if (distance < closestDistance)
        //                {
        //                    closestDistance = distance;
        //                    closestPoint = lineClosestPoint;
        //                }
        //            }
        //            // 如果路径是一个矩形
        //            else if (path.IsRect(out var rect))
        //            {
        //                // 计算矩形上最近的点
        //                var rectClosestPoint = GetClosestPointOnRect(point, rect);
        //                var distance = SKPoint.Distance(point, rectClosestPoint);

        //                // 如果距离比之前更接近，则更新最近点的位置
        //                if (distance < closestDistance)
        //                {
        //                    closestDistance = distance;
        //                    closestPoint = rectClosestPoint;
        //                }
        //            }
        //        }
        //    }

        //    // 返回最近点
        //    return closestPoint;
        //}

    }
}
