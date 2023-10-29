#define LOG
#undef LOG

using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace BARKOCHBA
{
 public class Database
 {
  public Database()
  {
  }
  string connectionString;
  public void SetConnectionString()
  {
   string currentDirectory=System.IO.Directory.GetCurrentDirectory();
   if(currentDirectory=="C:\\Program Files (x86)\\IIS Express")
    connectionString = "Data Source=C:/BAREK/BARKOCHBA/BARKOCHBA.sqlite";
   else
    connectionString = "Data Source=D:/home/site/wwwroot/BARKOCHBA.sqlite";
  }
  public DomainModel GetFirstDomain()
  {
   //this will select the first record from the domains table
   DomainModel domain=new DomainModel();
   string query="SELECT * FROM domains ORDER BY id ASC LIMIT 1";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      if(reader.Read())
      {
       domain.Id=reader.GetInt32(reader.GetOrdinal("id"));
       domain.Name=reader.GetString(reader.GetOrdinal("name"));
      }
      else
      {
       Debug.WriteLine("SQL error");
      }
     }
    }
   }
   return domain;
  }
  /*
  public void ResetQuestions(List<QuestionModel> questions)
  {
   //reset all calculated fields to default
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    foreach(QuestionModel question in questions)
    {
     string str_sql=$"UPDATE questions SET asked=0 WHERE id=@Id";
     using(var command=new SQLiteCommand(str_sql,connection))
     {
      command.Parameters.AddWithValue("@Id",question.Id);
      command.ExecuteNonQuery();
     }
    }
   }
  }
  */
  public List<QuestionModel> QuestionsGetAll()
  {
   List<QuestionModel> list_questions=new List<QuestionModel>();
   string query="SELECT * FROM questions";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      while(reader.Read())
      {
       int id=reader.GetInt32(reader.GetOrdinal("id"));
       string text=reader.GetString(reader.GetOrdinal("text"));
       list_questions.Add(new QuestionModel()
       {
        Id=id,
        Text=text
       });;
      }
     }
    }
   }
   return list_questions;
  }
  public QuestionModel QuestionGetById(int question_id)
  {
   QuestionModel question=new QuestionModel();
   string query=$"SELECT * FROM questions WHERE id={question_id}";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      if(reader.Read())
      {
       int id=reader.GetInt32(reader.GetOrdinal("id"));
       string text=reader.GetString(reader.GetOrdinal("text"));
       question.Id=id;
       question.Text=text;
      }
      else
      {
       Debug.WriteLine("SQL error in QuestionGetById {0}",question_id);
      }
     }
    }
   }
   return question;
  }
  public List<ReactionModel> ReactionsGetAll()
  {
   List<ReactionModel> list_reactions=new List<ReactionModel>();
   string query="SELECT * FROM reactions";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      while(reader.Read())
      {
       int id=reader.GetInt32(reader.GetOrdinal("id"));
       string name=reader.GetString(reader.GetOrdinal("name"));
       string text=reader.GetString(reader.GetOrdinal("text"));
       double value=reader.GetDouble(reader.GetOrdinal("value"));
       //int value=5;//reader.GetInt32(reader.GetOrdinal("value"));
       list_reactions.Add(new ReactionModel()
       {
        Id=id,
        Name=name,
        Text=text,
        Value=value
       });
      }
     }
    }
   }
   return list_reactions;
  }
  public List<SolutionModel> SolutionsGetAll()
  {
   List<SolutionModel> list_solutions=new List<SolutionModel>();
   string query="SELECT * FROM solutions";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      while(reader.Read())
      {
       int id=reader.GetInt32(reader.GetOrdinal("id"));
       string text=reader.GetString(reader.GetOrdinal("text"));
       list_solutions.Add(new SolutionModel()
       {
        Id=id,
        Text=text
       });
      }
     }
    }
   }
   return list_solutions;
  }
  public List<ProblemModel> ProblemsGetAll()
  {
   List<ProblemModel> problems_list=new List<ProblemModel>();
   string query="SELECT * FROM problems";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      while(reader.Read())
      {
       int id=reader.GetInt32(reader.GetOrdinal("id"));
       int domain_id=reader.GetInt32(reader.GetOrdinal("domain_id"));
       int solution_id=reader.GetInt32(reader.GetOrdinal("solution_id"));
       problems_list.Add(new ProblemModel()
       {
        Id=id,
        DomainId=domain_id,
        SolutionId=solution_id
       });
      }
     }
    }
   }
   return problems_list;
  }
  public List<DetailModel> DetailsGetAll()
  {
   List<DetailModel> list_details = new List<DetailModel>();
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    string str_sql;
    str_sql="SELECT * FROM details";
    using(var command_details=new SQLiteCommand(str_sql,connection))
    {
     using(var reader_details=command_details.ExecuteReader())
     {
      while(reader_details.Read())
      {
       int id=reader_details.GetInt32(reader_details.GetOrdinal("id"));
       int problem_id=reader_details.GetInt32(reader_details.GetOrdinal("problem_id"));
       int question_id=reader_details.GetInt32(reader_details.GetOrdinal("question_id"));
       int reaction_id=reader_details.GetInt32(reader_details.GetOrdinal("reaction_id"));
       int solution_id=0;
       str_sql="SELECT solution_id FROM problems WHERE id=@problemId LIMIT 1";
       using(var command_problems=new SQLiteCommand(str_sql,connection))
       {
        command_problems.Parameters.AddWithValue("@problemId",problem_id);
        using (var reader_problems=command_problems.ExecuteReader())
        {
         if (reader_problems.Read())
         {
          solution_id=reader_problems.GetInt32(reader_problems.GetOrdinal("solution_id"));
         }
        }
       }
       double reaction_value=0;
       str_sql="SELECT value FROM reactions WHERE id=@reactionId LIMIT 1";
       using (var command_reactions = new SQLiteCommand(str_sql,connection))
       {
        command_reactions.Parameters.AddWithValue("@reactionId",reaction_id);
        using (var reader_reactions=command_reactions.ExecuteReader())
        {
         if (reader_reactions.Read())
         {
          reaction_value=reader_reactions.GetDouble(reader_reactions.GetOrdinal("value"));
         }
        }
       }
       list_details.Add(new DetailModel()
       {
        Id=id,
        ProblemId=problem_id,
        SolutionId=solution_id,
        QuestionId=question_id,
        ReactionId=reaction_id,
        ReactionValue=reaction_value
       });
      }
     }
    }
   }
   return list_details;
  }
  public List<DomainModel> DomainsGetAll()
  {
   List<DomainModel> list_domains = new List<DomainModel>();
   string query="SELECT * FROM domains";
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      while(reader.Read())
      {
       int id=reader.GetInt32(reader.GetOrdinal("id"));
       string name=reader.GetString(reader.GetOrdinal("name"));
       list_domains.Add(new DomainModel()
       {
        Id=id,
        Name=name
       });
      }
     }
    }
   }
   return list_domains;
  }
  /*
  public List<EpisodeModel> EpisodesGetAll()
  {
   List<EpisodeModel> list_episodes=new List<EpisodeModel>();
   string str_sql;
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    str_sql="DROP VIEW IF EXISTS episodes";
    using(var command=new SQLiteCommand(str_sql,connection))
    {
     int result=command.ExecuteNonQuery();
     if(result>=0)
     {
      #if LOG
      Debug.WriteLine("View 'episodes' dropped successfully.");
      #endif
     }
    }
    str_sql=@"CREATE VIEW episodes AS SELECT
               details.id,
               problems.solution_id,
               details.problem_id,
               details.question_id,
               details.reaction_id 
              FROM details
               INNER JOIN problems ON details.problem_id=problems.id";
    using(var command=new SQLiteCommand(str_sql,connection))
    {
     int result=command.ExecuteNonQuery();
     if(result>=0)
     {
      #if LOG
      Debug.WriteLine("View 'episodes' created successfully.");
      #endif
     }
    }
    str_sql="SELECT * FROM episodes";
    using(var command=new SQLiteCommand(str_sql,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      while(reader.Read())
      {
       int id=reader.GetInt32(0);
       int problem_id=reader.GetInt32(1);
       int solution_id=reader.GetInt32(2);
       int question_id=reader.GetInt32(3);
       int reaction_id=reader.GetInt32(4);
       list_episodes.Add(new EpisodeModel()
       {
        Id=id,
        ProblemId=problem_id,
        SolutionId=solution_id,
        QuestionId=question_id,
        ReactionId=reaction_id
       });;
      }
     }
    }
   }
   return list_episodes;
  }
  */
  public int CountRecords(string table)
  {
   string query=$"SELECT Count(*) FROM {table}";
   SetConnectionString();
   int n_records=0;
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      if(reader.Read())
      {
       n_records=reader.GetInt32(0);
      }
      else
       Debug.WriteLine("SQL error");
     }
    }
   }
   return n_records;
  }
  /*
  public void UpdateQuestions(List<QuestionModel> questions)
  {
   SetConnectionString();
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    foreach(QuestionModel question in questions)
    {
     string str_sql=$"UPDATE questions SET asked=@Asked WHERE id=@Id";
     using(var command=new SQLiteCommand(str_sql,connection))
     {
      command.Parameters.AddWithValue("@Id",question.Id);
      if(question.Asked==true)
       command.Parameters.AddWithValue("@Asked",1);
      else
       command.Parameters.AddWithValue("@Asked",0);
      command.ExecuteNonQuery();
     }
    }
   }
  }
  */
  public int CountDetailsWhereSolution(int solution_id)
  {
   string query="SELECT Count(*) FROM problems WHERE solution_id="+solution_id.ToString();
   SetConnectionString();
   int n_details=0;
   using(var connection=new SQLiteConnection(connectionString))
   {
    connection.Open();
    using(var command=new SQLiteCommand(query,connection))
    {
     using(var reader=command.ExecuteReader())
     {
      if(reader.Read())
      {
       n_details=reader.GetInt32(0);
      }
     }
    }
   }
   return n_details;
  }
 }
}