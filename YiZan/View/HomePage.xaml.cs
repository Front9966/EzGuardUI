using CommunityToolkit.Maui.Alerts;

namespace YiZan.View;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this,false);

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(() => { All.GetUpData(this); });
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    //立即登录被单击
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        await Navigation.PushAsync(new LoginPage());
        but.IsEnabled = true;
    }
    //注册账号被单击
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        await Navigation.PushAsync(new RegIsTerPage());
        but.IsEnabled = true;
    }
    //QQ快捷登录被单击
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        Toast.Make("暂未更新").Show();
        but.IsEnabled = true;
    }
    //微信快捷登录被单击
    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        Toast.Make("暂未更新").Show();
        but.IsEnabled = true;
    }


}