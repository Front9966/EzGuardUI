using CommunityToolkit.Maui.Alerts;
using System.Text.Json;

namespace YiZan.View;

public partial class UserPage : ContentPage
{
    private Json_ResJsonClass<Json_cifg> resJsonData;
    public UserPage()
	{
		InitializeComponent();
        Avatar.Source = "https://q1.qlogo.cn/g?b=qq&nk=" + All.qqNumber + "&s=100";
        Nicname.Text = All.nicname;
        QQNumber.Text = All.qqNumber;
        Task.Run(GetCs);
	}
    private async void GetCs()
    {
        string url_path = All.hostname + "/api/index/index";
        HttpClient client = new HttpClient();
        var res = await client.GetAsync(url_path);
        if (res.IsSuccessStatusCode)
        {
            string res_string = await res.Content.ReadAsStringAsync();
            resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<Json_cifg>>(res_string);
        }

    }
    //�����˺Ź���
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        fun.IsEnabled = false;
        await Navigation.PushAsync(new AccountControlPage());
        fun.IsEnabled = true;
    }
    //�������޼�¼
    private async void TapGestureRecognizer_Tapped1(object sender, TappedEventArgs e)
    {
        fun1.IsEnabled = false;
        Toast.Make("δ����").Show();
        fun1.IsEnabled = true;
    }
    //������������
    private async void TapGestureRecognizer_Tapped2(object sender, TappedEventArgs e)
    {
        fun2.IsEnabled = false;
        Toast.Make("δ����").Show();
        fun2.IsEnabled = true;
    }
    //����APP����
    private async void TapGestureRecognizer_Tapped3(object sender, TappedEventArgs e)
    {
        fun3.IsEnabled = false;
        await Navigation.PushAsync(new SettingPage());
        fun3.IsEnabled = true;
    }
    //����ʹ�ð���
    private async void TapGestureRecognizer_Tapped4(object sender, TappedEventArgs e)
    {
        fun4.IsEnabled = false;
        await Navigation.PushAsync(new WebPage(resJsonData.data.help));
        fun4.IsEnabled = true;
    }
    //�����ͬ��
    private async void TapGestureRecognizer_Tapped5(object sender, TappedEventArgs e)
    {
        fun5.IsEnabled = false;
        await Navigation.PushAsync(new WebPage(resJsonData.data.apply));
        fun5.IsEnabled = true;
    }
    //������ϵ�ͷ�
    private async void TapGestureRecognizer_Tapped6(object sender, TappedEventArgs e)
    {
        fun6.IsEnabled = false;
        await Navigation.PushAsync(new CsPage());
        fun6.IsEnabled = true;
    }
    //������ԱȺ��
    private async void TapGestureRecognizer_Tapped7(object sender, TappedEventArgs e)
    {
        fun7.IsEnabled = false;
        await Launcher.OpenAsync(resJsonData.data.chat);
        fun7.IsEnabled = true;
    }
    //������ͨVIP
    private void Button_Clicked(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        Toast.Make("δ����").Show();
        _temp.IsEnabled = true;
    }
    //Json��������
    public class Json_cifg
    {
        public Json_user_agreement user_agreement { get; set; }
        public Json_privacy_agreement privacy_agreement { get; set; }
        public string? contact_qq { get; set; }
        public string? contact_wechat { get; set; }
        public string? help { get; set; }
        public string? chat { get; set; }
        public string? apply { get; set; }
    }
    public class Json_user_agreement
    {
        public string? title { get; set; }
        public string? sys_user_agree { get; set; }
    }
    public class Json_privacy_agreement
    {
        public string? title { get; set; }
        public string? sys_userr_privacy { get; set; }
    }

}