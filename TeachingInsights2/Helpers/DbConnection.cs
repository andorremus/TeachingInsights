﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TeachingInsights2
{
    public class DBConnection
    {
        private DBConnection() { }
        private MySqlConnection Connection = null;
        private static DBConnection _instance = null;
        private string databaseName = "TeachingInsights";

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
        public string Password { get; set; }
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;

        }

        public bool IsConnect()
        {
            bool result = true;
            if (Connection == null)
            {
                if (databaseName == string.Empty)
                    result = false;
                string StrCon = string.Format("Server=194.81.104.22; database={0}; UID=s13430492; password=remian10", databaseName);
                Connection = new MySqlConnection(StrCon);
                Connection.Open();
                result = true;
            }

            return result;
        }

        public MySqlConnection GetConnection()
        {
            return Connection;
        }

        public void Close()
        {
            Connection.Close();
        }


    }
}
