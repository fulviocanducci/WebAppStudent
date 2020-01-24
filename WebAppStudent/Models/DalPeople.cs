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
         using (SqlCommand command = SqlConnection.CreateCommand())
         {
            command.CommandText = "INSERT INTO People(Name, Active) VALUES(@Name, @Active);SELECT SCOPE_IDENTITY();";
            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = model.Name;
            command.Parameters.Add("@Active", System.Data.SqlDbType.Bit).Value = model.Active;
            if (SqlConnection.State == System.Data.ConnectionState.Closed)
            {
               SqlConnection.Open();
            }
            if (int.TryParse(command.ExecuteScalar().ToString(), out int id))
            {
               model.Id = id;
            }
         }
         SqlConnection.Close();
         return model;
      }

      public bool Edit(People model)
      {
         bool status = false;
         using (SqlCommand command = SqlConnection.CreateCommand())
         {
            command.CommandText = "UPDATE People SET Name=@Name, Active=@Active WHERE Id=@Id";
            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = model.Name;
            command.Parameters.Add("@Active", System.Data.SqlDbType.Bit).Value = model.Active;
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = model.Id;
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            status = command.ExecuteNonQuery() > 0;
         }
         if (SqlConnection.State == System.Data.ConnectionState.Open) SqlConnection.Close();
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
