using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.Blazor.Extensions;
using TutorLizard.Blazor.Models;
using TutorLizard.Web.Extensions;

namespace TutorLizard.Web.Tests.Controllers.Browse;
public class BrowseControllerSearchTests : BrowseControllerTestsBase
{
    [Fact]
    public void Search_WhenCalled_ShouldCorrectlyRedirectToAdsAction()
    {
        // Arrange
        AdSearchCriteriaViewModel searchCriteria = Fixture.Create<AdSearchCriteriaViewModel>();
        string searchJson = searchCriteria.Serialize();
        string expectedSearchString = searchCriteria.ToDto().ToBase64String();

        // Act
        var result = BrowseController.Search(searchJson);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Ads", ((RedirectToActionResult)result).ActionName);
        Assert.Equal("Browse", ((RedirectToActionResult)result).ControllerName);
        Assert.Equivalent(expectedSearchString, ((RedirectToActionResult)result).RouteValues?["search"]);
        Assert.False(((RedirectToActionResult)result).RouteValues?.ContainsKey("id"));
    }
}
