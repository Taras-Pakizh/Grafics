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
        private double _step = 1;

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
            _isReflacting = true;
            if (ScaleRectangle(_scale, info))
                _isReflacting = true;
            else _isReflacting = false;
        }

        public bool ScaleRectangle(double value, CanvasInfo info)
        {
            _scale = value;
            Point RecCenter = new Point()
            {
                X = _rectangle.Average(x => x.X),
                Y = _rectangle.Average(x => x.Y)
            };

            var saveRec = _scaleRectangle;
            var saveRef = saveRec;
            _scaleRectangle = Transformation.Scale(_rectangle, RecCenter, value);

            if (_isReflacting)
            {
                Point ReflactingCenter = new Point()
                {
                    X = _reflacting.Average(x => x.X),
                    Y = _reflacting.Average(x => x.Y)
                };

                saveRef = _scaleReflacting;
                _scaleReflacting = Transformation.Scale(_reflacting, ReflactingCenter, value);
            }

            if (!_IsInCanvas(info))
            {
                _scaleRectangle = saveRec;
                if (IsReflacting)
                    _scaleReflacting = saveRef;
                return false;
            }
            return true;
        }

        public void Move(CanvasInfo info)
        {
            _ChangeDirection(info);
            _rectangle = Transformation.MoveYX(_rectangle, _step);
            _reflacting = Transformation.ReflectYX(_rectangle, info);
            if (!ScaleRectangle(_scale, info))
                _step *= -1;
        }

        private void _ChangeDirection(CanvasInfo info)
        {
            if (_scaleRectangle.Any(x => x.X <= 0 || x.Y >= info.Heigth))
            {
                _step = 1;
                return;
            }
            if (_scaleRectangle.Any(x => x.X >= info.Width || x.Y <= 0))
            {
                _step = -1;
                return;
            }
            if (IsReflacting)
            {
                if (_scaleReflacting.Any(x => x.X <= 0 || x.Y >= info.Heigth))
                {
                    _step = 1;
                    return;
                }
                if (_scaleReflacting.Any(x => x.X >= info.Width || x.Y <= 0))
                {
                    _step = -1;
                    return;
                }
            }
        }

        public bool _IsInCanvas(CanvasInfo info)
        {
            var allPoints = new List<Point>();
            allPoints.AddRange(_scaleRectangle);
            if (IsReflacting)
                allPoints.AddRange(_scaleReflacting);
            if (allPoints.Any(point => point.X <= 0 || point.X >= info.Width || point.Y <= 0 || point.Y >= info.Heigth))
                return false;
            return true;
        }
    
        public void SetNew(List<Point> point, CanvasInfo info)
        {
            if (point.Count != 4)
                throw new Exception();

            _rectangle = point;
            if (IsReflacting)
            {
                _isReflacting = false;
                SetReflacting(info);
            }
            ScaleRectangle(_scale, info);
        }
    }
}
