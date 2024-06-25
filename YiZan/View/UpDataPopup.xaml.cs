using CommunityToolkit.Maui.Views;
namespace YiZan.View;
public partial class UpDataPopup : Popup
{
    string url_Android;
    string url_IOS;
    public UpDataPopup(string version, string androidAddress, string iosAddress, string UpData)
    {
        InitializeComponent();
        this.url_Android = androidAddress;
        this.url_IOS = iosAddress;
        this.UpVersion.Text += " v" + version;
        if (UpData == "1")
        {
            cccc.IsVisible = false;
            CanBeDismissedByTappingOutsideOfPopup = false;
        }
    }
    //前往更新
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (All.IsAndroid())
        {
            await Launcher.OpenAsync(url_Android);
        }
        else if (All.IsIOS()) 
        {
            await Launcher.OpenAsync(url_IOS);
        }
    }
    //前往更新
    private async void Button_Clicked1(object sender, EventArgs e)
    {
        Close();
    }
}