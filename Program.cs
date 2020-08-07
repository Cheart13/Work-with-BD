using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string adr = @"server=localhost;Data Source=SQLite Database; Version = 3; ";
            string adr2 = @"Data Source=C:\Users\Artem\source\repos\ConsoleApp1\ConsoleApp1\NewDB.db; Version=3;";
            SQLiteConnection connection = new SQLiteConnection(adr2);//enter her your path to DB

            connection.Open();
            
            string Select = "Select * FROM [books]";
            //string Delete = "Delete FROM [Books] where [number]=3";
            
             
            int com;
            bool swither = true;
            SQLiteCommand Command=new SQLiteCommand();
            while (swither)
            {
                
                string command;
            sim:
                Console.WriteLine("Input query: 1-Select 2-Insert 3-Update 4-Delete or 0 to exit\n/");
                com = Convert.ToInt32(Console.ReadLine());

                switch (com)
                {
                    case 0: swither = false; break;

                    case 1://select all
                        Console.WriteLine("All books:");
                        Console.WriteLine("id \t name \t\t\t author \t\t price  ");
                        command = Select;
                        Command = new SQLiteCommand(command, connection);
                        
                       SQLiteDataReader reader = Command.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0].ToString() + " \t " + reader[1].ToString()+" \t "+ reader[2].ToString()+ " \t "+reader[3].ToString());
                        }
                        reader.Close();

                        break;

                    case 2://inpyt
                        Console.WriteLine("Please input via coma name book, author and price:");
                        string name, author, price;
                        string st = Console.ReadLine();
                        string[] str = new string[3];// тут должно считыватся и разделятся по трем переменным три слова
                        str = st.Split(',');
                        name = str[0]; author = str[1]; price = str[2];

                        command = String.Format($"insert into books(b_name, b_author, price) values(\"{name}\",\"{author}\",{price});");
                        //connection.Open();
                        Command = new SQLiteCommand(command, connection);
                        Command.ExecuteNonQuery();
                        Console.WriteLine("A new row  inserted");
                        break;
                    case 3://update
                        Console.WriteLine("Please input id of book you want to change information about:");
                    m: int id;
                        try
                        {
                            id = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException e)
                        { Console.WriteLine("id must contains only whole numbers"); goto m; }
                        m1:
                        Console.WriteLine("Please input column name and it's value in format:[column name]=[value]");
                        string str1 = Console.ReadLine();
                        command = String.Format($"update books set {str1} where [B_id]= {id};");
                        //connection.Open();
                        
                        try { 
                            Command = new SQLiteCommand(command, connection);
                            Command.ExecuteNonQuery();
                        }catch(SqlException e) { Console.WriteLine("No such column or incorrect value");
                            goto m1;
                        }
                        break;

                    case 4://delete
                        Console.WriteLine("Please input id of notice which you want to delete:");
                        id = Convert.ToInt32(Console.ReadLine());
                        command = String.Format($"delete from [books] where [B_id]= {id}");
                        //connection.Open();
                        Command = new SQLiteCommand(command, connection);
                        Command.ExecuteNonQuery();
                        Console.WriteLine("row " + id + " deleted");
                        break;

                    

                    default:
                        Console.WriteLine("incorrect comand, please choose from proposed");
                        goto sim;// я извиняюсь но так проще
                        
                }

            }//end of while

            connection.Close();
            Console.WriteLine("Conection closed");
            // write code here



        }
    }
}
