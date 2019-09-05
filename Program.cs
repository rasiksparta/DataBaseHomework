using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LocalDB
{
    class Program
    {
        static void Main(string[] args)
        {

            Game game = new Game("Age of Empire 1", "RTS", "Strategy", "Very good");
            Game game2 = new Game("Age of Empire 2", "RTS", "Strategy", "Legendary");
            GameDBHandler dBHandler = new GameDBHandler();
            dBHandler.CreateConnection();
            //dBHandler.DbInsert(game);
            string[] array = { "name", "Age of Empire 1" };
            //dBHandler.DbRemove(array);
            //dBHandler.DbUpdate(game2, array);
            List<Game> list = dBHandler.DbRead();
            foreach(Game obj in list)
            {
                obj.Display();
            }
            dBHandler.CloseConnection();
            Console.Read();
        }
    }

    class GameDBHandler
    {
        const string CONNECTIONSTRING = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = MyGame; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection connection;

        public bool CreateConnection()
        {
            try
            {
                connection = new SqlConnection(CONNECTIONSTRING);
                connection.Open();
                return true;
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool DbInsert(Game game)
        {
            string query = String.Format("INSERT INTO mygame (name, genre, type, review) VALUES ('{0}','{1}','{2}','{3}')", game.Name, game.Genre, game.Type, game.Review);
            try
            {
                SqlCommand command = new SqlCommand(query, this.connection);
                command.ExecuteReader();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return false;
        }

        public List<Game> DbRead()
        {
            List<Game> list = new List<Game>();
            string query = String.Format("SELECT * FROM mygame");
            try
            {
                SqlCommand command = new SqlCommand(query, this.connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Game(reader["name"].ToString(), reader["genre"].ToString(), reader["type"].ToString(), reader["review"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }

        public bool DbRemove(string[] where)
        {
            string query = $"DELETE FROM mygame WHERE {where[0]} = '{where[1]}' ";
            try
            {
                SqlCommand command = new SqlCommand(query, this.connection);
                command.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool DbUpdate(Game game, string[] where)
        {
            
            string query = $"UPDATE mygame SET name = '{game.Name}', genre = '{game.Genre}', type = '{game.Type}', review = '{game.Review}' " +
                $"WHERE {where[0]} = '{where[1]}'";
            try
            {
                SqlCommand command = new SqlCommand(query, this.connection);
                command.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool DbDrop()
        {
            string query = "DROP TABLE mygame";
            try
            {
                SqlCommand command = new SqlCommand(query, this.connection);
                command.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public void CloseConnection()
        {
            if(connection != null)
            {
                connection.Close();
            }
        }
    }

    class Game
    {
        string name = "";
        string genre = "";
        string type = "";
        string review = "";
        //private GameDBHandler dbHandler;

        public Game(string name, string genre, string type, string review)
        {
            this.name = name;
            this.genre = genre;
            this.type = type;
            this.review = review;
        }

        public string Name { get => name; set => name = value; }
        public string Genre { get => genre; set => genre = value; }
        public string Type { get => type; set => type = value; }
        public string Review { get => review; set => review = value; }

        /**
         * Display  
         */
        public void Display()
        {
            Console.WriteLine($"Name: {this.name}, Genre: {this.Genre}, Type: {this.Type}, Review: {this.Review}");
        }


    }
}
