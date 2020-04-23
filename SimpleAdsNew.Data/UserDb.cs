using System.Data.SqlClient;

namespace SimpleAdsNew.Data
{
    public class UserDb
    {
        private string _connectionString;

        public UserDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Users (Name, Email, PasswordHash) " +
                                  "VALUES (@name, @email, @hash) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@hash", hash);
                connection.Open();
                user.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user; //success!!
            }

            return null;
        }

        public User GetByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
                cmd.Parameters.AddWithValue("@email", email);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) //no user with current email found...
                {
                    return null;
                }

                return new User
                {
                    Id = (int)reader["Id"],
                    Email = (string)reader["Email"],
                    Name = (string)reader["Name"],
                    PasswordHash = (string)reader["PasswordHash"]
                };
            }
        }

        public bool IsEmailAvailable(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE email = @email";
                cmd.Parameters.AddWithValue("@email", email);
                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }
    }
}