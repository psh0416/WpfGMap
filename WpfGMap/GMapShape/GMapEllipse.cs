using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    class GMapEllipse : GMapBaseShape
    {
        public GMapEllipse(PointLatLng pos, PointLatLng pos2) : base(pos)
        {
            //this.Position = new PointLatLng((pos.Lat + pos2.Lat) / 2, (pos.Lng + pos2.Lng) / 2);
            Points = new List<PointLatLng>();
            Points.Add(pos);
            Points.Add(pos2);

            Path path = new Path();
            Shape = path;

            Stroke = Brushes.Red;
            StrokeThickness = 2;
            Fill = new SolidColorBrush(Color.FromArgb(150, 204, 204, 255));

            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            path.Data = myEllipseGeometry;
            path.IsHitTestVisible = false;
        }

        public override List<PointLatLng> Points { get; set; }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = GetShape();
            EllipseGeometry myEllipseGeometry = path.Data as EllipseGeometry;
            var centerX = (localPath[1].X - localPath[0].X) / 2;
            var centerY = (localPath[1].Y - localPath[0].Y) / 2;
            myEllipseGeometry.Center = new Point(centerX, centerY);
            myEllipseGeometry.RadiusX = Math.Abs(centerX);
            myEllipseGeometry.RadiusY = Math.Abs(centerY);

            return path;
        }
    }
}
