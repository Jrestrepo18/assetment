using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Interfaces;
using CoursePlatform.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CoursePlatform.Tests;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _courseService = new CourseService(_courseRepositoryMock.Object);
    }

    [Fact]
    public async Task PublishCourse_WithLessons_ShouldSucceed()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course
        {
            Id = courseId,
            Title = "Test Course",
            Status = CourseStatus.Draft
        };

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _courseRepositoryMock.Setup(x => x.GetActiveLessonsCountAsync(courseId))
            .ReturnsAsync(3);

        // Act
        await _courseService.PublishAsync(courseId);

        // Assert
        course.Status.Should().Be(CourseStatus.Published);
        _courseRepositoryMock.Verify(x => x.UpdateAsync(course), Times.Once);
    }

    [Fact]
    public async Task PublishCourse_WithoutLessons_ShouldFail()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course
        {
            Id = courseId,
            Title = "Test Course",
            Status = CourseStatus.Draft
        };

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _courseRepositoryMock.Setup(x => x.GetActiveLessonsCountAsync(courseId))
            .ReturnsAsync(0);

        // Act
        var act = async () => await _courseService.PublishAsync(courseId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot publish a course without active lessons");
    }

    [Fact]
    public async Task DeleteCourse_ShouldBeSoftDelete()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act
        await _courseService.DeleteAsync(courseId);

        // Assert
        _courseRepositoryMock.Verify(x => x.DeleteAsync(courseId), Times.Once);
    }
}