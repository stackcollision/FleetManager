﻿using System;
using System.Data.SQLite;
using ANWI.Database.Model;
using NLog;

namespace ANWI.Database
{
    public static class DBI
    {
        static SQLiteConnection dbConn = null;

		private static NLog.Logger logger = LogManager.GetLogger("DBI");

		/// <summary>
		/// Opens a database connection
		/// </summary>
		/// <param name="dbFileName"></param>
		/// <returns></returns>
		public static bool Open(string dbFileName = "fleetManager.sqlite3db")
        {
            if (dbConn != null)
                return false;
			if (dbFileName == null)
				dbFileName = "fleetManager.sqlite3db";
            dbConn = 
				new SQLiteConnection($"Data Source={dbFileName};Version=3;");
            dbConn.Open();
            return true;
        }

		/// <summary>
		/// Closes a database connection
		/// </summary>
        public static void Close()
        {
            if (dbConn == null)
                return;
            dbConn.Close();
        }

		/// <summary>
		/// Checks if the connection is open
		/// </summary>
		/// <returns></returns>
		public static bool IsOpen() {
			return dbConn != null;
		}

		/// <summary>
		/// Runs a query which returns data
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
        public static SQLiteDataReader DoQuery(string query)
        {
            Open();
			logger.Info("Running query: " + query);
            return new SQLiteCommand(query, dbConn).ExecuteReader();
        }

		/// <summary>
		/// Runs a query which does not return data
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
        public static int DoAction(string query)
        {
            Open();
			logger.Info("Running action: " + query);
            return new SQLiteCommand(query, dbConn).ExecuteNonQuery();
        }

		/// <summary>
		/// Returns the last inserted row id
		/// </summary>
        public static int LastInsertRowId
        {
            get
            {
                return (int)dbConn.LastInsertRowId;
            }
        }
    }
}