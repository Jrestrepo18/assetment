using FluentAssertions;
using Moq;
using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Services;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Interfaces;
using Xunit;

namespace OnlineCoursesPlatform.Tests.Services;

public class LessonServiceTests
{
    private readonly Mock<ILessonRepository> _mockLessonRepository;
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly LessonService _lessonService;

    public LessonServiceTests()
    {
        _mockLessonRepository = new Mock<ILessonRepository>();
        _mockCourseRepository = new Mock<ICourseRepository>();
        _lessonService = new LessonService(_mockLessonRepository.Object, _mockCourseRepository.Object);
    }

    [Fact]
    public async Task GetLessonsByCourseAsync_ShouldReturnLessonsForCourse()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var lessons = new List<Lesson>
        {
            new Lesson { Id = Guid.NewGuid(), Title = "Lesson 1", CourseId = courseId, Order = 1 },
            new Lesson { Id = Guid.NewGuid(), Title = "Lesson 2", CourseId = courseId, Order = 2 }
        };

        _mockLessonRepository.Setup(r => r.GetLessonsByCourseAsync(courseId))
            .ReturnsAsync(lessons);

        // Act
        var result = await _lessonService.GetLessonsByCourseAsync(courseId);

        // Assert
        result.Should().HaveCount(2);
        _mockLessonRepository.Verify(r => r.GetLessonsByCourseAsync(courseId), Times.Once);
    }

    [Fact]
    public async Task GetLessonByIdAsync_WhenLessonExists_ShouldReturnLesson()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var lesson = new Lesson 
        { 
            Id = lessonId, 
            Title = "Test Lesson",
            Content = "Test Content",
            Order = 1
        };

        _mockLessonRepository.Setup(r => r.GetLessonWithCourseAsync(lessonId))
            .ReturnsAsync(lesson);

        // Act
        var result = await _lessonService.GetLessonByIdAsync(lessonId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(lessonId);
        result.Title.Should().Be("Test Lesson");
    }

    [Fact]
    public async Task CreateLessonAsync_ShouldCreateLessonWithNextOrder()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var createDto = new CreateLessonDto
        {
            Title = "New Lesson",
            Content = "Lesson Content",
            DurationInMinutes = 30,
            CourseId = courseId
        };

        _mockLessonRepository.Setup(r => r.GetMaxOrderByCourseAsync(courseId))
            .ReturnsAsync(2);
        _mockLessonRepository.Setup(r => r.AddAsync(It.IsAny<Lesson>()))
            .ReturnsAsync((Lesson l) => l);
        _mockLessonRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _lessonService.CreateLessonAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Lesson");
        result.Order.Should().Be(3); // maxOrder + 1
    }

    [Fact]
    public async Task UpdateLessonAsync_WhenLessonExists_ShouldUpdateAndReturn()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var lesson = new Lesson { Id = lessonId, Title = "Old Title" };
        var updateDto = new UpdateLessonDto
        {
            Title = "Updated Title",
            Content = "Updated Content",
            DurationInMinutes = 45
        };

        _mockLessonRepository.Setup(r => r.GetByIdAsync(lessonId))
            .ReturnsAsync(lesson);
        _mockLessonRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _lessonService.UpdateLessonAsync(lessonId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated Title");
        lesson.Content.Should().Be("Updated Content");
    }

    [Fact]
    public async Task DeleteLessonAsync_WhenLessonExists_ShouldSoftDelete()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var lesson = new Lesson { Id = lessonId, IsDeleted = false };

        _mockLessonRepository.Setup(r => r.GetByIdAsync(lessonId))
            .ReturnsAsync(lesson);
        _mockLessonRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _lessonService.DeleteLessonAsync(lessonId);

        // Assert
        result.Should().BeTrue();
        lesson.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task ReorderLessonsAsync_ShouldCallRepository()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var lessonIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        _mockLessonRepository.Setup(r => r.ReorderLessonsAsync(courseId, lessonIds))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _lessonService.ReorderLessonsAsync(courseId, lessonIds);

        // Assert
        result.Should().BeTrue();
        _mockLessonRepository.Verify(r => r.ReorderLessonsAsync(courseId, lessonIds), Times.Once);
    }
}
