using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;

namespace YiZan.View;

public partial class KuaiShouFunPage : ContentPage
{
    Json_ResJsonClass<Json_Account> resJsonData;
    public KuaiShouFunPage(string res)
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        resJsonData = JsonConvert.DeserializeObject<Json_ResJsonClass<Json_Account>>(res);
        headImage.Source = resJsonData.data.get_like.user_img;
        nicname.Text = resJsonData.data.get_like.user_name;
        qian.Text = resJsonData.data.get_like.user_id;
        text1.Text = resJsonData.data.get_like.user_follow.ToString();
        text2.Text = resJsonData.data.get_like.user_fans;
        text3.Text = resJsonData.data.get_like.user_works;
    }
    //�û���������
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var _temp = (ImageButton)sender;
        _temp.IsEnabled = false;
        await Navigation.PopAsync();
        _temp.IsEnabled = true;
    }

    public class Json_Account
    {
        public Json_AccountInfo get_like { get; set; }
        public Json_AccountInfo likes { get; set; }
    }

    public class Json_AccountInfo
    {
        public string? user_id { get; set; }//�˻�ID
        public string? user_fans { get; set; }//��˿
        public int? user_follow { get; set; }//��ע
        public string? user_works { get; set; }//��Ʒ
        public string? user_img { get; set; }//ͷ��
        public string? user_name { get; set; }//�ǳ�
    }
    //������Ʒ����
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        DianZan.IsEnabled = false;
        var a = new CommitPopup(0);
        this.ShowPopup(a);
        DianZan.IsEnabled = true;
    }
    //������Ʒ����
    private void TapGestureRecognizer_Tapped1(object sender, TappedEventArgs e)
    {
        PinLun.IsEnabled = false;
        var a = new CommitPopup(1);
        this.ShowPopup(a);
        PinLun.IsEnabled = true;
    }
    //�����û���ע
    private void TapGestureRecognizer_Tapped2(object sender, TappedEventArgs e)
    {
        GuanZhu.IsEnabled = false;
        var a = new CommitPopup(2);
        this.ShowPopup(a);
        GuanZhu.IsEnabled = true;
    }
    //�������۵���
    private void TapGestureRecognizer_Tapped3(object sender, TappedEventArgs e)
    {
        PinLunGood.IsEnabled = false;
        Toast.Make("������").Show();
        PinLunGood.IsEnabled = true;
    }
}