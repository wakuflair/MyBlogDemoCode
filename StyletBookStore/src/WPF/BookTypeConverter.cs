using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StyletBookStore.Models;

namespace StyletBookStore.WPF
{
    public class BookTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is BookType)) return DependencyProperty.UnsetValue;
            var bookType = (BookType) value;
            switch (bookType)
            {
                case BookType.Undefined:
                    return "未知";
                case BookType.Fantastic:
                    return "奇幻";
                case BookType.Biography:
                    return "传记";
                case BookType.Horror:
                    return "恐怖";
                case BookType.Mystery:
                    return "悬疑";
                case BookType.Programming:
                    return "编程";
                case BookType.ScienceFiction:
                    return "科幻";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}