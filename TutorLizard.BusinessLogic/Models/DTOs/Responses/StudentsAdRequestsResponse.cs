using System;

namespace TutorLizard.BusinessLogic.Models.DTOs.Responses
{
	public class StudentsAdRequestsResponse
	{
		public List<AdRequestDto> AdRequests { get; set; } = new();
	}

}
