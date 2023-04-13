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
    public partial class AssessmentAddPage : ContentPage
    {
        //use for references
        Course selectedCourse;
        TermHomePage _termHomePage;

        
        public AssessmentAddPage(Course selectedCourse, TermHomePage termHomePage)
        {
            InitializeComponent();

            this.selectedCourse = selectedCourse;
            _termHomePage = termHomePage;
            //use to bind the course associated with the particular assessment
            Title = selectedCourse.CourseTitle;
        }

       

        //validates the entry - string
        private bool isValidString(string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        //method to validate the entries - if any of the conditions are not met
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
            //checks start date cannot be later than the end date
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
        //saves the assessment
        private void btnSaveAssessmentType_Clicked(object sender, EventArgs e)
        {
            //assign the values received from the entries to assign to Assessment properties
            Assessment assessment = new Assessment()
            {
                AssessmentTitle = txtAssessmentTitleEntry.Text,
                AssessmentType = pickerAssessmentSelect.SelectedItem.ToString(),
                AssessmentStart = pickerStartDate.Date,
                AssessmentEnd = pickerDueDate.Date,
                AssessmentNotification = pickerNotificationSelect.SelectedIndex,
                Course = selectedCourse.Id
            };
            //check if all the entries are all valid
            if (isValidEntry())
            {

                try
                {
                    //establish connection to the database
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {

                        //create the Assessment table
                        connection.CreateTable<Assessment>();


                        //performs a query to count the current number of performance assessment
                        var performanceAssessCountQuery = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course = '{selectedCourse.Id}' AND AssessmentType = 'Performance Assessment'");



                        //performs a query to count the current number of objective assessment
                        var objectiveAssessCountQuery = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course = '{selectedCourse.Id}' AND AssessmentType = 'Objective Assessment'");



                        // performs a query count check if the selected performance assessment type has not been selected
                        if (assessment.AssessmentType.ToString() == "Performance Assessment" && performanceAssessCountQuery.Count == 0)
                        {
                            //insert the assessment
                            int rows = connection.Insert(assessment);
                            _termHomePage.assessments.Add(assessment);

                            if (rows > 0)
                            {
                                DisplayAlert("Success", "Assessment has been added", "Ok");
                            }

                            Navigation.PopAsync();
                        }
                        // performs a query count check if the selected objective assessment type has not been selected
                        if (assessment.AssessmentType.ToString() == "Objective Assessment" && objectiveAssessCountQuery.Count == 0)
                        {
                            //insert the assessment
                            int rows = connection.Insert(assessment);
                            _termHomePage.assessments.Add(assessment);

                            if (rows > 0)
                            {
                                DisplayAlert("Success", "Assessment has been added", "Ok");

                            }

                            Navigation.PopAsync();
                        }
                        else
                        {
                            //otherwise, throw an error display letting the user know they cannot exceed two assessments of the same type
                            DisplayAlert("Error", "You cannot have two assessments of the same type", "Ok");
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            else
            {
                //throw an error if any of the required fields are not filled out.
                DisplayAlert("Error", "Please make sure to fill out all required fields, start date cannot be later than the end date.", "Ok");

            }

        }

        //cancels the page
        private void btnCancelAssessmentType_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}