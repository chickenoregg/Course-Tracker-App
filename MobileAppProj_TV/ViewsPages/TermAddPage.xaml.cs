using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileAppProj_TV.Models;
using SQLite;

namespace MobileAppProj_TV.ViewsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermAddPage : ContentPage
    {

        public TermAddPage()
        {
            InitializeComponent();
        }



        private bool isValidString(string text)
        {
            return !string.IsNullOrEmpty(text);
        }
        //method to check validity of entries
        private bool isValidEntry()
        {

            if (!isValidString(txtNewTermEntry.Text))
            {
                return false;
            }
            if (txtNewTermStartDate.Date == null)
            {
                return false;
            }
            if (txtNewTermEndDate.Date == null)
            {
                return false;
            }
            if (txtNewTermEndDate.Date < txtNewTermStartDate.Date)
            {
                return false;
            }


            return true;

         }


        //saves the entries
        private void btnSaveTermEntry_Clicked(object sender, EventArgs e)
        {
            //assigning the value received to the properties of Term class
            Term term = new Term()
            {
                TermTitle = txtNewTermEntry.Text,
                TermStart = txtNewTermStartDate.Date,
                TermEnd = txtNewTermEndDate.Date
            };

            if (isValidEntry())
            {
                
                //connects to the database and closes automatically via using statement
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {
                    connection.CreateTable<Term>();

                    //each term on the table
                    int rows = connection.Insert(term);

                    //checks if term has been successfully inserted to the table
                    if (rows > 0)
                    {
                        DisplayAlert("Success", "New term has been saved", "Ok");
                        Navigation.PopAsync();
                    }
     
                }
            }
            else
            {
                //otherwise, throws an error display when condiitions are not met
                DisplayAlert("Error", "Please make sure all fields are filled out, start date cannot be later than the end date", "Ok");
            }
            
        }

        //cancels the add term page
        private void btnCancelTermEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}