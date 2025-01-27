using Hauna.Urooj.Hauna.Urooj.DataAccess;
using Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository;
using Hauna.Urooj.Hauna.Urooj.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hauna.Urooj.Hauna.Urooj.Infrastructure.Repository
{
    public class StationaryRepository : BaseDataAccess, IStationaryRepository
    {
        public StationaryRepository(IConfiguration configuration) : base(configuration) { }

        public void CreateStationary(StationaryModel stationary)
        {
            string sqlQuery = "INSERT INTO STATIONERY (Title, StationaryDescription, StationaryPrice, IsActice, StationaryUrl, Modified, ModifiedBy) " +
                      "VALUES (@Title, @StationaryDescription, @StationaryPrice, 1, @StationaryUrl, @ModifiedDate, @ModifiedBy);";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Title", SqlDbType.NVarChar, 50) { Value = stationary.Title },
                    new SqlParameter("@StationaryDescription", SqlDbType.NVarChar, 300) { Value = stationary.StationaryDescription },
                    new SqlParameter("@StationaryPrice", SqlDbType.NVarChar, 50) { Value = stationary.StationaryPrice },
                    new SqlParameter("@StationaryUrl", SqlDbType.NVarChar, int.MaxValue) { Value = stationary.StationaryUrl },
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 10) { Value = stationary.ModifiedBy }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while inserting the stationery item.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred while inserting the stationery item.", ex);
            }
        }

        public void EditStationary(StationaryModel editedStationary)
        {
            string sqlQuery = "UPDATE STATIONERY " +
                      "SET Title = @Title, " +
                      "    StationaryDescription = @StationaryDescription, " +
                      "    StationaryPrice = @StationaryPrice, " +
                      "    IsActice = @IsActive, " +
                      "    StationaryUrl = @StationaryUrl, " +
                      "    Modified = @ModifiedDate, " +
                      "    ModifiedBy = @ModifiedBy " +
                      "WHERE StationaryId = @StationaryId;";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Title", SqlDbType.NVarChar, 50) { Value = editedStationary.Title ?? (object)DBNull.Value },
                    new SqlParameter("@StationaryDescription", SqlDbType.NVarChar, 300) { Value = editedStationary.StationaryDescription ?? (object)DBNull.Value },
                    new SqlParameter("@StationaryPrice", SqlDbType.NVarChar, 50) { Value = editedStationary.StationaryPrice ?? (object)DBNull.Value },
                    new SqlParameter("@IsActive", SqlDbType.Bit) { Value = 1},
                    new SqlParameter("@StationaryUrl", SqlDbType.NVarChar, int.MaxValue) { Value = editedStationary.StationaryUrl },
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@StationaryId", SqlDbType.Int) { Value = editedStationary.StationaryId },
                    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 10) { Value = editedStationary.ModifiedBy }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while updating the stationery item.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public IEnumerable<StationaryModel> GetAllStationary()
        {
            string sqlQuery = "SELECT StationaryId, Title, StationaryDescription, StationaryPrice, IsActice, StationaryUrl, Modified, ModifiedBy " +
                              "FROM STATIONERY WHERE IsActice = 1";
            var parameters = new SqlParameter[] { };
            var stationaryList = new List<StationaryModel>();

            try
            {
                using (var reader = ExecuteReader(sqlQuery, CommandType.Text, parameters))
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (reader.Read())
                    {
                        var stationary = new StationaryModel
                        {
                            StationaryId = reader.IsDBNull(reader.GetOrdinal("StationaryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StationaryId")),
                            Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                            StationaryDescription = reader.IsDBNull(reader.GetOrdinal("StationaryDescription")) ? null : reader.GetString(reader.GetOrdinal("StationaryDescription")),
                            StationaryPrice = reader.IsDBNull(reader.GetOrdinal("StationaryPrice")) ? null : reader.GetString(reader.GetOrdinal("StationaryPrice")),
                            StationaryUrl = reader.IsDBNull(reader.GetOrdinal("StationaryUrl")) ? null : reader.GetString(reader.GetOrdinal("StationaryUrl")),
                            Modified = (reader.IsDBNull(reader.GetOrdinal("Modified")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Modified"))),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy"))
                        };

                        stationaryList.Add(stationary);
                    }
                }

                return stationaryList;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return null;
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"Argument Error: {argEx.Message}");
                return null;
            }
            catch (NullReferenceException nullEx)
            {
                Console.WriteLine($"Null Reference Error: {nullEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public StationaryModel GetStationaryById(int stationaryId)
        {
            string sqlQuery = "SELECT StationaryId, Title, StationaryDescription, StationaryPrice, IsActice, StationaryUrl, Modified, ModifiedBy " +
                      "FROM STATIONERY WHERE IsActice = 1 AND StationaryId = @StationaryId";

            var parameters = new SqlParameter[]
            {
        new SqlParameter("@StationaryId", SqlDbType.Int) { Value = stationaryId }
            };

            try
            {
                using (var reader = ExecuteReader(sqlQuery, CommandType.Text, parameters))
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    reader.Read();

                    return new StationaryModel
                    {
                        StationaryId = reader.IsDBNull(reader.GetOrdinal("StationaryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StationaryId")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                        StationaryDescription = reader.IsDBNull(reader.GetOrdinal("StationaryDescription")) ? null : reader.GetString(reader.GetOrdinal("StationaryDescription")),
                        StationaryPrice = reader.IsDBNull(reader.GetOrdinal("StationaryPrice")) ? null : reader.GetString(reader.GetOrdinal("StationaryPrice")),
                        StationaryUrl = reader.IsDBNull(reader.GetOrdinal("StationaryUrl")) ? null : reader.GetString(reader.GetOrdinal("StationaryUrl")),
                        Modified = reader.IsDBNull(reader.GetOrdinal("Modified")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Modified")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy"))
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public void removeStationary(int stationaryId, string ModifiedBy)
        {
            string sqlQuery = "DELETE FROM STATIONERY WHERE StationaryId = @StationaryId;";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@StationaryId", SqlDbType.Int) { Value = stationaryId },
                    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 10) { Value = ModifiedBy ?? (object)DBNull.Value }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred while deactivating the stationary item.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }
    }
}
