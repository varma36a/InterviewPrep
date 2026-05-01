namespace SaaSPlatform.ArchitectureTests;

public sealed class DependencyRulesTests
{
    [Fact]
    public void DomainProject_ShouldNotReferenceInfrastructureProject()
    {
        var repositoryRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var domainProjectPath = Path.Combine(
            repositoryRoot,
            "src",
            "SaaSPlatform.Domain",
            "SaaSPlatform.Domain.csproj");
        var projectContents = File.ReadAllText(domainProjectPath);

        Assert.DoesNotContain("SaaSPlatform.Infrastructure", projectContents, StringComparison.OrdinalIgnoreCase);
    }
}
