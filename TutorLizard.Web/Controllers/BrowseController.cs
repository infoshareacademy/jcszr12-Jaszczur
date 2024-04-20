using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Controllers;
public class BrowseController : Controller
{
    private readonly IBrowseService _browseService;
    private readonly int _pageSize;

    public BrowseController(IBrowseService browseService)
    {
        _browseService = browseService;
        _pageSize = 10;
    }
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Ads));
    }

    public IActionResult Ads(int id = 1)
    {
        // TODO ask service for ads to show (using request)

        // TODO customize routing, so that parameter is page, not id
        int pageNumber = id;
        GetBrowseAdsPageRequest request = new(pageNumber, _pageSize);

        // mock results:
        GetBrowseAdsPageResponse response = new();
        response.PageNumber = pageNumber;
        response.PageSize = _pageSize;
        response.TotalPages = 5;

        response.Ads = [
                new(
                    id: 1,
                    tutorId: 1,
                    tutorName: "Nauczyciel 1",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 1,
                    categoryName: "Matematyka",
                    price: 100m,
                    location: "Warszawa",
                    isRemote: true),
                new(
                    id: 2,
                    tutorId: 2,
                    tutorName: "Nauczyciel 2",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 2,
                    categoryName: "Język polski",
                    price: 150m,
                    location: "Kraków",
                    isRemote: false),
                new(
                    id: 3,
                    tutorId: 1,
                    tutorName: "Nauczyciel 1",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 3,
                    categoryName: "Język angielski",
                    price: 1000m,
                    location: "Kosmos",
                    isRemote: true),
                new(
                    id: 4,
                    tutorId: 3,
                    tutorName: "Nauczyciel 3",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 4,
                    categoryName: "Geografia",
                    price: 10.50m,
                    location: "Papua Nowa Gwinea",
                    isRemote: true),
                new(
                    id: 5,
                    tutorId: 4,
                    tutorName: "Nauczyciel 4",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 1,
                    categoryName: "Matematyka",
                    price: 500m,
                    location: "Gdańsk",
                    isRemote: false),
                new(
                    id: 6,
                    tutorId: 5,
                    tutorName: "Nauczyciel 5",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 1,
                    categoryName: "Matematyka",
                    price: 499.99m,
                    location: "Gdańsk",
                    isRemote: false),
                new(
                    id: 7,
                    tutorId: 5,
                    tutorName: "Nauczyciel 5",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 2,
                    categoryName: "Język polski",
                    price: 99.99m,
                    location: "Gdańsk",
                    isRemote: true),
                new(
                    id: 8,
                    tutorId: 1,
                    tutorName: "Nauczyciel 1",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 5,
                    categoryName: "WF",
                    price: 159.73m,
                    location: "Warszawa",
                    isRemote: true),
                new(
                    id: 9,
                    tutorId: 2,
                    tutorName: "Nauczyciel 2",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 3,
                    categoryName: "Język angielski",
                    price: 10m,
                    location: "Gniezno",
                    isRemote: false),
                new(
                    id: 10,
                    tutorId: 19,
                    tutorName: "Nauczyciel 19",
                    subject: "tematyka",
                    title: "tytuł",
                    description: "opis",
                    categoryId: 19,
                    categoryName: "Metafizyka",
                    price: .99m,
                    location: "Berlin",
                    isRemote: false),
            ];

        return View(response);
    }

    public IActionResult AdDetails(int id)
    {
        // TODO ask service for ad to show based on id
        return View();
    }

    public IActionResult Schedule()
    {
        // TODO get userId from HttpContext and ask service for ScheduleItems based on it
        return View();
    }
}
