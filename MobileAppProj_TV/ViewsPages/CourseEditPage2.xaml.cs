using MobileAppProj_TV.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppProj_TV.ViewsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseEditPage : ContentPage
    {
        //use for references
        Course selectedCourse;
        Term selectedTerm;
        TermHomePage _termHomePage;

        //use to bind with the dropdowns - course status
        public List<string> courseStatusPick = new List<string>
        { "In Progress", "Completed", "Dropped", "Plan to Take" };

        //selections for the dropdown - notification
        public List<string> courseNotify = new List<string> { "Yes", "No" };
        public CourseEditPage(Term selectedTerm, TermHomePage termHomePage, Course selectedCourse)
        {

            
            InitializeComponent();
            this.selectedCourse = selectedCourse;
            this.selectedTerm = selectedTerm;
            _termHomePage = termHomePage;
        }


        //data appears on the form upon loading
        protected override void OnAppearing()
        {
            courseStatusEntry.ItemsSource = courseStatusPick;
            courseStatusEntry.SelectedIndex = courseStatusPick.FindIndex(courseStat => courseStat == selectedCourse.CourseStatus);
            txtCourseTitleEntry.Text = selectedCourse.CourseTitle;
            courseStatusEntry.SelectedItem = selectedCourse.CourseStatus;
            startCourseDateEntry.Date = selectedCourse.CourseStart.Date;
            endCourseDateEntry.Date = selectedCourse.CourseEnd.Date;
            txtInstructorNameEntry.Text = selectedCourse.InstructorName;
            txtInstructorPhoneEntry.Text = selectedCourse.InstructorPhone;
            txtInstructorEmailEntry.Text = selectedCourse.InstructorEmail;
            txtNotesEntry.Text = selectedCourse.CourseNotes;
            // this checks whether notification is yes or no
            if (selectedCourse.CourseNotification == 1)
            {
                notifications.SelectedIndex = 1;
            }
            else
            {
                notifications.SelectedIndex = 0;
            }
            base.OnAppearing();
        }

        //cancels the page
        private void btnDiscardEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btnSaveEntry_Clicked(object sender, EventArgs e)
        {
            //input received from the textboxes are assigned to selecteCourse
            selectedCourse.CourseTitle = txtCourseTitleEntry.Text;
            selectedCourse.CourseStatus = courseStatusEntry.SelectedItem.ToString();
            selectedCourse.CourseStart = startCourseDateEntry.Date;
            selectedCourse.CourseEnd = endCourseDateEntry.Date;
            selectedCourse.InstructorName = txtInstructorNameEntry.Text;
            selectedCourse.InstructorPhone = txtInstructorPhoneEntry.Text;
            selectedCourse.InstructorEmail = txtInstructorEmailEntry.Text;
            selectedCourse.CourseNotes = txtNotesEntry.Text;
            selectedCourse.CourseNotification = notifications.SelectedIndex;
            selectedCourse.Term = selectedTerm.Id;

            //check if the conditions are met 
            if (isValidEntry())
            {
                try
                {
                    //establish connnection to the database
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {
                        //if table already exists this gets ignored
                        connection.CreateTable<Course>();
                        //otherwise, it updates the selectedCourse
                        int rows = connection.Update(selectedCourse);

                        //throw a message when succesful
                        if (rows > 0)
                        {
                             DisplayAlert("Success", "Course has been updated", "Ok");
                            
                        }

                        //redirects to the course display page
                            Navigation.PushAsync(new CourseDisplayPage(selectedTerm, _termHomePage, selectedCourse));
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            else
            {
                //DisplayAlert("Error", "Check entries", "Ok");
                DisplayAlert("Error", "Please make sure all fields are filled out. Start date must be before the end date. Email must be in the correct format", "Ok");

            }

        }

        //-------------------


        //method to validate entries

        private bool isValidString(string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        //method to validate the email - correct format
        private bool isValidEmail(string text)
        {
            try
            {
                //create an instance of MailAddress to use
                MailAddress mailAddress = new MailAddress(text);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        //checks the validity of entires, if any of these are null or dates are not correct
        private bool isValidEntry()
        {
            if (!isValidString(txtCourseTitleEntry.Text))
            {
                return false;
            }
            if (courseStatusEntry.SelectedItem == null)
            {
                return false;
            }
            if (startCourseDateEntry.Date == null && endCourseDateEntry.Date == null)
            {
                return false;
            }
            //start date cannot be later than the end date
            if (endCourseDateEntry.Date < startCourseDateEntry.Date)
            {
                return false;
            }
            if (!isValidString(txtInstructorNameEntry.Text))
            {
                return false;
            }
            if (!isValidString(txtInstructorPhoneEntry.Text))
            {
                return false;
            }
            if (notifications.SelectedItem == null)
            {
                return false;
            }
            if (!isValidEmail(txtInstructorEmailEntry.Text))
            {
                return false;
            }


            return true;

        }


            public async Task shareCourseNote()
        {
            //performs the share feaure
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = txtNotesEntry.Text,
                Title = "Enter your notes:"
            });
        }


        //directs to the assessment page
        private void btnAssessmentsEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentDisplayPage(selectedCourse, _termHomePage, selectedTerm));
        }

        //deletes a course entry
        private async void btnDeleteEntry_Clicked(object sender, EventArgs e)
        {
            //display warning message before delete action
            var delete = await this.DisplayAlert("Warning", "Are you sure you would like to delete?", "Yes", "No");

            if (delete)
            {

                try
                {
                    //establish connection to the database
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {
                        //deletes the selected course from the database
                        int rows = connection.Delete(selectedCourse);
                        await Navigation.PopAsync();

                        if (rows > 0)
                        {
                            await DisplayAlert("Success", "Course has been deleted", "Ok");
                        }

                        else
                        {
                            await DisplayAlert("Failed", "Please try again", "Ok");
                        }
                        //directs to the course display plage
                        await Navigation.PushAsync(new CourseDisplayPage(selectedTerm, _termHomePage, selectedCourse));
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
                }
            
        }

        //notes feature activated 
        private async void btnShareNotes_Clicked(object sender, EventArgs e)
        {
            //notes feature activated 
            await shareCourseNote();

        }
    }

}