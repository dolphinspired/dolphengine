using System.Collections.Generic;
using System.Linq;

namespace DolphEngine
{
    public struct Polygon2d
    {
        #region Constructors

        public Polygon2d(params Position2d[] points)
        {
            this.Points = points?.ToList() ?? new List<Position2d>(0);
        }

        public Polygon2d(params Vector2d[] strokes)
            : this(Position2d.Zero, strokes)
        {
        }

        public Polygon2d(Position2d start, params Vector2d[] strokes)
        {
            this.Points = new List<Position2d> { start };

            foreach (var stroke in strokes)
            {
                this.AddStroke(stroke);
            }
        }

        #endregion

        #region Properties

        public List<Position2d> Points;

        #endregion

        #region Methods

        public Polygon2d AddPoint(Position2d point)
        {
            if (this.Points == null)
            {
                this.Points = new List<Position2d>(1);
            }

            this.Points.Add(point);
            return this;
        }

        public Polygon2d AddStroke(Vector2d vector)
        {
            if (this.Points == null)
            {
                this.Points = new List<Position2d>(2) { Position2d.Zero };
            }
            else if (this.Points.Count == 0)
            {
                this.Points.Add(Position2d.Zero);
            }

            this.Points.Add(Points[Points.Count - 1] + vector);
            return this;
        }

        public Polygon2d Close()
        {
            if (this.Points == null || this.Points.Count < 2)
            {
                return this;
            }

            this.Points.Add(Points[0]);
            return this;
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"[ {string.Join(',', Points)} ]";
        }

        #endregion
    }
}
