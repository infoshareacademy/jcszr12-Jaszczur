﻿using System;
using System.ComponentModel.DataAnnotations;
namespace TutorLizard.BusinessLogic.Models;

public class AdRequest
{
    public AdRequest()
    {
    }

    public AdRequest(int id,
                     int adId,
                     int studentId,
                     bool isAccepted,
                     string message,
                     bool isRemote)
    {
        Id = id;
        AdId = adId;
        IsAccepted = isAccepted;
        StudentId = studentId;
        Message = message;
        IsRemote = isRemote;
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide Ad Id!")]
    [Range(0, int.MaxValue, ErrorMessage = "Ad Id must be greater than 0!")]
    public int AdId { get; set; }

    [Required(ErrorMessage = "Please provide your id!")]
    [Range(0, int.MaxValue, ErrorMessage = "Id must be greater than 0!")]
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }

    [Required(ErrorMessage = "Please write a message!")]
    [MaxLength(250)]
    public string Message { get; set; }
    public string ReplyMessage { get; set; }
    DateTime ReviewDate { get; set; }
    public bool IsRemote { get; set; }
}
