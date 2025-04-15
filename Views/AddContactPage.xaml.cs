// Views/AddContactPage.xaml.cs
using ContactMauiApp.ViewModels;

namespace ContactMauiApp.Views;

// code-behind for the page where new contacts are added.
// links the ui (xaml) with its brain (the viewmodel).
public partial class AddContactPage : ContentPage
{
    public AddContactPage(AddContactViewModel viewModel)
    {
        InitializeComponent(); // sets up the visual elements
        BindingContext = viewModel; // provides the data and actions for the ui
    }
}

// ethan rettinger
