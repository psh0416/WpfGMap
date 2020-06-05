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
    class GSPolygon : GMapBaseShape
    {
        public GSPolygon(IEnumerable<PointLatLng> points) : base(points.FirstOrDefault())
        {
            Points = new List<PointLatLng>(points);
            var path = MyPath;
        }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = MyPath;
            if (localPath.Count > 1)
            {
                var geometry = new StreamGeometry();
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(localPath[0], true, true);

                    foreach (var item in localPath)
                    {
                        ctx.LineTo(item, true, true);
                    }
                }
                path.Data = geometry;
            }
            return path;
        }
    }
}
