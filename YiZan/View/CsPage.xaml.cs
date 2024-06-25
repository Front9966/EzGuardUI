using CommunityToolkit.Maui.Alerts;
using System.Text.Json;
namespace YiZan.View;

public partial class CsPage : ContentPage
{
    private Json_ResJsonClass<Json_BookContent> resJsonData;
    public CsPage()
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this,false);
        Task.Run(GetCs);
    }
	private async void GetCs() 
	{
        string url_path = All.hostname + "/api/index/index";
        HttpClient client = new HttpClient();
        try
        {
            var res = await client.GetAsync(url_path);
            if (res.IsSuccessStatusCode)
            {
                string res_string = await res.Content.ReadAsStringAsync();
                resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<Json_BookContent>>(res_string);
            }
        } 
        catch (HttpRequestException ex)
        {
            Toast.Make("请求数据失败，网络异常！！！").Show();
        }
    }
    //JSON数据类型
    public class Json_BookContent
    {
        public Dictionary<string,string> privacy_agreement { get; set; }
        public Dictionary<string,string> user_agreement { get; set; }
        public string? contact_qq { get; set; }
        public string? contact_wechat { get; set; }
        public string? help {  get; set; }
        public string? chat {  get; set; }
        public string? apply {  get; set; }
    }
    //返回键被单击
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    //联系QQ被单击
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("http://wpa.qq.com/msgrd?v=3&uin=" + resJsonData.data.contact_qq + "&site=qq&menu=yes");
    }
    //联系微信单击
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(resJsonData.data.contact_wechat);
        Toast.Make("已自动拷贝微信号,请前往微信粘贴搜索").Show();
    }
}