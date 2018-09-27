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
        public CanvasInfo info { get; set; }
        public int Stage { get; protected set; }
        public PathGeometry this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException();
                Fractal result = this;
                for(int i = 0; i < index; ++i)
                {
                    if (result.nextStage != null)
                        result = result.nextStage;
                    else throw new IndexOutOfRangeException();
                }
                return result.ScaleToCanvas();
            }
        }

        public void CreateNextStages(int amoung)
        {
            if (amoung < 1) throw new IndexOutOfRangeException();
            var currentFractal = TopFractal();
            for(int i = 0; i < amoung; ++i)
            {
                currentFractal.nextStage = CreateNextFractal(TopFractal());
                currentFractal = currentFractal.nextStage;
            }
        }

        public PathGeometry TopGeometry()
        {
            Fractal result = this;
            while (result.nextStage != null)
            {
                result = result.nextStage;
            }
            return result.ScaleToCanvas();
        }

        public Fractal TopFractal()
        {
            Fractal result = this;
            while (result.nextStage != null)
            {
                result = result.nextStage;
            }
            return result;
        }

        public abstract Fractal CreateNextFractal(Fractal prevFractal);

        public abstract PathGeometry ScaleToCanvas();
    }
}
