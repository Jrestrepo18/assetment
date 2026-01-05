using FluentAssertions;
using Moq;
using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Services;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Enums;
using OnlineCoursesPlatform.Domain.Interfaces;
using Xunit;

namespace OnlineCoursesPlatform.Tests.Services;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _mockCourseRepository = new Mock<ICourseRepository>();
        _courseService = new CourseService(_mockCourseRepository.Object);
    }

    [Fact]
    public async Task GetAllCoursesAsync_ShouldReturnAllCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = Guid.NewGuid(), Title = "Course 1", Status = CourseStatus.Published },
            new Course { Id = Guid.NewGuid(), Title = "Course 2", Status = CourseStatus.Draft }
        };

        _mockCourseRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(courses);

        // Act
        var result = await _courseService.GetAllCoursesAsync();

        // Assert
        result.Should().HaveCount(2);
        _mockCourseRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCourseByIdAsync_WhenCourseExists_ShouldReturnCourse()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            Lessons = new List<Lesson>()
        };

        _mockCourseRepository.Setup(r => r.GetCourseWithLessonsAsync(courseId))
            .ReturnsAsync(course);

        // Act
        var result = await _courseService.GetCourseByIdAsync(courseId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(courseId);
        result.Title.Should().Be("Test Course");
    }

    [Fact]
    public async Task GetCourseByIdAsync_WhenCourseDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        _mockCourseRepository.Setup(r => r.GetCourseWithLessonsAsync(courseId))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.GetCourseByIdAsync(courseId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCourseAsync_ShouldCreateAndReturnCourse()
    {
        // Arrange
        var instructorId = Guid.NewGuid();
        var createDto = new CreateCourseDto
        {
            Title = "New Course",
            Description = "Course Description",
            Price = 49.99m,
            DurationInHours = 10
        };

        _mockCourseRepository.Setup(r => r.AddAsync(It.IsAny<Course>()))
            .ReturnsAsync((Course c) => c);
        _mockCourseRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _courseService.CreateCourseAsync(createDto, instructorId);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Course");
        result.Status.Should().Be(CourseStatus.Draft);
        _mockCourseRepository.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Once);
        _mockCourseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCourseAsync_WhenCourseExists_ShouldReturnTrue()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course { Id = courseId, Title = "Course to Delete" };

        _mockCourseRepository.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _mockCourseRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _courseService.DeleteCourseAsync(courseId);

        // Assert
        result.Should().BeTrue();
        course.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCourseAsync_WhenCourseDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        _mockCourseRepository.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.DeleteCourseAsync(courseId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PublishCourseAsync_WhenCourseExists_ShouldSetStatusToPublished()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course { Id = courseId, Status = CourseStatus.Draft };

        _mockCourseRepository.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _mockCourseRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _courseService.PublishCourseAsync(courseId);

        // Assert
        result.Should().BeTrue();
        course.Status.Should().Be(CourseStatus.Published);
    }
}
