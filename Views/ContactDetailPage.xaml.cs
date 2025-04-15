// views/contactdetailpage.xaml.cs
using ContactMauiApp.ViewModels;
using System.Globalization; // For Converters
using CommunityToolkit.Mvvm.Input; // For IRelayCommand
using System.Windows.Input; // For ICommand

namespace ContactMauiApp.Views;

// this is the code-behind for the contact detail page.
// mostly just sets up the binding context to the viewmodel.
public partial class ContactDetailPage : ContentPage
{
    public ContactDetailPage(ContactDetailViewModel viewModel)
    {
        InitializeComponent(); // loads the ui defined in xaml
        BindingContext = viewModel; // connects the ui to its data and commands
    }
}

// --- value converters --- 
// these little helpers change data for the ui bindings.
// useful for things like flipping a boolean or changing button text dynamically.

// simple converter to flip a boolean value (true becomes false, false becomes true)
public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b) return !b;
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b) return !b;
        return false;
    }
}

// changes button text based on whether we're in edit mode
public class EditModeToButtonTextConverter : IValueConverter
{
     public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
     {
         if (value is bool isEditing)
         {
             // return different text depending on the edit state
             return isEditing ? "Cancel Edit" : "Back";
         }
         return "Back"; // default text if something goes wrong
     }

     // we don't need to convert back from text to bool, so this is not implemented
     public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
     {
         throw new NotImplementedException("going back from text to edit mode doesn't make sense here");
     }
}

// selects the correct command for a button based on edit mode
public class EditModeToButtonCommandConverter : IValueConverter
{
     public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
     {
         // checks if we're editing and gets the viewmodel from the parameter
         if (value is bool isEditing && parameter is ContactDetailViewModel viewModel)
         {
            // returns either the toggle edit command or the go back command
            return isEditing ? viewModel.ToggleEditCommand : viewModel.GoBackAsyncCommand;
         }

         // if things aren't right, return null (the binding will likely just ignore it)
         return null;
     }

     // again, converting back doesn't apply here
     public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
     {
         throw new NotImplementedException("converting back from command to edit mode? nah.");
     }
}

// ethan rettinger
