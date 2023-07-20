using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp4.Model;
using WpfApp4.ViewModel;

namespace WpfApp4.Repository
{
    class MainRepository
    {
        public readonly string dbfile = "URI=file:SQLiteDB.db";
        public string commandLine;

        void ConnectToTable(string commandLine)
        {
            using SQLiteConnection connection = new SQLiteConnection(dbfile);
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(commandLine, connection);
                command.ExecuteNonQuery();
            }
        }
        public void CreateTable()
        {
            commandLine = "create table Contacts (ID integer, NAME text, NUMBER text, EMAIL text);";
            ConnectToTable(commandLine);
        }

        public void AddRow(Contact contact)
        {
            commandLine = $"insert into Contacts (ID,NAME,NUMBER,EMAIL) values ({contact.Id},'{contact.Name}','{contact.Number}','{contact.Email}');";
            ConnectToTable(commandLine);
        }
        public void DeleteRow(ContactViewModel contact)
        {
            commandLine = $"delete from Contacts where ID={contact.Id};";
            ConnectToTable(commandLine);

        }

        public void LoadInfo(ObservableCollection<ContactViewModel> contacts)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryContacts = "SELECT ID, NAME, NUMBER, EMAIL FROM Contacts;";

                SQLiteCommand commandContacts = new(queryContacts, connection);

                SQLiteDataReader readerContacts = commandContacts.ExecuteReader();
                while (readerContacts.Read())
                {

                    int id = readerContacts.GetInt32(0);
                    string name = readerContacts.GetString(1);
                    string number = readerContacts.GetString(2);
                    string email = readerContacts.GetString(3);

                    Contact contact = new Contact(id, name, number, email);


                    //string queryLinks = $"SELECT ID, [CONTACT ID], [OPTION ID], LINK FROM Links WHERE [CONTACT ID] = {contact.Id};";
                    //SQLiteCommand commandLinks = new(queryLinks, connection);
                    //using SQLiteDataReader readerLinks = commandLinks.ExecuteReader();
                    //while (readerLinks.Read())
                    //{
                    //    int key = readerLinks.GetInt32(2);

                    //    if (!contact.Links.ContainsKey(key))
                    //    {
                    //        contact.Links[key] = new List<string>();
                    //    }

                    //    contact.Links[key].Add(readerLinks.GetString(3));
                    //}

                    contacts.Add(new ContactViewModel(contact));
                }
            }
        }
    }
}
