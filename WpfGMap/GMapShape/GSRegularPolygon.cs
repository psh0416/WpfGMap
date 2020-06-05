using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    class GSRegularPolygon : GMapBaseShape
    {
        public GSRegularPolygon(PointLatLng pos, PointLatLng pos2, int pointCount) : base(pos)
        {
            Points = new List<PointLatLng>();
            Points.Add(pos);
            Points.Add(pos2);
            var path = MyPath;
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            path.Data = myEllipseGeometry;

            PointRatioList = GetPointList(pointCount);
        }

        IEnumerable<Point> PointRatioList;

        public IEnumerable<Point> GetPointList(int count)
        {
            Point[] pointlist = new Point[count];
            var rad = Math.PI * 2.0 / count;
            double x = 0, y = -1;
            
            pointlist[0] = new Point(x, y);
            double xMax = x, xMin = x, yMin = y, yMax = y;
            for (int i = 1; i < count; i++)
            {
                var tempx = x * Math.Cos(rad * i) - y * Math.Sin(rad * i);
                var tempy = x * Math.Sin(rad * i) + y * Math.Cos(rad * i);
                xMax = Math.Max(tempx, xMax);
                xMin = Math.Min(tempx, xMin);
                yMax = Math.Max(tempy, yMax);
                yMin = Math.Min(tempy, yMin);
                pointlist[i] = new Point(tempx, tempy);
            }
            
            return pointlist.Select(item => new Point(
                (item.X - xMin) / (xMax - xMin), (item.Y - yMin) / (yMax - yMin)));
        }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = MyPath;

            var points = PointRatioList.Select(item => new Point(
                item.X * (localPath[1].X - localPath[0].X) + localPath[0].X, 
                item.Y * (localPath[1].Y - localPath[0].Y) + localPath[0].Y)).ToArray();

            if (localPath.Count > 1)
            {
                StreamGeometry geometry = new StreamGeometry();

                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(points[0], true, true);

                    for (int i = 1; i < points.Length; i++)
                    {
                        ctx.LineTo(points[i], true, true);
                    }
                }
                path.Data = geometry;
            }
            return path;
        }
    }
}
