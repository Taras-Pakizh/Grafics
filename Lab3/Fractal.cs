using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lab3
{
    abstract class Fractal
    {
        public Fractal nextStage;

        public PathGeometry geometry { get; set; }
        private CanvasInfo canvasInfo;
        public CanvasInfo info
        {
            get
            {
                return canvasInfo;
            }
            set
            {
                canvasInfo = value;
                Fractal fractal = this;
                while(fractal.nextStage != null)
                {
                    fractal = fractal.nextStage;
                    fractal.canvasInfo = value;
                }
            }
        }
        public int Stage { get; protected set; }
        public PathGeometry this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException();
                Fractal result = this;
                for(int i = 0; i < index; ++i)
                {
                    if (result.nextStage != null && result.nextStage.geometry != null)
                        result = result.nextStage;
                    else throw new IndexOutOfRangeException();
                }
                if (result.geometry != null)
                    return result.ScaleToCanvas();
                else throw new ArgumentNullException();
            }
        }
        public int Count
        {
            get
            {
                var top = TopFractal();
                int result = top.Stage;
                if (top.geometry == null)
                    result--;
                return result;
            }
        }

        public PathGeometry TopGeometry()
        {
            Fractal result = this;
            while (result.nextStage != null && result.nextStage.geometry != null)
            {
                result = result.nextStage;
            }
            return result.ScaleToCanvas();
        }

        public Fractal TopFractal()
        {
            Fractal result = this;
            while (result.nextStage != null && result.nextStage.geometry != null)
            {
                result = result.nextStage;
            }
            return result;
        }

        public abstract PathGeometry CreateNextFractal(Fractal prevFractal);

        public abstract PathGeometry ScaleToCanvas();

        public abstract void CreateNextStages(int amoung);
    }
}
