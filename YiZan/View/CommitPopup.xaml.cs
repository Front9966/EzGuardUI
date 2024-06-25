using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using System.Text.Json;
namespace YiZan.View;

public partial class CommitPopup : Popup
{
    private int Code;
	public CommitPopup(int code)
	{
		InitializeComponent();
        if (code != 1)
        {
            msg.IsVisible = false;
        }
        if (code == 2)
        {
            link.IsVisible = false;
        }
        Code = code;
    }

    //单击关闭按钮
    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
    //单击提交按钮
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        var httpclient = new HttpClient();
        httpclient.DefaultRequestHeaders.Add("X-Token",All.Token);
        string url = null;
        Json_ResJsonClass<string> resJsonData = null;
        Dictionary<string, string> postContent = new Dictionary<string, string>();
        if (Code == 0 && link.Text != "")
        {
            url = All.hostname + "/api/ks/star";
            postContent.Add("link", link.Text);
        }
        else if (Code == 1 && link.Text != "" && msg.Text != "")
        {
            url = All.hostname + "/api/ks/comment";
            postContent.Add("link", link.Text);
            postContent.Add("content", msg.Text);
        }
        else if (Code == 2)
        {
            url = All.hostname + "/api/ks/collect";
        }
        if (url == "")
            return;

        var res = httpclient.PostAsync(url, new FormUrlEncodedContent(postContent)).Result;
        var res_string = res.Content.ReadAsStringAsync().Result;
        try
        {
            resJsonData = JsonSerializer.Deserialize<Json_ResJsonClass<string>>(res_string);

            if (resJsonData.status == 200)
            {
                Snackbar.Make(resJsonData.message).Show();
            }
            else
            {
                Snackbar.Make(resJsonData.message).Show();
            }
        }
        catch(Exception ex) 
        {
            Toast.Make("请求出现异常！！！").Show();
        }
        finally
        {
            this.Close();
        }
    }
}