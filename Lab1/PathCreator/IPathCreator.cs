﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Lab1
{
    interface IPathCreator
    {
        Path GetPath(Brush brush);
    }
}
