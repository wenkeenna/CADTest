using SkiaSharp;
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
                        _currentDrawingCommand.Start(point.ToSKPoint());
                        break;
                    case DrawCommand.Arc:
                        _currentDrawingCommand = new DrawArcCommand();
                        break;
                    case DrawCommand.Circle:
                        var point_Circle = e.GetPosition(sk_Canvas);
                        _currentDrawingCommand = new DrawCircleCommand();
                        _currentDrawingCommand.Start(point_Circle.ToSKPoint());
                        break;
                    case DrawCommand.Rectangle:
                        var point_Rect = e.GetPosition(sk_Canvas);
                        _currentDrawingCommand = new DrawRectangleCommand();
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
                    _currentDrawingCommand.End(point.ToSKPoint());
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
                    if (_currentDrawingCommand._startPoint.X > point.X  || _currentDrawingCommand._startPoint.Y> point.Y)
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

            test(sender,e);
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

       
 
         

       
            // 绘制网格
            float gridSize = 20;
            float xStart = 0;
            float yStart = 0;


            while (xStart < width)
            {
                canvas.DrawLine(xStart, 0, xStart, width, paint);
                xStart += gridSize;
            }
            while (yStart < height)
            {
                canvas.DrawLine(0, yStart, height, yStart, paint);
                yStart += gridSize;
            }
        }

    }

    enum DrawCommand
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
