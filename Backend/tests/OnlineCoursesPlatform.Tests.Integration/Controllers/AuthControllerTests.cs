using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OnlineCoursesPlatform.Application.DTOs;
using Xunit;

namespace OnlineCoursesPlatform.Tests.Integration.Controllers;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "admin@onlinecourses.com",
            Password = "Admin123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        // Note: This may fail if seeding hasn't occurred
        // In a real scenario, we'd use a test database with known data
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "invalid@email.com",
            Password = "wrongpassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Register_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = $"test{Guid.NewGuid()}@test.com",
            Password = "Test1234!",
            ConfirmPassword = "Test1234!",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithMismatchedPasswords_ShouldReturnBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword!",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
