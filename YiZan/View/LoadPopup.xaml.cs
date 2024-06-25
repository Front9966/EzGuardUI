using CommunityToolkit.Maui.Views;
namespace YiZan.View;
public partial class LoadPopup : Popup
{
	public LoadPopup()
	{
		InitializeComponent();
        Task.Run(async () => {
            while (true)
            {
                await image1.RelRotateTo(360, 2000);
            }
        });
    }
}