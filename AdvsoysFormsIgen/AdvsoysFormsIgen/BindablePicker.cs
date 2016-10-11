using System;
using System.Collections;
using Xamarin.Forms;

namespace AdvsoysFormsIgen
{
    /// <summary>
    /// https://forums.xamarin.com/discussion/30801/xamarin-forms-bindable-picker
    /// </summary>
    public class BindablePicker : Picker
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker, IList>(p => p.ItemsSource, default(IList),
                propertyChanged: OnItemsSourcePropertyChanged);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay,
                propertyChanged: OnSelectedItemChanged);


        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public IList ItemsSource
        {
            get { return (IList) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, IList value, IList newValue)
        {
            var picker = bindable as BindablePicker;

            if (picker == null)
            {
                return;
            }

            picker.Items.Clear();

            if (newValue == null)
            {
                return;
            }

            foreach (var item in newValue)
            {
                picker.Items.Add(item.ToString());
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = ItemsSource[SelectedIndex];
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = bindable as BindablePicker;

            if (picker == null)
            {
                return;
            }

            picker.SelectedIndex = picker.ItemsSource.IndexOf(newValue);
        }
    }
}