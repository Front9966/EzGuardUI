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
    //������¼������
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        await Navigation.PushAsync(new LoginPage());
        but.IsEnabled = true;
    }
    //ע���˺ű�����
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        await Navigation.PushAsync(new RegIsTerPage());
        but.IsEnabled = true;
    }
    //QQ��ݵ�¼������
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        Toast.Make("��δ����").Show();
        but.IsEnabled = true;
    }
    //΢�ſ�ݵ�¼������
    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        var but = (Button)sender;
        but.IsEnabled = false;
        Toast.Make("��δ����").Show();
        but.IsEnabled = true;
    }


}