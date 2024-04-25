using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Controllers;
[Authorize]
public class TutorController : Controller
{
    private readonly ITutorService _tutorService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IDbRepository<Category> _categoryRepository;

    public TutorController(ITutorService tutorService,
                           IUserAuthenticationService userAuthenticationService,
                           IDbRepository<Category> categoryRepository)
    {
        _tutorService = tutorService;
        _userAuthenticationService = userAuthenticationService;
        _categoryRepository = categoryRepository;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> CreateAd()
    {
        // TODO - move obtaining categories to some service
        await AddCategoriesToViewBag();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAd(CreateAdRequest request)
    {
        try
        {
            if(ModelState.IsValid == false)
                return View(request);

            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if(tutorId is null)
            {
                return View(request);
            }
            request.TutorId = (int)tutorId;

            CreateAdResponse response = new()
            {
                SuccessfullyCreated = true,
                CreatedAdId = 1
            };

            if (response.SuccessfullyCreated)
            {
                // TODO show notification
                // TODO redirect to created Ad's details
            }
            else
            {
                // TODO show notification
            }
        }
        catch
        {
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptScheduleItemRequest(int scheduleItemRequestId, int adId)
    {

        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            // TODO - show failure notification
            // TODO redirect to details of ad with Id == adId
            return RedirectToAction(nameof(Index));
        }
        AcceptScheduleItemRequestRequest request = new()
        {
            ScheduleItemRequestId = scheduleItemRequestId,
            TutorId = (int)tutorId
        };

        // TODO - replace mock response with call to _tutorService
        AcceptScheduleItemRequestResponse response = new()
        {
            Success = true
        };

        if (response.Success)
        {
            // TODO - show success notification

        }
        else
        {
            // TODO - show failure notification

        }

        // TODO redirect to details of ad with Id == adId
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnacceptScheduleItemRequest(int scheduleItemRequestId, int adId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            // TODO - show failure notification
            // TODO redirect to details of ad with Id == adId
            return RedirectToAction(nameof(Index));
        }
        UnacceptScheduleItemRequestRequest request = new()
        {
            ScheduleItemRequestId = scheduleItemRequestId,
            TutorId = (int)tutorId
        };

        // TODO - replace mock response with call to _tutorService
        UnacceptScheduleItemRequestResponse response = new()
        {
            Success = true
        };

        if (response.Success)
        {
            // TODO - show success notification

        }
        else
        {
            // TODO - show failure notification

        }

        // TODO redirect to details of ad with Id == adId
        return RedirectToAction(nameof(Index));
    }

    private async Task AddCategoriesToViewBag()
    {
        List<Category> categories = await _categoryRepository
                        .GetAll()
                        .ToListAsync();

        List<CategoryDto> categoryDtos = categories.Select(c => c.ToDto()).ToList();

        ViewBag.Categories = new SelectList(items: categoryDtos,
                                            dataValueField: nameof(CategoryDto.Id),
                                            dataTextField: nameof(CategoryDto.Name));
    }
}
