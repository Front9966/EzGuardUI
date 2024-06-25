using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;

namespace YiZan.View;

public partial class GoodPage : ContentPage
{
	public GoodPage()
	{
		InitializeComponent();
	}
	//���ֱ�����
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        KuaiShou.IsEnabled = false;
        await Navigation.PushAsync(new AccountControlPage());
        KuaiShou.IsEnabled = true;
    }
    //����������
    private void TapGestureRecognizer_Tapped1(object sender, TappedEventArgs e)
    {
        DouYin.IsEnabled = false;
        Toast.Make("δ����").Show();
        DouYin.IsEnabled = true;
    }
    //��������������
    private void TapGestureRecognizer_Tapped2(object sender, TappedEventArgs e)
    {
        Bili.IsEnabled = false;
        Toast.Make("δ����").Show();
        Bili.IsEnabled = true;
    }
    //С���鱻����
    private void TapGestureRecognizer_Tapped3(object sender, TappedEventArgs e)
    {
        HongShu.IsEnabled = false;
        Toast.Make("δ����").Show();
        HongShu.IsEnabled = true;
    }
}