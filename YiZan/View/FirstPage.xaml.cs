
namespace YiZan.View;
public partial class FirstPage : ContentPage
{
	public FirstPageModel model;
    public FirstPage()
	{
		InitializeComponent();
        model = new FirstPageModel();
        this.BindingContext = model;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(() => { All.GetUpData(this); });
    }
}