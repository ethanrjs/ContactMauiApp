// views/contactspage.xaml.cs
using ContactMauiApp.ViewModels;

namespace ContactMauiApp.Views;

// code-behind for the main contacts list page.
// connects the list ui (xaml) with its data source (the viewmodel).
public partial class ContactsPage : ContentPage
{
    public ContactsPage(ContactsViewModel viewModel)
    {
        InitializeComponent(); // builds the page visually
        BindingContext = viewModel; // wires up the data and commands
    }
}

// ethan rettinger
