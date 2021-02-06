using System;
using System.Globalization;
using System.Windows.Data;

namespace Ricciolo.Controls
{
	public class LevelToIndentConverter : IValueConverter
	{
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			double result = 0.0;
			if (parameter != null)
			{
				double.TryParse(parameter.ToString(), out result);
			}
			return (double)(int)o * result;
		}

		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
