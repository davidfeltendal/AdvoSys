using Xamarin.Forms;

namespace AdvsoysFormsIgen
{
    public class TapCell : ViewCell
    {
        /// <summary>
        /// Identifies the Label bindable property.
        /// </summary>
        public static readonly BindableProperty LabelProperty =
            BindableProperty.Create<TapCell, string>(p => p.Label, string.Empty);

        /// <summary>
        /// Identifies the Text bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<TapCell, string>(p => p.Text, null);

        public TapCell()
        {
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                TranslationX = 16
            };

            var label = new Label
            {
                Style = Device.Styles.BodyStyle,
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false)
            };

            var text = new Label
            {
                Style = Device.Styles.BodyStyle,
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false)
            };

            Grid.SetColumn(label, 0);
            Grid.SetColumn(text, 1);

            label.SetBinding(Xamarin.Forms.Label.TextProperty, Label);
            text.SetBinding(Xamarin.Forms.Label.TextProperty, Text);

            View = grid;
        }

        /// <summary>
        /// Gets or sets the label to be displayed. This is a bindable property.
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text to be displayed. This is a bindable property.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}