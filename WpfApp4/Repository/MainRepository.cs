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
            commandLine = "create table Links (ID integer, [CONTACT ID] integer, [OPTION ID] integer, NAME text);";
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
        public void UpdateRow(ContactViewModel contact)
        {
            commandLine = $"update Contacts set NAME='{contact.Name}', NUMBER='{contact.Number}', EMAIL='{contact.Email}' where ID={contact.Id}";
            ConnectToTable(commandLine);
        }

        public void AddOption(Option option)
        {
            commandLine = $"insert into Options (ID, NAME) values ({option.Id}, '{option.Name}');";
            ConnectToTable(commandLine);
        }

        public void RemoveOption(Option option)
        {
            commandLine = $"delete from Options where ID='{option.Id}'";
            ConnectToTable(commandLine);
        }

        public void AddLink(Link link)
        {
            commandLine = $"insert into Links (ID, [CONTACT ID], [OPTION ID], NAME) VALUES ({link.Id}, {link.ContactId}, {link.OptionId}, '{link.Name}')";
            ConnectToTable(commandLine);
        }

        public void RemoveLink(Link link)
        {
            commandLine = $"delete from Links where ID={link.Id};";
            ConnectToTable(commandLine);
        }

        public void UpdateLink(Link link)
        {
            commandLine = $"update Links set NAME='{link.Name}' where ID={link.Id}";
            ConnectToTable(commandLine);
        }

        public void RemoveLinksFromContact(ContactViewModel contact)
        {
            commandLine = $"delete from Links where [CONTACT ID]={contact.Id};";
            ConnectToTable(commandLine);
        }

        public void RemoveLinksFromOptions(Option option)
        {
            commandLine = $"delete from Links where [OPTION ID]={option.Id};";
            ConnectToTable(commandLine);
        }

        public void LoadInfo(ObservableCollection<ContactViewModel> contacts, ObservableCollection<Option> options)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryContacts = "SELECT ID, NAME, NUMBER, EMAIL FROM Contacts;";
                string queryOptions = "SELECT ID, NAME FROM Options;";

                SQLiteCommand commandContacts = new(queryContacts, connection);
                SQLiteCommand commandOptions = new(queryOptions, connection);

                SQLiteDataReader readerOptions = commandOptions.ExecuteReader();
                while (readerOptions.Read())
                {
                    int id = readerOptions.GetInt32(0);
                    string name = readerOptions.GetString(1);
                    Option option = new Option(id, name);
                    options.Add(option);
                }


                SQLiteDataReader readerContacts = commandContacts.ExecuteReader();
                while (readerContacts.Read())
                {

                    int id = readerContacts.GetInt32(0);
                    string name = readerContacts.GetString(1);
                    string number = readerContacts.GetString(2);
                    string email = readerContacts.GetString(3);

                    Contact contact = new Contact(id, name, number, email);


                    string queryLinks = $"SELECT ID, [CONTACT ID], [OPTION ID], NAME FROM Links WHERE [CONTACT ID] = {contact.Id};";
                    SQLiteCommand commandLinks = new(queryLinks, connection);
                    using SQLiteDataReader readerLinks = commandLinks.ExecuteReader();
                    while (readerLinks.Read())
                    {
                        int linkId = readerLinks.GetInt32(0);
                        int contactId = readerLinks.GetInt32(1);
                        int optionId = readerLinks.GetInt32(2);
                        string linkName = readerLinks.GetString(3);

                        contact.Links.Add(new Link(linkId, contactId, optionId, linkName));
                    }

                    contacts.Add(new ContactViewModel(contact));
                }
            }
        }

        public int IdNum(string table)
        {

            using SQLiteConnection connection = new(dbfile);
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand($"SELECT ID FROM {table}", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    List<int> ids = new List<int>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        ids.Add(id);
                    }

                    for (int i = 1; i <= ids.Count + 1; i++)
                    {
                        if (!ids.Contains(i))
                        {
                            return i;
                        }
                    }

                    return ids.Count + 1;
                }
            }
        }
    }
}
