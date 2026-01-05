using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OnlineCoursesPlatform.Application.DTOs;
using Xunit;

namespace OnlineCoursesPlatform.Tests.Integration.Controllers;

public class LessonsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public LessonsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetLessonsByCourse_ShouldReturnOk()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/lessons/course/{courseId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetLessonsByCourse_ShouldReturnListOfLessons()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/lessons/course/{courseId}");
        var lessons = await response.Content.ReadFromJsonAsync<IEnumerable<LessonDto>>();

        // Assert
        lessons.Should().NotBeNull();
    }

    [Fact]
    public async Task GetLesson_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/lessons/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateLesson_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var createDto = new CreateLessonDto
        {
            Title = "Test Lesson",
            Content = "Test Content",
            DurationInMinutes = 30,
            CourseId = Guid.NewGuid()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/lessons", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateLesson_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var updateDto = new UpdateLessonDto
        {
            Title = "Updated Lesson",
            Content = "Updated Content",
            DurationInMinutes = 45
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/lessons/{lessonId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteLesson_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var lessonId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/lessons/{lessonId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ReorderLessons_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var lessonIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/lessons/course/{courseId}/reorder", lessonIds);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
