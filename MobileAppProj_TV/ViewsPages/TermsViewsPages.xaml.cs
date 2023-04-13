using MobileAppProj_TV.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//term details

namespace MobileAppProj_TV.ViewsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsViewsPages : ContentPage
    {
        Term selectedTerm;
        Course selectedCourse;
        TermHomePage termHomePage;
        
        public TermsViewsPages(Term selectedTerm, Course selectedCourse, TermHomePage termHomePage)
        {
            
            
            InitializeComponent();
            this.selectedTerm = selectedTerm;
            this.selectedCourse = selectedCourse;
            this.termHomePage = termHomePage;
            //use to bind term
            Title = selectedTerm.TermTitle;
        }


        //directs to the new course page
        private void btnNewCourseEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermDisplayPage(selectedTerm));
        }

     
        //deletes the selected term
        private async void btnDeleteTermEntry_Clicked(object sender, EventArgs e)
        {
            //display a warning message to the user whether continue with the delete operation or not
            var delete = await this.DisplayAlert("Reminder", "Are you sure you want to delete?", "Yes", "No");

            if (delete)
            {
                //displays a message
                
                try
                {
                    //establish connection to the databse
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {
                        //creates a term table
                        connection.CreateTable<Term>();

                        //each post on the table
                        int rows = connection.Delete(selectedTerm);

                        //checks if term has been successfully deleted
                        if (rows > 0)
                        {
                            await DisplayAlert("Success", "New term has been deleted", "Ok");

                        }
                        else
                        {
                            await DisplayAlert("Failed", "Please try again", "Ok");
                        }
                        await Navigation.PopAsync();

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        protected override void OnAppearing()
        {
            //the start and end appears upon opening the page
            termStart.Text = selectedTerm.TermStart.ToString("dddd, dd MMMMM yyyy");
            termEnd.Text = selectedTerm.TermEnd.ToString("dddd, dd MMMMM yyyy");
            //base.OnAppearing();


            //establish connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {

                //create the course table
                connection.CreateTable<Course>();
                //return term table query
                
                //performs a query to find the course from specied term
                var coursesPerTerm = connection.Query<Course>($"SELECT * FROM Course WHERE Term = '{selectedTerm.Id}'");

                //assigns the found item to the list
                coursesList.ItemsSource = coursesPerTerm;
            }

        }


        //directs to the course add page
        private void btnNewCourseEntry_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CourseAddPage(selectedCourse, termHomePage, selectedTerm));
        }

        //directs to the update term page
        private void btnUpdateTermEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermDisplayPage(selectedTerm));;
        }


        //directs to TermViewsPage when an item is selected
        private void coursesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var selectedCourses = coursesList.SelectedItem as Course;

            if (selectedCourses != null)
            {
                Navigation.PushAsync(new CourseDisplayPage(selectedTerm, termHomePage, selectedCourses));
            }
        }
        //directs to the home page
        //private void btnHomePage_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PopToRootAsync();
        //}


    }
}