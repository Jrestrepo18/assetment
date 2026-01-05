using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OnlineCoursesPlatform.Application.DTOs;
using Xunit;

namespace OnlineCoursesPlatform.Tests.Integration.Controllers;

public class CoursesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CoursesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCourses_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCourses_ShouldReturnListOfCourses()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");
        var courses = await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>();

        // Assert
        courses.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCourse_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/courses/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SearchCourses_WithEmptyTerm_ShouldReturnEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/courses/search?term=");
        var courses = await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        courses.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateCourse_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var createDto = new CreateCourseDto
        {
            Title = "Test Course",
            Description = "Test Description",
            Price = 29.99m,
            DurationInHours = 5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/courses", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateCourse_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var updateDto = new UpdateCourseDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Price = 39.99m,
            DurationInHours = 8
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/courses/{courseId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteCourse_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/courses/{courseId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
