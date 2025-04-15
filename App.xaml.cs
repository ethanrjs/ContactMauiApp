namespace ContactMauiApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	// this is where the magic starts! we create the main window 
	// and tell it to show our AppShell (which holds the pages)
	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}

// ethan rettinger