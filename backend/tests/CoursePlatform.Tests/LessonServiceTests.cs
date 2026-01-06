using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Interfaces;
using CoursePlatform.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CoursePlatform.Tests;

public class LessonServiceTests
{
    private readonly Mock<ILessonRepository> _lessonRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly LessonService _lessonService;

    public LessonServiceTests()
    {
        _lessonRepositoryMock = new Mock<ILessonRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _lessonService = new LessonService(_lessonRepositoryMock.Object, _courseRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateLesson_WithUniqueOrder_ShouldSucceed()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course { Id = courseId, Title = "Test Course" };

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _lessonRepositoryMock.Setup(x => x.OrderExistsInCourseAsync(courseId, 1, null))
            .ReturnsAsync(false);
        _lessonRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Lesson>()))
            .ReturnsAsync((Lesson l) => l);

        // Act
        var result = await _lessonService.CreateAsync(courseId, "Test Lesson", 1);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Lesson");
        result.Order.Should().Be(1);
        _lessonRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Lesson>()), Times.Once);
    }

    [Fact]
    public async Task CreateLesson_WithDuplicateOrder_ShouldFail()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course { Id = courseId, Title = "Test Course" };

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _lessonRepositoryMock.Setup(x => x.OrderExistsInCourseAsync(courseId, 1, null))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _lessonService.CreateAsync(courseId, "Test Lesson", 1);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Order 1 already exists in this course");
    }
}