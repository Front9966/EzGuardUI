namespace YiZan.View;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();
		Shell.SetTabBarIsVisible(this, false);
		version.Text = AppInfo.Current.VersionString;
		adb.Text = "Ò×ÔÞ" + AppInfo.Current.VersionString;
    }
	//·µ»Ø¼ü
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		var _temp = (ImageButton)sender;
		_temp.IsEnabled = false;
		Navigation.PopAsync();
		_temp.IsEnabled = true;
    }
	//ÍË³öµÇÂ¼
    private void Button_Clicked(object sender, EventArgs e)
    {
        var _temp = (Button)sender;
        _temp.IsEnabled = false;
        All.Token = "";
		All.nicname = "";
		All.qqNumber = "";
		All.KuaishouAccount_Get_Like = "";
		All.KuaishouAccount_Like = "";

        var dataPath = FileSystem.Current.AppDataDirectory + "/data.ini";
		File.Delete(dataPath);

        All.LoginCode = 2;
		Navigation.PushAsync(new HomePage());
        _temp.IsEnabled = true;
    }
}