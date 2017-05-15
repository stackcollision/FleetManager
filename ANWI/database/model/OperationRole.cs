﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANWI.Database.Model {

	/// <summary>
	/// Non-permanent assignments in an operation
	/// </summary>
	public class OperationRole {
		#region Model
		public int id;
		public string name;
		public int rate;
		public bool onShips;
		public bool onBoats;
		public bool inSquads;

		private OperationRole(int id, string name, int rate, bool ships,
			bool boats, bool squads) {
			this.id = id;
			this.name = name;
			this.rate = rate;
			this.onShips = ships;
			this.onBoats = boats;
			this.inSquads = squads;
		}
		#endregion

		#region Class Members
		public static OperationRole Factory() {
			OperationRole result = new OperationRole(
				id: -1,
				name: "",
				rate: -1,
				ships: false,
				boats: false,
				squads: false
				);
			return result;
		}

		public static OperationRole Factory(int id, string name, int rate,
			bool ships, bool boats, bool squads) {
			OperationRole result = new OperationRole(
				id: id,
				name: name,
				rate: rate,
				ships: ships,
				boats: boats,
				squads: squads
				);
			return result;
		}

		public static OperationRole Factory(SQLiteDataReader reader) {
			OperationRole result = new OperationRole(
				id: Convert.ToInt32(reader["id"]),
				name: (string)reader["name"],
				rate: Convert.ToInt32(reader["associatedRate"]),
				ships: Convert.ToBoolean(reader["onShips"]),
				boats: Convert.ToBoolean(reader["onBoats"]),
				squads: Convert.ToBoolean(reader["inSquads"])
				);
			return result;
		}
		
		public static bool Create(ref OperationRole output, string name,
			int rate, bool ships, bool boats, bool squads) {
			int result = DBI.DoAction(
				$@"INSERT INTO OperationRole 
				(name, rate, onShips, onBoats, inSquads)
				VALUES ('{name}', {rate}, {ships}, {boats}, {squads});");
			if(result == 1) {
				return OperationRole.FetchById(ref output, DBI.LastInsertRowId);
			}
			return false;
		}

		/// <summary>
		/// Fetches a specific operation role
		/// </summary>
		/// <param name="output"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool FetchById(ref OperationRole output, int id) {
			SQLiteDataReader reader = DBI.DoQuery(
				$"SELECT * FROM OperationRole WHERE id = {id} LIMIT 1;");
			if(reader != null && reader.Read()) {
				output = OperationRole.Factory(reader);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Fetches all of the operation roles which can be assigned to a ship
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public static bool FetchAllShips(ref List<OperationRole> output) {
			output = new List<OperationRole>();

			SQLiteDataReader reader = DBI.DoQuery(
				$@"SELECT * FROM OperationRole
				WHERE onShips = 1
				ORDER BY id ASC;");
			while(reader != null && reader.Read()) {
				output.Add(OperationRole.Factory(reader));
			}

			return true;
		}

		/// <summary>
		/// Fetches all operation roles which can be assigned to a boat
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public static bool FetchAllBoats(ref List<OperationRole> output) {
			output = new List<OperationRole>();

			SQLiteDataReader reader = DBI.DoQuery(
				$@"SELECT * FROM OperationRole
				WHERE onBoats = 1
				ORDER BY id ASC;");
			while (reader != null && reader.Read()) {
				output.Add(OperationRole.Factory(reader));
			}

			return true;
		}
		#endregion
	}
}
