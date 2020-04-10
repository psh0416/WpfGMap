using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
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

            gMapPolygon = new GMapPolygon(pointLatLngs);
            gMapControl.RegenerateShape(gMapPolygon);
            (gMapPolygon.Shape as Path).StrokeThickness = 2;
            (gMapPolygon.Shape as Path).Stroke = Brushes.Red;
            (gMapPolygon.Shape as Path).Fill = new SolidColorBrush(Color.FromArgb(150, 150, 200, 0));
            //gMapPolygon.Shape.IsHitTestVisible = true;
            gMapControl.Markers.Add(gMapPolygon);
            var thumbs = new Thumb[pointLatLngs.Length];
            for (int i = 0; i < thumbs.Length; i++)
            {
                thumbs[i] = new Thumb();
                thumbs[i].Width = 12;
                thumbs[i].Height = 12;
                GMapMarker marker = new GMapMarker(pointLatLngs[i]);
                marker.Shape = thumbs[i];
                marker.Offset = new Point(-thumbs[i].Width / 2, -thumbs[i].Height / 2);
                thumbs[i].DragDelta += Thumb_DragDelta;
                thumbs[i].DragStarted += Thumb_DragStarted;
                thumbs[i].DragCompleted += Thumb_DragCompleted;
                thumbs[i].Background = Brushes.Blue;
                thumbs[i].Tag = i;
                gMapControl.Markers.Add(marker);
                dicMarker.Add(thumbs[i], marker);
            }
            //GMapPolygon polygon = new GMapPolygon(gpollist, "Circle");
            //marker.Shape = recShape;
            //marker.Tag = "PolyDot";
            //marker.Offset = new System.Windows.Point(-recShape.Width / 2, -recShape.Height / 2);


            GMapCircle gMapCircle = new GMapCircle(new PointLatLng(38, 128), new PointLatLng(38.001, 128.001));
            gMapControl.Markers.Add(gMapCircle);
        }
        GMapPolygon gMapPolygon;
        Dictionary<Thumb, GMapMarker> dicMarker = new Dictionary<Thumb, GMapMarker>();
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            int index = (int)thumb.Tag;
            GMapMarker marker = dicMarker[thumb];
            GPoint StartPoint = gMapControl.FromLatLngToLocal(dicMarker[(sender as Thumb)].Position);
            double posX = StartPoint.X + e.HorizontalChange;
            double posY = StartPoint.Y + e.VerticalChange;

            var pos = gMapControl.FromLocalToLatLng((int)posX, (int)posY);
            marker.Position = pos;
            gMapPolygon.Points[index] = pos;
            gMapControl.RegenerateShape(gMapPolygon);
            Console.WriteLine(gMapPolygon.Points[index]);
            //Console.WriteLine($"{e.HorizontalChange}, {e.VerticalChange}");
        }

        GPoint StartPoint;
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            (sender as Thumb).Background = Brushes.Blue;
        }

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            (sender as Thumb).Background = Brushes.Orange;
            StartPoint = gMapControl.FromLatLngToLocal(dicMarker[(sender as Thumb)].Position);
            //StartPoint.X = dicMarker[(sender as Thumb)].LocalPositionX;
            //StartPoint.Y = dicMarker[(sender as Thumb)].LocalPositionY;
        }
    }
}
