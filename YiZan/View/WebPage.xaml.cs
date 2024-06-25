namespace YiZan.View;

public partial class WebPage : ContentPage
{
	public WebPage(string url)
	{
		InitializeComponent();
		Shell.SetTabBarIsVisible(this,false);
		NavigationPage.SetHasNavigationBar(this,false);
		WebViews.Source = url;
    }
}