using System;
using AdvsoysFormsIgen.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(MyDatePickerRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(MyTimePickerRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]

namespace AdvsoysFormsIgen.iOS
{
    public class MyDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.TextColor = TintColor;
            }
        }
    }

    public class MyTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.TextColor = TintColor;

                var timePicker = (UIDatePicker) Control.InputView;
                timePicker.Locale = new NSLocale("da_DK");
                timePicker.ValueChanged += TimePickerOnValueChanged;
            }
        }

        private void TimePickerOnValueChanged(object sender, EventArgs e)
        {
            var timePicker = (UIDatePicker) sender;
            Control.Text = timePicker.Date.ToDateTime().ToString("HH:mm");
        }
    }

    public class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.AdjustsFontSizeToFitWidth = true;
            }
        }
    }
}