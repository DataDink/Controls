using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace System.Windows
{
    public static class PointExtensions
    {
        /// <summary>
        /// Plots a new point from this point at the given angle and distance
        /// </summary>
        public static Point Plot(this Point from, double angle, double distance)
        {
            var radians = Math.PI / 180 * angle;
            var xplot = Math.Cos(radians) * distance;
            var yplot = Math.Sin(radians) * distance;
            return new Point(from.X + (float)xplot, from.Y + (float)yplot);
        }

        /// <summary>
        /// Gets the distance to this point from 0,0
        /// </summary>
        public static double GetDistance(this Point to)
        {
            return Math.Sqrt(Math.Pow(to.X, 2) + Math.Pow(to.Y, 2));
        }

        /// <summary>
        /// Gets the distance from this point to the specified point
        /// </summary>
        public static double GetDistance(this Point from, Point to)
        {
            return Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));
        }

        /// <summary>
        /// Gets the angle to this point from 0,0
        /// </summary>
        public static double GetAngle(this Point to)
        {
            return Math.Atan(to.Y / to.X) * 180 / Math.PI;
        }

        /// <summary>
        /// Gets the angle from this point to the specified point
        /// </summary>
        public static double GetAngle(this Point from, Point to)
        {
            return Math.Atan((to.Y - from.Y) / (to.X - from.X)) * 180 / Math.PI;
        }
    }
}
