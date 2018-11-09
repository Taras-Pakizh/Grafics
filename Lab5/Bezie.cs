﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5
{
    public static class Bezie
    {
        public static List<Point> Traditional(List<Point> points, double step)
        {
            List<Point> result = new List<Point>();
            for(double t = step; t < 1; t += step)
            {
                result.Add(_GetPointTraditional(t, points));
            }
            return result;
        }

        public static List<Point> Recurtion(List<Point> points, double step)
        {
            List<Point> result = new List<Point>();
            for(double t = step; t < 1; t += step)
            {
                result.Add(_GetPointRecurtion(t, points));
            }
            return result;
        }

        private static Point _GetPointRecurtion(double t, List<Point> points)
        {
            if(points.Count == 1)
            {
                return _GetPointTraditional(t, points);
            }
            Point first = _GetPointRecurtion(t, points.Take(points.Count - 1).ToList()).Multiple(1 - t);
            Point second = _GetPointRecurtion(t, points.Skip(1).ToList()).Multiple(t);
            return first.Add(second);
        }

        private static Point _GetPointTraditional(double t, List<Point> points)
        {
            Point result = new Point(0, 0);
            double temp;
            for(int i = 0; i < points.Count; ++i)
            {
                temp = _Bernshtein(points.Count - 1, t, i);
                result.X += temp * points[i].X;
                result.Y += temp * points[i].Y;
            }
            return result;
        }

        private static int _Factorial(int value)
        {
            if (value < 0)
                throw new IndexOutOfRangeException();
            if (value == 0)
                return 1;
            return value * _Factorial(value - 1);
        }

        private static double _Bernshtein(int n, double t, int i)
        {
            return (_Factorial(n) / (_Factorial(i) * _Factorial(n - i))) * Math.Pow(t, i) * Math.Pow(1 - t, n - i);
        }
    }
}