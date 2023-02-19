using Npgsql;

namespace Contacts
{
    public class QueryContacts
    {
        public static List<Contact> GetContacts(NpgsqlConnection connection)
        {
            List<Contact> contacts = new List<Contact>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT ContactID, Email, CellNumber, AlternativeNumber FROM Contacts ",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            ContactID = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            CellNumber = reader.GetString(2),
                            AlternativeNumber = reader.GetString(3)

                        });
                    }
                }
            }
            connection.Close();

            return contacts;
        }




        public static List<Contact> GetContactById(NpgsqlConnection connection, int ContactID)
        {
            List<Contact> contacts = new List<Contact>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT ContactID, Email, CellNumber, AlternativeNumber FROM Contacts WHERE ContactID = @ContactID",
                connection))
            {
                command.Parameters.AddWithValue("ContactID", ContactID);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            ContactID = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            CellNumber = reader.GetString(2),
                            AlternativeNumber = reader.GetString(3)

                        });
                    }
                }
            }
            connection.Close();

            return contacts;
        }



        public static Contact[] AddContacts(NpgsqlConnection connection, string Email, string CellNumber, string AlternativeNumber)
        {
            List<Contact> contacts = new();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "INSERT INTO Contacts (Email, CellNumber, AlternativeNumber) VALUES (@Email, @CellNumber, @AlternativeNumber)",
                connection))
            {
                command.Parameters.AddWithValue("Email", Email);
                command.Parameters.AddWithValue("CellNumber", CellNumber);
                command.Parameters.AddWithValue("AlternativeNumber", AlternativeNumber);
                command.ExecuteNonQuery();
                Contact contact = new Contact();
             
                contact.CellNumber = CellNumber;
                contacts.Add(contact);
            }
            connection.Close();

            return contacts.ToArray();
        }



        public static bool DeleteContact(NpgsqlConnection connection, int ContactID)
        {
            try
            {

                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Contacts WHERE ContactID = @ContactID", connection))
                {
                    command.Parameters.AddWithValue("ContactID", ContactID);

                    int affectedRows = command.ExecuteNonQuery();

                    return affectedRows > 0;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally { connection.Close(); }

        }







    }
}
