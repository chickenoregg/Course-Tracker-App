using MobileAppProj_TV.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//edit form
namespace MobileAppProj_TV.ViewsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermDisplayPage : ContentPage
    {
        Term selectedTerm;

        //takes in selectedTerm
        public TermDisplayPage(Term selectedTerm)
        {
            
            InitializeComponent();
            this.selectedTerm = selectedTerm;

            //loads the data upon loading the page
            txtUpdateTermEntry.Text = selectedTerm.TermTitle;
            termUpdateStartEntry.Date = selectedTerm.TermStart.Date;
            termUpdateEndEntry.Date = selectedTerm.TermEnd.Date;
        }
 
        //validates the string
        private bool isValidString(string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        //method to validate entries
        private bool isValidEntry()
        {
            bool isValid = true;

            if (!isValidString(txtUpdateTermEntry.Text))
            {
                return false;
            }
            if (termUpdateStartEntry.Date == null)
            {
                return false;
            }
            if (termUpdateEndEntry.Date == null)
            {
                return false;
            }
            //checks if start date cannot be later than the end date
            if (termUpdateEndEntry.Date < termUpdateStartEntry.Date)
            {
                return false;
            }


            return isValid;

        }


        //updates the entry
        private async void btnUpdateTermEntry_Clicked(object sender, EventArgs e)
        {
            //input received from the textboxes are assigned to term
            selectedTerm.TermTitle = txtUpdateTermEntry.Text;
            selectedTerm.TermStart = termUpdateStartEntry.Date;
            selectedTerm.TermEnd = termUpdateEndEntry.Date;

    

            //checks the validity of the entries
            if (isValidEntry())
            {

                //establish connection
                using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
                {

                    //check if table exists if not it ignores and updates table
                    //connection.CreateTable<Term>();

                    //update term
                    int rows = connection.Update(selectedTerm);

                    //throw a message when succesful
                    if (rows > 0)
                    {
                        await DisplayAlert("Success", "Term has been updated", "Ok");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        //otherwise, display a error message to try and connect to the database
                         await DisplayAlert("Failed", "Try again", "Ok");
                    }
                   
                    

                }
            }

            else
            {
                //otherwise, throws an error display when condiitions are not met
                 await DisplayAlert("Error", "Please make sure all fields are filled out, start date cannot be later than the end date", "Ok");
                
            }


        }

  
        //cancels the term edit page and redirects to the previous page
        private void btnCancelTermEditPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}