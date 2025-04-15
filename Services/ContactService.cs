// services/contactservice.cs
using System.Collections.ObjectModel;
using ContactMauiApp.Models;
using Contact = ContactMauiApp.Models.Contact; // Add alias

namespace ContactMauiApp.Services
{
    // this class handles all the logic for managing contacts
    // think of it as the single source of truth for contact data
    public class ContactService
    {
        // observablecollection is neat because it tells the ui when things change,
        // automatically updating lists and stuff without extra code
        private readonly ObservableCollection<Contact> _contacts = new();

        public ContactService()
        {
            // just adding a couple of dummy contacts to start with
            // replace this with real data loading later!
            _contacts.Add(new Contact("Alice Smith", "alice@example.com", "111-222-3333", "Friend"));
            _contacts.Add(new Contact("Bob Johnson", "bob@example.com", "444-555-6666", "Colleague"));
        }

        // simply returns the current list of contacts
        public ObservableCollection<Contact> GetContacts()
        {
            return _contacts;
        }

        // adds a new contact to our collection
        // using task.completedtask because this in-memory operation is basically instant
        // if we were saving to a database, this would be an actual async operation
        public Task AddContactAsync(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            _contacts.Add(contact);
            return Task.CompletedTask;
        }

        // finds a contact by their unique id
        public Task<Contact?> GetContactByIdAsync(Guid id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            // returns the found contact or null if not found
            return Task.FromResult<Contact?>(contact);
        }

        // updates an existing contact's details
        public Task UpdateContactAsync(Contact updatedContact)
        {
            if (updatedContact == null)
                throw new ArgumentNullException(nameof(updatedContact));

            var existingContact = _contacts.FirstOrDefault(c => c.Id == updatedContact.Id);
            if (existingContact != null)
            {
                // copies the new details over the old ones
                existingContact.Name = updatedContact.Name;
                existingContact.Email = updatedContact.Email;
                existingContact.PhoneNumber = updatedContact.PhoneNumber;
                existingContact.Description = updatedContact.Description;
            }
            // again, using completedtask for this simple in-memory update
            return Task.CompletedTask;
        }

        // removes a contact from the list based on their id
        public Task DeleteContactAsync(Guid id)
        {
             var contactToRemove = _contacts.FirstOrDefault(c => c.Id == id);
             if (contactToRemove != null)
             {
                 _contacts.Remove(contactToRemove); // bye bye contact!
             }
             return Task.CompletedTask;
        }
    }
}

// ethan rettinger
