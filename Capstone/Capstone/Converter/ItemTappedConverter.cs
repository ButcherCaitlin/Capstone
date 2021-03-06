using System;
using System.Globalization;
using Xamarin.Forms;

using Capstone;

namespace Capstone.Converter
{
    public class ItemTappedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Xamarin.Forms.ItemTappedEventArgs eventArgs)
                return eventArgs.Item;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
