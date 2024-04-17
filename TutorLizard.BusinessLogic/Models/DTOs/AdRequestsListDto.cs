using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs
{
    public class AdRequestsListDto(int id,
                                   int studentId,
                                   int adId,
                                   bool isAccepted,
                                   string message,
                                   string replyMessage,
                                   bool isRemote)
    {
        public int Id { get; set; } = id;
        public int StudentId { get; set;} = studentId;
        public int AdId { get; set;} = adId;
        public bool IsAccepted { get; set; } = isAccepted;
        public string Message { get; set; } = message;
        public string ReplyMessage { get; set; } = replyMessage;
        public bool IsRemote { get; set; } = isRemote;
    }
}
