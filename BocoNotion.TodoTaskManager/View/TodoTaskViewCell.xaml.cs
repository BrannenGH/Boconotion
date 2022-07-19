using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BocoNotion.TodoTaskManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BocoNotion.TodoTaskManager.View
{
	public partial class TodoTaskViewCell : ViewCell
	{
		public event EventHandler OnClick;

		public TodoTaskViewCell ()
		{
			this.InitializeComponent();
		}

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
			OnClick?.Invoke(sender, e);
        }
    }
}
