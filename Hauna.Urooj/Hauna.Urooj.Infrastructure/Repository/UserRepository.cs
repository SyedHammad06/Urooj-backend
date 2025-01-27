using Hauna.Urooj.Hauna.Urooj.DataAccess;
using Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository;
using Hauna.Urooj.Hauna.Urooj.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;

namespace Hauna.Urooj.Hauna.Urooj.Infrastructure.Repository
{
    public class UserRepository : BaseDataAccess, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }
        public UserModel GetUser(LoginModel loginModel)
        {
            string sqlQuery = "SELECT UserId, EncryptedPassword, ModifiedDate, IsAdmin, IsActive " +
                              "FROM USERS WHERE UserId = @UserId AND EncryptedPassword = @EncryptedPassword AND IsActive = 1";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = loginModel.userName },
                new SqlParameter("@EncryptedPassword", SqlDbType.NVarChar) { Value = loginModel.password }
            };

            using (var reader = ExecuteReader(sqlQuery, CommandType.Text, parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new UserModel
                    {
                        UserId = reader.GetString(reader.GetOrdinal("UserId")),
                        EncryptedPassword = reader.GetString(reader.GetOrdinal("EncryptedPassword")),
                        ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                        IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public string CheckForDuplicateEmail(string email)
        {
            string sqlQuery = "SELECT PersonalEmail " +
                              "FROM USERINFO WHERE PersonalEmail = @email AND IsActive = 1";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@email", SqlDbType.NVarChar) { Value = email }
            };
            try
            {
                using (var reader = ExecuteReader(sqlQuery, CommandType.Text, parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetString(reader.GetOrdinal("PersonalEmail"));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while checking for duplicate email.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public void InsertUserInfo(UserInfoModel model)
        {
            string sqlQuery = "INSERT INTO USERINFO (FName, MName, LName, UserAddress, Phote, Adhaar, City, [State], Country, PersonalEmail, IsActive) " +
                      "VALUES (@FName, @MName, @LName, @UserAddress, @Phote, @Adhaar, @City, @State, @Country, @PersonalEmail, @IsActive)";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FName", SqlDbType.NVarChar) { Value = model.FName },
                    new SqlParameter("@MName", SqlDbType.NVarChar) { Value = model.MName },
                    new SqlParameter("@LName", SqlDbType.NVarChar) { Value = model.LName },
                    new SqlParameter("@UserAddress", SqlDbType.NVarChar) { Value = model.UserAddress },
                    new SqlParameter("@Phote", SqlDbType.VarBinary) { Value = model.Phote ?? (object)DBNull.Value },  // Handle null photo
                    new SqlParameter("@Adhaar", SqlDbType.NVarChar) { Value = model.Adhaar },
                    new SqlParameter("@City", SqlDbType.NVarChar) { Value = model.City },
                    new SqlParameter("@State", SqlDbType.NVarChar) { Value = model.State },
                    new SqlParameter("@Country", SqlDbType.NVarChar) { Value = model.Country },
                    new SqlParameter("@PersonalEmail", SqlDbType.NVarChar) { Value = model.PersonalEmail },
                    new SqlParameter("@IsActive", SqlDbType.Bit) { Value = 1 }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new ApplicationException("An error occurred while inserting user information.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        public int fetchInfoId(string email)
        {
            string sqlQuery = "SELECT InfoId " +
                              "FROM USERINFO WHERE PersonalEmail = @email AND IsActive = 1";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@email", SqlDbType.NVarChar) { Value = email }
            };
            try
            {
                using (var reader = ExecuteReader(sqlQuery, CommandType.Text, parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetInt32(reader.GetOrdinal("InfoId"));
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while checking for duplicate email.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }
        public void InsertUser(UserModel userModel)
        {
            string sqlQuery = "INSERT INTO USERS (UserId, EncryptedPassword, ModifiedDate, IsAdmin, IsActive, InfoId) " +
                              "VALUES (@UserId, @EncryptedPassword, @ModifiedDate, @IsAdmin, @IsActive, @InfoId)";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = userModel.UserId },
                new SqlParameter("@EncryptedPassword", SqlDbType.NVarChar) { Value = userModel.EncryptedPassword },
                new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = userModel.ModifiedDate }, // Current date
                new SqlParameter("@IsAdmin", SqlDbType.Bit) { Value = userModel.IsAdmin },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = 1 },
                new SqlParameter("@InfoId", SqlDbType.Int) { Value = userModel.InfoId }
                };
                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while checking for duplicate email.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public IEnumerable<UsersDisplayModel> GetAllActiveUsers()
        {
            string sqlQuery = "SELECT FName+' '+MName+' '+LName as FullName,Phote,PersonalEmail,UserId From USERS " +
                "INNER JOIN USERINFO ON USERS.InfoId = USERINFO.InfoId " +
                "WHERE Users.IsActive=1";

            try
            {
                using (var reader = ExecuteReader(sqlQuery, CommandType.Text))
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    var users = new List<UsersDisplayModel>();
                    while (reader.Read())
                    {
                        users.Add(new UsersDisplayModel()
                        {
                            FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString(reader.GetOrdinal("FullName")),
                            Photo = reader.IsDBNull(reader.GetOrdinal("Phote")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Phote")),
                            PersonalEmail = reader.IsDBNull(reader.GetOrdinal("PersonalEmail")) ? null : reader.GetString(reader.GetOrdinal("PersonalEmail")),
                            UserName = reader.IsDBNull(reader.GetOrdinal("UserId")) ? null : reader.GetString(reader.GetOrdinal("UserId"))
                        });
                    }
                    return users;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while fetching.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public void ChangePassword(string userId, string newEncryptedPassword)
        {
            string sqlQuery = "UPDATE USERS SET EncryptedPassword = @EncryptedPassword, ModifiedDate = @ModifiedDate WHERE UserId = @UserId AND IsActive = 1";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@EncryptedPassword", SqlDbType.NVarChar) { Value = newEncryptedPassword },
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = userId }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while updating the password.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public void SetIsActiveInUsers(string userId)
        {
            string sqlQuery1 = "UPDATE USERS SET IsActive = 0, ModifiedDate = @ModifiedDate WHERE UserId = @UserId;";
            string sqlQuery2 = "UPDATE USERINFO SET IsActive = 0 WHERE InfoId = (SELECT InfoId FROM USERS WHERE UserId = @UserId);";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = userId }
                };

                SqlParameter[] parameters2 = new SqlParameter[]
                {
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = userId }
                };

                ExecuteNonQuery(sqlQuery1, CommandType.Text, parameters);
                ExecuteNonQuery(sqlQuery2, CommandType.Text, parameters2);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while updating the password.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public UserDetail GetUserData(string UserName)
        {
            string sqlQuery = "Select FName, MName, LName, UserAddress, Phote, Adhaar, City, [State], Country, PersonalEmail, IsAdmin " +
                              "From USERS INNER Join USERINFO ON USERINFO.InfoId = USERS.InfoId " +
                              "WHERE USERS.IsActive = 1 AND UserId = @UserId";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserName }
            };
            try
            {
                using (var reader = ExecuteReader(sqlQuery, CommandType.Text, parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new UserDetail
                        {
                            FName = reader.IsDBNull(reader.GetOrdinal("FName")) ? null : reader.GetString(reader.GetOrdinal("FName")),
                            MName = reader.IsDBNull(reader.GetOrdinal("MName")) ? null : reader.GetString(reader.GetOrdinal("MName")),
                            LName = reader.IsDBNull(reader.GetOrdinal("LName")) ? null : reader.GetString(reader.GetOrdinal("LName")),
                            UserAddress = reader.IsDBNull(reader.GetOrdinal("UserAddress")) ? null : reader.GetString(reader.GetOrdinal("UserAddress")),

                            Photo = reader.IsDBNull(reader.GetOrdinal("Phote")) ? null : (byte[])reader["Phote"],

                            Adhaar = reader.IsDBNull(reader.GetOrdinal("Adhaar")) ? null : reader.GetString(reader.GetOrdinal("Adhaar")),
                            City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                            State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                            Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                            PersonalEmail = reader.IsDBNull(reader.GetOrdinal("PersonalEmail")) ? null : reader.GetString(reader.GetOrdinal("PersonalEmail")),

                            IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin")) == true ? 1 : 0
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while updating the password.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }
    }
}
