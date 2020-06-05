using GMap.NET;
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
    class GSCircularArc : GMapBaseShape
    {
        public GSCircularArc(PointLatLng pos, PointLatLng pos2) : base(pos)
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
            if (localPath.Count > 1)
            {
                Point center = new Point((localPath[0].X + localPath[1].X) / 2.0, localPath[1].Y);
                var radius = Math.Pow(Math.Pow(localPath[0].X - center.X, 2) + Math.Pow(localPath[0].Y - center.Y, 2), 0.5);
                var geometry = new StreamGeometry();
                var deg = Math.Atan2(center.Y - localPath[0].Y, center.X - localPath[0].X) * 2.0 * 180.0 / Math.PI;
                var direction = deg > 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                var isLargeArc = (deg > 180) || (deg < -180);
                Console.WriteLine("deg:" + deg);
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(localPath[0], true, true);
                    ctx.ArcTo(new Point(localPath[1].X, localPath[0].Y), new Size(radius, radius), deg, isLargeArc, direction, true, true);
                    ctx.LineTo(center, true, true);
                }
                path.Data = geometry;

                //var geometry = new PathGeometry();
                //var pathFigure = new PathFigure();
                //var arcSegment = new ArcSegment();
                //arcSegment.Point = new Point(localPath[1].X, localPath[0].Y);
                //arcSegment.Size = new Size(radius, radius);
                //pathFigure.StartPoint = localPath[0];
                //pathFigure.Segments.Add(arcSegment);
                //geometry.Figures.Add(pathFigure);
                //path.Data = geometry;
            }
            return path;
        }
    }
}
