using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace TeachingInsights2
{
    public class DataViewModel : ViewModelBase
    {
        public static ObservableCollection<Point> Collection { get; set; }
        public static ObservableCollection<Point> BrowFurrowCollection { get; set; }

        public DataViewModel()
        {
            Collection = new ObservableCollection<Point>();
            BrowFurrowCollection = new ObservableCollection<Point>();
            //GenerateDatas();
        }
        private void GenerateDatas()
        {
            DataViewModel.Collection.Add(new Point{ X = 0, Y =  1});
            DataViewModel.Collection.Add(new Point { X = 1, Y = 2 });
            DataViewModel.Collection.Add(new Point { X = 3, Y = 4 });
            DataViewModel.Collection.Add(new Point { X = 4, Y = 5 });
           
        }

        public static void Add(Point point)
        {
            if(Collection.Count > 99)
            {
                Collection.Clear();
            }
            Collection.Add(point);
        }
        public static void AddFurrow(Point point)
        {
            if (BrowFurrowCollection.Count > 99)
            {
                BrowFurrowCollection.Clear();
            }
            BrowFurrowCollection.Add(point);
        }

        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}