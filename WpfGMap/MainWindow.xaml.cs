using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGMap.GMapEditor;
using WpfGMap.GMapShape;

namespace WpfGMap
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ShapeEditor shapeEditor = new ShapeEditor();
        IShapable SelectShape;
        List<IShapable> ShapeList = new List<IShapable>();
        private void gMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            gMapControl.CenterPosition = new PointLatLng(38, 128);
            gMapControl.MapProvider = GMapProviders.GoogleHybridMap;
            gMapControl.Zoom = 15;

            //GMapMarker marker = new GMapMarker(new PointLatLng(36.42632834385, 127.315889959));
            Rectangle recShape = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.Red

            };

            PointLatLng[] pointLatLngs =
            {
                new PointLatLng(38, 128),
                new PointLatLng(38.001, 128),
                new PointLatLng(38, 128.001),
                new PointLatLng(38.001, 128),
                new PointLatLng(38, 128.001)
            };

            GMapPolygon gMapPolygon = new GMapPolygon(pointLatLngs);
            gMapControl.RegenerateShape(gMapPolygon);
            (gMapPolygon.Shape as Path).StrokeThickness = 2;
            (gMapPolygon.Shape as Path).Stroke = Brushes.Red;
            (gMapPolygon.Shape as Path).Fill = new SolidColorBrush(Color.FromArgb(150, 150, 200, 0));
            gMapPolygon.Shape.IsHitTestVisible = true;
            
            gMapControl.IgnoreMarkerOnMouseWheel = true;


            GMapCircle gMapCircle = new GMapCircle(new PointLatLng(38, 128), new PointLatLng(38.001, 128.001));
            

            GMapRectangle gMapRectangle = new GMapRectangle(new PointLatLng(38, 128), new PointLatLng(38.001, 128.001));
            
            gMapRectangle.Shape.IsHitTestVisible = true;

            gMapControl.Markers.Add(gMapPolygon);
            gMapControl.Markers.Add(gMapCircle);
            gMapControl.Markers.Add(gMapRectangle);
            ShapeList.Add(gMapPolygon);
            ShapeList.Add(gMapCircle);
            ShapeList.Add(gMapRectangle);
        }

        private void ShapeEdit_Click(object sender, RoutedEventArgs e)
        {
            shapeEditor.ClearMarker();
            int index = ShapeList.IndexOf(SelectShape);
            if (++index >= ShapeList.Count)
                index = 0;
            SelectShape = ShapeList[index];
            shapeEditor.SetMarker(SelectShape, gMapControl);
        }

        private void ShapeCancel_Click(object sender, RoutedEventArgs e)
        {
            shapeEditor.ClearMarker();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                if (AddType == ShapeAddType.Polygon)
                {
                    ShapeList.Add(AddShape);
                    //(AddShape as GMapMarker).Clear();
                    AddType = ShapeAddType.None;
                    AddShape = null;
                }
            }
            else if (e.Key == Key.Escape)
            {
                (AddShape as GMapMarker).Clear();
                AddType = ShapeAddType.None;
                AddShape = null;
            }

        }

        ShapeAddType AddType;
        IShapable AddShape;
        private void gMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(gMapControl);
            PointLatLng LatLng = gMapControl.FromLocalToLatLng((int)point.X, (int)point.Y);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (AddType == ShapeAddType.Circle || AddType == ShapeAddType.Rectangle)
                {
                    AddShape.Points[1] = LatLng;
                    gMapControl.RegenerateShape(AddShape);
                }
            }
        }

        private void ShapeAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddType == ShapeAddType.None)
            {
                string content = (sender as Button).Content.ToString();
                if (content == "다각형 추가")
                    AddType = ShapeAddType.Polygon;
                else if (content == "원 추가")
                    AddType = ShapeAddType.Circle;
                else if (content == "사각형 추가")
                    AddType = ShapeAddType.Rectangle;

                if (AddShape == null)
                {
                    ShapeList.Remove(AddShape);
                }
            }
        }

        private void gMapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(gMapControl);
            PointLatLng LatLng = gMapControl.FromLocalToLatLng((int)point.X, (int)point.Y);
            if (AddType == ShapeAddType.Circle) 
            {
                AddShape = new GMapCircle(LatLng, LatLng);
                gMapControl.Markers.Add(AddShape as GMapMarker);
            }
            else if (AddType == ShapeAddType.Rectangle)
            {
                AddShape = new GMapRectangle(LatLng, LatLng);
                gMapControl.Markers.Add(AddShape as GMapMarker);
            }
            else if (AddType == ShapeAddType.Polygon)
            {
                if (AddShape == null)
                {
                    var list = new List<PointLatLng>();
                    list.Add(LatLng);
                    AddShape = new GMapPolygon(list);
                    gMapControl.Markers.Add(AddShape as GMapMarker);
                }
                else
                {
                    AddShape.Points.Add(LatLng);
                    gMapControl.RegenerateShape(AddShape);
                }
            }
            
        }

        private void gMapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AddType == ShapeAddType.Circle || AddType == ShapeAddType.Rectangle)
            {
                AddType = ShapeAddType.None;
                ShapeList.Add(AddShape);
                AddShape = null;
            }
        }
    }
    enum ShapeAddType
    {
        None, Polygon, Circle, Rectangle
    }
}
