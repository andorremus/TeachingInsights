using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingInsights2.Helpers
{
    public class PointsCollection : ObservableCollection<Point>
    {
        private int counter = 0;

        public PointsCollection()
        {

        }

        public void AddPoint(int expressionValue)
        {
            if (this.Count > 99)
            {
                this.Clear();
                counter = 0;
            }
            this.Add(new Point { X = counter, Y = expressionValue });
            counter++;
        }
    }


    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

}
