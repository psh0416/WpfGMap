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
    class GMapCircle : GMapMarker, IShapable
    {
        public GMapCircle(PointLatLng pos, PointLatLng pos2) : base(new PointLatLng((pos.Lat + pos2.Lat) / 2, (pos.Lng + pos2.Lng) / 2))
        {
            //this.Position = new PointLatLng((pos.Lat + pos2.Lat) / 2, (pos.Lng + pos2.Lng) / 2);
            points = new List<PointLatLng>();
            points.Add(pos);
            points.Add(pos2);

            Path path = new Path();
            //(Shape as Path)
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 2;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(150, 204, 204, 255);
            path.Fill = mySolidColorBrush;
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            //GeometryGroup myGeometryGroup = new GeometryGroup();
            //myGeometryGroup.Children.Add(myEllipseGeometry);
            path.Data = myEllipseGeometry;
            path.IsHitTestVisible = false;
            Shape = path;
        }

        private List<PointLatLng> points;
        public List<PointLatLng> Points { get => points; set => points = value; }

        public Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = (Shape as Path);
            EllipseGeometry myEllipseGeometry = path.Data as EllipseGeometry;
            //EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            var point1 = Map.FromLatLngToLocal(points[0]);
            var point2 = Map.FromLatLngToLocal(points[1]);
            var radiusX = Math.Abs(point1.X - point2.X) / 2;
            var radiusY = Math.Abs(point1.Y - point2.Y) / 2;
            myEllipseGeometry.Center = new Point(radiusX, -radiusY);
            myEllipseGeometry.RadiusX = radiusX;
            myEllipseGeometry.RadiusY = radiusY;

            //GeometryGroup myGeometryGroup = new GeometryGroup();
            //myGeometryGroup.Children.Add(myEllipseGeometry);


            //Shape = path;
            return path;
        }
    }
}
