using Presenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessagePage : Page, IMessagePage
    {
        public MessagePage()
        {
            this.InitializeComponent();
            this.Loaded += MessagePage_Loaded;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            messageTxtBlk.Text = e.Parameter as string;
        }

        private void MessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            MainPresenter.MessagePage = this;
        }

        public event EventHandler ButtonPressed;

        public void SetMessage(string message)
        {
            messageTxtBlk.Text = message;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, EventArgs.Empty);
        }
    }
}
