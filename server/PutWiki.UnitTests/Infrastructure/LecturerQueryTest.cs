using System;
using System.Threading.Tasks;

using Application.Errors;
using Application.Features.Lecturers.Queries;

using Domain.Lecturers;

using FluentAssertions;

using Infrastructure;
using Infrastructure.Services;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PutWiki.UnitTests.Infrastructure;

public sealed class LecturerQueryTest : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly GetLecturerQueryHandler _sut;
    private readonly Lecturer _lecturer;

    public LecturerQueryTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _lecturer = Lecturer.Create("USOS-1001", ["dr"], "Jan Kowalski", "jan@example.edu");
        _context.Lecturers.Add(_lecturer);
        _context.SaveChanges();

        _sut = new GetLecturerQueryHandler(new LecturerQueryService(_context));
    }

    [Fact]
    public async Task ExecuteAsync_WithGuidIdentifier_ShouldReturnLecturerById()
    {
        var result = await _sut.ExecuteAsync(new GetLecturerQuery(_lecturer.Id.ToString()));

        result.IsSuccess.Should().BeTrue();
        Guid.Parse(result.Value.Id).Should().Be(_lecturer.Id);
    }

    [Fact]
    public async Task ExecuteAsync_WithSlugIdentifier_ShouldReturnLecturerBySlug()
    {
        var result = await _sut.ExecuteAsync(new GetLecturerQuery(_lecturer.Slug.Value));

        result.IsSuccess.Should().BeTrue();
        result.Value.Slug.Should().Be(_lecturer.Slug.Value);
    }

    [Fact]
    public async Task ExecuteAsync_WithUnknownValidSlug_ShouldReturnNotFound()
    {
        var result = await _sut.ExecuteAsync(new GetLecturerQuery("unknown-lecturer"));

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<NotFoundError>();
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidIdentifier_ShouldReturnValidationError()
    {
        var result = await _sut.ExecuteAsync(new GetLecturerQuery("not/a/valid-identifier"));

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<ValidationError>();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}