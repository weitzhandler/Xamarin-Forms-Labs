using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
  /// <summary>
  /// A view that renders its content based on a data template. Typical usage is to either set an explicit 
  /// <see cref="BindableObject.BindingContext"/> on this element or use an inhereted one, then set a display.
  /// </summary>
  [ContentProperty(nameof(ContentTemplate))]
  public class DataContentView : ContentView
  {
    /// <summary>
    /// The content template property
    /// </summary>
    public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(DataContentView), null);
    /// <summary>
    /// A <see cref="DataTemplate"/> used to render the view. This property is bindable.
    /// </summary>
    public DataTemplate ContentTemplate
    {
      get => (DataTemplate)GetValue(ContentTemplateProperty);
      set => SetValue(ContentTemplateProperty, value);
    }

    protected override void OnBindingContextChanged()
    {
      var template = ContentTemplate;

      if (template is DataTemplateSelector dts)
      {
        var item = GetValue(BindingContextProperty);
        template = dts.SelectTemplate(item, this);
      }

      Content = (View)template?.CreateContent();
    }


  }
}
