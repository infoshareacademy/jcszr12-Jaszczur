using System;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests
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
