using System;

namespace TutorLizard.Shared.Models.DTOs.Responses
{
	public class GetStudentsAdRequestsResponse
	{
		public List<AdRequestsListDto> AdRequests { get; set; } = new();
	}

}
