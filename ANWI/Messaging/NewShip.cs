﻿namespace ANWI.Messaging {

	/// <summary>
	/// Client -> Server
	/// Creates a new ship
	/// </summary>
	public class NewShip : IMessagePayload {
		public int hullId;
		public string name;
		public bool LTI;
		public int ownerId;

		public NewShip() {
			hullId = 0;
			name = "";
			LTI = false;
			ownerId = 0;
		}

		public NewShip(int hullId, string name, bool lti, int owner) {
			this.hullId = hullId;
			this.name = name;
			this.LTI = lti;
			this.ownerId = owner;
		}

		public override string ToString() {
			return $"Type: NewShip. Hull: {hullId} Name: {name} LTI: {LTI}" +
				" Owner: {ownerId}";
		}
	}
}
