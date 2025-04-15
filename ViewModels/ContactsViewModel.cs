// ViewModels/ContactsViewModel.cs
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactMauiApp.Models;
using ContactMauiApp.Services;
using ContactMauiApp.Views; // Needed for nameof pages
using System.Windows.Input; // Added for ICommand

namespace ContactMauiApp.ViewModels
{
    // viewmodel for the main page that lists all contacts.
    public partial class ContactsViewModel : BaseViewModel
    {
        private readonly ContactService _contactService;

        // the collection of contacts displayed in the list.
        // this is directly from the service. since it's observable, ui updates automatically!
        public ObservableCollection<Models.Contact> Contacts { get; private set; }

        // holds the contact that the user taps on in the list.
        // bound to the listview's selecteditem in xaml.
        [ObservableProperty]
        Models.Contact? _selectedContact;

        // command to navigate to the "add contact" page.
        public IAsyncRelayCommand GoToAddContactAsyncCommand { get; }

        public ContactsViewModel(ContactService contactService)
        {
            _contactService = contactService;
            Title = "Contacts"; // page title
            // grab the contacts list from the service
            Contacts = _contactService.GetContacts(); 
            // set up the command for the "add" button
            GoToAddContactAsyncCommand = new AsyncRelayCommand(GoToAddContactAsync);
        }

        // this magic method is called automatically when selectedcontact changes (i.e., user taps a contact).
        partial void OnSelectedContactChanged(Models.Contact? value)
        {
            if (value != null) // if a contact was actually selected (not deselected)
            {
                // navigate to the detail page, passing the contact's id
                // the detail viewmodel will pick up this "contactid" query parameter
                Shell.Current.GoToAsync($"{nameof(ContactDetailPage)}?ContactId={value.Id}", true);
                
                // IMPORTANT: reset selectedcontact back to null.
                // if we don't do this, tapping the same contact again won't trigger the change 
                // because the value hasn't technically changed from the viewmodel's perspective.
                SelectedContact = null; 
            }
        }

        // method executed by gotoaddcontactasynccommand.
        async Task GoToAddContactAsync()
        {
            // just navigate to the page for adding new contacts.
            await Shell.Current.GoToAsync(nameof(AddContactPage), true);
        }

    }
}

// ethan rettinger
