using Hauna.Urooj.Hauna.Urooj.DataAccess;
using Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository;
using Hauna.Urooj.Hauna.Urooj.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace Hauna.Urooj.Hauna.Urooj.Infrastructure.Repository
{
    public class BookRepository : BaseDataAccess, IBookRepository
    {
        public BookRepository(IConfiguration configuration) : base(configuration) { }

        public IEnumerable<BooksModel> GetAllBooks()
        {
            string sqlQuery = "SELECT BookId, BookName, BookDescription, BookContent, Subject, Class, HProgram, BookUrl, Modified, ModifiedBy " +
                          "FROM BOOKS WHERE IsActive = 1";
            var parameters = new SqlParameter[]
            {};
            var bookList = new List<BooksModel>();
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
                        var book = new BooksModel
                        {
                            BookId = reader.IsDBNull(reader.GetOrdinal("BookId")) ? 0 : reader.GetInt32(reader.GetOrdinal("BookId")),
                            BookName = reader.IsDBNull(reader.GetOrdinal("BookName")) ? null : reader.GetString(reader.GetOrdinal("BookName")),
                            BookDescription = reader.IsDBNull(reader.GetOrdinal("BookDescription")) ? null : reader.GetString(reader.GetOrdinal("BookDescription")),
                            BookContent = reader.IsDBNull(reader.GetOrdinal("BookContent")) ? null : reader.GetString(reader.GetOrdinal("BookContent")),
                            Subject = reader.IsDBNull(reader.GetOrdinal("Subject")) ? null : reader.GetString(reader.GetOrdinal("Subject")),
                            Class = reader.IsDBNull(reader.GetOrdinal("Class")) ? null : reader.GetString(reader.GetOrdinal("Class")),
                            HProgram = reader.IsDBNull(reader.GetOrdinal("HProgram")) ? null : reader.GetString(reader.GetOrdinal("HProgram")),
                            BookUrl = reader.IsDBNull(reader.GetOrdinal("BookUrl")) ? null : reader.GetString(reader.GetOrdinal("BookUrl")),
                            Modified = reader.IsDBNull(reader.GetOrdinal("Modified")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Modified")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy"))
                        };
                        bookList.Add(book);
                    }
                }
                return bookList;
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

        public BooksModel GetBookById(int BookId)
        {
            string sqlQuery = "SELECT BookId, BookName, BookDescription, BookContent, Subject, Class, HProgram, BookUrl, Modified, ModifiedBy " +
                          "FROM BOOKS WHERE IsActive = 1 AND BookId = @BookId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@BookId", SqlDbType.Int) { Value = BookId },
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
                    return new BooksModel
                    {
                        BookId = reader.IsDBNull(reader.GetOrdinal("BookId")) ? 0 : reader.GetInt32(reader.GetOrdinal("BookId")),
                        BookName = reader.IsDBNull(reader.GetOrdinal("BookName")) ? null : reader.GetString(reader.GetOrdinal("BookName")),
                        BookDescription = reader.IsDBNull(reader.GetOrdinal("BookDescription")) ? null : reader.GetString(reader.GetOrdinal("BookDescription")),
                        BookContent = reader.IsDBNull(reader.GetOrdinal("BookContent")) ? null : reader.GetString(reader.GetOrdinal("BookContent")),
                        Subject = reader.IsDBNull(reader.GetOrdinal("Subject")) ? null : reader.GetString(reader.GetOrdinal("Subject")),
                        Class = reader.IsDBNull(reader.GetOrdinal("Class")) ? null : reader.GetString(reader.GetOrdinal("Class")),
                        HProgram = reader.IsDBNull(reader.GetOrdinal("HProgram")) ? null : reader.GetString(reader.GetOrdinal("HProgram")),
                        BookUrl = reader.IsDBNull(reader.GetOrdinal("BookUrl")) ? null : reader.GetString(reader.GetOrdinal("BookUrl")),
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

        public void removeBook(int bookId,string ModifiedBy)
        {
            string sqlQuery1 = "DELETE FROM BOOKS WHERE BookId = @BookId;";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@BookId", SqlDbType.NVarChar) { Value = bookId },
                    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 10) { Value = ModifiedBy ?? (object)DBNull.Value }
                };

                ExecuteNonQuery(sqlQuery1, CommandType.Text, parameters);
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

        public void CreateBook(BooksModel book)
        {
            string sqlQuery = "INSERT INTO BOOKS (BookName, BookDescription, BookContent, [Subject], class, HProgram, IsActive, BookUrl, Modified, ModifiedBy) " +
                   "VALUES (@BookName, @BookDescription, @BookContent, @Subject, @Class, @HProgram, 1, @BookUrl, @ModifiedDate, @ModifiedBy);";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookName", SqlDbType.NVarChar, 50) { Value = book.BookName },
                    new SqlParameter("@BookDescription", SqlDbType.NVarChar, 300) { Value = book.BookDescription },
                    new SqlParameter("@BookContent", SqlDbType.NVarChar) { Value = book.BookContent },
                    new SqlParameter("@Subject", SqlDbType.NVarChar, 50) { Value = book.Subject },
                    new SqlParameter("@Class", SqlDbType.NVarChar, 10) { Value = book.Class },
                    new SqlParameter("@HProgram", SqlDbType.NVarChar, 10) { Value = book.HProgram },
                    new SqlParameter("@BookUrl", SqlDbType.NVarChar, 60) { Value = book.BookUrl},
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 10) { Value = book.ModifiedBy ?? (object)DBNull.Value }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred ", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        public void EditBook(BooksModel editedBook)
        {
            string sqlQuery = "UPDATE BOOKS " +
                  "SET BookName = @BookName, " +
                  "    BookDescription = @BookDescription, " +
                  "    BookContent = @BookContent, " +
                  "    [Subject] = @Subject, " +
                  "    class = @Class, " +
                  "    HProgram = @HProgram, " +
                  "    BookUrl = @BookUrl, " +
                  "    Modified = @ModifiedDate, " +
                  "    ModifiedBy = @ModifiedBy " +
                  "WHERE BookId = @BookId;";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookName", SqlDbType.NVarChar, 50) { Value = editedBook.BookName ?? (object)DBNull.Value },
                    new SqlParameter("@BookDescription", SqlDbType.NVarChar, 300) { Value = editedBook.BookDescription ?? (object)DBNull.Value },
                    new SqlParameter("@BookContent", SqlDbType.NVarChar) { Value = editedBook.BookContent ?? (object)DBNull.Value },
                    new SqlParameter("@Subject", SqlDbType.NVarChar, 50) { Value = editedBook.Subject ?? (object)DBNull.Value },
                    new SqlParameter("@Class", SqlDbType.NVarChar, 10) { Value = editedBook.Class ?? (object)DBNull.Value },
                    new SqlParameter("@HProgram", SqlDbType.NVarChar, 10) { Value = editedBook.HProgram ?? (object)DBNull.Value },
                    new SqlParameter("@BookUrl", SqlDbType.NVarChar, 60) { Value = editedBook.BookUrl ?? (object)DBNull.Value },
                    new SqlParameter("@ModifiedDate", SqlDbType.Date) { Value = DateTime.Now },
                    new SqlParameter("@BookId", SqlDbType.Int) { Value = editedBook.BookId },
                    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 10) { Value = editedBook.ModifiedBy ?? (object)DBNull.Value }
                };

                ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw new InvalidOperationException("An error occurred ", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }
    }
}
