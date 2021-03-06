﻿using MsgPack.Serialization;
using WebSocketSharp;
using System.IO;

namespace ANWI.Messaging {

	/// <summary>
	/// A message transmitted between the client and server and vice versa
	/// </summary>
	public class Message {

		public int sequence { get; private set; }
		public string source { get; private set; }

		/// <summary>
		/// Polymorphic payload
		/// </summary>
		[MessagePackKnownType("logr", typeof(LoginRequest))]
		[MessagePackKnownType("regr", typeof(RegisterRequest))]
		[MessagePackKnownType("lr", typeof(LoginResponse))]
		[MessagePackKnownType("rr", typeof(RegisterResponse))]
		[MessagePackKnownType("rv", typeof(Request))]
		[MessagePackKnownType("fvr", typeof(FullVesselReg))]
		[MessagePackKnownType("fol", typeof(FullOperationsList))]
		[MessagePackKnownType("ros", typeof(FullRoster))]
		[MessagePackKnownType("acd", typeof(AllCommonData))]
		[MessagePackKnownType("ar", typeof(AddRate))]
		[MessagePackKnownType("fpro", typeof(FullProfile))]
		[MessagePackKnownType("ack", typeof(ConfirmUpdate))]
		[MessagePackKnownType("fvd", typeof(FullVessel))]
		[MessagePackKnownType("enda", typeof(EndAssignment))]
		[MessagePackKnownType("newa", typeof(NewAssignment))]
		[MessagePackKnownType("css", typeof(ChangeShipStatus))]
		[MessagePackKnownType("ns", typeof(NewShip))]
		[MessagePackKnownType("op_cn", typeof(Ops.CreateNewOp))]
		[MessagePackKnownType("op_ctd", typeof(Ops.NewOpCreated))]
		[MessagePackKnownType("op_snap", typeof(Ops.FullOperationSnapshot))]
		[MessagePackKnownType("op_us", typeof(Ops.UpdateStatus))]
		[MessagePackKnownType("op_upros", typeof(Ops.UpdateRoster))]
		[MessagePackKnownType("op_addunit", typeof(Ops.AddOOBUnit))]
		[MessagePackKnownType("op_upships", typeof(Ops.UpdateUnitsShips))]
		[MessagePackKnownType("op_upwings", typeof(Ops.UpdateUnitsWings))]
		[MessagePackKnownType("op_upboats", typeof(Ops.UpdateUnitsBoats))]
		[MessagePackKnownType("op_delunit", typeof(Ops.DeleteOOBElement))]
		[MessagePackKnownType("op_assus", typeof(Ops.AssignUser))]
		[MessagePackKnownType("op_upass", typeof(Ops.UpdateAssignments))]
		[MessagePackKnownType("op_modunit", typeof(Ops.ModifyUnit))]
		[MessagePackKnownType("op_addpos", typeof(Ops.AddPosition))]
		[MessagePackKnownType("op_delpos", typeof(Ops.DeletePosition))]
		[MessagePackKnownType("op_uppos", typeof(Ops.UpdatePositions))]
		[MessagePackKnownType("op_poscrit", typeof(Ops.SetPositionCritical))]
		[MessagePackKnownType("op_upship", typeof(Ops.UpdateShip))]
		[MessagePackKnownType("op_upwing", typeof(Ops.UpdateWing))]
		[MessagePackKnownType("op_sfm", typeof(Ops.SetFreeMove))]
		[MessagePackKnownType("op_upset", typeof(Ops.UpdateSettings))]
		[MessagePackKnownType("op_setc2", typeof(Ops.SetC2Type))]
		[MessagePackKnownType("up_chk", typeof(Updater.Check))]
		[MessagePackKnownType("up_st", typeof(Updater.CheckResult))]
		[MessagePackKnownType("up_chunk", typeof(Updater.Chunk))]
		public IMessagePayload payload;

		public Message() {
			payload = null;
		}

		/// <summary>
		/// Normal message constructor
		/// </summary>
		/// <param name="seq"></param>
		/// <param name="data"></param>
		public Message(int seq, IMessagePayload data) {
			sequence = seq;
			payload = data;
		}

		/// <summary>
		/// Push message constructor (for server use only)
		/// </summary>
		/// <param name="src"></param>
		/// <param name="data"></param>
		public Message(string src, IMessagePayload data) {
			sequence = -1;
			source = src;
			payload = data;
		}

		/// <summary>
		/// Serializes the message and sends it to the server
		/// </summary>
		/// <param name="sock"></param>
		/// <param name="m"></param>
		public static void Send(WebSocket sock, Message m) {
			MemoryStream stream = new MemoryStream();
			MessagePackSerializer.Get<Message>().Pack(stream, m);
			sock.Send(stream.ToArray());
		}

		/// <summary>
		/// Convenience function for sending a message
		/// </summary>
		/// <param name="sock"></param>
		/// <param name="returnTo"></param>
		/// <param name="data"></param>
		public static void Send(WebSocket sock, int sequence, 
			IMessagePayload data) {
			Send(sock, new Message(sequence, data));
		}

		public static void Push(WebSocket sock, string source, 
			IMessagePayload data) {
			Send(sock, new Message(source, data));
		}

		/// <summary>
		/// Deserializes a message from a byte array
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Message Receive(byte[] data) {
			MemoryStream stream = new MemoryStream(data);
			return MessagePackSerializer.Get<Message>().Unpack(stream);
		}
	}
}
