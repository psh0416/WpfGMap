﻿using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGMap.GMapShape;

namespace WpfGMap.GMapEditor
{
    class ShapeEditor
    {
        public EventHandler MovingCompleted;
        public EventHandler PointMovingCompleted;
        GMapControl gMapControl;
        Dictionary<Thumb, GMapMarker> dicMarker = new Dictionary<Thumb, GMapMarker>();
        IShapable gMapShape;
        GSRectangle border;

        public ShapeEditor()
        {
            border = new GSRectangle(new PointLatLng(), new PointLatLng());
            (border.Shape as Path).Stroke = Brushes.Black;
            (border.Shape as Path).StrokeThickness = 1;
            (border.Shape as Path).Fill = new SolidColorBrush(Color.FromArgb(50, 200, 200, 200));
            (border.Shape as Path).Cursor = Cursors.SizeAll;
            moveThumb = new Thumb();
            moveThumb.Style = Application.Current.FindResource("MovingThumb") as Style;
            moveThumb.Width = 18;
            moveThumb.Height = 18;
            moveMarker.Offset = new Point(-moveThumb.Width - 4, -moveThumb.Height - 4);
            moveThumb.DragDelta += Move_DragDelta;
            moveThumb.DragCompleted += Thumb_DragCompleted;
            moveMarker.Shape = moveThumb;
        }
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            int index = (int)thumb.Tag;
            GMapMarker marker = dicMarker[thumb];
            GPoint StartPoint = gMapControl.FromLatLngToLocal(marker.Position);
            double posX = StartPoint.X + e.HorizontalChange;
            double posY = StartPoint.Y + e.VerticalChange;

            var pos = gMapControl.FromLocalToLatLng((int)posX, (int)posY);
            marker.Position = pos;
            gMapShape.Points[index] = pos;
            gMapControl.RegenerateShape(gMapShape);

            SetBorder(gMapShape.Points);
            gMapControl.RegenerateShape(border);
        }

        private void Move_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //var list = dicMarker.GetEnumerator();
            foreach (var item in dicMarker)
            {
                int i = (int)item.Key.Tag;
                var StartPoint = gMapControl.FromLatLngToLocal(gMapShape.Points[i]);
                double posX = StartPoint.X + e.HorizontalChange;
                double posY = StartPoint.Y + e.VerticalChange;
                var pos = gMapControl.FromLocalToLatLng((int)posX, (int)posY);
                gMapShape.Points[i] = pos;

                item.Value.Position = pos;
            }
            gMapControl.RegenerateShape(gMapShape);

            SetBorder(gMapShape.Points);
            gMapControl.RegenerateShape(border);
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == moveThumb)
                MovingCompleted?.Invoke(this, new EventArgs());
            else if (thumb != null)
                PointMovingCompleted?.Invoke(this, new EventArgs());
        }
        
        Thumb moveThumb;
        GMapMarker moveMarker = new GMapMarker(new PointLatLng());
        internal void SetMarker(IShapable shape, GMapControl mapControl)
        {
            gMapShape = shape;
            gMapControl = mapControl;
            
            SetBorder(shape.Points);
            gMapControl.Markers.Add(border);

            var thumbs = new Thumb[shape.Points.Count];
            for (int i = 0; i < thumbs.Length; i++)
            {
                thumbs[i] = new Thumb();
                thumbs[i].Width = 12;
                thumbs[i].Height = 12;
                GMapMarker marker = new GMapMarker(shape.Points[i]);
                marker.Shape = thumbs[i];
                marker.Offset = new Point(-thumbs[i].Width / 2, -thumbs[i].Height / 2);
                thumbs[i].DragDelta += Thumb_DragDelta;
                thumbs[i].DragCompleted += Thumb_DragCompleted;
                thumbs[i].Tag = i;
                thumbs[i].Style = Application.Current.FindResource("PointThumb") as Style;
                gMapControl.Markers.Add(marker);
                dicMarker.Add(thumbs[i], marker);
            }
            
            gMapControl.Markers.Add(moveMarker);
            //GMapPolygon polygon = new GMapPolygon(gpollist, "Circle");
            //marker.Shape = recShape;
            //marker.Tag = "PolyDot";
            //marker.Offset = new System.Windows.Point(-recShape.Width / 2, -recShape.Height / 2);
        }

        public void SetBorder(List<PointLatLng> points)
        {
            double minLat = points[0].Lat,
                minLng = points[0].Lng,
                maxLat = points[0].Lat,
                maxLng = points[0].Lng;
            for (int i = 1; i < points.Count; i++)
            {
                if (minLat > points[i].Lat)
                    minLat = points[i].Lat;
                if (maxLat < points[i].Lat)
                    maxLat = points[i].Lat;
                if (minLng > points[i].Lng)
                    minLng = points[i].Lng;
                if (maxLng < points[i].Lng)
                    maxLng = points[i].Lng;
            }
            border.Points[0] = new PointLatLng(minLat, minLng);
            border.Points[1] = new PointLatLng(maxLat, maxLng);
            moveMarker.Position = new PointLatLng(maxLat, minLng);
        }

        public void ClearMarker()
        {
            foreach (var item in dicMarker)
            {
                //gMapControl.Markers.Remove(item.Value);
                item.Value.Clear();
            }
            dicMarker.Clear();
            if (gMapControl != null)
            {
                gMapControl.Markers.Remove(border);
                gMapControl.Markers.Remove(moveMarker);
            }
                
        }
    }
}
