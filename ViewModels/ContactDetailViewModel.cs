// viewmodels/contactdetailviewmodel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactMauiApp.Models;
using ContactMauiApp.Services;
using System.Windows.Input; // Added for ICommand

namespace ContactMauiApp.ViewModels
{
    // this viewmodel manages the data and actions for the contact detail page.
    // it receives the contact's id via navigation query parameters.
    [QueryProperty(nameof(ContactId), "ContactId")]
    public partial class ContactDetailViewModel : BaseViewModel
    {
        private readonly ContactService _contactService; // used to load/save contact data

        // the actual contact object being displayed or edited.
        // [observableproperty] makes it magic - changes here update the ui.
        [ObservableProperty]
        Models.Contact? _contact;

        // stores the id passed from the contacts list page.
        // when this changes (via navigation), oncontactidchanged gets called.
        [ObservableProperty]
        string? _contactId;

        // flag to track if the ui should be in view mode or edit mode.
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveChangesAsyncCommand))] // re-check if save is allowed when this changes
        bool _isEditing = false;

        // --- commands for ui actions --- 
        // these are explicitly defined here instead of using [relaycommand] for potentially more control
        // or due to source generator quirks.
        public IRelayCommand GoBackAsyncCommand { get; }
        public IAsyncRelayCommand SaveChangesAsyncCommand { get; }
        public IRelayCommand ToggleEditCommand { get; }

        public ContactDetailViewModel(ContactService contactService)
        {
            _contactService = contactService;
            Title = "Contact Details";
            
            // wire up the commands to their methods
            GoBackAsyncCommand = new AsyncRelayCommand(GoBackAsync); // command -> method
            SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSave); // command -> method, with a condition (cansave)
            ToggleEditCommand = new RelayCommand(ToggleEdit); // command -> method
        }

        // this magic partial method is auto-called by the mvvm toolkit when contactid changes.
        // it kicks off loading the actual contact data.
        async partial void OnContactIdChanged(string? value)
        {
            if (!string.IsNullOrEmpty(value) && Guid.TryParse(value, out Guid id))
            {
                await LoadContactAsync(id); // load the contact based on the parsed id
            }
            else
            {
                // handle cases where the id is missing or invalid
                await Shell.Current.DisplayAlert("Error", "Invalid Contact ID.", "OK");
                await GoBackAsync(); // can't show details without a valid id, so go back
            }
        }

        // fetches the contact details from the service using the id.
        private async Task LoadContactAsync(Guid id)
        {
            IsBusy = true; // show a loading indicator
            try
            {
                var contact = await _contactService.GetContactByIdAsync(id);
                if (contact != null)
                {
                    Contact = contact; // update the contact property (ui updates!)
                    Title = $"Details: {Contact.Name}"; // update the page title
                }
                else
                {
                    // handle case where contact with that id doesn't exist
                    await Shell.Current.DisplayAlert("Error", "Contact not found.", "OK");
                    await GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                // catch any unexpected errors during loading
                await Shell.Current.DisplayAlert("Error", $"Failed to load contact: {ex.Message}", "OK");
                await GoBackAsync();
            }
            finally
            {
                IsBusy = false; // hide loading indicator regardless of success/failure
            }
        }

        // method executed by gobackasynccommand.
        async Task GoBackAsync()
        {
            IsEditing = false; // ensure we leave edit mode if active
            await Shell.Current.GoToAsync("..", true); // navigate back up the stack
        }

        // method executed by toggleeditcommand.
        void ToggleEdit()
        {
            IsEditing = !IsEditing; // flip the edit mode flag
            // update the title to reflect the current mode
            Title = IsEditing ? $"Editing: {Contact?.Name}" : $"Details: {Contact?.Name}";
        }

        // method executed by savechangesasynccommand.
        async Task SaveChangesAsync()
        {
             if (Contact == null) return; // safety check

             IsBusy = true;
             try
             {
                 await _contactService.UpdateContactAsync(Contact); // tell the service to save changes
                 IsEditing = false; // exit edit mode after saving
                 Title = $"Details: {Contact.Name}"; // reset title
                 await Shell.Current.DisplayAlert("Success", "Contact updated.", "OK");
             }
             catch (Exception ex)
             {
                 // handle potential save errors
                 await Shell.Current.DisplayAlert("Error", $"Failed to update contact: {ex.Message}", "OK");
             }
             finally
             {
                 IsBusy = false;
             }
        }

        // determines if the savechangesasynccommand can execute (i.e., if the save button is enabled).
        // requires a valid contact object and non-empty name/email.
        private bool CanSave() => Contact != null && 
                                  !string.IsNullOrWhiteSpace(Contact.Name) && 
                                  !string.IsNullOrWhiteSpace(Contact.Email) &&
                                  IsEditing; // only allow save when editing

        // this partial method is called when the entire contact object reference changes.
        // we need to hook/unhook the propertychanged listener for the cansave logic.
        partial void OnContactChanged(Models.Contact? oldValue, Models.Contact? newValue)
        {
             if (oldValue != null)
             {
                 // stop listening to the old contact's property changes
                 oldValue.PropertyChanged -= Contact_PropertyChanged;
             }

             if (newValue != null)
             {
                 // start listening to the new contact's property changes
                 newValue.PropertyChanged += Contact_PropertyChanged;
             }
             // re-evaluate if save is possible now that the contact object itself changed
             SaveChangesAsyncCommand.NotifyCanExecuteChanged();
        }

        // this event handler listens for changes to properties *within* the current contact object.
        private void Contact_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
             // if the name or email changed, we need to re-evaluate if the save button should be enabled.
             if (e.PropertyName == nameof(Contact.Name) || e.PropertyName == nameof(Contact.Email))
             {
                 SaveChangesAsyncCommand.NotifyCanExecuteChanged();
             }
        }

        // // partial method called when isediting changes (handled by attribute now)
         // partial void OnIsEditingChanged(bool value)
         // {
         //     SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         // }
    }
}

// ethan rettinger

