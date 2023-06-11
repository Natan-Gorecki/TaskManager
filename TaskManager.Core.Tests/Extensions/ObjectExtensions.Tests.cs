using TaskManager.Core.Extensions;
using TaskManager.Core.Models;

namespace TaskManager.Core.Tests.Extensions;

[TestFixture]
internal class ObjectExtensions
{
    [Test]
    public void CopyFrom_ShouldCopyFromITask()
    {
        // GIVEN
        Task destination = new()
        {
            Id = 1,
            Name = "destinationName",
            Status = ETaskStatus.Waiting
        };

        Task source = new()
        {
            Id = 2,
            Name = "sourceName",
            Status = ETaskStatus.Completed
        };

        // WHEN
        destination.CopyFrom(source);

        // THEN
        Assert.Multiple(() =>
        {
            Assert.That(destination.Id, Is.EqualTo(source.Id));
            Assert.That(destination.Name, Is.EqualTo(source.Name));
            Assert.That(destination.Status, Is.EqualTo(source.Status));
        });
    }

    [Test]
    public void CopyFrom_ShouldCopyFromTask()
    {
        // GIVEN
        Task destination = new()
        {
            Id = 1,
            Name = "destinationName",
            Status = ETaskStatus.Waiting
        };

        Task source = new()
        {
            Id = 2,
            Name = "sourceName",
            Status = ETaskStatus.Completed
        };

        // WHEN
        destination.CopyFrom(source);

        // THEN
        Assert.Multiple(() =>
        {
            Assert.That(destination.Id, Is.EqualTo(source.Id));
            Assert.That(destination.Name, Is.EqualTo(source.Name));
            Assert.That(destination.Status, Is.EqualTo(source.Status));
        });
    }
}
