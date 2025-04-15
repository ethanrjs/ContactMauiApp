// models/contact.cs
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContactMauiApp.Models
{
    // represents a single contact.
    // inherits from observableobject and uses [observableproperty] 
    // so the ui automatically updates when contact details change.
    // the mvvm toolkit source generators write the necessary boilerplate code for us!
    public partial class Contact : ObservableObject
    {
        // a unique identifier for each contact, generated automatically
        public Guid Id { get; set; } = Guid.NewGuid();

        // the [observableproperty] attribute tells the source generator
        // to create a public property (e.g., Name) that wraps this private field (_name)
        // and handles the property change notifications.
        [ObservableProperty]
        string? _name;

        [ObservableProperty]
        string? _email;

        [ObservableProperty]
        string? _phoneNumber;

        [ObservableProperty]
        string? _description;

        // default constructor - sometimes needed for serialization or frameworks
        public Contact() { }

        // a handy constructor to create a contact with initial details
        public Contact(string name, string email, string? phoneNumber, string? description)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Description = description;
        }

        // we define equality based on the unique id.
        // this means two contact objects are considered the same if they have the same id,
        // regardless of other property values.
        public override bool Equals(object? obj)
        {
            return obj is Contact contact && Id.Equals(contact.Id);
        }

        // if you override equals, you should always override gethashcode too.
        // it's important for performance in collections like dictionaries or hashsets.
        // basing it on the id is consistent with our equals logic.
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

// ethan rettinger
