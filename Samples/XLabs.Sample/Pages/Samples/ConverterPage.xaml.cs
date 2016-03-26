using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Data;

namespace XLabs.Sample.Pages.Samples
{
    public partial class ConverterPage
    {
        public ConverterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.BindingContext = new MyViewModel();
        }

        public class MyViewModel : ObservableObject
        {
            private bool isTrue;

            public bool IsTrue
            {
                get { return this.isTrue; }
                set
                {
                    this.SetProperty(ref this.isTrue, value);
                }
            }
        }
    }
}
