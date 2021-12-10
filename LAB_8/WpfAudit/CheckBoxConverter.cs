using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfAudit
{
    public class CheckBoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isChecked = values.Contains(true);
            bool isUnchecked = values.Contains(false);
            if (isChecked && isUnchecked)
            {
                // some checked and uncheked
                return null;
            }
            else if (isChecked)
            {
                return true;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            int count = targetTypes.Length;
            object[] result = new object[count];
            if ((bool)value == true)
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = true;
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = false;
                }
            }
            return result;
        }
    }
}
