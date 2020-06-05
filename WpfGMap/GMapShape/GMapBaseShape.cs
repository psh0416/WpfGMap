using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    public abstract class GMapBaseShape : GMapMarker, IShapable
    {
        public EventHandler<string> ShapeChanged;
        public GMapBaseShape(PointLatLng point) : base(point)
        {
            PatternBrush = GetInitPatternBrush(30);
            Opacity = 1;
            StartPosition = point;
            Path path = new Path();
            Shape = path;

            Stroke = Brushes.Red;
            StrokeThickness = 2;
            Fill = new SolidColorBrush(Color.FromArgb(150, 204, 204, 255));


        }

        public Path MyPath => Shape as Path;
        public void AddContextMenu()
        {
            MyPath.ContextMenu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Click += ContextMenu_Click;
            item.Header = "삭제";
            MyPath.ContextMenu.Items.Add(item);

            item = new MenuItem();
            item.Click += ContextMenu_Click;
            item.Header = "수정";
            MyPath.ContextMenu.Items.Add(item);
        }

        public void ShapeComplete()
        {
            Shape.IsHitTestVisible = true;
            Shape.MouseLeftButtonDown += Shape_MouseLeftDown;
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
             
        }

        private void Shape_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                ShapeChanged?.Invoke(this, "선택");
        }

        public PointLatLng StartPosition { get; private set; }

        Brush PatternBrush;
        Rectangle PatternRect1;
        Rectangle PatternRect2;
        RotateTransform PatternRotate = new RotateTransform();
        private Brush GetInitPatternBrush(double size)
        {
            var brush = new VisualBrush();
            brush.TileMode = TileMode.Tile;
            brush.Viewport = new Rect(0, 0, size, size / 2);
            brush.Viewbox = new Rect(0, 0, size, size);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            brush.ViewboxUnits = BrushMappingMode.Absolute;
            brush.Transform = PatternRotate;
            UniformGrid grid = new UniformGrid();
            grid.Height = size; grid.Width = size;
            grid.Rows = 2;
            PatternRect1 = new Rectangle();
            PatternRect2 = new Rectangle();
            grid.Children.Add(PatternRect1);
            grid.Children.Add(PatternRect2);
            brush.Visual = grid;
            return brush;
        }

        private bool isPattern;
        public bool IsPattern
        {
            get => isPattern;
            set
            {
                if (isPattern != value)
                {
                    isPattern = value;
                    SetPathFill();
                }
            }
        }
        
        public double PatternAngle
        {
            get => PatternRotate.Angle;
            set
            {
                if (PatternRotate.Angle != value)
                {
                    PatternRotate.Angle = value;
                }
            }
        }

        private bool isFill = true;
        public bool IsFill
        {
            get => isFill;
            set
            {
                if (isFill != value)
                {
                    isFill = value;
                    if (isFill)
                        PatternRect1.Visibility = Visibility.Visible;
                    else
                        PatternRect1.Visibility = Visibility.Hidden;
                    SetPathFill();
                }
            }
        }

        private Brush fill;
        public Brush Fill
        {
            get => fill;
            set
            {
                fill = value;
                PatternRect1.Fill = value;
                SetPathFill();
            }
        }

        private Brush patternFill;
        public Brush PatternFill
        {
            get => patternFill;
            set
            {
                patternFill = value;
                PatternRect2.Fill = value;
            }
        }

        private void SetPathFill()
        {
            if (IsPattern)
            {
                MyPath.Fill = PatternBrush;
            }
            else
            {
                if (IsFill)
                    MyPath.Fill = Fill;
                else
                    MyPath.Fill = Brushes.Transparent;
            }
        }

        private Brush stroke;
        public Brush Stroke
        {
            get => stroke;
            set
            {
                stroke = value;
                if (MyPath != null)
                    MyPath.Stroke = value;
            }
        }

        private double strokeThickness;
        public double StrokeThickness
        {
            get => strokeThickness;
            set
            {
                strokeThickness = value;
                if (MyPath != null)
                    MyPath.StrokeThickness = value;
            }
        }

        private double opactiy = 1;
        public double Opacity
        {
            get => opactiy;
            set
            {
                opactiy = value;
                if (Shape != null)
                    Shape.Opacity = value;
            }
        }

        private Effect effect;
        public Effect Effect
        {
            get => effect;
            set
            {
                effect = value;
                if (Shape != null)
                    Shape.Effect = value;
            }
        }

        private Visibility visibility;
        public Visibility Visibility
        {
            get => visibility;
            set
            {
                Shape.Visibility = value;
                visibility = value;
            }
        }

        public List<PointLatLng> Points { get; set; }

        public PointLatLng GetMinPoint()
        {
            double minLat = Points[0].Lat,
                minLng = Points[0].Lng;
            for (int i = 1; i < Points.Count; i++)
            {
                if (minLat > Points[i].Lat)
                    minLat = Points[i].Lat;
                if (minLng > Points[i].Lng)
                    minLng = Points[i].Lng;
            }
            return new PointLatLng(minLat, minLng);
        }

        public PointLatLng GetMaxPoint()
        {
            double maxLat = Points[0].Lat,
                maxLng = Points[0].Lng;
            for (int i = 1; i < Points.Count; i++)
            {
                if (maxLat < Points[i].Lat)
                    maxLat = Points[i].Lat;
                if (maxLng < Points[i].Lng)
                    maxLng = Points[i].Lng;
            }
            return new PointLatLng(maxLat, maxLng);
        }

        public virtual Path CreatePath(List<Point> localPath, bool addBlurEffect) 
            => throw new System.NotImplementedException();
    }
}
