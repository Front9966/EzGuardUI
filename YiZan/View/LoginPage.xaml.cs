using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using System.Text.Json;

namespace YiZan.View;
public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
    }
    //登录
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        var a = new LoadPopup();
        this.ShowPopup(a);
        await Task.Run(async () =>
        {
            try
            {
                HttpClient client = new HttpClient();
                var postInfo = new Dictionary<string, string>();
                postInfo.Add("account", User.Text);
                postInfo.Add("password", Pawwword.Text);
                HttpResponseMessage res = await client.PostAsync(All.hostname + "/api/auth/login", new FormUrlEncodedContent(postInfo));
                string res_string = await res.Content.ReadAsStringAsync();
                var resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<Json_Login>>(res_string);
                if (resJsonData.status == 200)
                {
                    All.Token = resJsonData.data.token;
                    res = await client.GetAsync("https://api.usuuu.com/qq/" + User.Text);
                    res_string = await res.Content.ReadAsStringAsync();
                    var resQQInfoJson = JsonSerializer.Deserialize<Json_ResJsonClass<Json_qqInfo>>(res_string);
                    All.qqNumber = User.Text;
                    All.nicname = resQQInfoJson.data.name;

                    var dataPath = FileSystem.Current.AppDataDirectory + "/data.ini";
                    StreamWriter streamWriter = new StreamWriter(dataPath);
                    streamWriter.WriteLine(User.Text);
                    streamWriter.WriteLine(Pawwword.Text);
                    streamWriter.Close();

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Navigation.PopAsync();
                        Navigation.PopAsync();
                        if (All.LoginCode == 2)
                        {
                            Navigation.PopAsync();
                        }
                    });
                }
                else
                {
                    Snackbar.Make(resJsonData.message).Show();
                }
            }
            catch (HttpRequestException e) {
                Toast.Make("请求数据失败，网络异常！！！").Show();
            }

        });
        a.Close();
        _temp.IsEnabled = true;
    }
    //单击用户协议
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        user.IsEnabled = false;
        Navigation.PushAsync(new BookPage(1));
        user.IsEnabled = true;
    }
    //单击隐私政策
    private void TapGestureRecognizer_Tapped1(object sender, TappedEventArgs e)
    {
        Yyinsi.IsEnabled = false;
        Navigation.PushAsync(new BookPage(0));
        Yyinsi.IsEnabled = true;
    }

    //Json数据类型
    public class Json_Login
    {
        public Json_LoginInfo user { get; set; }
        public string? token { get; set; }
        public int exp { get; set; }
        public long expires_time { get; set; }
    }
    public class Json_LoginInfo
    {
        public int uid { get; set; }
        public int wechat_user_id { get; set; }
        public string? account { get; set; }
        public int sex { get; set; }
        public string? nickname { get; set; }
        public string? avatar { get; set; }
        public string? phone { get; set; }
        public string? cancel_time { get; set; }
        public string? now_money { get; set; }
        public string? spread_limit { get; set; }
        public int brokerage_level { get; set; }
        public string? user_type { get; set; }
        public string? promoter_time { get; set; }
        public int is_promoter { get; set; }
        public int pay_count { get; set; }
        public int spread_pay_count { get; set; }
        public string? spread_pay_price { get; set; }
        public int integral { get; set; }
        public int member_level { get; set; }
        public int member_value { get; set; }
        public int count_start { get; set; }
        public int count_fans { get; set; }
    }
    public class Json_qqInfo 
    {
        public string? qq { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? avatar { get; set; }
    }
    //勾选事件
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        if (checkBox.IsChecked)
        {
            loginBtton.BackgroundColor = Color.FromArgb("#0094e9");
            loginBtton.IsEnabled = true;
        }
        else 
        {
            loginBtton.BackgroundColor = Color.FromArgb("#AAA");
            loginBtton.IsEnabled = false;
        }
    }
}