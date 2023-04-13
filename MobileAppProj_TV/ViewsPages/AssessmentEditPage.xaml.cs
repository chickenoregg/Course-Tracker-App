using MobileAppProj_TV.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppProj_TV.ViewsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentEditPaage : ContentPage
    {
        //use for references
        Assessment selectedAssessment;
        Course selectedCourse;
        //public TermHomePage _termHomePage;

        //list of assessment types and tied when the page loads
        public List<string> selections = new List<string> { "Objective Assessment", "Performance Assessment" };

        public AssessmentEditPaage(Course selectedCourse, Assessment selectedAssessment)
        {

            InitializeComponent();

            this.selectedCourse = selectedCourse;
            this.selectedAssessment = selectedAssessment;
            Title = selectedAssessment.AssessmentTitle;
        }

        //checks if the string is valid or not
        private bool isValidString(string text)
        {
            return !string.IsNullOrEmpty(text);
        }
        
        //method that checks whether all entries are valid
        private bool isValidEntry()
        {
            bool isValid = true;

            if (!isValidString(txtAssessmentTitleEntry.Text))
            {
                return false;
            }
            if (pickerAssessmentSelect.SelectedItem == null)
            {
                return false;
            }
            if (pickerDueDate.Date == null)
            {
                return false;
            }
            //checks if the date is in the correct format
            if (pickerDueDate.Date < pickerStartDate.Date)
            {
                return false;
            }
            if (pickerNotificationSelect.SelectedItem == null)
            {
                return false;
            }
            
            return isValid;
        }

        //updates the assessment
        private void btnUpdateAssessmentType_Clicked(object sender, EventArgs e)
        {
            //if all conditions are met
            if (isValidEntry())
            {
                bool isUpdated = false;

                if (selectedAssessment.AssessmentType.ToString() != pickerAssessmentSelect.SelectedItem.ToString())
                {
                    isUpdated = true;
                }

                selectedAssessment.AssessmentTitle = txtAssessmentTitleEntry.Text;
                selectedAssessment.AssessmentStart = pickerStartDate.Date;
                selectedAssessment.AssessmentEnd = pickerDueDate.Date;
                selectedAssessment.AssessmentNotification = pickerNotificationSelect.SelectedIndex;

                //establish connection to the database

                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    //perform a query to find the course and assessment with performance assessment
                    var performanceAssessCount = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course = '{selectedCourse.Id}' AND AssessmentType = 'Performance Assessment'");

                    //perform a query to find the course and assessment with objective assessment

                    var objectiveAssessCount = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course = '{selectedCourse.Id}' AND AssessmentType = 'Objective Assessment'");

                    //check is the assessment matches with performance assessment and the count is at 0
                    if (selectedAssessment.AssessmentType.ToString() == "Performance Assessment" && performanceAssessCount.Count == 0)
                    {
                        //assign the assessment as the type
                        selectedAssessment.AssessmentType = pickerAssessmentSelect.SelectedItem.ToString();
                        connection.Update(selectedAssessment);
                        Navigation.PopAsync();
                    }
                    //otherwise, perform task but with objective assessment
                    else if (selectedAssessment.AssessmentType.ToString() == "Objective Assessment" && objectiveAssessCount.Count == 0)
                    {
                        selectedAssessment.AssessmentType = pickerNotificationSelect.SelectedItem.ToString();
                        connection.Update(selectedAssessment);
                        Navigation.PopAsync();
                    }

                    //check if both assessments have been already assigned
                    else if (((selectedAssessment.AssessmentType.ToString() == "Performance Assessment" && performanceAssessCount.Count == 1) ||
                             (selectedAssessment.AssessmentType.ToString() == "Objective Assessment" && objectiveAssessCount.Count == 1)) &&
                             !(String.IsNullOrEmpty(selectedAssessment.Id.ToString())) &&
                              !isUpdated)
                    {
                        //update the assessment
                        connection.Update(selectedAssessment);
                        DisplayAlert("Message", "Assessment has been updated", "Ok");
                        Navigation.PopAsync();
                    }

                    else
                    {
                        //otherwise, display an error message letting the user know that only assessment type is allowerd per course
                        DisplayAlert("Error", "You can only have one assessment type per course.", "Ok");
                    }
                }
            }
            else
            {
                //displays an error message to let the user know all fields are required
                DisplayAlert("Error", "Please make sure to fill out all required fields, start date cannot be later than the end date.", "Ok");
            }
            
        }
        //data appears on the form upon loading
        protected override void OnAppearing()
        {

            txtAssessmentTitleEntry.Text = selectedAssessment.AssessmentTitle;
            pickerAssessmentSelect.ItemsSource = selections;
            pickerAssessmentSelect.SelectedIndex = selections.FindIndex(assessmentStat => assessmentStat == selectedAssessment.AssessmentType);
            pickerStartDate.Date = selectedAssessment.AssessmentStart.Date;
            pickerDueDate.Date = selectedAssessment.AssessmentEnd.Date;
            // this checks whether notification is yes or no
            if (selectedAssessment.AssessmentNotification == 0)
            {
                //no
                pickerNotificationSelect.SelectedIndex = 0;

            }
            else
            {
                //yes
                pickerNotificationSelect.SelectedIndex = 1;

            }
            base.OnAppearing();


        }


        //deletes the assesment
        private async void btnDeleteAssessment_Clicked(object sender, EventArgs e)
        {
            //display a warning message whether to continue with the delete action or not
            var message = await this.DisplayAlert("Warning", "Are you sure you want to delete?", "Yes", "Cancel");

            if (message)
            {
                //if true, establish connection to the database
                using(SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    //deletes the assessment selected
                    connection.Delete(selectedAssessment);

                    await Navigation.PopAsync();
                }
            }
        }
        //cancels the assessment page and redirects to the previous page
        private void btnCancelAssessment_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        
    }
}