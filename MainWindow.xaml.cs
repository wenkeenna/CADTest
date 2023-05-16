using HandyControl.Expression.Media;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;



namespace CAD_1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrawCommand _currentCommand = DrawCommand.None;

        private DrawingCommand _currentDrawingCommand;
        private readonly List<DrawingCommand> _commands = new List<DrawingCommand>();

        private bool _mouseDownCount = false;
        public MainWindow()
        {
            InitializeComponent();
            _currentDrawingCommand = new DrawLineCommand();
        }


        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SKCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_mouseDownCount)
            {
                _mouseDownCount = true;
                switch (_currentCommand)
                {
                    case DrawCommand.Line:
                        var point = e.GetPosition(sk_Canvas);
                        _currentDrawingCommand = new DrawLineCommand();
                        _currentDrawingCommand.drawCommand=DrawCommand.Line;
                        if (FindNearestPoint(point) == point)
                        {
                            _currentDrawingCommand.Start(point.ToSKPoint());
                        }
                        else
                        {
                            _currentDrawingCommand.Start(FindNearestPoint(point).ToSKPoint());
                        }
                       
                        break;
                    case DrawCommand.Arc:
                        _currentDrawingCommand = new DrawArcCommand();
                        _currentDrawingCommand.drawCommand = DrawCommand.Arc;
                        break;
                    case DrawCommand.Circle:
                        var point_Circle = e.GetPosition(sk_Canvas);
                        _currentDrawingCommand = new DrawCircleCommand();
                        _currentDrawingCommand.drawCommand = DrawCommand.Circle;
                        if (FindNearestPoint(point_Circle) == point_Circle)
                        {
                            _currentDrawingCommand.Start(point_Circle.ToSKPoint());
                        }
                        else
                        {
                            _currentDrawingCommand.Start(FindNearestPoint(point_Circle).ToSKPoint());
                        }
                        //   _currentDrawingCommand.Start(point_Circle.ToSKPoint());
                        break;
                    case DrawCommand.Rectangle:
                        var point_Rect = e.GetPosition(sk_Canvas);
                        _currentDrawingCommand = new DrawRectangleCommand();
                        _currentDrawingCommand.drawCommand = DrawCommand.Rectangle;

                        _currentDrawingCommand.Start(point_Rect.ToSKPoint());
                        break;
                }
            }
            else
            {
                _mouseDownCount = false;
                if (_currentCommand != null)
                {
                    var point = e.GetPosition(sk_Canvas);
                    if (_currentCommand == DrawCommand.Circle || _currentCommand == DrawCommand.Rectangle)
                    {
                        if (_currentDrawingCommand._startPoint.X > point.X || _currentDrawingCommand._startPoint.Y > point.Y)
                        {
                            return;
                        }
                    }

                    if (FindNearestPoint(point) == point)
                    {
                        _currentDrawingCommand.End(point.ToSKPoint());
                    }
                    else
                    {
                        _currentDrawingCommand.End(FindNearestPoint(point).ToSKPoint());
                    }
                //    _currentDrawingCommand.End(point.ToSKPoint());
                    _commands.Add(_currentDrawingCommand);
                    sk_Canvas.InvalidateVisual();

                }
            }
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SKCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (/*e.LeftButton == MouseButtonState.Pressed &&*/  _mouseDownCount && _currentCommand != null)
            {
                var point = e.GetPosition(sk_Canvas);
                if (_currentCommand == DrawCommand.Circle || _currentCommand == DrawCommand.Rectangle)
                {
                    if (_currentDrawingCommand._startPoint.X > point.X || _currentDrawingCommand._startPoint.Y > point.Y)
                    {
                        return;
                    }
                }
                _currentDrawingCommand.Move(point.ToSKPoint());
               
                sk_Canvas.InvalidateVisual();
            }
        }


        /// <summary>
        /// 页面重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sk_Canvas_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.White);

            foreach (var command in _commands)
            {
                command.Draw(e, sk_Canvas, _commands);
            }

            if (_currentCommand != null)
            {
                _currentDrawingCommand.Draw(e, sk_Canvas, _commands);
            }

            test(sender, e);
        }

      


        /// <summary>
        /// 绘制命令选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Content)
            {
                case "直线":
                    _currentCommand = DrawCommand.Line;
                    break;
                case "圆弧":
                    _currentCommand = DrawCommand.Arc;
                    break;
                case "圆":
                    _currentCommand = DrawCommand.Circle;
                    break;
                case "矩形":
                    _currentCommand = DrawCommand.Rectangle;
                    break;
                case "剪切":
                    _currentCommand = DrawCommand.Cut;
                    break;
            }
        }

        private void test(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            // 获取画布对象
            SKCanvas canvas = e.Surface.Canvas;

            // 设置画笔样式
            SKPaint paint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.LightGray,
                StrokeWidth = 1,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true
            };
            // 获取视图的宽度和高度
            float width = (float)sk_Canvas.ActualWidth;
            float height = (float)sk_Canvas.ActualHeight;

            // 获取设备的像素密度
            var dpi = VisualTreeHelper.GetDpi(sk_Canvas);
            // 根据设备像素比例调整元素大小
            var scaledWidth = width * dpi.DpiScaleX;
            var scaledHeight = height * dpi.DpiScaleY;


            // 绘制网格
            float gridSize = 20;
            float xStart =10;
            float yStart = 10 ;

            int xLineCoun = (int)(scaledWidth / gridSize);
            int yLineCoun = (int)(scaledHeight / gridSize);

            for (int i = 0; i < xLineCoun; i++)
            {
                canvas.DrawLine(xStart, 10, xStart, (float)yLineCoun* gridSize-10, paint);
                xStart += gridSize;
            }
            for (int i = 0; i < yLineCoun; i++)
            {
                canvas.DrawLine(10, yStart, (float)xLineCoun * gridSize-10, yStart, paint);
                yStart += gridSize;
            }
        }


        private int snapRange = 10; // 捕捉范围
        private System.Windows.Point FindNearestPoint(System.Windows.Point currentPoint)
        {
            // 如果点列表为空，返回空点
            if (_commands.Count == 0)
            {
                return currentPoint;
            }
            //System.Windows.Point nearestPoint = _commands[0]._startPoint.ToPoint(); // 先将第一个点作为最近点
            System.Windows.Point nearestPoint = currentPoint; // 先将第一个点作为最近点
                                                              // double shortestDistance = Distance(currentPoint, nearestPoint); // 计算当前点到最近点的距离

            // 从第二个点开始遍历，找到距离当前点最近的点
         
            for (int i = 0; i < _commands.Count; i++)
            {
                if (_commands[i].drawCommand == DrawCommand.Line)
                {
                    double distance_s = Distance(currentPoint, _commands[i]._startPoint.ToPoint()); // 计算当前点到第i个点的距离
                    double distance_e = Distance(currentPoint, _commands[i]._endPoint.ToPoint()); // 计算当前点到第i个点的距离
                    if (distance_s < distance_e)
                    {
                        if (distance_s < snapRange)
                        {
                            nearestPoint = _commands[i]._startPoint.ToPoint(); // 更新最近点
                                                                               // shortestDistance = distance_s; // 更新最短距离
                        }
                    }
                    else
                    {
                        if (distance_e < snapRange)
                        {
                            nearestPoint = _commands[i]._endPoint.ToPoint(); // 更新最近点
                                                                             //  shortestDistance = distance_e; // 更新最短距离
                        }
                    }
                }
                if (_commands[i].drawCommand == DrawCommand.Circle)
                {
                    double radius = Math.Sqrt(Math.Pow(_commands[i]._startPoint.X - _commands[i]._endPoint.X, 2) + Math.Pow(_commands[i]._startPoint.Y - _commands[i]._endPoint.Y, 2));
                    //// 计算鼠标点击点和圆心的距离
                    //double distance_s = Math.Sqrt(Math.Pow(currentPoint.X - _commands[i]._startPoint.X, 2) + Math.Pow(currentPoint.Y - _commands[i]._startPoint.Y, 2));
                    //double distance_e = Math.Sqrt(Math.Pow(currentPoint.X - _commands[i]._endPoint.X, 2) + Math.Pow(currentPoint.Y - _commands[i]._endPoint.Y, 2));

                    // 如果距离小于半径，说明鼠标点击点在圆内，最近点为圆心
                    //if (distance_s < distance_e)
                    //{
                    //    if (distance_s < snapRange)
                    //    {
                    //        nearestPoint = _commands[i]._startPoint.ToPoint();
                    //    }
                    //}
                    //else
                    //{
                        // 计算鼠标点击点和圆上的点的连线方向向量
                        double dx = currentPoint.X - _commands[i]._startPoint.X;
                        double dy = currentPoint.Y - _commands[i]._startPoint.Y;
                        double length = Math.Sqrt(dx * dx + dy * dy);
                        double directionX = dx / length;
                        double directionY = dy / length;

                        // 计算圆上的点
                        System.Windows.Point circlePoint = new System.Windows.Point((int)(_commands[i]._startPoint.X + directionX * radius), (int)(_commands[i]._startPoint.Y + directionY * radius));
                    // 最近点即为圆上的点
                    double distance_s = Distance(currentPoint, circlePoint); // 计算当前点到第i个点的距离
                    if (distance_s < snapRange)
                    {
                        nearestPoint = circlePoint;
                    }
                       
                    //}
                }
                if (_commands[i].drawCommand == DrawCommand.Rectangle)
                {

                }
            }
            return nearestPoint;
        }
        private double Distance(System.Windows.Point p1, System.Windows.Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

   public enum DrawCommand
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 直线
        /// </summary>
        Line,
        /// <summary>
        /// 圆弧
        /// </summary>
        Arc,
        /// <summary>
        /// 圆
        /// </summary>
        Circle,
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 裁剪
        /// </summary>
        Cut
    }
}
