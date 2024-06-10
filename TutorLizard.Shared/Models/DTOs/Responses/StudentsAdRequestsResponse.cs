using System;

namespace TutorLizard.Shared.Models.DTOs.Responses
{
	public class StudentsAdRequestsResponse
	{
		public List<AdRequestsListDto> AdRequests { get; set; } = new();
	}

}
