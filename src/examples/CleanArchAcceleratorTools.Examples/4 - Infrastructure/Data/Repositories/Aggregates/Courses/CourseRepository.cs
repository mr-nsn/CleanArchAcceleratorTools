using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Repositories;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;
using CleanArchAcceleratorTools.Infrastructure.DataTables;
using CleanArchAcceleratorTools.Infrastructure.ParallelProcessing;
using CleanArchAcceleratorTools.Infrastructure.Repository;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Repositories.Aggregates.Courses;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly DataContext _dataContext;
    private readonly IDbContextRegistratorService<DataContext> _registeredContextFactory;
    private readonly IApplicationLogger _logger;

    public CourseRepository(
        DataContext context,
        IDbContextRegistratorService<DataContext> registeredContextFactory,
        IApplicationLogger logger
    ) : base(context, new DbContextRegistratorServiceAdapter(registeredContextFactory))
    {
        _dataContext = context;
        _registeredContextFactory = registeredContextFactory;
        _logger = logger;
    }

    public async Task<ICollection<Course>> GetAllParallelAsync()
    {
        return await ParallelQueryExecutor.DoItParallelAsync
        (
            () => _registeredContextFactory.Create(Guid.NewGuid().ToString()).Courses.OrderBy(x => x.Id).AsQueryable(),
            new ParallelParams
            {
                TotalRegisters = _dataContext.Courses.Count(),
                BatchSize = 10,
                MaximumDegreeOfParalelism = Environment.ProcessorCount,
                MaxDegreeOfProcessesPerThread = 1
            },
            _logger
        );
    }

    public async Task AddInstructorsProcedureAsync(ICollection<Instructor> instructors)
    {
        var columnsOrder = new Dictionary<string, int>()
        {
            { "Id", 0 },
            { "FullName", 1 },
            { "CreatedAt", 2 }
        };        

        var dataTable = instructors.ToDataTable(columnsOrder, _context);

        var parametersAdd = new SqlParameter[]
        {
            new SqlParameter("@INSTRUCTORS", SqlDbType.Structured)
            {
                TypeName = "dbo.TP_TB_INSTRUCTOR",
                Value = dataTable
            }
        };

        await _context.Database.ExecuteSqlRawAsync($"EXEC [dbo].[USP_ADD_INSTRUCTORS] " + $"@INSTRUCTORS", parametersAdd);
    }

}
