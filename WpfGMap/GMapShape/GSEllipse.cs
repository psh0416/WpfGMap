using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    class GSEllipse : GMapBaseShape
    {
        public GSEllipse(PointLatLng pos, PointLatLng pos2) : base(pos)
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
            var centerX = (localPath[1].X - localPath[0].X) / 2;
            var centerY = (localPath[1].Y - localPath[0].Y) / 2;
            myEllipseGeometry.Center = new Point(centerX, centerY);
            myEllipseGeometry.RadiusX = Math.Abs(centerX);
            myEllipseGeometry.RadiusY = Math.Abs(centerY);
            
            return path;
        }
    }
}
