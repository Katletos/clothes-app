using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.UnitTests;

public class ArchitectureTest
{
    private const string DomainNamespace = "Domain";

    private const string ApplicationNamespace = "Application";

    private const string InfrastructureNamespace = "Infrastructure";

    private const string WebNamespace = "WebAPI";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            WebNamespace,
        };

        var result = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        var otherProjects = new[]
        {
            InfrastructureNamespace,
            WebNamespace,
        };

        var result = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        var otherProjects = new[]
        {
            WebNamespace,
        };

        var result = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}