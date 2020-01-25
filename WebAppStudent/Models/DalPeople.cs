using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebAppStudent.Models
{
   public class DalPeople : IDalPeople
   {
      public SqlConnection SqlConnection { get; }

      public DalPeople(SqlConnection sqlConnection)
      {
         SqlConnection = sqlConnection;
      }

      public People Add(People model)
      {
         SqlTransaction transaction = null;
         if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
         try
         {
            using (transaction = SqlConnection.BeginTransaction())
            {
               using (SqlCommand command = SqlConnection.CreateCommand())
               {
                  command.Transaction = transaction;
                  command.CommandText = "INSERT INTO People(Name, Active) VALUES(@Name, @Active);SELECT SCOPE_IDENTITY();";
                  command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = model.Name;
                  command.Parameters.Add("@Active", System.Data.SqlDbType.Bit).Value = model.Active;

                  if (int.TryParse(command.ExecuteScalar().ToString(), out int id))
                  {
                     model.Id = id;
                  }
               }
               transaction?.Commit();
               SqlConnection.Close();
            }            
         }
         catch
         {
            transaction?.Rollback();
         }
         finally
         {
            transaction?.Dispose();
         }
         return model;
      }

      public bool Edit(People model)
      {
         bool status = false;
         SqlTransaction transaction = null;
         try
         {
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            using (transaction = SqlConnection.BeginTransaction())
            {
               using (SqlCommand command = SqlConnection.CreateCommand())
               {
                  command.Transaction = transaction;
                  command.CommandText = "UPDATE People SET Name=@Name, Active=@Active WHERE Id=@Id";
                  command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = model.Name;
                  command.Parameters.Add("@Active", System.Data.SqlDbType.Bit).Value = model.Active;
                  command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = model.Id;
                  status = command.ExecuteNonQuery() > 0;
               }
               transaction.Commit();
               if (SqlConnection.State == System.Data.ConnectionState.Open) SqlConnection.Close();
            }
         }
         catch
         {
            transaction?.Rollback();
         }
         finally
         {
            transaction?.Dispose();
         }
         return status;
      }

      public bool Delete(int id)
      {
         bool status = false;
         using (SqlCommand command = SqlConnection.CreateCommand())
         {
            command.CommandText = "DELETE FROM People WHERE Id=@Id";
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            status = command.ExecuteNonQuery() > 0;
         }
         if (SqlConnection.State == System.Data.ConnectionState.Open) SqlConnection.Close();
         return status;
      }

      public IEnumerable<People> All()
      {
         using (SqlCommand command = SqlConnection.CreateCommand())
         {
            command.CommandText = "SELECT Id, Name, Active FROM People";
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
               if (reader.HasRows)
               {
                  while (reader.Read())
                  {
                     yield return new People()
                     {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Active = reader.GetBoolean(2)
                     };
                  }
               }
            }
            if (SqlConnection.State == System.Data.ConnectionState.Open) SqlConnection.Close();
         }
      }

      public People Find(int id)
      {
         People people = null;
         using (SqlCommand command = SqlConnection.CreateCommand())
         {
            command.CommandText = "SELECT Id, Name, Active FROM People WHERE Id=@Id";
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
               if (reader.HasRows)
               {
                  if (reader.Read())
                  {
                     people = new People()
                     {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Active = reader.GetBoolean(2)
                     };
                  }
               }
            }
            if (SqlConnection.State == System.Data.ConnectionState.Open) SqlConnection.Close();
         }
         return people;
      }
   }
}
