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
    class GSLine : GMapBaseShape
    {
        public GSLine(PointLatLng startPoint, PointLatLng endPoint) : base(startPoint)
        {
            Points = new List<PointLatLng>();
            Points.Add(startPoint);
            Points.Add(endPoint);

            Path path = new Path();
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 2;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            LineGeometry myLineGeometry = new LineGeometry();
            path.Data = myLineGeometry;
            path.IsHitTestVisible = false;
            Shape = path;
        }

        public override Path CreatePath(List<Point> localPath, bool addBlurEffect)
        {
            Path path = MyPath;
            LineGeometry myLineGeometry = path.Data as LineGeometry ?? new LineGeometry();
            myLineGeometry.StartPoint = localPath[0];
            myLineGeometry.EndPoint = localPath[1];
            return path;
        }
    }
}
