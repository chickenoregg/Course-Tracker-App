using MobileAppProj_TV.Models;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


//dashboard term
namespace MobileAppProj_TV.ViewsPages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class TermHomePage : ContentPage
    {
        //for references
        TermHomePage termHomePage;
        Course selectedCourse;
        Term selectedTerm;
        //evaluation purposes
        //list for references 
        public List<Course> courses = new List<Course>();
        public List<Term> terms = new List<Term>();
        public List<Assessment> assessments = new List<Assessment>();

        bool isEvaluationRunOnce = true;


        public TermHomePage()
        {
            InitializeComponent();

            //display name at log-in
            MyName.Text = $"Welcome, {Application.Current.Properties["Name"].ToString()}";
            termHomePage = this;
            //DisplayEvaluationSample(1);

        }


        protected override void OnAppearing()
        {

            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //create the table
                //connection.CreateTable<Course>;

                connection.CreateTable<Term>();
                //connection.CreateTable<Course>;
                connection.CreateTable<Assessment>();
                //return term table query

                //check quick watch to see if entries are in the database
                terms = connection.Table<Term>().ToList();
                //termsList.ItemsSource = terms;

            }

            if (isEvaluationRunOnce)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                   
                    connection.CreateTable<Term>();
                    connection.CreateTable<Course>();
                    connection.CreateTable<Assessment>();

                    DisplayEvaluationSample(1);


                }

                //checks whether the dummy data has been ran once
                isEvaluationRunOnce = false;
                //loops through the Term to pull course and assessment to be set for notification
                foreach (Term term in terms)
                {

                    //establish connection to the database
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {
                        //assigns the value to courseItem
                        var courseItem = connection.Query<Course>($"SELECT * FROM Course WHERE Term == '{term.Id}'");

                        foreach (Course course in courseItem)
                        {
                            // checks whether the date start matches today's date and if notification is "yes" - trigger the notification for course
                            if (course.CourseStart == DateTime.Today && course.CourseNotification == 1)
                                CrossLocalNotifications.Current.Show("Reminder: Your course ", $"{course.CourseTitle} starts today { DateTime.Today}");

                            // checks whether the date end matches today's date and if notification is "yes" - trigger the notification for course
                            if (course.CourseEnd == DateTime.Today && course.CourseNotification == 1)
                                CrossLocalNotifications.Current.Show("Reminder: Your course", $"{course.CourseTitle} ends today {DateTime.Today}");


                            //assigns the value to assessmentItem
                            var assessmentItem = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course == '{course.Id}'");

                            foreach (Assessment assessment in assessmentItem)
                            {
                                // checks whether the date start matches today's date and if notification is "yes" - trigger the notification for assessment
                                if (assessment.AssessmentStart == DateTime.Today && assessment.AssessmentNotification == 1)
                                    CrossLocalNotifications.Current.Show("Reminder: Your assessment", $"{assessment.AssessmentTitle} starts today {DateTime.Today}"/*, assessmentId*/);


                                // checks whether the date start matches today's date and if notification is "yes" - trigger the notification for assessment
                                if (assessment.AssessmentEnd == DateTime.Today && assessment.AssessmentNotification == 1)
                                    CrossLocalNotifications.Current.Show("Reminder: Your assessment", $"{assessment.AssessmentTitle} ends today {DateTime.Today}"/*, assessmentId*/);

                            }

                        }

                    }

                }


            }
            else if (isEvaluationRunOnce)
            {
                DisplayEvaluationSample(1);
                isEvaluationRunOnce = false;
                //loops through the Term to pull course and assessment to be set for notification
                foreach (Term term in terms)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                    {
                        //assigns the value to courseItem
                        var courseItem = connection.Query<Course>($"SELECT * FROM Course WHERE Term == '{term.Id}'");

                        foreach (Course course in courseItem)
                        {
                            // checks whether the date start matches today's date and if notification is "yes" - trigger the notification for course
                            if (course.CourseStart == DateTime.Today && course.CourseNotification == 1)
                                CrossLocalNotifications.Current.Show("Reminder: Your course ", $"{course.CourseTitle} starts today { DateTime.Today}");

                            // checks whether the date end matches today's date and if notification is "yes" - trigger the notification for course
                            if (course.CourseEnd == DateTime.Today && course.CourseNotification == 1)
                                CrossLocalNotifications.Current.Show("Reminder: Your course", $"{course.CourseTitle} ends today {DateTime.Today}");


                            //assigns the value to assessmentItem
                            var assessmentItem = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE Course == '{course.Id}'");

                            foreach (Assessment assessment in assessmentItem)
                            {
                                // checks whether the date start matches today's date and if notification is "yes" - trigger the notification for assessment
                                if (assessment.AssessmentStart == DateTime.Today && assessment.AssessmentNotification == 1)
                                    CrossLocalNotifications.Current.Show("Reminder: Your assessment", $"{assessment.AssessmentTitle} starts today {DateTime.Today}"/*, assessmentId*/);


                                // checks whether the date start matches today's date and if notification is "yes" - trigger the notification for assessment
                                if (assessment.AssessmentEnd == DateTime.Today && assessment.AssessmentNotification == 1)
                                    CrossLocalNotifications.Current.Show("Reminder: Your assessment", $"{assessment.AssessmentTitle} ends today {DateTime.Today}"/*, assessmentId*/);

                            }

                        }

                    }

                }

            }

            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //assigns the terms as the item source to the terms list
                terms = connection.Table<Term>().ToList();
                termsList.ItemsSource = terms;
            }
            base.OnAppearing();

        }


        
        //redirects to TermViewsPage when an item is clicked
        private void termsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedTerm = termsList.SelectedItem as Term;

            //checks if the term list is not empty
            if (selectedTerm != null)
            {
                Navigation.PushAsync(new TermsViewsPages(selectedTerm, selectedCourse, termHomePage));
            }
        }

        //add new term from homepage
        private void btnAddNewTerm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermAddPage());

        }

        //cancels the homepage and redirects to the log in page
        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


        //dummy data for Term
        private void DisplayEvaluationSample(int term)
        {
            Term dummyTerm = new Term()
            {
                TermTitle = "DummyTerm" + term.ToString(),
                TermStart = new DateTime(2022, 09, 01),
                TermEnd = new DateTime(2023, 02, 28)
            };

            //establish connection to the daabase
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //insert the data for dummyTerm
                connection.Insert(dummyTerm);
            }

            //dummy data for Course
            Course dummyCourse = new Course()
            {
                Term = dummyTerm.Id,
                CourseTitle = "Software I C#",
                CourseStatus = "Completed",
                CourseStart = new DateTime(2022, 09, 01),
                CourseEnd = new DateTime(2023, 01, 26),
                InstructorName = "Thea Villanueva",
                InstructorEmail = "tvilla5@wgu.edu",
                InstructorPhone = "206-123-4567",
                CourseNotes = "This project is great!",
                CourseNotification = 1,

            };
            //establish connection to the daabase

            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //insert the data for the dummyCourse
                connection.Insert(dummyCourse);
            }


            //dummy data for Performance Assessment
            Assessment dummyPAssement = new Assessment()
            {
                AssessmentTitle = "BFM1",
                AssessmentStart = new DateTime(2022, 09, 01),
                AssessmentEnd = new DateTime(2022, 11, 30),
                AssessmentType = "Performance Assessment",
                Course = dummyCourse.Id,
                AssessmentNotification = 0
            };

            //establish connection to the daabase

            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //insert the data for the performance assessment
                connection.Insert(dummyPAssement);
            }

            //dummy data for Objective Assessment
            Assessment dummyOAssessment = new Assessment()
            {
                AssessmentTitle = "ABC2",
                AssessmentStart = new DateTime(2022, 09, 01),
                AssessmentEnd = new DateTime(2022, 12, 15),
                AssessmentType = "Objective Assessment",
                Course = dummyCourse.Id,
                AssessmentNotification = 0
            };

            //establish connection with the database
            using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
            {
                //insert the data for the objective asssessment
                connection.Insert(dummyOAssessment);
            }

           
        }
    }

}