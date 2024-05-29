using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TutorLizard.Web.Services;
using TutorLizard.Web.Strings;

namespace TutorLizard.Web.Tests.Services;
public class UiMessagesServiceTests
{
    private readonly UiMessagesService _uiMessagesService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new();
    private readonly Mock<ITempDataDictionaryFactory> _mockTempDataFactory = new();
    private readonly Mock<ITempDataDictionary> _mockTempDataDictionary = new();
    public UiMessagesServiceTests()
    {
        _uiMessagesService = new(_mockHttpContextAccessor.Object,
                                 _mockTempDataFactory.Object);
        SetupMockHttpContext();
        SetupMockTempData();
    }

    [Fact]
    public void ShowMessage_WhenHttpContextIsNull_ShouldNotThrowException()
    {
        // Arrange
        _mockHttpContextAccessor
            .Setup(x => x.HttpContext)
            .Returns<HttpContext>(null!);

        // Act
        var act = () => _uiMessagesService.ShowMessage("", "");
        var exception = Record.Exception(act);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ShowMessage_WhenMessageIsNull_ShouldReplaceItWithEmptyString()
    {
        // Arrange
        var capturedTempData = SetupCaptureOfTempData();
        string message = null!;
        string expectedMessage = "";
        string messageType = "Test";

        // Act
        _uiMessagesService.ShowMessage(message, messageType);
        var actualMessage = capturedTempData[messageType];

        // Assert
        Assert.NotNull(actualMessage);
        Assert.IsType<string>(actualMessage);
        Assert.Equal(expectedMessage, actualMessage);
    }

    [Fact]
    public void ShowMessage_WhenMessageTypeIsNull_ShouldReplaceItWithEmptyString()
    {
        // Arrange
        var capturedTempData = SetupCaptureOfTempData();
        string message = "Test message";
        string messageType = null!;
        string expecedMessageType = "";

        // Act
        _uiMessagesService.ShowMessage(message, messageType);
        var actualMessageType = capturedTempData.Keys.Single();

        // Assert
        Assert.NotNull(actualMessageType);
        Assert.Equal(expecedMessageType, actualMessageType);
    }

    [Fact]
    public void ShowMessage_WhenCalled_ShouldCorrectlyAddMessageToTempData()
    {
        // Arrange
        var capturedTempData = SetupCaptureOfTempData();
        string message = "Test message";
        string messageType = "Test";
        string expectedMessage = message;
        string expectedMessageType = messageType;

        // Act
        _uiMessagesService.ShowMessage(message, messageType);
        var actualMessage = capturedTempData.Values.Single();
        var actualMessageType = capturedTempData.Keys.Single();

        // Assert
        Assert.IsType<string>(actualMessage);
        Assert.Equal(expectedMessage, actualMessage);
        Assert.Equal(expectedMessageType, actualMessageType);
    }

    [Fact]
    public void ShowSuccessMessage_WhenCalled_ShouldAddMessageWithCorrectType()
    {
        // Arrange
        var capturedTempData = SetupCaptureOfTempData();
        string message = "Test message";
        string expectedMessage = message;
        string expectedMessageType = MessageType.Success;

        // Act
        _uiMessagesService.ShowSuccessMessage(message);
        var actualMessage = capturedTempData.Values.Single();
        var actualMessageType = capturedTempData.Keys.Single();

        // Assert
        Assert.IsType<string>(actualMessage);
        Assert.Equal(expectedMessage, actualMessage);
        Assert.Equal(expectedMessageType, actualMessageType);
    }

    [Fact]
    public void ShowFailureMessage_WhenCalled_ShouldAddMessageWithCorrectType()
    {
        // Arrange
        var capturedTempData = SetupCaptureOfTempData();
        string message = "Test message";
        string expectedMessage = message;
        string expectedMessageType = MessageType.Failure;

        // Act
        _uiMessagesService.ShowFailureMessage(message);
        var actualMessage = capturedTempData.Values.Single();
        var actualMessageType = capturedTempData.Keys.Single();

        // Assert
        Assert.IsType<string>(actualMessage);
        Assert.Equal(expectedMessage, actualMessage);
        Assert.Equal(expectedMessageType, actualMessageType);
    }

    private void SetupMockHttpContext()
    {
        _mockHttpContextAccessor
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());
    }

    private void SetupMockTempData()
    {
        _mockTempDataFactory
            .Setup(x => x.GetTempData(It.IsAny<HttpContext>()))
            .Returns(_mockTempDataDictionary.Object);
    }

    private Dictionary<string, object?> SetupCaptureOfTempData()
    {
        Dictionary<string, object?> output = [];

        _mockTempDataDictionary
            .SetupSet(x => x[It.IsAny<string>()] = It.IsAny<object?>())
            .Callback<string, object?>((key, value) => output[key] = value);

        return output;
    }
}
