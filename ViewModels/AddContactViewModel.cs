// ViewModels/AddContactViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactMauiApp.Models;
using ContactMauiApp.Services;
using ContactMauiApp.Views; // Needed for nameof(ContactsPage)
using Contact = ContactMauiApp.Models.Contact; // Alias to resolve ambiguity
using System.Windows.Input; // Added for ICommand

namespace ContactMauiApp.ViewModels
{
    // viewmodel for the page where users add new contacts.
    public partial class AddContactViewModel : BaseViewModel
    {
        private readonly ContactService _contactService; // service to actually save the contact

        // --- properties bound to the input fields in the ui --- 
        // using [observableproperty] so the ui sees changes and vice versa
        [ObservableProperty]
        string? _name;

        [ObservableProperty]
        string? _email;

        [ObservableProperty]
        string? _phoneNumber;

        [ObservableProperty]
        string? _description;

        // --- commands for the save and cancel/back buttons --- 
        public IAsyncRelayCommand SaveContactAsyncCommand { get; }
        public IAsyncRelayCommand GoBackAsyncCommand { get; }

        public AddContactViewModel(ContactService contactService)
        {
            _contactService = contactService;
            Title = "Add New Contact";
            // link commands to their respective methods
            SaveContactAsyncCommand = new AsyncRelayCommand(SaveContactAsync);
            GoBackAsyncCommand = new AsyncRelayCommand(GoBackAsync);
        }

        // executed when the save command is triggered
        async Task SaveContactAsync()
        {
            // basic validation: make sure name and email aren't empty
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email))
            {
                await Shell.Current.DisplayAlert("Hold Up!", "Name and Email can't be empty.", "OK");
                return; // stop processing if validation fails
            }

            IsBusy = true; // show loading spinner
            try
            {
                // create a new contact object from the form data
                // using ! (null-forgiving operator) because we just checked for null/whitespace above
                var newContact = new Contact(Name!, Email!, PhoneNumber, Description); 
                await _contactService.AddContactAsync(newContact); // save it via the service

                // success! clear the form fields
                ClearForm();

                // all done, navigate back to the contacts list
                await Shell.Current.GoToAsync("..", true);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Couldn't save the contact: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false; // hide loading spinner
            }
        }

         // executed when the back/cancel command is triggered
         async Task GoBackAsync()
         {
            // clear the form data before leaving the page
            ClearForm();
            await Shell.Current.GoToAsync("..", true); // navigate back
         }

        // helper method to reset all form fields
        private void ClearForm()
        {
            Name = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Description = string.Empty;
        }
    }
}

// ethan rettinger
