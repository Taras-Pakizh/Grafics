using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab6
{
    class RectangleDataContent
    {
        private List<Point> _rectangle;
        private List<Point> _reflacting;
        private List<Point> _scaleRectangle;
        private List<Point> _scaleReflacting;

        private bool _isReflacting;
        private double _scale = 1;

        public bool IsReflacting
        {
            get { return _isReflacting; }
            set
            {
                _isReflacting = value;
                if (!value)
                {
                    _reflacting = null;
                    _scaleReflacting = null;
                }
            }
        }
        public List<Point> Rectancle
        {
            get { return _scaleRectangle; }
        }
        public List<Point> Reflacting
        {
            get { return _scaleReflacting; }
        }
        public List<Point> RealRectancle
        {
            get { return _rectangle; }
        }

        public RectangleDataContent(List<Point> points)
        {
            _rectangle = points;
            _scaleRectangle = points;
            _isReflacting = false;
        }

        public void SetReflacting(CanvasInfo info)
        {
            _reflacting = Transformation.ReflectYX(_rectangle, info);
            if (!_isReflacting)
                _isReflacting = true;
            ScaleRectangle(_scale);
        }

        public void ScaleRectangle(double value)
        {
            _scale = value;
            Point RecCenter = new Point()
            {
                X = _rectangle.Average(x => x.X),
                Y = _rectangle.Average(x => x.Y)
            };

            _scaleRectangle = Transformation.Scale(_rectangle, RecCenter, value);

            if (_isReflacting)
            {
                Point ReflactingCenter = new Point()
                {
                    X = _reflacting.Average(x => x.X),
                    Y = _reflacting.Average(x => x.Y)
                };

                _scaleReflacting = Transformation.Scale(_reflacting, ReflactingCenter, value);
            }
        }

        public void Move(CanvasInfo info, double step)
        {
            _rectangle = Transformation.MoveYX(_rectangle, step);
            _reflacting = Transformation.ReflectYX(_rectangle, info);
            ScaleRectangle(_scale);
        }
    }
}
