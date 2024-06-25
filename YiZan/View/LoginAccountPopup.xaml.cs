using CommunityToolkit.Maui.Views;
namespace YiZan.View;
public partial class LoginAccountPopup : Popup
{
	private KuaiShouApi kuaiShouApi;
    private int AccountType;
    public delegate void Se();
    private Se sc;
    Thread thread;
    public LoginAccountPopup(int accountType, Se se = null )// 1:���� 2:����
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
            status.Text = "��ȡ��ά�뵱��";
        });
        kuaiShouApi.GetQr();
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            qrimage.Source = "data:image/png;base64," + kuaiShouApi.Qr.imageData;
            status.Text = "�ȴ�ɨ���ά��......";
        });
        if (kuaiShouApi.GetLoginCode(kuaiShouApi.Qr.qrLoginToken, kuaiShouApi.Qr.qrLoginSignature) == 1)
        {
            if (status != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    status.Text = "ɨ��ɹ�,�ȴ�ȷ��......";
                    status.TextColor = Color.FromHex("#00f91a");
                });
                var Cookie = kuaiShouApi.GetCookie(kuaiShouApi.Qr.qrLoginToken, kuaiShouApi.Qr.qrLoginSignature).Result;
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    status.Text = "�󶨿����˺ųɹ�";
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
                    status.Text = "��ά����ڻ���ʧЧ";
                    status.TextColor = Color.FromHex("#ff2700");
                });
            }
        }
        
    }
    //�����رհ�ť
    private void Button_Clicked(object sender, EventArgs e)
    {
        //thread.Suspend();
        //thread.Abort();
        this.Close();
    }
}