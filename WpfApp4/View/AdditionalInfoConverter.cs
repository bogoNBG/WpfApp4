using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfApp4.Model;

namespace WpfApp4.View
{
    class AdditionalInfoConverter : IMultiValueConverter
    {         

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int optionId && values[1] is ObservableCollection<Option> options)
            {
                var option = options.FirstOrDefault(o => o.Id == optionId);
                if (option != null)
                {
                    return option.Name;
                }
            }
            return "Unknown Option";

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
