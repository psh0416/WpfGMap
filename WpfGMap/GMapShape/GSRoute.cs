using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    class GSRoute : GMapBaseShape
    {
        public GSRoute(IEnumerable<PointLatLng> points) : base(points.FirstOrDefault())
        {
            Points = new List<PointLatLng>(points);

            Stroke = Brushes.Red;
            Fill = Brushes.Transparent;
            StrokeThickness = 2;
        }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = MyPath;
            if (localPath.Count > 1)
            {
                StreamGeometry geometry = new StreamGeometry();

                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(localPath[0], true, false);

                    for (int i = 1; i < localPath.Count; i++)
                    {
                        ctx.LineTo(localPath[i], true, false);
                    }
                }
                path.Data = geometry;
            }
            return path;
        }
    }
}
