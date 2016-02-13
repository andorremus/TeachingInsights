using GalaSoft.MvvmLight;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachingInsights2.Helpers;

namespace TeachingInsights2.ViewModel
{
    public class ConfusionChartViewModel : ViewModelBase
    {
        public PointsCollection Collection { get; set; }

        public ConfusionChartViewModel()
        {
            Collection = new PointsCollection();
        }

    }
}
