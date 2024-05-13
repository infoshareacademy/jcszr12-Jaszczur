using System;

namespace TutorLizard.BusinessLogic.Models.DTOs.Responses
{
	public class StudentsAdRequestsResponse
	{
		public List<AdRequestsListDto> AdRequests { get; set; } = new();
	}

}
