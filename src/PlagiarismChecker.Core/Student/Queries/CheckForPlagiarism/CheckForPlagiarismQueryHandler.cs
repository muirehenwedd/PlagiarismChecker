using PlagiarismChecker.Core.Common.Extensions;
using PlagiarismChecker.Domain.Repository;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Common.Services;
using PlagiarismChecker.Core.Common.Services.Models;
using PlagiarismChecker.Core.Options;
using PlagiarismChecker.Core.Student.Exceptions;

namespace PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

public sealed class CheckForPlagiarismQueryHandler
    : IQueryHandler<CheckForPlagiarismQuery, CheckForPlagiarismQueryResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDocumentComparerService _documentComparerService;
    private readonly IOptions<PlagiarismCheckOptions> _options;

    public CheckForPlagiarismQueryHandler(
        IApplicationDbContext dbContext,
        IDocumentComparerService documentComparerService,
        IOptions<PlagiarismCheckOptions> options
    )
    {
        _dbContext = dbContext;
        _documentComparerService = documentComparerService;
        _options = options;
    }

    public async ValueTask<CheckForPlagiarismQueryResult> Handle(
        CheckForPlagiarismQuery query,
        CancellationToken cancellationToken
    )
    {
        var userId = query.User.GetUserId();

        var assignment = await _dbContext.StudentAssignments
            .AsNoTracking()
            .Include(a => a.AssignmentFiles)
            .ThenInclude(file => file.Document)
            .FirstOrDefaultAsync(a => a.Id == query.AssignmentId, cancellationToken);

        if (assignment is null)
            throw new AssignmentNotFoundException();

        if (assignment.OwnerId != userId)
            throw new AssignmentAccessDeniedException();

        var allBaseFilesDocuments = _dbContext
            .BaseFiles
            .AsNoTracking()
            .Select(file => new {file.Document, file.FileName})
            .AsAsyncEnumerable();

        var otherStudentDocuments = _dbContext
            .AssignmentFiles
            .AsNoTracking()
            .Where(af => af.Assignment.OwnerId != userId)
            .Select(file => new {file.Document, file.FileName})
            .AsAsyncEnumerable();

        var baseDocuments = allBaseFilesDocuments.Concat(otherStudentDocuments);

        var matches = new List<CheckForPlagiarismQueryResult.Match>();

        var comparisonParameters = new DocumentComparisonParameters(
            MismatchPercentage: query.MismatchPercentage ?? _options.Value.MismatchPercentage,
            MismatchTolerance: query.MismatchTolerance ?? _options.Value.MismatchTolerance,
            PhraseLength: query.PhraseLength ?? _options.Value.PhraseLength,
            WordThreshold: query.WordThreshold ?? _options.Value.WordThreshold
        );

        await foreach (var group1Document in baseDocuments)
        {
            foreach (var studentAssignments in assignment.AssignmentFiles)
            {
                var pairResult = _documentComparerService.Compare(
                    group1Document.Document,
                    studentAssignments.Document,
                    comparisonParameters);

                if (pairResult.PerfectMatch > comparisonParameters.WordThreshold)
                {
                    var match = new CheckForPlagiarismQueryResult.Match(
                        DocumentNameLeft: group1Document.FileName,
                        DocumentNameRight: studentAssignments.FileName,
                        PerfectMatch: pairResult.PerfectMatch,
                        PerfectMatchPercentLeft: pairResult.PerfectMatchPercentLeft,
                        PerfectMatchPercentRight: pairResult.PerfectMatchPercentRight,
                        OverallMatchCountLeft: pairResult.OverallMatchCountLeft,
                        OverallMatchCountRight: pairResult.OverallMatchCountRight,
                        OverallMatchPercentLeft: pairResult.OverallMatchPercentLeft,
                        OverallMatchPercentRight: pairResult.OverallMatchPercentRight
                    );

                    matches.Add(match);
                }
            }
        }

        return new CheckForPlagiarismQueryResult
        (
            PlagiarismFound: matches.Count != 0,
            Matches: matches
        );
    }
}