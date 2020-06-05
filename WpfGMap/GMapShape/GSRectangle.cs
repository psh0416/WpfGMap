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
    class GSRectangle : GMapBaseShape
    {
        double radius = 0;
        public GSRectangle(PointLatLng pos, PointLatLng pos2) : base(new PointLatLng((pos.Lat + pos2.Lat) / 2, (pos.Lng + pos2.Lng) / 2))
        {
            Points = new List<PointLatLng>();
            Points.Add(pos);
            Points.Add(pos2);

            var path = MyPath;
            path.Data = new RectangleGeometry();
        }

        public GSRectangle(PointLatLng pos, PointLatLng pos2, double radius) : this(pos, pos2)
        {
            this.radius = radius;
        }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = MyPath;
            RectangleGeometry myRectangleGeometry = path.Data as RectangleGeometry;
            var point1 = localPath[0];
            var point2 = localPath[1];
            myRectangleGeometry.Rect = new Rect(new Point(0, 0), new Point(point2.X - point1.X, point2.Y - point1.Y));
            var minSize = Math.Min(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y));
            var rad = minSize * radius;
            myRectangleGeometry.RadiusX = rad;
            myRectangleGeometry.RadiusY = rad;
            return path;
        }
    }
}
