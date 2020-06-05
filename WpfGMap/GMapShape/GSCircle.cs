using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace WpfGMap.GMapShape
{
    class GSCircle : GMapBaseShape
    {
        public GSCircle(PointLatLng pos, PointLatLng pos2) : base(pos)
        {
            Points = new List<PointLatLng>();
            Points.Add(pos);
            Points.Add(pos2);
            var path = MyPath;
            var myEllipseGeometry = new EllipseGeometry();
            path.Data = myEllipseGeometry;
        }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = MyPath;
            var myEllipseGeometry = path.Data as EllipseGeometry;
            var radius = Math.Pow(Math.Pow(localPath[1].X - localPath[0].X, 2) + Math.Pow(localPath[1].Y - localPath[0].Y, 2), 0.5);
            myEllipseGeometry.Center = new Point(localPath[0].X, localPath[0].Y);
            myEllipseGeometry.RadiusX = Math.Abs(radius);
            myEllipseGeometry.RadiusY = Math.Abs(radius);

            return path;
        }
    }
}
