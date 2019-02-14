﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MedianFilter
{
    public class ValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values)
        {
            if (values.Length < 1) return Binding.DoNothing;
            return values.Clone();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1) return Binding.DoNothing;
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
