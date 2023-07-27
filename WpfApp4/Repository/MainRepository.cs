using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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

        bool Check(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name=@TableName;";
                using (SQLiteCommand command = new SQLiteCommand(checkTableQuery, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public void CreateTables()
        {

            if ( !(Check("Contacts") && Check("Options") && Check("Links")) )
            {
                commandLine = "create table Contacts (ID integer, NAME text, NUMBER text, EMAIL text);";
                ConnectToTable(commandLine);
                commandLine = "create table Options (ID integer, NAME text);";
                ConnectToTable(commandLine);
                commandLine = "create table Links (ID integer, [CONTACT ID] integer, [OPTION ID] integer, NAME text);";
                ConnectToTable(commandLine);
            }
        }

        public void AddRow(Contact contact)
        {
            commandLine = $"insert into Contacts (NAME,NUMBER,EMAIL) values ('{contact.Name}','{contact.Number}','{contact.Email}');";
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
            commandLine = $"insert into Options (NAME) values ('{option.Name}');";
            ConnectToTable(commandLine);
        }

        public void RemoveOption(OptionViewModel option)
        {
            commandLine = $"delete from Options where ID='{option.Id}'";
            ConnectToTable(commandLine);
        }

        public void AddLink(Link link)
        {
            commandLine = $"insert into Links ([CONTACT ID], [OPTION ID], NAME) VALUES ({link.ContactId}, {link.OptionId}, '{link.Value}')";
            ConnectToTable(commandLine);
        }

        public void RemoveLink(LinkViewModel link)
        {
            commandLine = $"delete from Links where ID={link.Id};";
            ConnectToTable(commandLine);
        }

        public void UpdateLink(LinkViewModel link)
        {
            commandLine = $"update Links set NAME='{link.Value}' where ID={link.Id}";
            ConnectToTable(commandLine);
        }

        public void GetContactsLinksFromDB(Contact contact, ObservableCollection<LinkViewModel> links)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand($"select * from Links where [CONTACT ID] = {contact.Id}", connection);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int contactId = reader.GetInt32(1);
                    int optionId = reader.GetInt32(2);
                    string name = reader.GetString(3);

                    Link link = new Link(id, contactId, optionId, name);
                    LinkViewModel link2 = new(link, this);
                    links.Add(link2);
                    contact.Links.Add(link);//dsadsa
                    
                }
            }
        }

        public void GetOptionsFromDB(ObservableCollection<OptionViewModel> options)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryOptions = "SELECT ID, NAME FROM Options;";
                SQLiteCommand commandOptions = new(queryOptions, connection);

                SQLiteDataReader readerOptions = commandOptions.ExecuteReader();
                while (readerOptions.Read())
                {
                    int id = readerOptions.GetInt32(0);
                    string name = readerOptions.GetString(1);

                    Option option = new(id, name);
                    options.Add(new OptionViewModel(option));
                }
            }
        }

        public Option GetOptionByIdFromDB(int optionId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryOptions = $"SELECT ID, NAME FROM Options where id = {optionId};";
                SQLiteCommand commandOptions = new(queryOptions, connection);

                SQLiteDataReader readerOptions = commandOptions.ExecuteReader();
                while (readerOptions.Read())
                {
                    int id = readerOptions.GetInt32(0);
                    string name = readerOptions.GetString(1);

                    return new(id, name);
                }

                return null;
            }
        }

        public void GetContactsFromDB(ObservableCollection<ContactViewModel> contacts)
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
                    contacts.Add(new ContactViewModel(contact, this));
                }
            }
        }

        public void RemoveLinksFromContact(ContactViewModel contact)
        {
            commandLine = $"delete from Links where [CONTACT ID]={contact.Id};";
            ConnectToTable(commandLine);
        }

        public void RemoveLinksFromOptions(OptionViewModel option)
        {
            commandLine = $"delete from Links where [OPTION ID]={option.Id};";
            ConnectToTable(commandLine);
        }



        public void LoadInfo(ObservableCollection<ContactViewModel> contacts, ObservableCollection<OptionViewModel> options)
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
                    options.Add(new OptionViewModel(option));
                }


                SQLiteDataReader readerContacts = commandContacts.ExecuteReader();
                while (readerContacts.Read())
                {

                    int id = readerContacts.GetInt32(0);
                    string name = readerContacts.GetString(1);
                    string number = readerContacts.GetString(2);
                    string email = readerContacts.GetString(3);

                    Contact contact = new(id, name, number, email);


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

                    contacts.Add(new ContactViewModel(contact, this));
                }
            }
        }       
    }
}
