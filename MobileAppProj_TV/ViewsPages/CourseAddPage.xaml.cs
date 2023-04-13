using MobileAppProj_TV.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppProj_TV.ViewsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseAddPage : ContentPage
    {
        //use for references
        Course selectedCourse;
        Term selectedTerm;
        TermHomePage termHomePage;

        public CourseAddPage(Course selectedCourse, TermHomePage termHomePage, Term selectedTerm)
        {
            

            InitializeComponent();
            this.selectedCourse = selectedCourse;
            this.selectedTerm = selectedTerm;
            this.termHomePage = termHomePage;
        }



        private void btnSaveEntry_Clicked(object sender, EventArgs e)
        {
            //assign the values received from the entries to assign to Course properties
            Course course = new Course()
            {
                CourseTitle = txtCourseTitleEntry.Text,
                CourseStatus = courseStatusEntry.SelectedItem.ToString(),
                CourseStart = startCourseDateEntry.Date,
                CourseEnd = endCourseDateEntry.Date,
                InstructorName = txtInstructorNameEntry.Text,
                InstructorPhone = txtInstructorPhoneEntry.Text,
                InstructorEmail = txtInstructorEmailEntry.Text,
                CourseNotes = txtNotesEntry.Text,
                CourseNotification = notifications.SelectedIndex,
                Term = selectedTerm.Id
            };

           //checks the validity of the entries
            if (isValidEntry())
            {

                try
                {
                    //connects to the database
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {
                        connection.CreateTable<Course>();

                        //insert each course on the table
                        int rows = connection.Insert(course);
                        termHomePage.courses.Add(course);

                        //display message if successful
                        if (rows > 0)
                        {
                             DisplayAlert("Success", "New course has been saved", "Ok");
                             Navigation.PopAsync();
                        }
                        else
                        {
                            //otherwise, throw an error message
                              DisplayAlert("Error", "No connection", "Ok");
                        }
                        //directs to the term details page
                         Navigation.PopAsync();
                         //await Navigation.PushAsync(new CourseDisplayPage(selectedTerm, termHomePage, selectedCourse));

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            else
            {
                //throws an error message letting the user know that all fields are required, email and dates are in the correct format
                  DisplayAlert("Error", "Please make sure to fill out all required fields, start date cannot be later than the end date, email should be in the correct format.", "Ok");
            }


        }

        //validates the entry - string
        private bool isValidString(string text)
        {
            return !string.IsNullOrEmpty(text);
        }


        //validates the email format
        public bool isValidEmail(string text)
        {
            try
            {
                //creates an instance of Mailaddress that takes in a string
                MailAddress mailAddress = new MailAddress(text);
                return true;
            }
            catch (Exception)
            {
                //throws an error message when email format is incorrect
                DisplayAlert("Invalid email", "Check your email format", "Ok");
                return false;
            }
        }

        //checks the validity of entries whether conditions are met
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
       
 
        //cancels the page and redirects to the previous page
        private void btnCancelEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}