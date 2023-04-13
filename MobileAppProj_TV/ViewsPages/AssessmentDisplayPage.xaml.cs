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
    public partial class AssessmentDisplayPage : ContentPage
    {

        //use for references
        Course selectedCourse;
        TermHomePage _termHomePage;
        Term selectedTerm;


        public AssessmentDisplayPage(Course selectedCourse, TermHomePage termHomePage, Term selectedTerm )
        {
            

            InitializeComponent();
            this.selectedCourse = selectedCourse;
            _termHomePage = termHomePage;
            this.selectedTerm = selectedTerm;
            Title = selectedCourse.CourseTitle;
        }


        protected override void OnAppearing()
        {

            //the start and end appears upon opening the page

            CourseStart.Text = selectedCourse.CourseStart.ToString("dddd, dd MMMMM yyyy");
            CourseEnd.Text = selectedCourse.CourseEnd.ToString("dddd, dd MMMMM yyyy");

            //establish connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //perform a query to find the course with a particular id
                connection.CreateTable<Assessment>();
                var assessmentPerAddedCourse = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course = '{selectedCourse.Id}'");
                //assign as the source
                AssessmentsList.ItemsSource = assessmentPerAddedCourse;
            }
            base.OnAppearing();
        }
   
        //checks if there are more than two assessments 
        private void btnNewAssessmentEntry_Clicked(object sender, EventArgs e)
        {
          // more than two assessments is not allowed
            if (countMaxAssessment() < 2)
            {
                Navigation.PushAsync(new AssessmentAddPage(selectedCourse, _termHomePage));

            }
            else
            {
                //otherwise, display an error message
                DisplayAlert("Error","Only two assessments per course are allowed", "Ok");
            }

        }

 
        //method to count the number of assessments stored in the database
        private int countMaxAssessment()
        {
            int counterAssessment = 0;

            //establish connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //perform a query 
                var counter = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course = '{selectedCourse.Id}'");
                counterAssessment = counter.Count;
            }
            return counterAssessment;
        }

        //allows the assessment to be selected
        private void AssessmentsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //assign the selected assessment 
            var selectedAssessment = AssessmentsList.SelectedItem as Assessment;

            if (selectedAssessment != null)
            {
                Navigation.PushAsync(new AssessmentEditPaage(selectedCourse,selectedAssessment));
            }
        }

        //redirects to view the courses
        private void btnViewCourses_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CourseDisplayPage(selectedTerm, _termHomePage, selectedCourse));
        }



        //private void btnCoursePage_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new TermDisplayPage(term, _termHomePage));
        //}
    }
}