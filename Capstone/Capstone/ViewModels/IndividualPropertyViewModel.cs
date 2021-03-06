using System;
using System.Collections.Generic;
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
        private TimeSpan proposedShowtime;
        private bool ownerView;
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
            if (!ownerView && canSchedule) App.NavigationService.NavigateToModal(ViewNames.ConfirmationPageView, new List<object>() { property, proposedShowtime });
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
                PopulateNextShowtime(await App.DataService.GetUserAsync(property.OwnerID));
        }

        private void PopulateEditButton()
        {
            ownerView = true;
            ButtonText = "Edit Property";
        }

        public void PopulateNextShowtime(User user)
        {
            ownerView = false;
            TimeBlock todaysAvailability;
            if(user.Availability.TryGetValue(DateTimeOffset.Now.DayOfWeek, out todaysAvailability))
            {
                TimeSpan now = new TimeSpan(
                    DateTimeOffset.Now.Hour,
                    DateTimeOffset.Now.Minute,
                    DateTimeOffset.Now.Second);
                if(now.Add(new TimeSpan(1, 15, 0)) <= todaysAvailability.End)
                {
                    if (now.Add(new TimeSpan(0, 15, 0)) >= todaysAvailability.Start)
                    {
                        //display a time 15 minutes from now.
                        proposedShowtime = now.Add(new TimeSpan(0, 15, 0));
                        ButtonText = "See at: " + TimeFormatter(now.Add(new TimeSpan(0, 15, 0)));
                        canSchedule = true;
                    }
                    else
                    {
                        proposedShowtime = todaysAvailability.Start;
                        ButtonText = "See at: " + TimeFormatter(todaysAvailability.Start);
                        canSchedule = true;
                    }
                }
                else
                {
                    ButtonText = "Not Available";
                    canSchedule = false;
                }
            }
            else
            {
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
