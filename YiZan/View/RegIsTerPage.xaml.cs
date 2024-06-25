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
    //注册被单击
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        if (QQNumBer.Text == "" && QQNumBer.Text.Length > 10)
        {
            Snackbar.Make("请输入正确的QQ号码").Show();
        }
        else if (Password.Text == "" && Password.Text.Length >= 6)
        {
            Snackbar.Make("账户密码不得低于6位数").Show();
        }
        else 
        {
            //注册
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
                Snackbar.Make("注册成功").Show();
            }
        }
        _temp.IsEnabled = true;
    }
    //QQ号码变更
    private async void QQNumBer_TextChanged(object sender, TextChangedEventArgs e)
    {
        image1.Source = "https://q1.qlogo.cn/g?b=qq&nk=" + e.NewTextValue + "&s=100";
    }
    //JSON数据类型
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