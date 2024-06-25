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
            Toast.Make("��������ʧ�ܣ������쳣������").Show();
        }
    }
    //JSON��������
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
    //���ؼ�������
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    //��ϵQQ������
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("http://wpa.qq.com/msgrd?v=3&uin=" + resJsonData.data.contact_qq + "&site=qq&menu=yes");
    }
    //��ϵ΢�ŵ���
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(resJsonData.data.contact_wechat);
        Toast.Make("���Զ�����΢�ź�,��ǰ��΢��ճ������").Show();
    }
}