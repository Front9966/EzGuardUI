using CommunityToolkit.Maui.Alerts;
using System.Text.Json;

namespace YiZan.View;

public partial class RegIsTerPage : ContentPage
{
	public RegIsTerPage()
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
    }
    //ע�ᱻ����
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        if (QQNumBer.Text == "" && QQNumBer.Text.Length > 10)
        {
            Snackbar.Make("��������ȷ��QQ����").Show();
        }
        else if (Password.Text == "" && Password.Text.Length >= 6)
        {
            Snackbar.Make("�˻����벻�õ���6λ��").Show();
        }
        else 
        {
            //ע��
            HttpClient httpClient = new HttpClient();
            var postContent = new Dictionary<string, string>();
            postContent.Add("account", QQNumBer.Text);
            postContent.Add("password", Password.Text);
            var res = await httpClient.PostAsync(All.hostname + "/api/auth/register", new FormUrlEncodedContent(postContent));
            var res_string = await res.Content.ReadAsStringAsync();
            var resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<Json_Register>>(res_string);
            if (resJsonData.status != 200)
            {
                Snackbar.Make(resJsonData.message).Show();
            }
            else
            {
                Snackbar.Make("ע��ɹ�").Show();
            }
        }
        _temp.IsEnabled = true;
    }
    //QQ������
    private async void QQNumBer_TextChanged(object sender, TextChangedEventArgs e)
    {
        image1.Source = "https://q1.qlogo.cn/g?b=qq&nk=" + e.NewTextValue + "&s=100";
    }
    //JSON��������
    public class Json_Register 
    {
        public Json_RegisterInfo user { get; set; }
        public string? token { get; set; }
        public int exp { get; set; }
        public long expires_time { get; set; }
    }
    public class Json_RegisterInfo 
    {
        public string? account { get; set; }
        public string? nickname { get; set; }
        public string? avatar { get; set; }
        public int now_money { get; set; }
        public int pay_price { get; set; }
        public string? user_type { get; set; }
        public int uid { get; set; }
        public bool isNew { get; set; }
    }
}