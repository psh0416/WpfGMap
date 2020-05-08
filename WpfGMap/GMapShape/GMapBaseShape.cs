using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace WpfGMap.GMapShape
{
    abstract class GMapBaseShape : GMapMarker, IShapable
    {
        public GMapBaseShape(PointLatLng point) : base(point)
        {
            Opacity = 1;
        }
        public abstract List<PointLatLng> Points { get; set; }

        public abstract Path CreatePath(List<Point> localPath, bool addBlurEffect);

        private Brush fill;
        public Brush Fill
        {
            get => fill;
            set
            {
                fill = value;
                if (Shape != null)
                    (Shape as Path).Fill = value;
            }
        }

        private Brush stroke;
        public Brush Stroke
        {
            get => stroke;
            set
            {
                stroke = value;
                if (Shape != null)
                    (Shape as Path).Stroke = value;
            }
        }

        private double strokeThickness;
        public double StrokeThickness
        {
            get => strokeThickness;
            set
            {
                strokeThickness = value;
                if (Shape != null)
                    (Shape as Path).StrokeThickness = value;
            }
        }

        private double opactiy;
        public double Opacity
        {
            get => opactiy;
            set
            {
                opactiy = value;
                if (Shape != null)
                    Shape.Opacity = value;
            }
        }

        private Effect effect;
        public Effect Effect
        {
            get => effect;
            set
            {
                effect = value;
                if (Shape != null)
                    Shape.Effect = value;
            }
        }

        protected Path GetShape()
        {
            if (Shape == null)
            {
                Path path = new Path();
                path.Fill = Fill;
                path.Stroke = Stroke;
                path.StrokeThickness = StrokeThickness;
                path.Effect = Effect;
                path.Opacity = Opacity;
                return path;
            }
            return (Shape as Path);
        }
    }
}
