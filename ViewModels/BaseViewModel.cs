// ViewModels/BaseViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContactMauiApp.ViewModels
{
    // a base class for our viewmodels to inherit common stuff.
    // inheriting from observableobject gives us property change notification magic for free.
    public partial class BaseViewModel : ObservableObject
    {
        // flag to indicate if a long-running operation (like loading data) is happening.
        // typically bound to an activityindicator's isrunning property in the ui.
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool _isBusy;

        // holds the title displayed on the page (e.g., in the navigation bar).
        [ObservableProperty]
        string? _title;

        // a handy inverted version of isbusy.
        // useful for disabling buttons while busy: bind button's isenabled to isnotbusy.
        public bool IsNotBusy => !IsBusy;
    }
}

// ethan rettinger
