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
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            //GeometryGroup myGeometryGroup = new GeometryGroup();
            //myGeometryGroup.Children.Add(myEllipseGeometry);
            path.Data = myEllipseGeometry;
            path.IsHitTestVisible = false;
            Shape = path;
        }

        public List<PointLatLng> Points { get; set; }

        public Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = (Shape as Path);
            EllipseGeometry myEllipseGeometry = path.Data as EllipseGeometry;
            //EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            var point1 = Map.FromLatLngToLocal(Points[0]);
            var point2 = Map.FromLatLngToLocal(Points[1]);
            var centerX = (point2.X - point1.X) / 2;
            var centerY = (point2.Y - point1.Y) / 2;
            myEllipseGeometry.Center = new Point(centerX, centerY);
            myEllipseGeometry.RadiusX = Math.Abs(centerX);
            myEllipseGeometry.RadiusY = Math.Abs(centerY);

            //GeometryGroup myGeometryGroup = new GeometryGroup();
            //myGeometryGroup.Children.Add(myEllipseGeometry);


            //Shape = path;
            return path;
        }
    }
}
