using CommunityToolkit.Maui.Views;
namespace YiZan.View;
public partial class LoginAccountPopup : Popup
{
	private KuaiShouApi kuaiShouApi;
    private int AccountType;
    public delegate void Se();
    private Se sc;
    Thread thread;
    public LoginAccountPopup(int accountType, Se se = null )// 1:获赞 2:点赞
	{
		InitializeComponent();
        AccountType = accountType;
        sc += se;
        ThreadStart threadStart = new ThreadStart(Init);
        thread = new Thread(threadStart);
        thread.Start();
	}
    private void Init()
    {
        kuaiShouApi = new KuaiShouApi();
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            status.Text = "获取二维码当中";
        });
        kuaiShouApi.GetQr();
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            qrimage.Source = "data:image/png;base64," + kuaiShouApi.Qr.imageData;
            status.Text = "等待扫描二维码......";
        });
        if (kuaiShouApi.GetLoginCode(kuaiShouApi.Qr.qrLoginToken, kuaiShouApi.Qr.qrLoginSignature) == 1)
        {
            if (status != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    status.Text = "扫描成功,等待确认......";
                    status.TextColor = Color.FromHex("#00f91a");
                });
                var Cookie = kuaiShouApi.GetCookie(kuaiShouApi.Qr.qrLoginToken, kuaiShouApi.Qr.qrLoginSignature).Result;
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    status.Text = "绑定快手账号成功";
                    status.TextColor = Color.FromHex("#00f91a");
                });
                var userId = kuaiShouApi.loginInfo.userId.ToString();
                var eid = kuaiShouApi.eid;
                var url = All.hostname + "/api/ks/bind";
                var postContent = new Dictionary<string, string>() {
                    { "type", AccountType.ToString() },
                    { "cookie", Cookie },
                    { "user_id", userId },
                    { "eid", eid}
                };
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-Token", All.Token);
                var res = httpClient.PostAsync(url, new FormUrlEncodedContent(postContent)).Result;
                if (sc != null)
                    sc();
                MainThread.InvokeOnMainThreadAsync(() => { Close(); });
            }
        }
        else
        {
            if (status != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    status.Text = "二维码过期或已失效";
                    status.TextColor = Color.FromHex("#ff2700");
                });
            }
        }
        
    }
    //单击关闭按钮
    private void Button_Clicked(object sender, EventArgs e)
    {
        //thread.Suspend();
        //thread.Abort();
        this.Close();
    }
}