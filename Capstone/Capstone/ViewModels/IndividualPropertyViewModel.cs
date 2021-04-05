using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Capstone.Models;
using Capstone.Services;
using Capstone.Utility;
using Xamarin.Forms;

namespace Capstone.ViewModels
{
    public class IndividualPropertyViewModel : BaseViewModel
    {
        private Property property;
        private string buttonText;
        private bool canSchedule;
        private bool ownerView;
        private List<Showing> possibleShowings;
        public Property Property
        {
            get => property;
            set
            {
                property = value;
                OnPropertyChanged();
            }
        }
        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }
        public ICommand ButtonClicked { get; }
        public IndividualPropertyViewModel()
        {
            Title = "Property";
            Property = new Property();

            ButtonClicked = new Command(OnSeeHomeClickedCommand);
            //commands and initializers in here.
            MessagingCenter.Subscribe<EditPropertyViewModel, Property>
                (this, MessageNames.PropertyUpdatedMessage, MessagePropertyUpdated);
        }

        private void MessagePropertyUpdated(EditPropertyViewModel sender, Property property)
        {
            Property = property;
        }

        public void OnSeeHomeClickedCommand()
        {
            if (!ownerView && canSchedule) App.NavigationService.NavigateToModal(ViewNames.ConfirmationPageView, new List<object>() { property });
            else if (ownerView) App.NavigationService.NavigateToModal(ViewNames.EditPropertyView, property);
        }

        public async override void Initialize(object parameter)
        {
            if (parameter == null)
                Property = new Property();
            else
                Property = parameter as Property;

            if (Property.OwnerID == App.User)
                PopulateEditButton();
            else
            {
                List<object> participants = new List<object>();
                participants.Add(await App.DataService.GetUserAsync(property.OwnerID));
                participants.Add(await App.DataService.GetUserAsync(App.User));
                participants.Add(property);
                PopulateNextShowtime(participants);
            }
        }

        private void PopulateEditButton()
        {
            ownerView = true;
            ButtonText = "Edit Property";
        }

        // Generate the showtimes that the property is available for today, and display the next
        // available showtime. Create a
        public void PopulateNextShowtime(List<object> participants)
        {
            possibleShowings = Availability.GetPossibleShowings(participants);

            ownerView = false;
            if (possibleShowings.Count > 0)
            {
                TimeSpan now = new TimeSpan(
                    DateTimeOffset.Now.Hour,
                    DateTimeOffset.Now.Minute,
                    DateTimeOffset.Now.Second);

                Showing next = possibleShowings
                    .OrderBy(s => s.StartTime)
                    .Where(s => (s.StartTime.TimeOfDay > now) && (s.Available = true))
                    .FirstOrDefault();

                if (next != null)
                {
                    ButtonText = "See at: " + TimeFormatter(next.StartTime.TimeOfDay);
                    canSchedule = true;
                }
                else
                {
                    // the available showings have passed
                    ButtonText = "Not Available";
                    canSchedule = false;
                }
            }
            else
            {
                // no showings were available today
                ButtonText = "Not Available";
                canSchedule = false;
            }
        }

        public string TimeFormatter(TimeSpan timeSpan)
        {
            if (timeSpan.Hours <= 11)
            {
                return $"{timeSpan.ToString(@"h\:mm")} AM";
            } 
            else
            {
                if (timeSpan.Hours > 12) timeSpan = timeSpan - new TimeSpan(12, 0, 0);
                return $"{timeSpan.ToString(@"h\:mm")} PM";
            }
        }
    }
}
