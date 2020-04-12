using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    class GMapRectangle : GMapMarker, IShapable
    {
        public GMapRectangle(PointLatLng pos, PointLatLng pos2) : base(new PointLatLng((pos.Lat + pos2.Lat) / 2, (pos.Lng + pos2.Lng) / 2))
        {
            //this.Position = new PointLatLng((pos.Lat + pos2.Lat) / 2, (pos.Lng + pos2.Lng) / 2);
            Points = new List<PointLatLng>();
            Points.Add(pos);
            Points.Add(pos2);

            Path path = new Path();
            //(Shape as Path)
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 2;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(150, 204, 204, 255);
            path.Fill = mySolidColorBrush;
            RectangleGeometry myRectangleGeometry = new RectangleGeometry();
            path.Data = myRectangleGeometry;
            path.IsHitTestVisible = false;
            Shape = path;
        }

        public List<PointLatLng> Points { get; set; }

        public Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = (Shape as Path);
            RectangleGeometry myRectangleGeometry = path.Data as RectangleGeometry;
            var point1 = Map.FromLatLngToLocal(Points[0]);
            var point2 = Map.FromLatLngToLocal(Points[1]);
            myRectangleGeometry.Rect = new Rect(new Point(0, 0), new Point(point2.X - point1.X, point2.Y - point1.Y));

            return path;
        }
    }
}
