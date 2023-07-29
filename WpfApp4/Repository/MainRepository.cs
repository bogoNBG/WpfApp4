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
                using (SQLiteCommand command = new SQLiteCommand(commandLine, connection)) 
                { 
                    command.ExecuteNonQuery(); 
                }
                
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
        public void DeleteRow(int contactId)
        {
            commandLine = $"delete from Contacts where ID={contactId};";
            ConnectToTable(commandLine);
        }
        public void UpdateRow(string name, string number, string email, int contactId)
        {
            commandLine = $"update Contacts set NAME='{name}', NUMBER='{number}', EMAIL='{email}' where ID={contactId}";
            ConnectToTable(commandLine);
        }

        public void AddOption(Option option)
        {
            commandLine = $"insert into Options (NAME) values ('{option.Name}');";
            ConnectToTable(commandLine);
        }

        public void RemoveOption(int optionId)
        {
            commandLine = $"delete from Options where ID='{optionId}'";
            ConnectToTable(commandLine);
        }

        public void AddLink(int contactId, int optionId, string linkValue)
        {
            commandLine = $"insert into Links ([CONTACT ID], [OPTION ID], NAME) VALUES ({contactId},{optionId},'{linkValue}')"; //moje da izbie
            ConnectToTable(commandLine);
        }

        public void RemoveLink(int linkId)
        {
            commandLine = $"delete from Links where ID={linkId};";
            ConnectToTable(commandLine);
        }

        public void UpdateLink(string linkValue, int linkId)
        {
            commandLine = $"update Links set NAME='{linkValue}' where ID={linkId}";
            ConnectToTable(commandLine);
        }

        public void RemoveLinksFromContact(int contactId)
        {
            commandLine = $"delete from Links where [CONTACT ID]={contactId};";
            ConnectToTable(commandLine);
        }

        public void RemoveLinksFromOptions(int optionId) //ti
        {
            commandLine = $"delete from Links where [OPTION ID]={optionId};";
            ConnectToTable(commandLine);
        }

        public List<Link> GetContactsLinksFromDB(Contact contact)
        {
            List<Link> links = new();
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand($"select * from Links where [CONTACT ID] = {contact.Id}", connection);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int contactId = reader.GetInt32(1);
                        int optionId = reader.GetInt32(2);
                        string name = reader.GetString(3);

                        links.Add(new Link(id, contactId, optionId, name));
                    }
                }
                
            }
            return links;
        }

        public List<Option> GetOptionsFromDB() //return List<Option>
        {
            List<Option> options = new();
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryOptions = "SELECT ID, NAME FROM Options;";
                SQLiteCommand commandOptions = new(queryOptions, connection);

                using (SQLiteDataReader readerOptions = commandOptions.ExecuteReader())
                {
                    while (readerOptions.Read())
                    {
                        int id = readerOptions.GetInt32(0);
                        string name = readerOptions.GetString(1);

                        options.Add(new Option(id, name));
                    }
                }
                
            }

            return options;

        }

        public Option GetOptionByIdFromDB(int optionId) //mnogo hubav primer
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryOptions = $"SELECT ID, NAME FROM Options where id = {optionId};";
                SQLiteCommand commandOptions = new(queryOptions, connection);

                using (SQLiteDataReader readerOptions = commandOptions.ExecuteReader())
                {
                    while (readerOptions.Read())
                    {
                        int id = readerOptions.GetInt32(0);
                        string name = readerOptions.GetString(1);

                        return new Option(id, name);
                    }
                }               
                return null;
            }
        }       

        public List<Contact> GetContactsFromDB()
        {
            List<Contact> contacts = new();
            using (SQLiteConnection connection = new SQLiteConnection(dbfile))
            {
                connection.Open();

                string queryContacts = "SELECT ID, NAME, NUMBER, EMAIL FROM Contacts;";

                SQLiteCommand commandContacts = new(queryContacts, connection);

                using (SQLiteDataReader readerContacts = commandContacts.ExecuteReader())
                {
                    while (readerContacts.Read())
                    {

                        int id = readerContacts.GetInt32(0);
                        string name = readerContacts.GetString(1);
                        string number = readerContacts.GetString(2);
                        string email = readerContacts.GetString(3);

                        Contact contact = new(id, name, number, email);


                        string queryLinks = $"SELECT ID, [CONTACT ID], [OPTION ID], NAME FROM Links WHERE [CONTACT ID] = {contact.Id};";
                        SQLiteCommand commandLinks = new(queryLinks, connection);
                        using (SQLiteDataReader readerLinks = commandLinks.ExecuteReader())
                        {
                            while (readerLinks.Read())
                            {
                                int linkId = readerLinks.GetInt32(0);
                                int contactId = readerLinks.GetInt32(1);
                                int optionId = readerLinks.GetInt32(2);
                                string linkName = readerLinks.GetString(3);

                                contact.Links.Add(new Link(linkId, contactId, optionId, linkName));
                            }
                        }

                        contacts.Add(contact);
                    }
                }

                return contacts;

            }
        }

    }
}
