using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Controllers;
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public StudentController(IStudentService studentService,
                             IUserAuthenticationService userAuthenticationService)
    {
        _studentService = studentService;
        _userAuthenticationService = userAuthenticationService;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult AdRequests()
    {
        int? studentId = _userAuthenticationService.GetLoggedInUserId();
        StudentsAdRequestsRequest request = new(studentId);

        try
        {
            if (ModelState.IsValid == false)
            {
                return View(request);
            }
                if(studentId is null)
            {
                return View(request);
            }
            request.StudentId = (int)studentId;

            StudentsAdRequestsResponse response = new()
            {
                //do poprawy, choć to test
                AdRequests = [new AdRequestDto(1, 1, 1, true, "xyz", "yxz", "2024, 04, 10", false)]
            };
        }

        catch
        {
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }
}
