using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FluentTranslate.UI.WPF
{
    public class FluentTranslateExtension : MarkupExtension
    {
        public string Content { get; }
        public object[] Arguments { get; }

        public FluentTranslateExtension(string content)
        {
            Content = content;
            
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var mb = new MultiBinding();

            var engine = new Binding
            {
                Source = new DynamicResourceExtension(FluentTranslateKeys.Engine),
                
            };

            mb.Bindings.Add(engine);

            foreach (var arg in Arguments) 
            {
                if (arg is Binding binding)
                {
                    mb.Bindings.Add(binding);
                }
            }

            return mb;
        }
    }

    public class FluentTranslateObject : DependencyObject
    {
        public string Content { get; }
    }

    public class FluentTranslateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
