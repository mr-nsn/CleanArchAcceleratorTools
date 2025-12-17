using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using CleanArchAcceleratorTools.Mediator.DomainNotification.Handler;
using Moq;

namespace CleanArchAcceleratorTools.Test.Base.Fixtures;

public class FixtureBase
{
    public Mock<ICleanArchMediator> MediatorMock { get; private set; }
    public Mock<DomainNotificationEventHandler> NotificationsMock { get; private set; }
    public Mock<IApplicationLogger> ApplicationLoggerMock { get; private set; }
    public DataContext Context { get; private set; }
    public Mock<IDbContextRegistratorService<DataContext>> DbContextRegistratorServiceMock { get; private set; }
    public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }

    protected FixtureBase()
    {
        MediatorMock = new Mock<ICleanArchMediator>();
        NotificationsMock = new Mock<DomainNotificationEventHandler>();
        ApplicationLoggerMock = new Mock<IApplicationLogger>();
        Context = new DataContextFixture().GetDataContext();
        DbContextRegistratorServiceMock = new Mock<IDbContextRegistratorService<DataContext>>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
    }

    public virtual void Reset()
    {
        MediatorMock = new Mock<ICleanArchMediator>();
        NotificationsMock = new Mock<DomainNotificationEventHandler>();
        ApplicationLoggerMock = new Mock<IApplicationLogger>();
        Context = new DataContextFixture().GetDataContext();
        DbContextRegistratorServiceMock = new Mock<IDbContextRegistratorService<DataContext>>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
    }

    public async void CommitAsync()
    {
        await Context.SaveChangesAsync();
    }

    public void Commit()
    {
        Context.SaveChanges();
    }
}
