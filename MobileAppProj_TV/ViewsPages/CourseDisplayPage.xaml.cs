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
    public partial class CourseDisplayPage : ContentPage
    {
        //use for references
        public Term selectedTerm;
        public TermHomePage termHomePage;
        public Course selectedCourse;
        public CourseDisplayPage(Term selectedTerm, TermHomePage termHomePage, Course selectedCourse)
        {


            InitializeComponent();
            Title = selectedTerm.TermTitle;
            this.termHomePage = termHomePage;
            this.selectedCourse = selectedCourse;
            this.selectedTerm = selectedTerm;
        }


        protected override void OnAppearing()
        {
            //establish connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //creates a course table
                connection.CreateTable<Course>();
                //query to find the course from a specified term using term id
                var courses = connection.Query<Course>($"SELECT * FROM Course WHERE Term = '{selectedTerm.Id}'");
                //assign the course to the course list view item source
                CourseListView.ItemsSource = courses;
            }
            base.OnAppearing();
        }

        //when a course gets selected / clicked 
        private void courseList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCourse = CourseListView.SelectedItem as Course;

            //when it is not empty, directs to the course edit page
            if (selectedCourse != null)
            {
                Navigation.PushAsync(new CourseEditPage(selectedTerm, termHomePage, selectedCourse));
            }
        }

        //redirects to the home page - terms page
        private void btnHomePage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermsViewsPages(selectedTerm, selectedCourse,termHomePage));
        }

        //redirects to the add course page
        private void btnNewCourseEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CourseAddPage(selectedCourse, termHomePage, selectedTerm));

        }
    }
}