using System.Text.Json;

namespace YiZan.View;

public partial class BookPage : ContentPage
{
	public BookPage(int code)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetTabBarIsVisible(this, false);
        Task.Run(async () => { 
			HttpClient httpClient = new HttpClient();
			HttpResponseMessage res = await httpClient.GetAsync(All.hostname + "/api/index/index");
			var res_string = await res.Content.ReadAsStringAsync();
			var content = JsonSerializer.Deserialize<Json_ResJsonClass<Json_cifg>>(res_string);
			await MainThread.InvokeOnMainThreadAsync(() => {
                if (code == 0)
                {
                    text1.Text = content.data.privacy_agreement.sys_userr_privacy;
                }
                else if (code == 1)
                {
                    text1.Text = content.data.user_agreement.sys_user_agree;
                }
				NavigationPage.SetHasNavigationBar(this,false);
				Shell.SetTabBarIsVisible(this,false);
            });
        });
	}

    //JSON数据类型
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