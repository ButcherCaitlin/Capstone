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
        private string scheduleText;
        private bool canSchedule;
        private TimeSpan proposedShowtime;
        public Property Property
        {
            get => property;
            set
            {
                property = value;
                OnPropertyChanged();
            }
        }
        public string ScheduleText
        {
            get => scheduleText;
            set
            {
                scheduleText = value;
                OnPropertyChanged();
            }
        }
        public ICommand SeeHomeClicked { get; }
        public IndividualPropertyViewModel()
        {
            Title = "Property";
            Property = new Property();

            SeeHomeClicked = new Command(OnSeeHomeClickedCommand);
            //commands and initializers in here.
        }

        public void OnSeeHomeClickedCommand()
        {
            if(canSchedule) App.NavigationService.NavigateToModal(ViewNames.ConfirmationPageView, new List<object>(){ property, proposedShowtime });
        }

        public async override void Initialize(object parameter)
        {
            if (parameter == null)
                Property = new Property();
            else
                Property = parameter as Property;

            PopulateNextShowtime(await App.DataService.GetUserAsync(property.OwnerID));
        }

        public void PopulateNextShowtime(User user)
        {
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
                        ScheduleText = "See at: " + TimeFormatter(now.Add(new TimeSpan(0, 15, 0)));
                        canSchedule = true;
                    }
                    else
                    {
                        proposedShowtime = todaysAvailability.Start;
                        ScheduleText = "See at: " + TimeFormatter(todaysAvailability.Start);
                        canSchedule = true;
                    }
                }
                else
                {
                    ScheduleText = "Not Available";
                    canSchedule = false;
                    //THIS CODE NEEDS TO BE REMOVED IT WAS ADDED FOR TESTING>>
                    proposedShowtime = todaysAvailability.Start;
                    ScheduleText = "See at: " + TimeFormatter(todaysAvailability.Start);
                    canSchedule = true;
                }
            }
            else
            {
                ScheduleText = "Not Available";
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
