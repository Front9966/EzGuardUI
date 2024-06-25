using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;

namespace YiZan.View;

public partial class GoodPage : ContentPage
{
	public GoodPage()
	{
		InitializeComponent();
	}
	//快手被单击
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        KuaiShou.IsEnabled = false;
        await Navigation.PushAsync(new AccountControlPage());
        KuaiShou.IsEnabled = true;
    }
    //抖音被单击
    private void TapGestureRecognizer_Tapped1(object sender, TappedEventArgs e)
    {
        DouYin.IsEnabled = false;
        Toast.Make("未更新").Show();
        DouYin.IsEnabled = true;
    }
    //哔哩哔哩被单击
    private void TapGestureRecognizer_Tapped2(object sender, TappedEventArgs e)
    {
        Bili.IsEnabled = false;
        Toast.Make("未更新").Show();
        Bili.IsEnabled = true;
    }
    //小红书被单击
    private void TapGestureRecognizer_Tapped3(object sender, TappedEventArgs e)
    {
        HongShu.IsEnabled = false;
        Toast.Make("未更新").Show();
        HongShu.IsEnabled = true;
    }
}