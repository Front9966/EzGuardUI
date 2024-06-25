using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YiZan.View;
public partial class AccountControlPage : ContentPage
{
    private LoadPopup loadPage;
    Json_ResJsonClass<Json_Account> resJsonData;
    string res_string;
	public AccountControlPage()
	{
		InitializeComponent();
		Shell.SetTabBarIsVisible(this,false);
        loadPage = new LoadPopup();
        this.ShowPopup(loadPage);
        Task.Run(GetKuaiShouAccountInfo);
    }
    private void GetKuaiShouAccountInfo() 
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Token", All.Token);
        var res = client.GetAsync(All.hostname + "/api/ks/getKsInfo").Result;
        res_string = res.Content.ReadAsStringAsync().Result;
        
        resJsonData = JsonConvert.DeserializeObject<Json_ResJsonClass<Json_Account>>(res_string);
        MainThread.InvokeOnMainThreadAsync(() => {
            if (resJsonData.data.get_like.user_id != null)
            {
                Grid1.IsVisible = true;
                Image1.Source = resJsonData.data.get_like.user_img;
                nicname1.Text = resJsonData.data.get_like.user_name;
                All.KuaishouAccount_Get_Like = resJsonData.data.get_like.user_id;
                userinfo1.Text = resJsonData.data.get_like.user_follow.ToString() + "����ע " + resJsonData.data.get_like.user_fans + "����˿ " + resJsonData.data.get_like.user_works + "����Ʒ";
            }
            else
            {
                Grid1.IsVisible = false;
            }
            if (resJsonData.data.likes.user_id != null)
            {
                Grid2.IsVisible = true;
                Image2.Source = resJsonData.data.likes.user_img;
                nicname2.Text = resJsonData.data.likes.user_name;
                All.KuaishouAccount_Like = resJsonData.data.likes.user_id;
            }
            else
            {
                Grid2.IsVisible = false;
            }
            if (loadPage != null)
            {
                loadPage.Close();
                loadPage = null;
            }
        });
    }

	//�û���������
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var _temp = (ImageButton)sender;
        _temp.IsEnabled = false;
		await Navigation.PopAsync();
        _temp.IsEnabled = true;
    }

    //���������˺�+��
    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        Image1.IsEnabled = false;
        if (resJsonData?.data.get_like.user_id != null && resJsonData?.data.get_like.user_id != "")
        {
            //��¼��Ĳ���
            if (resJsonData.data.likes.user_id == null)
            {
                Toast.Make("��󶨵����˺��ڽ��л�ȡŶ������").Show();
            }
            else 
            {
                Navigation.PushAsync(new KuaiShouFunPage(res_string));
            }
        }
        else 
        {
            //δ��¼��Ĳ���
            var a = new LoginAccountPopup(1, GetKuaiShouAccountInfo);
            this.ShowPopup(a);
#if DEBUG
            Navigation.PushAsync(new KuaiShouFunPage(res_string));
#endif
        }
        Image1.IsEnabled = true;
    }
    //���������˺�+��
    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        Image2.IsEnabled = false;
        if (resJsonData?.data.likes.user_id != null && resJsonData?.data.likes.user_id != "")
        {
            //��¼��Ĳ���

        }
        else 
        {
            //δ��¼�Ĳ���
            var a = new LoginAccountPopup(2, GetKuaiShouAccountInfo);
            this.ShowPopup(a);
        }
        Image2.IsEnabled = true;
    }

    //�����˺��л��˺�
    private void Button_Clicked(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        var a = new LoginAccountPopup(1);
        this.ShowPopup(a);
        _temp.IsEnabled = true;
    }
    //�����˺��˳��˺�
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Token", All.Token);
        var postContent = new Dictionary<string, string>() {
            { "type", "1"},
            { "eid", All.KuaishouAccount_Get_Like}
        };
        var res = httpClient.PostAsync(All.hostname + "/api/ks/cancleBind", new FormUrlEncodedContent(postContent)).Result;
        var res_string = res.Content.ReadAsStringAsync().Result;
        JObject resJon = JObject.Parse(res_string);
        Snackbar.Make((string)resJon["message"]).Show();
        Image1.Source = "account_add.png";
        nicname1.Text = "δ��";
        userinfo1.Text = "0����ע 0����˿ 0����Ʒ";
        Image1.IsEnabled = true;
        GetKuaiShouAccountInfo();
        _temp.IsEnabled = true;
    }
    //�����˺��л��˺�
    private void Button_Clicked_2(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        var a = new LoginAccountPopup(2);
        this.ShowPopup(a);
        _temp.IsEnabled = true;
    }
    //�����˺��˳��˺�
    private void Button_Clicked_3(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Token", All.Token);
        var postContent = new Dictionary<string, string>() {
            { "type", "2"},
            { "eid", All.KuaishouAccount_Like}
        };
        var res = httpClient.PostAsync(All.hostname + "/api/ks/cancleBind", new FormUrlEncodedContent(postContent)).Result;
        var res_string = res.Content.ReadAsStringAsync().Result;
        JObject resJon = JObject.Parse(res_string);
        Snackbar.Make((string)resJon["message"]).Show();
        Image2.Source = "account_add.png";
        nicname2.Text = "δ��";
        Image2.IsEnabled = true;
        GetKuaiShouAccountInfo();
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

}