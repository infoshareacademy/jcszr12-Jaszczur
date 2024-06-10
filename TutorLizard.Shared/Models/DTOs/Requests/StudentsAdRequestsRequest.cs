using System;

namespace TutorLizard.Shared.Models.DTOs.Requests
{
	public class StudentsAdRequestsRequest
	{
		public int StudentId { get; set; }
		public StudentsAdRequestsRequest(int? studentId)
		{
			StudentId = (int)studentId;
		}
	}
}
